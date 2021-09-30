import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';
import { Meta } from '@allors/meta/generated'
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import {
  Organisation,
  Facility,
  ProductType,
  ProductIdentificationType,
  Settings,
  Part,
  SupplierRelationship,
  InventoryItemKind,
  SupplierOffering,
  Brand,
  Model,
  PartNumber,
  UnitOfMeasure,
  PartCategory,
  NonUnifiedPart,
  Locale,
} from '@allors/domain/generated';
import { Equals, Sort } from '@allors/data/system';
import { FetcherService } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './nonunifiedpart-create.component.html',
  providers: [ContextService]
})
export class NonUnifiedPartCreateComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  public title = 'Add Part';

  part: Part;
  facility: Facility;
  locales: Locale[];
  supplierRelationships: SupplierRelationship[];
  inventoryItemKinds: InventoryItemKind[];
  productTypes: ProductType[];
  manufacturers: Organisation[];
  suppliers: Organisation[];
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
  partNumber: PartNumber;
  facilities: Facility[];
  unitsOfMeasure: UnitOfMeasure[];
  settings: Settings;
  currentSuppliers: Set<Organisation>;
  categories: PartCategory[];
  selectedCategories: PartCategory[] = [];

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<NonUnifiedPartCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    private saveService: SaveService,
    private snackBar: MatSnackBar,
    private fetcher: FetcherService
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const pulls = [
            this.fetcher.locales,
            this.fetcher.Settings,
            this.fetcher.warehouses,
            pull.UnitOfMeasure(),
            pull.InventoryItemKind(),
            pull.ProductIdentificationType(),
            pull.Ownership({ sort: new Sort(m.Ownership.Name) }),
            pull.PartCategory({ sort: new Sort(m.PartCategory.Name) }),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }),
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
          ];

          return this.allors.context
            .load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        const now = new Date().toISOString();

        this.inventoryItemKinds = loaded.collections.InventoryItemKinds as InventoryItemKind[];
        this.productTypes = loaded.collections.ProductTypes as ProductType[];
        this.brands = loaded.collections.Brands as Brand[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.facilities = loaded.collections.Facilities as Facility[];
        this.manufacturers = loaded.collections.Organisations as Organisation[];
        this.categories = loaded.collections.PartCategories as PartCategory[];
        this.settings = loaded.objects.Settings as Settings;

        const supplierRelationships = loaded.collections.SupplierRelationships as SupplierRelationship[];
        const currentsupplierRelationships = supplierRelationships.filter(v => isBefore(new Date(v.FromDate), new Date()) && (v.ThroughDate === null || isAfter(new Date(v.ThroughDate), new Date())));
        this.currentSuppliers = new Set(currentsupplierRelationships.map(v => v.Supplier).sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0)));

        this.unitsOfMeasure = loaded.collections.UnitsOfMeasure as UnitOfMeasure[];
        const piece = this.unitsOfMeasure.find((v) => v.UniqueId === 'f4bbdb52-3441-4768-92d4-729c6c5d6f1b');

        this.goodIdentificationTypes = loaded.collections.ProductIdentificationTypes as ProductIdentificationType[];
        const partNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === '5735191a-cdc4-4563-96ef-dddc7b969ca6');

        this.manufacturers = loaded.collections.Organisations as Organisation[];

        this.part = this.allors.context.create('NonUnifiedPart') as NonUnifiedPart;
        this.part.DefaultFacility = this.settings.DefaultFacility;
        this.part.UnitOfMeasure = piece;

        if (!this.settings.UsePartNumberCounter) {
          this.partNumber = this.allors.context.create('PartNumber') as PartNumber;
          this.partNumber.ProductIdentificationType = partNumberType;

          this.part.AddProductIdentification(this.partNumber);
        }
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
      .load(new PullRequest({ pulls }))
      .subscribe(() => {
        this.models = this.selectedBrand.Models ? this.selectedBrand.Models.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0)) : [];
      });
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
      .subscribe(() => {
        const data: IObject = {
          id: this.part.id,
          objectType: this.part.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
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
    this.selectedCategories.forEach((category: PartCategory) => {
      category.AddPart(this.part);
    });

    this.part.Brand = this.selectedBrand;
    this.part.Model = this.selectedModel;

    if (this.selectedSuppliers !== undefined) {
      this.selectedSuppliers.forEach((supplier: Organisation) => {
        const supplierOffering = this.allors.context.create('SupplierOffering') as SupplierOffering;
        supplierOffering.Supplier = supplier;
        supplierOffering.Part = this.part;
        supplierOffering.UnitOfMeasure = this.part.UnitOfMeasure;
        supplierOffering.Currency = this.settings.PreferredCurrency;
      });
    }
  }
}
