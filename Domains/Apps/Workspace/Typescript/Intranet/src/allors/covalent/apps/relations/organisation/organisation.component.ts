import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../angular";
import { Brand, Catalogue, CatScope, ContactMechanism, Currency, Facility, Good, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, ProductCategory, ProductCharacteristic, ProductCharacteristicValue, ProductFeature, ProductType, SalesInvoice, SalesInvoiceItem, SalesOrder, Singleton, VarianceReason, VatRate, VatRegime, RequestForQuote, ProductQuote, QuoteItem, SalesOrderItem, ProcessFlow, CommunicationEvent, TelecommunicationsNumber, CustomOrganisationClassification, IndustryClassification, CustomerRelationship } from "../../../../domain";
import { And, ContainedIn, Contains, Fetch, Like, Page, Path, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

@Component({
  template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="organisation" (submit)="save()">

    <div class="pad">
      <a-mat-media-upload [object]="organisation" [roleType]="m.Organisation.LogoImage" accept="image/*"></a-mat-media-upload>
      <a-mat-select [object]="organisation" [roleType]="m.Organisation.OrganisationRoles" [options]="roles" display="Name"></a-mat-select>
      <a-mat-select [object]="organisation" [roleType]="m.Organisation.OrganisationClassifications" [options]="classifications" display="Name" label="Classification"></a-mat-select>
      <a-mat-select [object]="organisation" [roleType]="m.Organisation.IndustryClassifications" [options]="industries" display="Name" label="Industry"></a-mat-select>
      <a-mat-input [object]="organisation" [roleType]="m.Organisation.Name"></a-mat-input>
      <a-mat-input [object]="organisation" [roleType]="m.Organisation.Comment"></a-mat-input>
      <a-mat-select [object]="organisation" [roleType]="m.Organisation.Locale" [options]="locales" display="Name"></a-mat-select>
    </div>

    <mat-divider></mat-divider>

    <mat-card-actions>
      <button mat-button color="primary" type="submit" [disabled]="!form.form.valid">SAVE</button>
      <button mat-button (click)="goBack()" type="button">CANCEL</button>
    </mat-card-actions>
  </form>

</td-layout-card-over>
`,
})
export class OrganisationComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string = "Organisation";
  public subTitle: string;

  public m: MetaDomain;

  public organisation: Organisation;

  public locales: Locale[];
  public roles: OrganisationRole[];
  public classifications: CustomOrganisationClassification[];
  public industries: IndustryClassification[];
  public customerRelationships: CustomerRelationship[];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;
  private customerRole: OrganisationRole;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope()
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");

        const fetch: Fetch[] = [
          new Fetch({
            name: "organisation",
            id,
          }),
          new Fetch({
            name: "customerRelationships",
            id,
            path: new Path({ step: this.m.Organisation.CustomerRelationshipsWhereCustomer }),
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "locales",
              objectType: this.m.Locale,
            }),
          new Query(
            {
              name: "roles",
              objectType: this.m.OrganisationRole,
            }),
          new Query(
            {
              name: "classifications",
              objectType: this.m.CustomOrganisationClassification,
            }),
          new Query(
            {
              name: "industries",
              objectType: this.m.IndustryClassification,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.subTitle = "edit organisation";
        this.organisation = loaded.objects.organisation as Organisation;
        this.customerRelationships = loaded.collections.customerRelationships as CustomerRelationship[];

        if (!this.organisation) {
          this.subTitle = "add a new organisation";
          this.organisation = this.scope.session.create("Organisation") as Organisation;
        }

        this.locales = loaded.collections.locales as Locale[];
        this.classifications = loaded.collections.classifications as CustomOrganisationClassification[];
        this.industries = loaded.collections.industries as IndustryClassification[];
        this.roles = loaded.collections.roles as OrganisationRole[];
        this.customerRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === "8B5E0CEE-4C98-42F1-8F18-3638FBA943A0");
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    if (this.organisation.OrganisationRoles.indexOf(this.customerRole) > -1 && this.customerRelationships === undefined) {
      const customerRelationship = this.scope.session.create("CustomerRelationship") as CustomerRelationship;
      customerRelationship.Customer = this.organisation;
    }

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public goBack(): void {
    window.history.back();
  }
}
