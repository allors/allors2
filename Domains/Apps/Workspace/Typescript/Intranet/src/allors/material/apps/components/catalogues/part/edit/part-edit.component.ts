import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription, BehaviorSubject, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, Saved, x, Allors, NavigationService, NavigationActivatedRoute, Scope } from '../../../../../../angular';
import { Facility, Locale, ProductType, Organisation, SupplierOffering, Brand, Model, InventoryItemKind, VendorProduct, InternalOrganisation, GoodIdentificationType, PartNumber, Part, SerialisedItemState, UnitOfMeasure, PriceComponent } from '../../../../../../domain';
import { PullRequest, Sort, Equals, Not, GreaterThan } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { Fetcher } from '../../../Fetcher';
import { StateService } from 'src/allors/material/apps/services/StateService';
import { MatSnackBar } from '@angular/material';

@Component({
  templateUrl: './part-edit.component.html',
  providers: [Allors]
})
export class PartEditComponent implements OnInit, OnDestroy {

  scope: Scope;
  m: MetaDomain;

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
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    @Self() public allors: Allors,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.m = this.allors.m;
    this.scope = this.allors.scope;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, refresh, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();

          const add = !id;

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

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {

        scope.session.reset();

        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
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

        if (add) {
          this.add = !(this.edit = false);

          this.part = scope.session.create('Part') as Part;

          this.partNumber = scope.session.create('PartNumber') as PartNumber;
          this.partNumber.GoodIdentificationType = partNumberType;

          this.part.AddGoodIdentification(this.partNumber);

        } else {
          this.edit = !(this.add = false);
          this.partNumber = this.part.GoodIdentifications.find(v => v.GoodIdentificationType === partNumberType);

          this.suppliers = this.part.SuppliedBy as Organisation[];
          this.selectedSuppliers = this.suppliers;

          this.selectedBrand = this.part.Brand;
          this.selectedModel = this.part.Model;
          this.brandSelected(this.selectedBrand);

          this.supplierOfferings = loaded.collections.SupplierOfferings as SupplierOffering[];
        }
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

    const { pull, scope } = this.allors;

    const pulls = [
      pull.Brand({
        object: brand,
        include: {
          Models: x,
        }
      }
      )
    ];

    scope
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {
    const { scope } = this.allors;
    this.onSave();

    scope
      .save()
      .subscribe((saved: Saved) => {
        this.navigationService.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {
    const { scope } = this.allors;

    const isNew = this.part.isNew;
    this.onSave();

    scope
      .save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        if (isNew) {
          this.navigationService.overview(this.part);
        } else {
          this.refresh();
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
    const { scope } = this.allors;

    const supplierOffering = scope.session.create('SupplierOffering') as SupplierOffering;
    supplierOffering.Supplier = supplier;
    supplierOffering.Part = this.part;
    return supplierOffering;
  }
}
