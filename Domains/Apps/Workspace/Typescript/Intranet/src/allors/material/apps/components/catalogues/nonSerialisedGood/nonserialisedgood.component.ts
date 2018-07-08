import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, UrlSegment } from '@angular/router';


import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, FilterFactory, Loaded, MediaService, Saved, Scope, WorkspaceService, LayoutService } from '../../../../../angular';
import { Brand, Facility, Good, InternalOrganisation, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationRole, Party, ProductCategory, ProductFeature, ProductType, Singleton, SupplierOffering, VarianceReason, VatRate, VendorProduct } from '../../../../../domain';
import { Contains, Equals, Fetch, Path, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './nonserialisedgood.component.html',
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
  public activeSuppliers: Organisation[];
  public selectedSuppliers: Organisation[];
  public supplierOfferings: SupplierOffering[];
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
  public addBrand = false;
  public addModel = false;
  public scope: Scope;

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;

  private fetcher: Fetcher;

  constructor(
    public layout: LayoutService,
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    public mediaService: MediaService,
    public stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([, , internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          this.fetcher.locales,
          this.fetcher.internalOrganisation,
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Good.PrimaryPhoto }),
              new TreeNode({ roleType: m.Good.Photos }),
              new TreeNode({ roleType: m.Good.LocalisedNames, nodes: [new TreeNode({ roleType: m.LocalisedText.Locale })] }),
              new TreeNode({ roleType: m.Good.LocalisedDescriptions, nodes: [new TreeNode({ roleType: m.LocalisedText.Locale })] }),
              new TreeNode({ roleType: m.Good.LocalisedComments, nodes: [new TreeNode({ roleType: m.LocalisedText.Locale })] }),
              new TreeNode({ roleType: m.Good.ProductCategories }),
              new TreeNode({ roleType: m.Good.InventoryItemKind }),
              new TreeNode({ roleType: m.Good.SuppliedBy }),
              new TreeNode({ roleType: m.Good.ManufacturedBy }),
              new TreeNode({ roleType: m.Good.StandardFeatures }),
            ],
            name: 'good',
          }),
          new Fetch({
            id,
            include: [
              new TreeNode({
                roleType: m.InventoryItem.InventoryItemVariances,
              }),
            ],
            name: 'inventoryItems',
            path: new Path({ step: this.m.Good.InventoryItemsWhereGood }),
          }),
          new Fetch({
            id,
            name: 'supplierOfferings',
            path: new Path({ step: this.m.Product.SupplierOfferingsWhereProduct }),
          }),
        ];

        const queries: Query[] = [
          new Query(this.m.VatRate),
          new Query({
            name: 'varianceReasons',
            objectType: m.VarianceReason,
            predicate: new Equals({ roleType: m.VarianceReason.IsActive, value: true }),
            sort: [
              new Sort({ roleType: m.VarianceReason.Name, direction: 'Asc' }),
            ],
          }),
          new Query({
            name: 'inventoryItemKinds',
            objectType: m.InventoryItemKind,
            sort: [
              new Sort({ roleType: m.InventoryItemKind.Name, direction: 'Asc' }),
            ],
          }),
          new Query({
            name: 'nonSerialisedInventoryItemStates',
            objectType: m.NonSerialisedInventoryItemState,
            sort: [
              new Sort({ roleType: m.NonSerialisedInventoryItemState.Name, direction: 'Asc' }),
            ],
          }),
          new Query(
            {
              name: 'productCategories',
              objectType: this.m.ProductCategory,
              sort: [new Sort({ roleType: m.ProductCategory.Name, direction: 'Asc' })],
            }),
          new Query(
            {
              name: 'productTypes',
              objectType: this.m.ProductType,
              sort: [new Sort({ roleType: m.ProductType.Name, direction: 'Asc' })],
            }),
          new Query(
            {
              name: 'brands',
              objectType: this.m.Brand,
              sort: [new Sort({ roleType: m.Brand.Name, direction: 'Asc' })],
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }))
          .switchMap((loaded) => {

            this.good = loaded.objects.good as Good;
            this.categories = loaded.collections.productCategories as ProductCategory[];
            this.productTypes = loaded.collections.productTypes as ProductType[];
            this.varianceReasons = loaded.collections.varianceReasons as VarianceReason[];
            this.vatRates = loaded.collections.VatRates as VatRate[];
            this.brands = loaded.collections.brands as Brand[];
            this.inventoryItemKinds = loaded.collections.inventoryItemKinds as InventoryItemKind[];
            this.inventoryItemObjectStates = loaded.collections.nonSerialisedInventoryItemStates as NonSerialisedInventoryItemState[];
            this.locales = loaded.collections.locales as Locale[];
            const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
            this.facility = internalOrganisation.DefaultFacility;
            this.activeSuppliers = internalOrganisation.ActiveSuppliers as Organisation[];
            this.activeSuppliers = this.activeSuppliers.sort( (a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0))

            const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);
            const inventoryItemKindNonSerialised = this.inventoryItemKinds.find((v: InventoryItemKind) => v.Name === 'Non serialised');

            if (this.good === undefined) {
              this.good = this.scope.session.create('Good') as Good;
              this.good.VatRate = vatRateZero;
              this.good.Sku = '';

              this.inventoryItem = this.scope.session.create('NonSerialisedInventoryItem') as NonSerialisedInventoryItem;
              this.good.InventoryItemKind = inventoryItemKindNonSerialised;
              this.inventoryItem.Good = this.good;
              this.inventoryItem.Facility = this.facility;

              this.vendorProduct = this.scope.session.create('VendorProduct') as VendorProduct;
              this.vendorProduct.Product = this.good;
              this.vendorProduct.InternalOrganisation = internalOrganisation;
            } else {
              this.suppliers = this.good.SuppliedBy as Organisation[];
              this.selectedSuppliers = this.suppliers;
              this.supplierOfferings = loaded.collections.supplierOfferings as SupplierOffering[];
              this.inventoryItems = loaded.collections.inventoryItems as NonSerialisedInventoryItem[];
              this.inventoryItem = this.inventoryItems[0];
              this.good.StandardFeatures.forEach((feature: ProductFeature) => {
                if (feature.objectType.name === 'Brand') {
                  this.selectedBrand = feature as Brand;
                  this.brandSelected(this.selectedBrand);
                }
                if (feature.objectType.name === 'Model') {
                  this.selectedModel = feature as Model;
                }
              });
            }

            this.title = this.good.Name;
            this.subTitle = 'Non Serialised';
            this.actualQuantityOnHand = this.good.QuantityOnHand;

            const Query2: Query[] = [
              new Query(
                {
                  name: 'manufacturers',
                  objectType: m.Organisation,
                  predicate: new Equals({ roleType: m.Organisation.IsManufacturer, value: true }),
                  sort: [new Sort({ roleType: m.Organisation.PartyName, direction: 'Asc' })],
                }),
            ];

            return this.scope.load('Pull', new PullRequest({ queries: Query2 }));
          });
      })
      .subscribe((loaded) => {
        this.manufacturers = loaded.collections.manufacturers as Organisation[];
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
    );
  }

  public brandAdded(brand: Brand): void {
    this.brands.push(brand);
    this.selectedBrand = brand;
    this.models = [];
    this.selectedModel = undefined;
  }

  public modelAdded(model: Model): void {
    this.selectedBrand.AddModel(model);
    this.models = this.selectedBrand.Models.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
    this.selectedModel = model;
  }

  public brandSelected(brand: Brand): void {

    const fetches: Fetch[] = [
      new Fetch(
        {
          id: brand.id,
          include: [new TreeNode({ roleType: this.m.Brand.Models })],
          name: 'selectedbrand',
        }),
    ];

    this.scope
      .load('Pull', new PullRequest({ fetches }))
      .subscribe((loaded) => {

        const selectedBrand = loaded.objects.selectedbrand as Brand;
        this.models = selectedBrand.Models.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
      },
        (error: Error) => {
          this.errorService.handle(error);
          this.goBack();
        },
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
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
      const reason = this.varianceReasons.find((v: VarianceReason) => v.Name === 'Unknown');

      const inventoryItemVariance = this.scope.session.create('InventoryItemVariance') as InventoryItemVariance;
      inventoryItemVariance.Quantity = this.actualQuantityOnHand - this.good.QuantityOnHand;
      inventoryItemVariance.Reason = reason;

      this.inventoryItem.AddInventoryItemVariance(inventoryItemVariance);
    }

    const suppliersToDelete = this.suppliers;

    if (this.selectedSuppliers !== undefined) {
      this.selectedSuppliers.forEach((supplier: Organisation) => {
        const index = suppliersToDelete.indexOf(supplier);
        if (index > -1) {
          suppliersToDelete.splice(index, 1);
        }

        const now = new Date();
        const supplierOffering = this.supplierOfferings.find((v) =>
          v.Supplier === supplier &&
          v.FromDate <= now &&
          (v.ThroughDate === null || v.ThroughDate >= now));

        if (supplierOffering === undefined) {
          this.supplierOfferings.push(this.newSupplierOffering(supplier, this.good));
        } else {
          supplierOffering.ThroughDate = null;
        }
      });
    }

    if (suppliersToDelete !== undefined) {
      suppliersToDelete.forEach((supplier: Organisation) => {
        const now = new Date();
        const supplierOffering = this.supplierOfferings.find((v) =>
          v.Supplier === supplier &&
          v.FromDate <= now &&
          (v.ThroughDate === null || v.ThroughDate >= now));

        if (supplierOffering !== undefined) {
          supplierOffering.ThroughDate = new Date();
        }
      });
    }
    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }

  private newSupplierOffering(supplier: Organisation, good: Good): SupplierOffering {
    const supplierOffering = this.scope.session.create('SupplierOffering') as SupplierOffering;
    supplierOffering.Supplier = supplier;
    supplierOffering.Product = good;
    return supplierOffering;
  }
}
