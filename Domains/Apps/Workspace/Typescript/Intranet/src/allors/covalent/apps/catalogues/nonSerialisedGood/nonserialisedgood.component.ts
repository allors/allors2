import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Filter, Loaded, MediaService, Saved, Scope, WorkspaceService } from "../../../../angular";
import { Brand, Facility, Good, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationRole, ProductCategory, ProductCharacteristic, ProductCharacteristicValue, ProductFeature, ProductType, Singleton, VarianceReason, VatRate } from "../../../../domain";
import { Contains, Fetch, Path, PullRequest, Query, Sort, TreeNode } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

@Component({
  templateUrl: "./nonserialisedgood.component.html",
})
export class NonSerialisedGoodComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public good: Good;

  public title: string;
  public subTitle: string;
  public singleton: Singleton;
  public facility: Facility;
  public locales: Locale[];
  public categories: ProductCategory[];
  public productTypes: ProductType[];
  public productCharacteristicValues: ProductCharacteristicValue[];
  public manufacturers: Organisation[];
  public suppliers: Organisation[];
  public brands: Brand[];
  public selectedBrand: Brand;
  public models: Model[];
  public selectedModel: Model;
  public varianceReasons: VarianceReason[];
  public inventoryItemKinds: InventoryItemKind[];
  public inventoryItems: NonSerialisedInventoryItem[];
  public inventoryItem: NonSerialisedInventoryItem;
  public inventoryItemObjectStates: NonSerialisedInventoryItemState[];
  public vatRates: VatRate[];
  public actualQuantityOnHand: number;

  public manufacturersFilter: Filter;
  public suppliersFilter: Filter;

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    public media: TdMediaService,
    public mediaService: MediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.manufacturersFilter = new Filter({scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name]});
    this.suppliersFilter = new Filter({scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name]});
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Good.PrimaryPhoto }),
              new TreeNode({ roleType: m.Good.Photos }),
              new TreeNode({ roleType: m.Good.LocalisedNames }),
              new TreeNode({ roleType: m.Good.LocalisedDescriptions }),
              new TreeNode({ roleType: m.Good.LocalisedComments }),
              new TreeNode({ roleType: m.Good.ProductCategories }),
              new TreeNode({ roleType: m.Good.InventoryItemKind }),
              new TreeNode({ roleType: m.Good.SuppliedBy }),
              new TreeNode({ roleType: m.Good.ManufacturedBy }),
              new TreeNode({ roleType: m.Good.StandardFeatures }),
            ],
            name: "good",
          }),
          new Fetch({
            id,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [new TreeNode({ roleType: m.ProductCharacteristic.LocalisedNames })],
                    roleType: m.ProductType.ProductCharacteristics,
                  }),
                ],
                roleType: m.InventoryItem.ProductType,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.ProductCharacteristicValue.ProductCharacteristic }),
                ],
                roleType: m.InventoryItem.ProductCharacteristicValues,
              }),
              new TreeNode({
                roleType: m.InventoryItem.InventoryItemVariances,
              }),
            ],
            name: "inventoryItems",
            path: new Path({ step: this.m.Good.InventoryItemsWhereGood }),
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              include: [
                new TreeNode({
                  nodes: [
                    new TreeNode({ roleType: m.Locale.Language }),
                    new TreeNode({ roleType: m.Locale.Country }),
                  ],
                  roleType: m.Singleton.Locales,
                }),
                new TreeNode({
                  nodes: [new TreeNode({ roleType: m.InternalOrganisation.DefaultFacility })],
                  roleType: m.Singleton.InternalOrganisation,
                }),
              ],
              name: "singletons",
              objectType: this.m.Singleton,
            }),
          new Query(
            {
              name: "organisationRoles",
              objectType: this.m.OrganisationRole,
            }),
          new Query(
            {
              name: "categories",
              objectType: this.m.ProductCategory,
            }),
          new Query(
            {
              name: "productTypes",
              objectType: this.m.ProductType,
            }),
          new Query(
            {
              name: "varianceReasons",
              objectType: this.m.VarianceReason,
            }),
          new Query(
            {
              name: "vatRates",
              objectType: this.m.VatRate,
            }),
          new Query(
            {
              name: "brands",
              objectType: this.m.Brand,
              sort: [new Sort({ roleType: m.Brand.Name, direction: "Asc" })],
            }),
          new Query(
            {
              name: "inventoryItemKinds",
              objectType: this.m.InventoryItemKind,
            }),
          new Query(
            {
              name: "nonSerialisedInventoryItemStates",
              objectType: this.m.NonSerialisedInventoryItemState,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }))
          .switchMap((loaded: Loaded) => {

            this.good = loaded.objects.good as Good;
            this.categories = loaded.collections.categories as ProductCategory[];
            this.productTypes = loaded.collections.productTypes as ProductType[];
            this.varianceReasons = loaded.collections.varianceReasons as VarianceReason[];
            this.vatRates = loaded.collections.vatRates as VatRate[];
            this.brands = loaded.collections.brands as Brand[];
            this.inventoryItemKinds = loaded.collections.inventoryItemKinds as InventoryItemKind[];
            this.inventoryItemObjectStates = loaded.collections.nonSerialisedInventoryItemStates as NonSerialisedInventoryItemState[];
            this.singleton = loaded.collections.singletons[0] as Singleton;
            this.facility = this.singleton.InternalOrganisation.DefaultFacility;
            this.locales = this.singleton.Locales;

            const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);
            const inventoryItemKindNonSerialised = this.inventoryItemKinds.find((v: InventoryItemKind) => v.Name === "Non serialised");

            if (this.good === undefined) {
              this.good = this.scope.session.create("Good") as Good;
              this.good.VatRate = vatRateZero;
              this.good.Sku = "";

              this.inventoryItem = this.scope.session.create("NonSerialisedInventoryItem") as NonSerialisedInventoryItem;
              this.good.InventoryItemKind = inventoryItemKindNonSerialised;
              this.inventoryItem.Good = this.good;
              this.inventoryItem.Facility = this.facility;
            } else {
              this.inventoryItems = loaded.collections.inventoryItems as NonSerialisedInventoryItem[];
              this.inventoryItem = this.inventoryItems[0];
              this.productCharacteristicValues = this.inventoryItem.ProductCharacteristicValues;
              this.good.StandardFeatures.forEach((feature: ProductFeature) => {
                if (feature instanceof (Brand)) {
                  this.selectedBrand = feature;
                  this.brandSelected(this.selectedBrand);
                }
                if (feature instanceof (Model)) {
                 this.selectedModel = feature;
               }
            });
           }

            this.title = this.good.Name;
            this.subTitle = "Non Serialised";
            this.actualQuantityOnHand = this.good.QuantityOnHand;

            const organisationRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const manufacturerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === "Manufacturer");
            const supplierRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === "Supplier");

            const Query2: Query[] = [
              new Query(
                {
                  name: "manufacturers",
                  objectType: m.Organisation,
                  predicate: new Contains({ roleType: m.Organisation.OrganisationRoles, object: manufacturerRole }),
                }),
              new Query(
                {
                  name: "suppliers",
                  objectType: m.Organisation,
                  predicate: new Contains({ roleType: m.Organisation.OrganisationRoles, object: supplierRole }),
                }),
            ];

            return this.scope.load("Pull", new PullRequest({ query: Query2 }));
          });
      })
      .subscribe((loaded: Loaded) => {
        this.manufacturers = loaded.collections.manufacturers as Organisation[];
        this.suppliers = loaded.collections.suppliers as Organisation[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public update(): void {

      this.scope
        .save()
        .subscribe((saved: Saved) => {
          this.refresh();
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    }

    public refresh(): void {
      this.refresh$.next(new Date());
  }

  public save(): void {
    this.good.StandardFeatures.forEach((feature: ProductFeature) => {
      this.good.RemoveStandardFeature(feature);
    });

    if (this.selectedBrand != null) {
      this.good.AddStandardFeature(this.selectedBrand);
    }

    if (this.selectedModel != null) {
      this.good.AddStandardFeature(this.selectedModel);
    }

    this.inventoryItem.ProductCharacteristicValues = this.productCharacteristicValues;

    if (this.actualQuantityOnHand !== this.good.QuantityOnHand) {
      const reason = this.varianceReasons.find((v: VarianceReason) => v.Name === "Unknown");

      const inventoryItemVariance = this.scope.session.create("InventoryItemVariance") as InventoryItemVariance;
      inventoryItemVariance.Quantity = this.actualQuantityOnHand - this.good.QuantityOnHand;
      inventoryItemVariance.Reason = reason;

      this.inventoryItem.AddInventoryItemVariance(inventoryItemVariance);
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

  public localisedName(productCharacteristic: ProductCharacteristic, locale: Locale): string {
    const localisedText: LocalisedText = productCharacteristic.LocalisedNames.find((v: LocalisedText) => v.Locale === locale);
    if (localisedText) {
      return localisedText.Text;
    }

    return productCharacteristic.Name;
  }

  public localisedProductCharacteristicValues(locale: Locale): ProductCharacteristicValue[] {
    return  this.inventoryItem.ProductCharacteristicValues.filter((v: ProductCharacteristicValue) => v.Locale === locale);
  }

  public brandSelected(brand: Brand): void {

    const fetch: Fetch[] = [
      new Fetch(
        {
          id: brand.id,
          include: [new TreeNode({ roleType: this.m.Brand.Models })],
          name: "selectedbrand",
        }),
    ];

    this.scope
      .load("Pull", new PullRequest({ fetch }))
      .subscribe((loaded: Loaded) => {

        const selectedbrand = loaded.objects.selectedbrand as Brand;
        this.models = selectedbrand.Models;
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }
}
