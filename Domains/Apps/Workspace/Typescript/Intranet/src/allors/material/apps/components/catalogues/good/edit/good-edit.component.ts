import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription, BehaviorSubject, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Saved, x, Allors, NavigationService, NavigationActivatedRoute } from '../../../../../../angular';
import { Good, Facility, Locale, ProductCategory, ProductType, Organisation, Brand, Model, VendorProduct, VatRate, Ownership, InvoiceItem, SalesInvoice, InternalOrganisation } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { Fetcher } from '../../../Fetcher';
import { StateService } from 'src/allors/material/apps/services/StateService';
import { MatSnackBar } from '@angular/material';

@Component({
  templateUrl: './good-edit.component.html',
  providers: [Allors]
})
export class GoodEditComponent implements OnInit, OnDestroy {

  public title = 'Good';

  public m: MetaDomain;
  public good: Good;

  public subTitle: string;
  public facility: Facility;
  public locales: Locale[];
  public categories: ProductCategory[];
  public productTypes: ProductType[];
  public manufacturers: Organisation[];
  public brands: Brand[];
  public selectedBrand: Brand;
  public models: Model[];
  public selectedModel: Model;
  public vendorProduct: VendorProduct;
  public vatRates: VatRate[];
  public ownerships: Ownership[];
  public invoiceItems: InvoiceItem[];
  public salesInvoice: SalesInvoice;
  public organisations: Organisation[];
  public addBrand = false;
  public addModel = false;

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    @Self() private allors: Allors,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.m = this.allors.m;
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
            pull.VatRate(),
            pull.ProductCategory({ sort: new Sort(m.ProductCategory.Name) })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                scope.session.reset();

                const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
                this.facility = internalOrganisation.DefaultFacility;

                this.good = loaded.objects.good as Good;
                this.categories = loaded.collections.productCategories as ProductCategory[];
                this.vatRates = loaded.collections.VatRates as VatRate[];
                this.locales = loaded.collections.locales as Locale[];
                this.invoiceItems = loaded.collections.invoiceItems as InvoiceItem[];

                const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);

                if (this.good === undefined) {
                  this.good = scope.session.create('Good') as Good;
                  this.good.VatRate = vatRateZero;
                  // this.good.Sku = '';

                  this.vendorProduct = scope.session.create('VendorProduct') as VendorProduct;
                  this.vendorProduct.Product = this.good;
                  this.vendorProduct.InternalOrganisation = internalOrganisation;
                }

                this.title = this.good.Name;

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
      .subscribe(() => {
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
      .subscribe(() => {
          this.navigation.back();
        },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {
    const { scope } = this.allors;

    const isNew = this.good.isNew;

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
}
