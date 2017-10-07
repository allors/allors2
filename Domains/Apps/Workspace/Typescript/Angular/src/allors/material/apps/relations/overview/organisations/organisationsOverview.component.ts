import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Subscription } from "rxjs/Rx";

import { TdDialogService, TdLoadingService, TdMediaService } from "@covalent/core";

import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "../../../../../angular";
import { And, ContainedIn, Contains, Equals, Like, Not, Or, Page, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../../domain";
import { Country, CustomOrganisationClassification, Organisation, OrganisationRole, PostalAddress, PostalBoundary } from "../../../../../domain";
import { MetaDomain } from "../../../../../meta/index";

interface SearchData {
  name: string;
  country: string;
  role: string;
  classification: string;
  contactFirstName: string;
  contactLastName: string;
}

@Component({
  templateUrl: "./organisationsOverview.component.html",
})
export class OrganisationsOverviewComponent implements AfterViewInit, OnDestroy {

  public title: string = "Organisations";
  public total: number;
  public searchForm: FormGroup;
  public data: Organisation[];

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

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [""],
      country: [""],
      role: [""],
      classification: [""],
      contactFirstName: [""],
      contactLastName: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable.combineLatest(search$, this.page$)
      .scan(([previousData, previousTake]: [SearchData, number], [data, take]: [SearchData, number]): [SearchData, number] => {
        return [
          data,
          data !== previousData ? 50 : take,
        ];
      }, [] as [SearchData, number]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.allors.meta;

        const organisationRolesQuery: Query[] = [
          new Query(
            {
              name: "organisationRoles",
              objectType: m.OrganisationRole,
            }),
          new Query(
            {
              name: "classifications",
              objectType: m.CustomOrganisationClassification,
            }),
          new Query(
            {
              name: "countries",
              objectType: m.Country,
              sort: [new Sort({ roleType: m.Country.Name })],
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: organisationRolesQuery }))
          .switchMap((loaded: Loaded) => {
            this.roles = loaded.collections.organisationRoles as OrganisationRole[];
            this.role = this.roles.find((v: OrganisationRole) => v.Name === data.role);

            this.countries = loaded.collections.countries as Country[];
            this.country = this.countries.find((v: Country) => v.Name === data.country);

            this.classifications = loaded.collections.classifications as CustomOrganisationClassification[];
            this.classification = this.classifications.find((v: CustomOrganisationClassification) => v.Name === data.classification);

            const contactPredicate: And = new And();
            const contactPredicates: Predicate[] = contactPredicate.predicates;

            if (data.contactFirstName) {
              const like: string = "%" + data.contactFirstName + "%";
              contactPredicates.push(new Like({ roleType: m.Person.FirstName, value: like }));
            }

            if (data.contactLastName) {
              const like: string = "%" + data.contactLastName + "%";
              contactPredicates.push(new Like({ roleType: m.Person.LastName, value: like }));
            }

            const contactQuery: Query = new Query(
              {
                name: "contacts",
                objectType: m.Person,
                predicate: contactPredicate,
              });

            const organisationContactRelationshipPredicate: And = new And();
            const organisationContactRelationshipPredicates: Predicate[] = organisationContactRelationshipPredicate.predicates;
            organisationContactRelationshipPredicates.push(new ContainedIn({ roleType: m.OrganisationContactRelationship.Contact, query: contactQuery }));

            const organisationContactRelationshipQuery: Query = new Query(
              {
                name: "organisationContactRelationships",
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
                name: "postalBoundaries",
                objectType: m.PostalBoundary,
                predicate: postalBoundaryPredicate,
              });

            const postalAddressPredicate: And = new And();
            const postalAddressPredicates: Predicate[] = postalAddressPredicate.predicates;
            postalAddressPredicates.push(new ContainedIn({ roleType: m.PostalAddress.PostalBoundary, query: postalBoundaryQuery }));

            const postalAddressQuery: Query = new Query(
              {
                name: "postalAddresses",
                objectType: m.PostalAddress,
                predicate: postalAddressPredicate,
              });

            const predicate: And = new And();
            const predicates: Predicate[] = predicate.predicates;

            if (data.role) {
              predicates.push(new Contains({ roleType: m.Organisation.OrganisationRoles, object: this.role }));
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
              const like: string = data.name.replace("*", "%") + "%";
              predicates.push(new Like({ roleType: m.Organisation.Name, value: like }));
            }

            const query: Query[] = [new Query(
              {
                name: "organisations",
                objectType: m.Organisation,
                predicate,
                page: new Page({ skip: 0, take: take }),
                include: [
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
              .load("Pull", new PullRequest({ query }));
          });
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.data = loaded.collections.organisations as Organisation[];
        this.total = loaded.values.organisations_total;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  more(): void {
    this.page$.next(this.data.length + 50);
  }

  goBack(): void {
    window.history.back();
  }

  ngAfterViewInit(): void {
    this.titleService.setTitle("Organisations");
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  refresh(): void {
    this.refresh$.next(new Date());
  }

  delete(organisation: Organisation): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this person?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(organisation.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  onView(organisation: Organisation): void {
    this.router.navigate(["/relations/organisation/" + organisation.id]);
  }
}
