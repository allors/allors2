import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription, BehaviorSubject, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, Saved, x, Allors, NavigationService, NavigationActivatedRoute } from '../../../../../../angular';
import { Good, Facility, Locale, ProductCategory, ProductType, Organisation, SupplierOffering, Brand, Model, InventoryItemKind, SerialisedInventoryItem, VendorProduct, SerialisedInventoryItemState, VatRate, Ownership, InvoiceItem, SalesInvoice, InternalOrganisation, GoodIdentificationType, GoodIdentification } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { Fetcher } from '../../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { StateService } from 'src/allors/material/apps/services/StateService';
import { MatSnackBar } from '@angular/material';

@Component({
  templateUrl: './part-edit.component.html',
  providers: [Allors]
})
export class PartEditComponent implements OnInit, OnDestroy {

  title = 'Email Address';
  m: MetaDomain;

  add: boolean;
  edit: boolean;

  good: Good;
  subTitle: string;
  facility: Facility;
  locales: Locale[];
  categories: ProductCategory[];
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
  vatRates: VatRate[];
  ownerships: Ownership[];
  invoiceItems: InvoiceItem[];
  salesInvoice: SalesInvoice;
  organisations: Organisation[];
  addBrand = false;
  addModel = false;
  goodIdentificationTypes: GoodIdentificationType[];
  goodNumberIdentification: GoodIdentification;

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    @Self() private allors: Allors,
    private navigation: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([refresh, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();

          const add = !id;

          // const fetch = new FetchFactory(this.workspaceService.metaPopulation);
          // const query = new QueryFactory(this.workspaceService.metaPopulation);

          // const fetches: Fetch[] = [
          //   this.fetcher.locales,
          //   this.fetcher.internalOrganisation,
          //   fetch.Good({
          //     id,
          //     include: {
          //       PrimaryPhoto: {},
          //       Photos: {},
          //       Product_LocalisedNames: {
          //         Localised_Locale: {}
          //       },
          //     },
          //   }),

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.Good({
              include: {
                PrimaryPhoto: x,
                ProductCategories: x,
                Photos: x,
                ElectronicDocuments: x,
                GoodIdentifications: {
                  GoodIdentificationType: x,
                },
                LocalisedNames: {
                  Locale: x,
                },
                LocalisedDescriptions: {
                  Locale: x,
                },
                LocalisedComments: {
                  Locale: x,
                },
              }
            }),
            pull.Good(
              {
                object: id,
                fetch: {
                  SalesInvoiceItemsWhereProduct: x
                }
              }
            ),
            pull.Product(
              {
                object: id,
                fetch: {
                  // TODO:
                  // SupplierOfferingsWhereProduct: x
                }
              }
            ),
            pull.GoodIdentificationType(),
            pull.VatRate(),
            pull.Ownership({
              sort: new Sort(m.Ownership.Name),
            }),
            pull.ProductCategory({ sort: new Sort(m.ProductCategory.Name) }),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }
            ),
            pull.Brand({ sort: new Sort(m.Brand.Name) })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                scope.session.reset();

                const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
                this.facility = internalOrganisation.DefaultFacility;

                this.good = loaded.objects.Good as Good;
                this.categories = loaded.collections.ProductCategories as ProductCategory[];
                this.productTypes = loaded.collections.ProductTypes as ProductType[];
                this.vatRates = loaded.collections.VatRates as VatRate[];
                this.brands = loaded.collections.Brands as Brand[];
                this.ownerships = loaded.collections.Ownerships as Ownership[];
                this.locales = loaded.collections.AdditionalLocales as Locale[];
                this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];
                this.invoiceItems = loaded.collections.InvoiceItems as InvoiceItem[];
                this.activeSuppliers = internalOrganisation.ActiveSuppliers as Organisation[];
                this.activeSuppliers = this.activeSuppliers.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));

                const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);
                const partNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === '5735191a-cdc4-4563-96ef-dddc7b969ca6');

                if (add) {
                  this.add = !(this.edit = false);

                  this.good = scope.session.create('Good') as Good;
                  this.good.VatRate = vatRateZero;

                  this.goodNumberIdentification = scope.session.create('GoodIdentification') as GoodIdentification;
                  this.goodNumberIdentification.GoodIdentificationType = partNumberType;

                  this.good.AddGoodIdentification(this.goodNumberIdentification);

                  this.vendorProduct = scope.session.create('VendorProduct') as VendorProduct;
                  this.vendorProduct.Product = this.good;
                  this.vendorProduct.InternalOrganisation = internalOrganisation;

                } else {
                  this.edit = !(this.add = false);

                  // this.suppliers = this.good.SuppliedBy as Organisation[];
                  // this.selectedSuppliers = this.suppliers;
                  // this.supplierOfferings = loaded.collections.supplierOfferings as SupplierOffering[];

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
                this.subTitle = 'Serialised';

                const pulls2 = [
                  pull.Organisation({
                    predicate: new Equals({ propertyType: m.Organisation.IsManufacturer, value: true }),
                    sort: new Sort(m.Organisation.PartyName),
                  })
                ];

                if (this.invoiceItems !== undefined && this.invoiceItems.length > 0) {
                  pulls2.push(
                    pull.SalesInvoiceItem({
                      object: this.invoiceItems[0].id,
                      fetch: {
                        SalesInvoiceWhereSalesInvoiceItem: x
                      }
                    })
                  );

                  return scope.load('Pull', new PullRequest({ pulls: pulls2 }));
                }
              }));
        })
      )
      .subscribe((loaded) => {
        this.manufacturers = loaded.collections.manufacturers as Organisation[];
        this.salesInvoice = loaded.objects.invoice as SalesInvoice;
      },
        (error: any) => {
          this.errorService.handle(error);
          this.navigation.back();
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

    const { pull, scope } = this.allors;

    const pulls = [
      pull.Brand({
        object: brand,
        include: {
          // TODO:
          // Models: x,
        }
      }
      )
    ];

    scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        // TODO:
        // this.models = selectedBrand.Models.sort((a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
      },
        (error: Error) => {
          this.errorService.handle(error);
          this.navigation.back();
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
    const { scope } = this.allors;

    scope
      .save()
      .subscribe((saved: Saved) => {
        this.navigation.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {
    const { scope } = this.allors;

    const isNew = this.good.isNew;
    this.onSave();

    scope
      .save()
      .subscribe(() => {
          this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
          if (isNew) {
            this.navigation.overview(this.good);
          } else {
            this.refresh();
          }
        },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  private onSave() {
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
    // TODO:
    // supplierOffering.Product = good;
    return supplierOffering;
  }
}
