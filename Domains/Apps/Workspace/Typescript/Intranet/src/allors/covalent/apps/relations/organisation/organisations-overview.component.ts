import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { TdDialogService, TdMediaService } from "@covalent/core";

import { ErrorService, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../angular";
import { Brand, Catalogue, CatScope, ContactMechanism, Currency, Facility, Good, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, ProductCategory, ProductCharacteristic, ProductCharacteristicValue, ProductFeature, ProductType, SalesInvoice, SalesInvoiceItem, SalesOrder, Singleton, VarianceReason, VatRate, VatRegime, RequestForQuote, ProductQuote, QuoteItem, SalesOrderItem, ProcessFlow, CommunicationEvent, TelecommunicationsNumber, Country, CustomOrganisationClassification } from "../../../../domain";
import { And, ContainedIn, Contains, Fetch, Like, Page, Path, Predicate, PullRequest, Query, Sort, TreeNode, Equals } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

interface SearchData {
  name: string;
  country: string;
  role: string;
  classification: string;
  contactFirstName: string;
  contactLastName: string;
}

@Component({
  template: `
<mat-toolbar>
  <div layout="row" layout-align="start center" flex>
    <button mat-icon-button tdLayoutManageListOpen [hideWhenOpened]="true" style="display: none">
          <mat-icon>arrow_back</mat-icon>
        </button>
    <span>{{title}}</span>
    <span flex></span>
  </div>
</mat-toolbar>

<mat-card>
  <div class="pad-top-xs pad-left pad-right">
    <form novalidate [formGroup]="searchForm">
      <div class="grid-8_xs-1">
        <mat-input-container class="col">
          <input fxFlex matInput placeholder="Name" formControlName="name">
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="country" name="country" [(ngModel)]="selectedCountry" placeholder="Country">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let country of countries" [value]="country.Name">{{ country.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="role" name="role" [(ngModel)]="selectedRole" placeholder="Role">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let role of roles" [value]="role.Name">{{ role.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="classification" name="classification" [(ngModel)]="selectedClassification" placeholder="Classification">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let classification of classifications" [value]="classification.Name">{{ classification.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <input fxFlex matInput placeholder="First Name" formControlName="contactFirstName">
        </mat-input-container>
        <mat-input-container class="col">
          <input fxFlex matInput placeholder="Last Name" formControlName="contactLastName">
          <mat-icon matSuffix>search</mat-icon>
        </mat-input-container>
      </div>
    </form>
  </div>

  <mat-divider></mat-divider>

  <ng-template tdLoading="organisations">
    <mat-list class="will-load">
      <div class="mat-padding" *ngIf="data && data.length === 0" layout="row" layout-align="center center">
        <h3>No organisations to display.</h3>
      </div>
      <ng-template let-organisation let-last="last" ngFor [ngForOf]="data">
        <mat-divider *ngIf="!last" mat-inset></mat-divider>
        <mat-list-item>

          <mat-icon *ngIf="!organisation.LogoImage" mat-list-avatar>insert_photo</mat-icon>
          <img *ngIf="organisation.LogoImage" mat-list-avatar src="http://localhost:5000/Media/Download/{{organisation.LogoImage.UniqueId}}?revision={{organisation.LogoImage.Revision}}"
          />

          <h3 mat-line [routerLink]="['/relations/organisation/' + organisation.id]"> {{organisation.PartyName}} </h3>

          <p *ngIf="organisation.OrganisationClassifications.length > 0" mat-line> {{organisation.OrganisationClassifications[0].Name}}</p>
          <p *ngIf="organisation.GeneralCorrespondence" mat-line> {{organisation.GeneralCorrespondence?.Address1}} {{organisation.GeneralCorrespondence?.Address2}} {{organisation.GeneralCorrespondence?.Address3}}</p>
          <p *ngIf="organisation.GeneralCorrespondence" mat-line> {{organisation.GeneralCorrespondence?.PostalBoundary?.PostalCode}} {{organisation.GeneralCorrespondence?.PostalBoundary?.Locality}}
            {{organisation.GeneralCorrespondence?.PostalBoundary?.Country.Name}} </p>
          <p *ngIf="organisation.GeneralPhoneNumber" mat-line> {{organisation.GeneralPhoneNumber.CountryCode}} {{organisation.GeneralPhoneNumber.AreaCode}} {{organisation.GeneralPhoneNumber.ContactNumber}}
            <p mat-line hide-gt-md class="mat-caption"> last modified: {{ organisation.LastModifiedDate | timeAgo }} </p>

            <span flex></span>

            <span hide-xs hide-sm hide-md flex-gt-xs="60" flex-xs="40" layout-gt-xs="row">
                    <div class="mat-caption tc-grey-500" flex-gt-xs="50"> {{ organisation.CreationDate | date }} </div>
                    <div class="mat-caption tc-grey-500" flex-gt-xs="50"> {{ organisation.LastModifiedDate | timeAgo }} </div>
              </span>

            <span>
              <button mat-icon-button [mat-menu-trigger-for]="menu">
              <mat-icon>more_vert</mat-icon>
              </button>
              <mat-menu x-position="before" #menu="matMenu">
              <a [routerLink]="['/relations/organisation/' + organisation.id]" mat-menu-item>Details</a>
              <button mat-menu-item (click)="delete(organisation)" [disabled]="!organisation.CanExecuteDelete">Delete</button>
              </mat-menu>
            </span>

        </mat-list-item>
      </ng-template>
    </mat-list>
  </ng-template>
</mat-card>

<mat-card body tdMediaToggle="gt-xs" [mediaClasses]="['push']" *ngIf="this.data && this.data.length !== total">
  <mat-card-content>
    <button mat-button (click)="more()">More</button> {{this.data?.length}}/{{total}}
  </mat-card-content>
</mat-card>

<a mat-fab color="accent" class="mat-fab-bottom-right fixed" [routerLink]="['/organisation']">
  <mat-icon>add</mat-icon>
</a>
`,
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
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope()
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

    const combined$: Observable<any> = Observable
    .combineLatest(search$, this.page$, this.refresh$)
    .scan(([previousData, previousTake, previousDate]: [SearchData, number, Date], [data, take, date]: [SearchData, number, Date]): [SearchData, number, Date] => {
      return [
        data,
        data !== previousData ? 50 : take,
        date,
      ];
    }, [] as [SearchData, number, Date]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

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

  public more(): void {
    this.page$.next(this.data.length + 50);
  }

  public goBack(): void {
    window.history.back();
  }

  public ngAfterViewInit(): void {
    this.titleService.setTitle("Organisations");
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
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
      .openConfirm({ message: "Are you sure you want to delete this organisation?" })
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

  public onView(organisation: Organisation): void {
    this.router.navigate(["/relations/organisation/" + organisation.id]);
  }
}
