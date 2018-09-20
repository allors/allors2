import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, MediaService, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { Country, CustomOrganisationClassification, Organisation, OrganisationRole, Media, OrganisationClassification } from '../../../../../domain';
import { And, ContainedIn, Contains, Equals, Like, Predicate, PullRequest, Sort, TreeNode, Filter } from '../../../../../framework';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap, tap } from 'rxjs/operators';

interface SearchData {
  name?: string;
  country?: Country;
  role?: OrganisationRole;
  classification?: OrganisationClassification;
  contactFirstName?: string;
  contactLastName?: string;
}

@Component({
  templateUrl: './organisations-overview.component.html',
})
export class OrganisationsOverviewComponent implements OnInit, OnDestroy {

  public title = 'Organisations';

  public countries: Country[];
  public roles: OrganisationRole[];
  public classifications: CustomOrganisationClassification[];
  public organisations: Organisation[];

  public search$: BehaviorSubject<SearchData>;
  public refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle('Organisations');

    this.scope = this.workspaceService.createScope();
    this.search$ = new BehaviorSubject<SearchData>({});
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  ngOnInit(): void {

    const { m, pull } = this.dataService;

    const search$ = this.search$
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
      );

    this.subscription = combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([data, refresh, internalOrganisationId]) => {

          const predicate: And = new And();
          const predicates: Predicate[] = predicate.operands;

          if (data.role) {
            if (data.role.UniqueId.toUpperCase() === '32E74BEF-2D79-4427-8902-B093AFA81661') {
              predicates.push(new Equals({ propertyType: m.Organisation.IsManufacturer, value: true }));
            }
          }

          if (data.classification) {
            predicates.push(new Contains({ propertyType: m.Organisation.OrganisationClassifications, object: data.classification }));
          }

          if (data.country) {
            const postalAddressQuery = new Filter(
              {
                objectType: m.PostalAddress,
                predicate: new ContainedIn({
                  propertyType: m.PostalAddress.PostalBoundary,
                  extent: new Filter(
                    {
                      objectType: m.PostalBoundary,
                      predicate: new Equals({ propertyType: m.PostalBoundary.Country, object: data.country }),
                    })
                }),
              });

            predicates.push(new ContainedIn({ propertyType: m.Organisation.GeneralCorrespondence, extent: postalAddressQuery }));
          }

          if (data.contactFirstName || data.contactLastName) {
            const contactPredicate: And = new And();
            const contactOperands: Predicate[] = contactPredicate.operands;

            if (data.contactFirstName) {
              const like: string = '%' + data.contactFirstName + '%';
              contactOperands.push(new Like({ roleType: m.Person.FirstName, value: like }));
            }

            if (data.contactLastName) {
              const like: string = '%' + data.contactLastName + '%';
              contactOperands.push(new Like({ roleType: m.Person.LastName, value: like }));
            }

            predicates.push(new ContainedIn({
              propertyType: m.Organisation.CurrentOrganisationContactRelationships,
              extent: new Filter(
                {
                  objectType: m.OrganisationContactRelationship,
                  predicate: new ContainedIn({
                    propertyType: m.OrganisationContactRelationship.Contact,
                    extent: new Filter(
                      {
                        objectType: m.Person,
                        predicate: contactPredicate,
                      })
                  }),
                })
            }));
          }

          if (data.name) {
            const like: string = data.name.replace('*', '%') + '%';
            predicates.push(new Like({ roleType: m.Organisation.Name, value: like }));
          }

          const pulls = [
            pull.OrganisationRole({
              sort: new Sort(m.OrganisationRole.Name)
            }),
            pull.CustomOrganisationClassification({
              sort: new Sort(m.CustomOrganisationClassification.Name)
            }),
            pull.Country({
              sort: new Sort(m.Country.Name)
            }),
            pull.Organisation(
              {
                predicate,
                include: {
                  LogoImage: x,
                  OrganisationClassifications: x,
                  GeneralPhoneNumber: x,
                  GeneralCorrespondence: {
                    PostalBoundary: {
                      Country: x
                    }
                  }
                },
              }
            )
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.scope.session.reset();

        this.roles = loaded.collections.OrganisationRoles as OrganisationRole[];
        this.countries = loaded.collections.Countries as Country[];
        this.classifications = loaded.collections.CustomOrganisationClassifications as CustomOrganisationClassification[];
        this.organisations = loaded.collections.Organisations as Organisation[];
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public delete(organisation: Organisation): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this organisation?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(organisation.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
                this.refresh();
              });
        }
      });
  }

  public goBack(): void {
    window.history.back();
  }

  public onView(organisation: Organisation): void {
    this.router.navigate(['/relations/organisation/' + organisation.id]);
  }
}
