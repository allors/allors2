import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { MdNativeDateModule, MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Filter, Loaded, Saved, Scope } from "../../../../angular";
import { Contains, Equals, Fetch, Like, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "../../../../domain";
import {
  Brand, Facility, Good, InventoryItem, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, Model, NonSerialisedInventoryItem,
  NonSerialisedInventoryItemState, Organisation, OrganisationRole, Ownership,
  ProductCategory, ProductCharacteristic, ProductCharacteristicValue, ProductFeature, ProductType,
  SerialisedInventoryItem, SerialisedInventoryItemState, Singleton, VarianceReason, VatRate,
} from "../../../../domain";
import { MetaDomain } from "../../../../meta/index";

@Component({
  templateUrl: "./edit.component.html",
})
export class GoodEditComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public good: Good;

  public title: string;
  public subTitle: string;
  public singleton: Singleton;
  public facility: Facility;
  public locales: Locale[];
  public selectedLocaleName: string;
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
  public inventoryItems: InventoryItem[];
  public inventoryItem: InventoryItem;
  public nonSerialisedInventoryItems: NonSerialisedInventoryItem[];
  public nonSerialisedinventoryItem: NonSerialisedInventoryItem;
  public serialisedInventoryItems: SerialisedInventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public serialisedInventoryItemStates: SerialisedInventoryItemState[];
  public nonSerialisedInventoryItemStates: NonSerialisedInventoryItemState[];
  public vatRates: VatRate[];
  public ownerships: Ownership[];
  public actualQuantityOnHand: number;

  public manufacturersFilter: Filter;
  public suppliersFilter: Filter;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.manufacturersFilter = new Filter(this.scope, this.m.Organisation, [this.m.Organisation.Name]);
    this.suppliersFilter = new Filter(this.scope, this.m.Organisation, [this.m.Organisation.Name]);
  }

  public ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Good.PrimaryPhoto }),
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
        ];

        const inventoryItemFetch: Fetch = new Fetch({
          id,
          include: [
            new TreeNode({ roleType: m.SerialisedInventoryItem.Ownership }),
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
          ],
          name: "inventoryItems",
          path: new Path({ step: this.m.Good.InventoryItemsWhereGood }),
        });

        if (id != null) {
          fetch.push(inventoryItemFetch);
        }

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
              name: "inventoryItemKinds",
              objectType: this.m.InventoryItemKind,
            }),
          new Query(
            {
              name: "brands",
              objectType: this.m.Brand,
              sort: [new Sort({ roleType: m.Brand.Name, direction: "Asc" })],
            }),
          new Query(
            {
              name: "serialisedInventoryItemStates",
              objectType: this.m.SerialisedInventoryItemState,
            }),
            new Query(
              {
                name: "nonSerialisedInventoryItemStates",
                objectType: this.m.NonSerialisedInventoryItemState,
              }),
            new Query(
            {
              name: "ownerships",
              objectType: this.m.Ownership,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }))
          .switchMap((loaded: Loaded) => {

            this.categories = loaded.collections.categories as ProductCategory[];
            this.productTypes = loaded.collections.productTypes as ProductType[];
            this.varianceReasons = loaded.collections.varianceReasons as VarianceReason[];
            this.vatRates = loaded.collections.vatRates as VatRate[];
            this.inventoryItemKinds = loaded.collections.inventoryItemKinds as InventoryItemKind[];
            this.brands = loaded.collections.brands as Brand[];
            this.serialisedInventoryItemStates = loaded.collections.serialisedInventoryItemStates as SerialisedInventoryItemState[];
            this.nonSerialisedInventoryItemStates = loaded.collections.nonSerialisedInventoryItemStates as NonSerialisedInventoryItemState[];
            this.ownerships = loaded.collections.ownerships as Ownership[];
            this.singleton = loaded.collections.singletons[0] as Singleton;
            this.facility = this.singleton.InternalOrganisation.DefaultFacility;
            this.locales = this.singleton.Locales;
            this.selectedLocaleName = this.singleton.DefaultLocale.Name;
            this.good = loaded.objects.good as Good;
            this.inventoryItems = loaded.collections.inventoryItems as InventoryItem[];
            this.inventoryItem = this.inventoryItems[0];

            this.good.StandardFeatures.forEach((feature: ProductFeature) => {
              if (feature instanceof (Brand)) {
                this.selectedBrand = feature;
                this.brandSelected(this.selectedBrand);
              }
              if (feature instanceof (Model)) {
                this.selectedModel = feature;
              }
            });

            this.setProductCharacteristicValues();

            if (this.SerialisedGood) {
              this.serialisedInventoryItems = loaded.collections.inventoryItems as SerialisedInventoryItem[];
              this.serialisedInventoryItem = this.serialisedInventoryItems[0];
            } else {
              this.nonSerialisedInventoryItems = loaded.collections.inventoryItems as NonSerialisedInventoryItem[];
              this.nonSerialisedinventoryItem = this.nonSerialisedInventoryItems[0];
            }

            this.title = this.good.Name;
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

    this.good.StandardFeatures.forEach((feature: ProductFeature) => {
      this.good.RemoveStandardFeature(feature);
    });

    if (this.selectedBrand != null) {
      this.good.AddStandardFeature(this.selectedBrand);
    }

    if (this.selectedModel != null) {
      this.good.AddStandardFeature(this.selectedModel);
    }

    if (!this.SerialisedGood && this.actualQuantityOnHand !== this.good.QuantityOnHand) {
      const reason = this.varianceReasons.find((v: VarianceReason) => v.Name === "Unknown");

      const inventoryItemVariance = this.scope.session.create("InventoryItemVariance") as InventoryItemVariance;
      inventoryItemVariance.Quantity = this.actualQuantityOnHand - this.good.QuantityOnHand;
      inventoryItemVariance.Reason = reason;

      this.nonSerialisedinventoryItem.AddInventoryItemVariance(inventoryItemVariance);
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

  public localisedName(productCharacteristic: ProductCharacteristic): string {
    const localisedText: LocalisedText = productCharacteristic.LocalisedNames.find((v: LocalisedText) => v.Locale === this.locale);
    if (localisedText) {
      return localisedText.Text;
    }

    return productCharacteristic.Name;
  }

  public setProductCharacteristicValues(): void {
    this.productCharacteristicValues = this.inventoryItem.ProductCharacteristicValues.filter((v: ProductCharacteristicValue) => v.Locale === this.locale);
  }

  get locale(): Locale {
    return this.locales.find((v: Locale) => v.Name === this.selectedLocaleName);
  }

  get SerialisedGood(): boolean {
    return this.good.InventoryItemKind === this.inventoryItemKinds.find((v: InventoryItemKind) => v.UniqueId.toUpperCase() === "2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE");
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
