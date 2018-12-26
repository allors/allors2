import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription, BehaviorSubject, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { ErrorService, Saved, ContextService, NavigationService, NavigationActivatedRoute, MetaService, RefreshService } from '../../../../../angular';
import { Facility, Locale, ProductType, Organisation, SupplierOffering, Brand, Model, InventoryItemKind, VendorProduct, InternalOrganisation, GoodIdentificationType, PartNumber, Part, SerialisedItemState, UnitOfMeasure, PriceComponent } from '../../../../../domain';
import { PullRequest, Sort, Equals, Not, GreaterThan } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { Fetcher } from '../../Fetcher';
import { StateService } from '../../../../';
import { CreateData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './part-create.component.html',
  providers: [ContextService]
})
export class PartCreateComponent implements OnInit, OnDestroy {

  m: Meta;

  add: boolean;
  edit: boolean;

  part: Part;
  subTitle: string;
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
    @Self() private allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<PartCreateComponent>,
    public metaService: MetaService,
    public navigationService: NavigationService,
    private refreshService: RefreshService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.m = this.metaService.m;

    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, internalOrganisationId]) => {

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
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

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.facility = internalOrganisation.DefaultFacility;

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

        this.part = this.allors.context.create('Part') as Part;

        this.partNumber = this.allors.context.create('PartNumber') as PartNumber;
        this.partNumber.GoodIdentificationType = partNumberType;

        this.part.AddGoodIdentification(this.partNumber);
      },
        (error: any) => {
          this.errorService.handle(error);
          this.navigationService.back();
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

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
  public save(): void {
    this.onSave();

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        this.navigationService.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {

    const isNew = this.part.isNew;
    this.onSave();

    this.allors.context
      .save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        if (isNew) {
          this.navigationService.overview(this.part);
        } else {
          this.refreshService.refresh();
        }
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
    return supplierOffering;
  }
}
