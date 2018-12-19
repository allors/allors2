import { Component, OnDestroy, OnInit, Self, SkipSelf } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';

import { ErrorService, ContextService, NavigationService, PanelService, RefreshService, MetaService } from '../../../../../../angular';
import { InternalOrganisation, Locale, Organisation, Good, Facility, ProductCategory, ProductType, Brand, Model, VendorProduct, Ownership, VatRate, Part, GoodIdentificationType, ProductNumber, PartNumber, UnitOfMeasure, PriceComponent, InventoryItemKind, SupplierOffering } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { Meta } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';

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
  vendorProduct: VendorProduct;
  organisations: Organisation[];
  addBrand = false;
  addModel = false;
  goodIdentificationTypes: GoodIdentificationType[];
  partNumber: PartNumber;
  facilities: Facility[];
  unitsOfMeasure: UnitOfMeasure[];
  currentSellingPrice: PriceComponent;

  private subscription: Subscription;
  private fetcher: Fetcher;

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    private metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public location: Location,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);

    panel.name = 'detail';
    panel.title = 'Part Details';
    panel.icon = 'person';
    panel.expandable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.Part.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Part({
          name: pullName,
          object: id,
        }),
      );
    };

    panel.onPulled = (loaded) => {
      this.part = loaded.objects[pullName] as Part;
    };
  }

  public ngOnInit(): void {

    // Maximized
    this.subscription = combineLatest(
      this.route.url,
      this.route.queryParams,
      this.refreshService.refresh$,
      this.stateService.internalOrganisationId$,
    )
      .pipe(
        filter((v) => {
          return this.panel.isExpanded;
        }),
        switchMap(([, , , internalOrganisationId]) => {

          this.part = undefined;

          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.Part({
              object: id,
              include: {
                PrimaryPhoto: x,
                Photos: x,
                Documents: x,
                ElectronicDocuments: x,
                ManufacturedBy: x,
                SerialisedItemCharacteristics: {
                  SerialisedItemCharacteristicType: {
                    UnitOfMeasure: x
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
            pull.Facility({
              predicate: new Equals({ propertyType: m.Facility.Owner, object: internalOrganisationId }),
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

        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.facility = internalOrganisation.DefaultFacility;

        this.part = loaded.objects.Part as Part;
        this.inventoryItemKinds = loaded.collections.InventoryItemKinds as InventoryItemKind[];
        this.productTypes = loaded.collections.ProductTypes as ProductType[];
        this.brands = loaded.collections.Brands as Brand[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.facilities = loaded.collections.Facilities as Facility[];
        this.unitsOfMeasure = loaded.collections.UnitsOfMeasure as UnitOfMeasure[];
        this.manufacturers = loaded.collections.Organisations as Organisation[];

        this.activeSuppliers = internalOrganisation.ActiveSuppliers as Organisation[];
        this.activeSuppliers = this.activeSuppliers.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

        this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];
        const partNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === '5735191a-cdc4-4563-96ef-dddc7b969ca6');

        this.manufacturers = loaded.collections.Organisations as Organisation[];

        this.partNumber = this.part.GoodIdentifications.find(v => v.GoodIdentificationType === partNumberType);

        this.suppliers = this.part.SuppliedBy as Organisation[];
        this.selectedSuppliers = this.suppliers;

        this.selectedBrand = this.part.Brand;
        this.selectedModel = this.part.Model;
        this.brandSelected(this.selectedBrand);

        this.supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];

      }, this.errorService.handler);

  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.navigationService.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
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
      .subscribe((loaded) => {
        this.models = this.selectedBrand.Models.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
      }, this.errorService.handler);
  }


}
