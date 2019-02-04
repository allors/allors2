import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { ErrorService, ContextService, NavigationService, PanelService, RefreshService, MetaService, Saved } from '../../../../../../angular';
import { Locale, Organisation, Facility, ProductType, Brand, Model, Part, GoodIdentificationType, PartNumber, UnitOfMeasure, PriceComponent, InventoryItemKind, SupplierOffering, Settings } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';
import { MatSnackBar } from '@angular/material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'part-overview-detail',
  templateUrl: './part-overview-detail.component.html',
  providers: [PanelService, ContextService]
})
export class PartOverviewDetailComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  part: Part;

  facility: Facility;
  locales: Locale[];
  inventoryItemKinds: InventoryItemKind[];
  productTypes: ProductType[];
  manufacturers: Organisation[];
  suppliers: Organisation[];
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
  goodIdentificationTypes: GoodIdentificationType[];
  partNumber: PartNumber;
  facilities: Facility[];
  unitsOfMeasure: UnitOfMeasure[];
  currentSellingPrice: PriceComponent;
  internalOrganisation: Organisation;
  settings: Settings;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private stateService: StateService,
    private snackBar: MatSnackBar) {

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Part Details';
    panel.icon = 'business';
    panel.expandable = true;

    // Collapsed
    const pullName = `${this.panel.name}_${this.m.Part.name}`;

    panel.onPull = (pulls) => {

      this.part = undefined;
      if (this.panel.isCollapsed) {
        const { pull } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.Part({
            name: pullName,
            object: id,
          }),
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.part = loaded.objects[pullName] as Part;
      }
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = combineLatest(this.refreshService.refresh$, this.panel.manager.on$)
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(([]) => {

          this.part = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;
          const fetcher = new Fetcher(this.stateService, this.metaService.pull);

          const pulls = [
            fetcher.locales,
            fetcher.internalOrganisation,
            fetcher.Settings,
            pull.Part({
              object: id,
              include: {
                PrimaryPhoto: x,
                Photos: x,
                Documents: x,
                ElectronicDocuments: x,
                ManufacturedBy: x,
                SuppliedBy: x,
                SerialisedItemCharacteristics: {
                  LocalisedValues: x,
                  SerialisedItemCharacteristicType: {
                    UnitOfMeasure: x,
                    LocalisedNames: x
                  }
                },
                Brand: {
                  Models: x
                },
                GoodIdentifications: {
                  GoodIdentificationType: x,
                },
                LocalisedComments: {
                  Locale: x,
                },
              }
            }),
            pull.Part(
              {
                object: id,
                fetch: {
                  SupplierOfferingsWherePart: x
                }
              }
            ),
            pull.Part(
              {
                object: id,
                fetch: {
                  PriceComponentsWherePart: x
                }
              }
            ),
            pull.UnitOfMeasure(),
            pull.InventoryItemKind(),
            pull.GoodIdentificationType(),
            pull.Ownership({ sort: new Sort(m.Ownership.Name) }),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }),
            pull.Brand({
              include: {
                Models: x
              },
              sort: new Sort(m.Brand.Name)
            }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsManufacturer, value: true }),
            })
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.part = loaded.objects.Part as Part;
        this.inventoryItemKinds = loaded.collections.InventoryItemKinds as InventoryItemKind[];
        this.productTypes = loaded.collections.ProductTypes as ProductType[];
        this.brands = loaded.collections.Brands as Brand[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.facilities = loaded.collections.Facilities as Facility[];
        this.unitsOfMeasure = loaded.collections.UnitsOfMeasure as UnitOfMeasure[];
        this.manufacturers = loaded.collections.Organisations as Organisation[];
        this.settings = loaded.objects.Settings as Settings;

        this.activeSuppliers = this.internalOrganisation.ActiveSuppliers as Organisation[];
        this.activeSuppliers = this.activeSuppliers.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

        this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];
        const partNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === '5735191a-cdc4-4563-96ef-dddc7b969ca6');

        this.manufacturers = loaded.collections.Organisations as Organisation[];

        this.partNumber = this.part.GoodIdentifications.find(v => v.GoodIdentificationType === partNumberType);

        this.suppliers = this.part.SuppliedBy as Organisation[];
        this.selectedSuppliers = this.suppliers;

        this.selectedBrand = this.part.Brand;
        this.selectedModel = this.part.Model;

        if (this.selectedBrand) {
        this.brandSelected(this.selectedBrand);
        }

        this.supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];

      }, this.errorService.handler);

  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public setDirty(): void {
    this.allors.context.session.hasChanges = true;
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
      .load('Pull', new PullRequest({ pulls }))
      .subscribe(() => {
        this.models = this.selectedBrand.Models.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
      }, this.errorService.handler);
  }

  public save(): void {

    this.onSave();

    this.allors.context.save()
      .subscribe(() => {
        this.panel.toggle();
      },
        (error: Error) => {
          this.errorService.handle(error);
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
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  private onSave() {

    this.part.Brand = this.selectedBrand;
    this.part.Model = this.selectedModel;

    if (this.suppliers !== undefined) {
      const suppliersToDelete = this.suppliers.filter(v => v);

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
            this.supplierOfferings.push(this.newSupplierOffering(supplier));
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
    }
  }

  private newSupplierOffering(supplier: Organisation): SupplierOffering {

    const supplierOffering = this.allors.context.create('SupplierOffering') as SupplierOffering;
    supplierOffering.Supplier = supplier;
    supplierOffering.Part = this.part;
    supplierOffering.UnitOfMeasure = this.part.UnitOfMeasure;
    supplierOffering.Currency = this.settings.PreferredCurrency;
    return supplierOffering;
  }
}
