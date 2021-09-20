import { Component, OnInit, Self, OnDestroy } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { MetaService, RefreshService, NavigationService, PanelService, ContextService } from '@allors/angular/services/core';
import { Organisation, Facility, UnifiedGood, InventoryItemKind, ProductType, ProductCategory, SupplierOffering, Brand, Model, ProductIdentificationType, ProductNumber, UnitOfMeasure, PriceComponent, Settings, SupplierRelationship, Locale } from '@allors/domain/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { Meta } from '@allors/meta/generated';
import { FetcherService } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { Sort, Equals } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'unifiedgood-overview-detail',
  templateUrl: './unifiedgood-overview-detail.component.html',
  providers: [PanelService, ContextService]
})
export class UnifiedGoodOverviewDetailComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  good: UnifiedGood;

  facility: Facility;
  facilities: Facility[];
  locales: Locale[];
  inventoryItemKinds: InventoryItemKind[];
  productTypes: ProductType[];
  categories: ProductCategory[];
  manufacturers: Organisation[];
  suppliers: Organisation[];
  currentSuppliers: Set<Organisation>;
  activeSuppliers: Organisation[];
  selectedSuppliers: Organisation[];
  supplierOfferings: SupplierOffering[];
  brands: Brand[];
  selectedBrand: Brand;
  models: Model[];
  selectedModel: Model;
  organisations: Organisation[];
  addBrand = false;
  addModel = false;
  goodIdentificationTypes: ProductIdentificationType[];
  productNumber: ProductNumber;
  originalCategories: ProductCategory[] = [];
  selectedCategories: ProductCategory[] = [];
  unitsOfMeasure: UnitOfMeasure[];
  currentSellingPrice: PriceComponent;
  internalOrganisation: Organisation;
  settings: Settings;

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    private saveService: SaveService,
    private fetcher: FetcherService,
    private snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject(new Date());

    panel.name = 'detail';
    panel.title = 'Good Details';
    panel.icon = 'person';
    panel.expandable = true;

    // Collapsed
    const pullName = `${this.panel.name}_${this.m.UnifiedGood.name}`;

    panel.onPull = (pulls) => {

      this.good = undefined;

      if (this.panel.isCollapsed) {
        const { pull } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.UnifiedGood({
            name: pullName,
            object: id,
          }),
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.good = loaded.objects[pullName] as UnifiedGood;
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = combineLatest(this.refresh$, this.panel.manager.on$)
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(() => {

          this.good = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.locales,
            this.fetcher.Settings,
            pull.UnifiedGood({
              object: id,
              include: {
                PrimaryPhoto: x,
                Photos: x,
                PublicElectronicDocuments: x,
                PrivateElectronicDocuments: x,
                PublicLocalisedElectronicDocuments: x,
                PrivateLocalisedElectronicDocuments: x,
                ManufacturedBy: x,
                SuppliedBy: x,
                DefaultFacility: x,
                SerialisedItemCharacteristics: {
                  LocalisedValues: x,
                  SerialisedItemCharacteristicType: {
                    UnitOfMeasure: x,
                    LocalisedNames: x
                  }
                },
                ProductIdentifications: {
                  ProductIdentificationType: x,
                },
                Brand: {
                  Models: x
                },
                Model: x,
                LocalisedNames: {
                  Locale: x,
                },
                LocalisedDescriptions: {
                  Locale: x,
                },
                LocalisedComments: {
                  Locale: x,
                },
                LocalisedKeywords: {
                  Locale: x,
                },
              },
            }),
            pull.UnifiedGood(
              {
                object: id,
                fetch: {
                  SupplierOfferingsWherePart: x
                }
              }
            ),
            pull.UnifiedGood(
              {
                object: id,
                fetch: {
                  PriceComponentsWherePart: x
                }
              }
            ),
            pull.UnitOfMeasure(),
            pull.InventoryItemKind(),
            pull.ProductIdentificationType(),
            pull.Facility(),
            pull.ProductIdentificationType(),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }),
            pull.ProductCategory({ sort: new Sort(m.ProductCategory.Name) }),
            pull.SupplierRelationship({
              include: {
                Supplier: x
              }
            }),
            pull.Brand({
              include: {
                Models: x
              },
              sort: new Sort(m.Brand.Name)
            }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsManufacturer, value: true }),
            }),
            pull.UnifiedGood({
              name: 'OriginalCategories',
              object: id,
              fetch: {
                ProductCategoriesWhereProduct: {
                  include: {
                    Products: x,
                  }
                }
              }
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        

        this.good = loaded.objects.UnifiedGood as UnifiedGood;
        this.originalCategories = loaded.collections.OriginalCategories as ProductCategory[];
        this.selectedCategories = this.originalCategories;

        this.inventoryItemKinds = loaded.collections.InventoryItemKinds as InventoryItemKind[];
        this.productTypes = loaded.collections.ProductTypes as ProductType[];
        this.brands = loaded.collections.Brands as Brand[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.facilities = loaded.collections.Facilities as Facility[];
        this.unitsOfMeasure = loaded.collections.UnitsOfMeasure as UnitOfMeasure[];
        this.manufacturers = loaded.collections.Organisations as Organisation[];
        this.settings = loaded.objects.Settings as Settings;
        this.goodIdentificationTypes = loaded.collections.ProductIdentificationTypes as ProductIdentificationType[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.manufacturers = loaded.collections.Organisations as Organisation[];
        this.categories = loaded.collections.ProductCategories as ProductCategory[];

        const supplierRelationships = loaded.collections.SupplierRelationships as SupplierRelationship[];
        const currentsupplierRelationships = supplierRelationships.filter(v => isBefore(new Date(v.FromDate), new Date()) && (v.ThroughDate === null || isAfter(new Date(v.ThroughDate), new Date())));
        this.currentSuppliers = new Set(currentsupplierRelationships.map(v => v.Supplier).sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0)));

        const goodNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === 'b640630d-a556-4526-a2e5-60a84ab0db3f');

        this.productNumber = this.good.ProductIdentifications.find(v => v.ProductIdentificationType === goodNumberType);

        this.suppliers = this.good.SuppliedBy as Organisation[];
        this.selectedSuppliers = this.suppliers;

        this.selectedBrand = this.good.Brand;
        this.selectedModel = this.good.Model;

        if (this.selectedBrand) {
          this.brandSelected(this.selectedBrand);
        }

        this.supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];

      });

  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public brandAdded(brand: Brand): void {
    this.brands.push(brand);
    this.selectedBrand = brand;
    this.models = [];
    this.selectedModel = undefined;
    this.allors.context.session.hasChanges = true;
    this.setDirty();
  }

  public modelAdded(model: Model): void {
    this.selectedBrand.AddModel(model);
    this.models = this.selectedBrand.Models.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
    this.selectedModel = model;
    this.allors.context.session.hasChanges = true;
    this.setDirty();
  }

  public brandSelected(brand: Brand): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Brand({
        object: brand,
        include: {
          Models: x,
        }
      }
      )
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe(() => {
        this.models = this.selectedBrand.Models.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
      });
  }

  public save(): void {

    this.onSave();

    this.allors.context.save()
      .subscribe(() => {
        this.refreshService.refresh();
        this.panel.toggle();
      });
  }

  public update(): void {
    const { context } = this.allors;

    this.onSave();

    context
      .save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  private onSave() {

    this.selectedCategories.forEach((category: ProductCategory) => {
      category.AddProduct(this.good);

      const index = this.originalCategories.indexOf(category);
      if (index > -1) {
        this.originalCategories.splice(index, 1);
      }
    });

    this.originalCategories.forEach((category: ProductCategory) => {
      category.RemoveProduct(this.good);
    });

    this.good.Brand = this.selectedBrand;
    this.good.Model = this.selectedModel;

    if (this.suppliers !== undefined) {
      const suppliersToDelete = this.suppliers.filter(v => v);

      if (this.selectedSuppliers !== undefined) {
        this.selectedSuppliers.forEach((supplier: Organisation) => {
          const index = suppliersToDelete.indexOf(supplier);
          if (index > -1) {
            suppliersToDelete.splice(index, 1);
          }

          
          const supplierOffering = this.supplierOfferings.find((v) =>
            v.Supplier === supplier &&
            isBefore(new Date(v.FromDate), new Date()) && (v.ThroughDate === null || isAfter(new Date(v.ThroughDate), new Date())));

          if (supplierOffering === undefined) {
            this.supplierOfferings.push(this.newSupplierOffering(supplier));
          } else {
            supplierOffering.ThroughDate = null;
          }
        });
      }

      if (suppliersToDelete !== undefined) {
        suppliersToDelete.forEach((supplier: Organisation) => {
          
          const supplierOffering = this.supplierOfferings.find((v) =>
            v.Supplier === supplier &&
            isBefore(new Date(v.FromDate), new Date()) && (v.ThroughDate === null || isAfter(new Date(v.ThroughDate), new Date())));

          if (supplierOffering !== undefined) {
            supplierOffering.ThroughDate = new Date().toISOString();
          }
        });
      }
    }
  }

  private newSupplierOffering(supplier: Organisation): SupplierOffering {

    const supplierOffering = this.allors.context.create('SupplierOffering') as SupplierOffering;
    supplierOffering.Supplier = supplier;
    supplierOffering.Part = this.good;
    supplierOffering.UnitOfMeasure = this.good.UnitOfMeasure;
    supplierOffering.Currency = this.settings.PreferredCurrency;
    return supplierOffering;
  }

  public setDirty(): void {
    this.allors.context.session.hasChanges = true;
  }
}
