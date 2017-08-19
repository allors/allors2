import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { MdNativeDateModule, MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Filter, Loaded, Saved, Scope } from "../../../../angular";
import { Contains, Equals, Fetch, Like, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "../../../../domain";
import {
  Facility, Good, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, NonSerializedInventoryItem, Organisation, OrganisationRole,
  ProductCategory, ProductCharacteristic, ProductCharacteristicValue, ProductType, Singleton, VarianceReason, VatRate
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
  public varianceReasons: VarianceReason[];
  public inventoryItemKinds: InventoryItemKind[];
  public inventoryItem: NonSerializedInventoryItem;
  public vatRates: VatRate[];
  public actualQuantityOnHand: number;

  public manufacturersFilter: Filter;

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
  }

  get locale(): Locale {
    return this.locales.find((v: Locale) => v.Name === this.selectedLocaleName);
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
              new TreeNode({ roleType: m.Product.LocalisedNames }),
              new TreeNode({ roleType: m.Product.LocalisedDescriptions }),
              new TreeNode({ roleType: m.Product.LocalisedComments }),
              new TreeNode({ roleType: m.Product.ProductCategories }),
              new TreeNode({ roleType: m.Good.ManufacturedBy }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [new TreeNode({ roleType: m.ProductCharacteristic.LocalisedNames })],
                    roleType: m.ProductType.ProductCharacteristics,
                  }),
                ],
                roleType: m.Good.ProductType,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.ProductCharacteristicValue.ProductCharacteristic }),
                ],
                roleType: m.Product.ProductCharacteristicValues,
              }),
            ],
            name: "good",
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
                  roleType: m.Singleton.DefaultInternalOrganisation,
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
            this.singleton = loaded.collections.singletons[0] as Singleton;
            this.facility = this.singleton.DefaultInternalOrganisation.DefaultFacility;
            this.locales = this.singleton.Locales;
            this.selectedLocaleName = this.singleton.DefaultLocale.Name;

            this.good = loaded.objects.good as Good;

            if (!this.good) {
              const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);
              const inventoryItemKindNonSerialized = this.inventoryItemKinds.find((v: InventoryItemKind) => v.Name === "Non serialized");

              this.good = this.scope.session.create("Good") as Good;
              this.good.InventoryItemKind = inventoryItemKindNonSerialized;
              this.good.VatRate = vatRateZero;
              this.good.Sku = "";

              this.inventoryItem = this.scope.session.create("NonSerializedInventoryItem") as NonSerializedInventoryItem;
              this.inventoryItem.Good = this.good;
              this.inventoryItem.Facility = this.facility;
            }

            this.title = this.good.Name;
            this.actualQuantityOnHand = this.good.QuantityOnHand;

            this.setProductCharacteristicValues();

            const organisationRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const manufacturerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === "Manufacturer");

            const manufacturersQuery: Query[] = [
              new Query(
                {
                  name: "manufacturers",
                  objectType: m.Organisation,
                  predicate: new Contains({ roleType: m.Organisation.OrganisationRoles, object: manufacturerRole }),
                }),
            ];

            if (!this.good) {
              const inventoryQuery: Query = new Query(
                {
                  include: [new TreeNode({ roleType: m.NonSerializedInventoryItem.InventoryItemVariances })],
                  name: "inventoryItems",
                  objectType: m.NonSerializedInventoryItem,
                  predicate: new Equals({ roleType: m.NonSerializedInventoryItem.Good, value: this.good }),
                });

              manufacturersQuery.push(inventoryQuery);
            }

            return this.scope.load("Pull", new PullRequest({ query: manufacturersQuery }));
          });
      })
      .subscribe((loaded: Loaded) => {
        this.manufacturers = loaded.collections.manufacturers as Organisation[];
        this.inventoryItem = loaded.collections.inventoryItems[0] as NonSerializedInventoryItem;
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

  public localisedName(productCharacteristic: ProductCharacteristic): string {
    const localisedText: LocalisedText = productCharacteristic.LocalisedNames.find((v: LocalisedText) => v.Locale === this.locale);
    if (localisedText) {
      return localisedText.Text;
    }

    return productCharacteristic.Name;
  }

  public setProductCharacteristicValues(): void {
    this.productCharacteristicValues = this.good.ProductCharacteristicValues.filter((v: ProductCharacteristicValue) => v.Locale === this.locale);
  }
}
