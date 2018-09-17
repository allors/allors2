import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar, MatSort, MatPaginator } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, MediaService, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { Country, CustomOrganisationClassification, InternalOrganisation, Organisation, OrganisationRole, Media } from '../../../../../domain';
import { And, ContainedIn, Contains, Equals, Like, Predicate, PullRequest, Sort, TreeNode, Filter } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

interface Row {
  object: Organisation;
  logo: Media;
  name: string;
  classification: string;
  phone: string;
  address: string;
  address2: string;
  address3: string;
}

interface SearchData {
  name: string;
  country: string;
  role: string;
  classifications: string;
  contactFirstName: string;
  contactLastName: string;
}

@Component({
  templateUrl: './organisations-overview.component.html',
})
export class OrganisationsOverviewComponent implements OnInit, OnDestroy {

  public title = 'Organisations';
  public total: number;
  public searchForm: FormGroup; public advancedSearch: boolean;

  public data: Organisation[];

  public columns = [{ prop: 'name' }, { prop: 'classification' }, { prop: 'phone' }, { prop: 'address' }, { prop: 'country' }, { prop: 'actions' }];
  public rows: Row[] = [];
  public selected: Row[] = [];

  public countries: Country[];
  public selectedCountry: Country;
  public country: Country;

  public roles: OrganisationRole[];
  public selectedRole: OrganisationRole;
  public role: OrganisationRole;

