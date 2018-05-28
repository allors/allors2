import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar, MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Invoked, Loaded, MediaService, Scope, WorkspaceService } from '../../../../../angular';
import { Country, CustomOrganisationClassification, InternalOrganisation, Organisation, OrganisationRole, Media } from '../../../../../domain';
import { And, ContainedIn, Contains, Equals, Like, Page, Path, Predicate, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';


interface Row {
  organisation: Organisation;
  logo: Media;
  name: string;
  classification: string;
  phone: string;
  address: string;
  country: string;
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
  public searchForm: FormGroup;

  public displayedColumns = ['logo', 'name', 'classification', 'phone', 'address', 'country', 'actions'];
  public dataSource = new MatTableDataSource();

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

  private page$: BehaviorSubject<number>;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    public mediaService: MediaService,
    private workspaceService: WorkspaceService,
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

    this.page$ = new BehaviorSubject<number>(50);

    const search$ = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$ = Observable.combineLatest(search$, this.page$, this.refresh$, this.stateService.internalOrganisationId$)
      .scan(([previousData, previousTake, previousDate, previousInternalOrganisationId], [data, take, date, internalOrganisationId]) => {
        return [
          data,
          data !== previousData ? 50 : take,
          date,
          internalOrganisationId,
        ];
      }, [] as [SearchData, number, Date, InternalOrganisation]);

    this.subscription = combined$
      .switchMap(([data, take, , internalOrganisationId]) => {
        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const organisationRolesQuery: Query[] = [
          new Query(
            {
              name: 'organisationRoles',
              objectType: m.OrganisationRole,
            }),
          new Query(
            {
              name: 'classifications',
              objectType: m.CustomOrganisationClassification,
            }),
          new Query(
            {
              name: 'countries',
              objectType: m.Country,
              sort: [new Sort({ roleType: m.Country.Name, direction: 'Asc' })],
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ queries: organisationRolesQuery }))
          .switchMap((loaded) => {
            this.roles = loaded.collections.organisationRoles as OrganisationRole[];
            this.role = this.roles.find((v: OrganisationRole) => v.Name === data.role);

            this.countries = loaded.collections.countries as Country[];
            this.country = this.countries.find((v: Country) => v.Name === data.country);

            this.classifications = loaded.collections.classifications as CustomOrganisationClassification[];
            this.classification = this.classifications.find((v: CustomOrganisationClassification) => v.Name === data.classification);

            const contactPredicate: And = new And();
            const contactPredicates: Predicate[] = contactPredicate.predicates;

            if (data.contactFirstName) {
              const like: string = '%' + data.contactFirstName + '%';
              contactPredicates.push(new Like({ roleType: m.Person.FirstName, value: like }));
            }

            if (data.contactLastName) {
              const like: string = '%' + data.contactLastName + '%';
              contactPredicates.push(new Like({ roleType: m.Person.LastName, value: like }));
            }

            const contactQuery: Query = new Query(
              {
                name: 'contacts',
                objectType: m.Person,
                predicate: contactPredicate,
              });

            const organisationContactRelationshipPredicate: And = new And();
            const organisationContactRelationshipPredicates: Predicate[] = organisationContactRelationshipPredicate.predicates;
            organisationContactRelationshipPredicates.push(new ContainedIn({ roleType: m.OrganisationContactRelationship.Contact, query: contactQuery }));

            const organisationContactRelationshipQuery: Query = new Query(
              {
                name: 'organisationContactRelationships',
                objectType: m.OrganisationContactRelationship,
                predicate: organisationContactRelationshipPredicate,
              });

            const postalBoundaryPredicate: And = new And();
            const postalBoundaryPredicates: Predicate[] = postalBoundaryPredicate.predicates;

            if (data.country) {
              postalBoundaryPredicates.push(new Equals({ roleType: m.PostalBoundary.Country, value: this.country }));
            }

            const postalBoundaryQuery: Query = new Query(
              {
                name: 'postalBoundaries',
                objectType: m.PostalBoundary,
                predicate: postalBoundaryPredicate,
              });

            const postalAddressPredicate: And = new And();
            const postalAddressPredicates: Predicate[] = postalAddressPredicate.predicates;
            postalAddressPredicates.push(new ContainedIn({ roleType: m.PostalAddress.PostalBoundary, query: postalBoundaryQuery }));

            const postalAddressQuery: Query = new Query(
              {
                name: 'postalAddresses',
                objectType: m.PostalAddress,
                predicate: postalAddressPredicate,
              });

            const predicate: And = new And();
            const predicates: Predicate[] = predicate.predicates;

            if (data.role) {
              if (this.role.UniqueId.toUpperCase() === '32E74BEF-2D79-4427-8902-B093AFA81661') {
                predicates.push(new Equals({ roleType: m.Organisation.IsManufacturer, value: true }));
              }
            }

            if (data.classification) {
              predicates.push(new Contains({ roleType: m.Organisation.OrganisationClassifications, object: this.classification }));
            }

            if (data.country) {
              predicates.push(new ContainedIn({ roleType: m.Organisation.GeneralCorrespondence, query: postalAddressQuery }));
            }

            if (data.contactFirstName || data.contactLastName) {
              predicates.push(new ContainedIn({ roleType: m.Organisation.CurrentOrganisationContactRelationships, query: organisationContactRelationshipQuery }));
            }

            if (data.name) {
              const like: string = data.name.replace('*', '%') + '%';
              predicates.push(new Like({ roleType: m.Organisation.Name, value: like }));
            }

            const queries: Query[] = [new Query(
              {
                name: 'organisations',
                objectType: m.Organisation,
                predicate,
                page: new Page({ skip: 0, take }),
                include: [
                  new TreeNode({ roleType: m.Organisation.LogoImage }),
                  new TreeNode({ roleType: m.Organisation.OrganisationClassifications }),
                  new TreeNode({ roleType: m.Organisation.GeneralPhoneNumber }),
                  new TreeNode({
                    roleType: m.Organisation.GeneralCorrespondence,
                    nodes: [
                      new TreeNode({
                        roleType: m.PostalAddress.PostalBoundary,
                        nodes: [
                          new TreeNode({ roleType: m.PostalBoundary.Country }),
                        ],
                      }),
                    ],
                  }),
                ],
                sort: [new Sort({ roleType: m.Organisation.Name })],
              })];

            return this.scope
              .load('Pull', new PullRequest({ queries }));
          });
      })
      .subscribe((loaded) => {
        this.scope.session.reset();

        this.dataSource.data = loaded.collections.organisations.map<Row>((v: Organisation) => {
          return {
            organisation: v,
            logo: v.LogoImage,
            name: v.PartyName,
            classification: v.OrganisationClassifications.map(v => v.Name).join(", ") || "none",
            phone: `${v.GeneralPhoneNumber.CountryCode || ''} ${v.GeneralPhoneNumber.AreaCode || ''} ${v.GeneralPhoneNumber.ContactNumber || ''}`,
            address: `${v.GeneralCorrespondence.Address1 || ''} ${v.GeneralCorrespondence.Address2 || ''} ${v.GeneralCorrespondence.Address3 || ''}`,
            country: `${v.GeneralCorrespondence.Country.Name || 'none'}`
          };
        });
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        });
  }

  public ngOnInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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
