import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Filter, Loaded, MediaService, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { Brand, Facility, Good, InternalOrganisation, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationRole, ProductCategory, ProductFeature, ProductType, Singleton, VarianceReason, VatRate, VendorProduct } from "../../../../../domain";
import { Contains, Fetch, Path, PullRequest, Query, Sort, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";
import { Fetcher } from "../../Fetcher";

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
  public locales: Locale[] = [];
  public categories: ProductCategory[];
  public productTypes: ProductType[];
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
  public vendorProduct: VendorProduct;
  public actualQuantityOnHand: number;

  public manufacturersFilter: Filter;
  public suppliersFilter: Filter;

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  private fetcher: Fetcher;

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

    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([, , internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          this.fetcher.locales,
          this.fetcher.internalOrganisation,
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
              new TreeNode({
                roleType: m.InventoryItem.InventoryItemVariances,
              }),
            ],
            name: "inventoryItems",
            path: new Path({ step: this.m.Good.InventoryItemsWhereGood }),
          }),
        ];

        const query: Query[] = [
          new Query(this.m.OrganisationRole),
          new Query(this.m.ProductCategory),
          new Query(this.m.ProductType),
          new Query(this.m.VarianceReason),
          new Query(this.m.VatRate),
          new Query(this.m.Ownership),
          new Query(this.m.InventoryItemKind),
          new Query(this.m.NonSerialisedInventoryItemState),
          new Query(
            {
              name: "brands",
              objectType: this.m.Brand,
              sort: [new Sort({ roleType: m.Brand.Name, direction: "Asc" })],
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }))
          .switchMap((loaded) => {

            this.good = loaded.objects.good as Good;
            this.categories = loaded.collections.ProductCategoryQuery as ProductCategory[];
            this.productTypes = loaded.collections.ProductTypeQuery as ProductType[];
            this.varianceReasons = loaded.collections.VarianceReasonQuery as VarianceReason[];
            this.vatRates = loaded.collections.VatRateQuery as VatRate[];
            this.brands = loaded.collections.brands as Brand[];
            this.inventoryItemKinds = loaded.collections.InventoryItemKindQuery as InventoryItemKind[];
            this.inventoryItemObjectStates = loaded.collections.NonSerialisedInventoryItemStateQuery as NonSerialisedInventoryItemState[];
            this.locales = loaded.collections.locales as Locale[];
            const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
            this.facility = internalOrganisation.DefaultFacility;

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

              this.vendorProduct = this.scope.session.create("VendorProduct") as VendorProduct;
              this.vendorProduct.Product = this.good;
              this.vendorProduct.InternalOrganisation = internalOrganisation;
            } else {
              this.inventoryItems = loaded.collections.inventoryItems as NonSerialisedInventoryItem[];
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
           }

            this.title = this.good.Name;
            this.subTitle = "Non Serialised";
            this.actualQuantityOnHand = this.good.QuantityOnHand;

            const organisationRoles: OrganisationRole[] = loaded.collections.OrganisationRoleQuery as OrganisationRole[];
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