  public classifications: CustomOrganisationClassification[];
  public selectedClassification: CustomOrganisationClassification;
  public classification: CustomOrganisationClassification;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle('Organisations');

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [''],
      country: [''],
      role: [''],
      classification: [''],
      contactFirstName: [''],
      contactLastName: [''],
    });
  }

  ngOnInit(): void {

    const { m, pull } = this.dataService;

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$ = combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousData, previousDate, previousInternalOrganisationId], [data, date, internalOrganisationId]) => {
          return [data, date, internalOrganisationId];
        }, []));

    this.subscription = combined$
      .pipe(
        switchMap(([data, take, , internalOrganisationId]) => {

          const pulls = [
            pull.OrganisationRole(
              {
                sort: new Sort(m.OrganisationRole.Name)
              }
            ),
            pull.CustomOrganisationClassification({
              sort: new Sort(m.CustomOrganisationClassification.Name)
            }),
            pull.Country({
              sort: new Sort(m.Country.Name)
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                this.roles = loaded.collections.organisationRoles as OrganisationRole[];
                this.role = this.roles.find((v: OrganisationRole) => v.Name === data.role);

                this.countries = loaded.collections.countries as Country[];
                this.country = this.countries.find((v: Country) => v.Name === data.country);

                this.classifications = loaded.collections.classifications as CustomOrganisationClassification[];
                this.classification = this.classifications.find((v: CustomOrganisationClassification) => v.Name === data.classification);

                const contactPredicate: And = new And();
                const contactPredicates: Predicate[] = contactPredicate.operands;

                if (data.contactFirstName) {
                  const like: string = '%' + data.contactFirstName + '%';
                  contactPredicates.push(new Like({ roleType: m.Person.FirstName, value: like }));
                }

                if (data.contactLastName) {
                  const like: string = '%' + data.contactLastName + '%';
                  contactPredicates.push(new Like({ roleType: m.Person.LastName, value: like }));
                }

                const contactQuery = new Filter(
                  {
                    name: 'contacts',
                    objectType: m.Person,
                    predicate: contactPredicate,
                  });

                const organisationContactRelationshipPredicate: And = new And();
                const organisationContactRelationshipPredicates: Predicate[] = organisationContactRelationshipPredicate.operands;
                organisationContactRelationshipPredicates.push(new ContainedIn({ propertyType: m.OrganisationContactRelationship.Contact, extent: contactQuery }));

                const organisationContactRelationshipQuery = new Filter(
                  {
                    name: 'organisationContactRelationships',
                    objectType: m.OrganisationContactRelationship,
                    predicate: organisationContactRelationshipPredicate,
                  });

                const postalBoundaryPredicate: And = new And();
                const postalBoundaryPredicates: Predicate[] = postalBoundaryPredicate.operands;

                if (data.country) {
                  postalBoundaryPredicates.push(new Equals({ propertyType: m.PostalBoundary.Country, value: this.country }));
                }

                const postalBoundaryQuery = new Filter(
                  {
                    name: 'postalBoundaries',
                    objectType: m.PostalBoundary,
                    predicate: postalBoundaryPredicate,
                  });

                const postalAddressPredicate: And = new And();
                const postalAddressPredicates: Predicate[] = postalAddressPredicate.operands;
                postalAddressPredicates.push(new ContainedIn({ propertyType: m.PostalAddress.PostalBoundary, extent: postalBoundaryQuery }));

                const postalAddressQuery = new Filter(
                  {
                    name: 'postalAddresses',
                    objectType: m.PostalAddress,
                    predicate: postalAddressPredicate,
                  });

                const predicate: And = new And();
                const predicates: Predicate[] = predicate.operands;

                if (data.role) {
                  if (this.role.UniqueId.toUpperCase() === '32E74BEF-2D79-4427-8902-B093AFA81661') {
                    predicates.push(new Equals({ propertyType: m.Organisation.IsManufacturer, value: true }));
                  }
                }

                if (data.classification) {
                  predicates.push(new Contains({ propertyType: m.Organisation.OrganisationClassifications, object: this.classification }));
                }

                if (data.country) {
                  predicates.push(new ContainedIn({ propertyType: m.Organisation.GeneralCorrespondence, extent: postalAddressQuery }));
                }

                if (data.contactFirstName || data.contactLastName) {
                  predicates.push(new ContainedIn({ propertyType: m.Organisation.CurrentOrganisationContactRelationships, extent: organisationContactRelationshipQuery }));
                }

                if (data.name) {
                  const like: string = data.name.replace('*', '%') + '%';
                  predicates.push(new Like({ roleType: m.Organisation.Name, value: like }));
                }

                const queries = [
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
                      }
                    }
                  )
                ];

                return this.scope
                  .load('Pull', new PullRequest({ pulls: queries }));
              })
            );
        })
      )
      .subscribe((loaded) => {
        this.scope.session.reset();

        this.data = loaded.collections.organisations as Organisation[];
        this.rows = this.data.map<Row>((v: Organisation) => {
          return {
            object: v,
            logo: v.LogoImage,
            name: v.PartyName,
            classification: v.OrganisationClassifications.map(w => w.Name).join(', '),
            phone: `${v.GeneralPhoneNumber ? v.GeneralPhoneNumber.CountryCode : ''} ${v.GeneralPhoneNumber ? v.GeneralPhoneNumber.AreaCode : ''} ${v.GeneralPhoneNumber ? v.GeneralPhoneNumber.ContactNumber : ''}`,
            address: `${v.GeneralCorrespondence && v.GeneralCorrespondence.Address1 ? v.GeneralCorrespondence.Address1 : ''} ${v.GeneralCorrespondence && v.GeneralCorrespondence.Address2 ? v.GeneralCorrespondence.Address2 : ''} ${v.GeneralCorrespondence && v.GeneralCorrespondence.Address3 ? v.GeneralCorrespondence.Address3 : ''}`,
            address2: `${v.GeneralCorrespondence && v.GeneralCorrespondence.PostalBoundary ? v.GeneralCorrespondence.PostalBoundary.PostalCode : ''} ${v.GeneralCorrespondence && v.GeneralCorrespondence.PostalBoundary ? v.GeneralCorrespondence.PostalBoundary.Locality : ''}`,
            address3: `${v.GeneralCorrespondence && v.GeneralCorrespondence.PostalBoundary.Country ? v.GeneralCorrespondence.PostalBoundary.Country.Name : ''}`
          };
        });
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
