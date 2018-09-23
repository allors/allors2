import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, MediaService, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { Brand, Facility, Good, InternalOrganisation, InventoryItemKind, InventoryItemVariance, Locale, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationRole, Party, ProductCategory, ProductFeature, ProductType, Singleton, SupplierOffering, VarianceReason, VatRate, VendorProduct } from '../../../../../domain';
import { Equals, Fetch, PullRequest, Sort, TreeNode } from '../../../../../framework';
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
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private stateService: StateService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    public mediaService: MediaService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.dataService.pull);
  }

  public ngOnInit(): void {

    const { m, pull } = this.dataService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,

            // TODO: Add to pull.Good
            // InventoryItemKind: x,
            // ManufacturedBy: x,
            // StandardFeatures: x,
            pull.Good({
              object: id,
              include: {
                PrimaryPhoto: x,
                Photos: x,
                ElectronicDocuments: x,
                LocalisedNames: {
                  Locale: x,
                },
                LocalisedDescriptions: {
                  Locale: x,
                },
                LocalisedComments: {
                  Locale: x,
                },
                ProductCategories: x,
                SuppliedBy: x,
              }
            }),
            // TODO:
            // new Fetch({
            //   id,
            //   include: [
            //     new TreeNode({
            //       roleType: m.InventoryItem.InventoryItemVariances,
            //     }),
            //   ],
            //   name: 'inventoryItems',
            //   path: new Path({ step: this.m.Good.InventoryItemsWhereGood }),
            // }),
            // new Fetch({
            //   id,
            //   name: 'supplierOfferings',
            //   path: new Path({ step: this.m.Product.SupplierOfferingsWhereProduct }),
            // }),
            pull.VatRate(),
            pull.VarianceReason(
              {
                predicate: new Equals({ propertyType: m.VarianceReason.IsActive, value: true }),
                sort: new Sort(m.VarianceReason.Name),
              }
            ),
            pull.InventoryItemKind({ sort: new Sort(m.InventoryItemKind.Name) }),
            pull.ProductCategory({ sort: new Sort(m.ProductCategory.Name) }),
            pull.ProductCategory(),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }),
            pull.Brand({ sort: new Sort(m.Brand.Name) })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {

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
                this.activeSuppliers = this.activeSuppliers.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

                const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);
                const inventoryItemKindNonSerialised = this.inventoryItemKinds.find((v: InventoryItemKind) => v.UniqueId === 'eaa6c331-0dd9-4bb1-8245-12a673304468');

                if (this.good === undefined) {
                  this.good = this.scope.session.create('Good') as Good;
                  this.good.VatRate = vatRateZero;
                  this.good.Sku = '';

                  this.inventoryItem = this.scope.session.create('NonSerialisedInventoryItem') as NonSerialisedInventoryItem;
                  // TODO:
                  // this.good.InventoryItemKind = inventoryItemKindNonSerialised;
                  // this.inventoryItem.Good = this.good;
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
                  // TODO:
                  // this.good.StandardFeatures.forEach((feature: ProductFeature) => {
                  //   if (feature.objectType.name === 'Brand') {
                  //     this.selectedBrand = feature as Brand;
                  //     this.brandSelected(this.selectedBrand);
                  //   }
                  //   if (feature.objectType.name === 'Model') {
                  //     this.selectedModel = feature as Model;
                  //   }
                  // });
                }

                this.title = this.good.Name;
                this.subTitle = 'Non Serialised';
                this.actualQuantityOnHand = this.good.QuantityOnHand;

                const queries2 = [
                  pull.Organisation({
                    predicate: new Equals({ propertyType: m.Organisation.IsManufacturer, value: true }),
                    sort: new Sort(m.Organisation.PartyName),
                  })
                ];

                return this.scope.load('Pull', new PullRequest({ pulls: queries2 }));
              })
            )
            ;
        })
      )
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
    // TODO:
    // this.selectedBrand.AddModel(model);
    // this.models = this.selectedBrand.Models.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
    this.selectedModel = model;
  }

  public brandSelected(brand: Brand): void {

    const { m, pull } = this.dataService;

    // TODO: include Model
    const pulls = [
      pull.Brand(
        {
          object: brand,
        }
      )
    ];

    this.scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        const selectedBrand = loaded.objects.selectedbrand as Brand;
        // this.models = selectedBrand.Models.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
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
    // TODO:
    // this.good.StandardFeatures.forEach((feature: ProductFeature) => {
    //   this.good.RemoveStandardFeature(feature);
    // });

    // if (this.selectedBrand != null) {
    //   this.good.AddStandardFeature(this.selectedBrand);
    // }

    // if (this.selectedModel != null) {
    //   this.good.AddStandardFeature(this.selectedModel);
    // }

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
    // TODO:
    // supplierOffering.Product = good;
    return supplierOffering;
  }
}
