import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar, MatTabChangeEvent } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { isType } from "@angular/core/src/type";
import { forEach } from "@angular/router/src/utils/collection";
import { ErrorService, Filter, Loaded, MediaService, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { Brand, Facility, Good, InternalOrganisation, InventoryItemKind, Locale, LocalisedText, Model, Organisation, OrganisationRole, Ownership, ProductCategory, ProductFeature, ProductType, SerialisedInventoryItem, SerialisedInventoryItemCharacteristic, SerialisedInventoryItemCharacteristicType, SerialisedInventoryItemState, Singleton, VatRate } from "../../../../../domain";
import { Contains, Fetch, Path, PullRequest, Query, Sort, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";

@Component({
  templateUrl: "./serialisedgood.component.html",
})
export class SerialisedGoodComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public good: Good;

  public title: string;
  public subTitle: string;
  public singleton: Singleton;
  public facility: Facility;
  public locales: Locale[];
  public categories: ProductCategory[];
  public productTypes: ProductType[];
  public productCharacteristicValues: SerialisedInventoryItemCharacteristic[];
  public manufacturers: Organisation[];
  public suppliers: Organisation[];
  public brands: Brand[];
  public selectedBrand: Brand;
  public models: Model[];
  public selectedModel: Model;
  public inventoryItemKinds: InventoryItemKind[];
  public inventoryItems: SerialisedInventoryItem[];
  public inventoryItem: SerialisedInventoryItem;
  public serialisedInventoryItemStates: SerialisedInventoryItemState[];
  public vatRates: VatRate[];
  public ownerships: Ownership[];

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
    public mediaService: MediaService,
    private stateService: StateService,
  ) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.manufacturersFilter = new Filter({scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name]});
    this.suppliersFilter = new Filter({scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name]});
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisation$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Good.PrimaryPhoto }),
              new TreeNode({ roleType: m.Good.Photos }),
              new TreeNode({ roleType: m.Good.LocalisedNames, nodes: [new TreeNode({ roleType: m.LocalisedText.Locale })] }),
              new TreeNode({ roleType: m.Good.LocalisedDescriptions, nodes: [new TreeNode({ roleType: m.LocalisedText.Locale })] }),
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
              new TreeNode({  roleType: m.SerialisedInventoryItem.Ownership }),
              new TreeNode({  roleType: m.SerialisedInventoryItem.SerialisedInventoryItemCharacteristics,
                              nodes: [
                                new TreeNode({ roleType: m.SerialisedInventoryItemCharacteristic.SerialisedInventoryItemCharacteristicType }),
                                new TreeNode({ roleType: m.SerialisedInventoryItemCharacteristic.LocalisedValues, nodes: [new TreeNode({ roleType: m.LocalisedText.Locale })] }),
                              ],
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
                  roleType: m.Singleton.AdditionalLocales,
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
              name: "ownerships",
              objectType: this.m.Ownership,
            }),
          new Query(
            {
              name: "inventoryItemKinds",
              objectType: this.m.InventoryItemKind,
            }),
          new Query(
            {
              name: "serialisedInventoryItemStates",
              objectType: this.m.SerialisedInventoryItemState,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }))
          .switchMap((loaded) => {

            this.good = loaded.objects.good as Good;
            this.categories = loaded.collections.categories as ProductCategory[];
            this.productTypes = loaded.collections.productTypes as ProductType[];
            this.vatRates = loaded.collections.vatRates as VatRate[];
            this.brands = loaded.collections.brands as Brand[];
            this.ownerships = loaded.collections.ownerships as Ownership[];
            this.inventoryItemKinds = loaded.collections.inventoryItemKinds as InventoryItemKind[];
            this.serialisedInventoryItemStates = loaded.collections.serialisedInventoryItemStates as SerialisedInventoryItemState[];
            this.singleton = loaded.collections.singletons[0] as Singleton;
            const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
            this.facility = internalOrganisation.DefaultFacility;
            this.locales = this.singleton.AdditionalLocales;

            const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);
            const inventoryItemKindSerialised = this.inventoryItemKinds.find((v: InventoryItemKind) => v.Name === "Serialised");

            if (this.good === undefined) {
              this.good = this.scope.session.create("Good") as Good;
              this.good.VatRate = vatRateZero;
              this.good.Sku = "";

              this.inventoryItem = this.scope.session.create("SerialisedInventoryItem") as SerialisedInventoryItem;
              this.good.InventoryItemKind = inventoryItemKindSerialised;
              this.inventoryItem.Good = this.good;
              this.inventoryItem.Facility = this.facility;
            } else {
              this.inventoryItems = loaded.collections.inventoryItems as SerialisedInventoryItem[];
              this.inventoryItem = this.inventoryItems[0];
              this.productCharacteristicValues = this.inventoryItem.SerialisedInventoryItemCharacteristics;
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
            this.subTitle = "Serialised";

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
      .subscribe((loaded) => {
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

    this.inventoryItem.SerialisedInventoryItemCharacteristics = this.productCharacteristicValues;

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
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

  public goBack(): void {
    window.history.back();
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
      .subscribe((loaded) => {

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