import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription, BehaviorSubject, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';

import { ErrorService, ContextService, NavigationService, NavigationActivatedRoute, MetaService } from '../../../../../angular';
import { Good, Facility, Locale, ProductCategory, ProductType, Organisation, Brand, Model, VendorProduct, VatRate, Ownership, InternalOrganisation, Part, GoodIdentificationType, ProductNumber } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { Fetcher } from '../../Fetcher';
import { StateService } from '../../../..';

@Component({
  templateUrl: './good-edit.component.html',
  providers: [ContextService]
})
export class GoodEditComponent implements OnInit, OnDestroy {

  m: MetaDomain;
  good: Good;

  add: boolean;
  edit: boolean;

  subTitle: string;
  facility: Facility;
  locales: Locale[];
  categories: ProductCategory[];
  productTypes: ProductType[];
  manufacturers: Organisation[];
  brands: Brand[];
  selectedBrand: Brand;
  models: Model[];
  selectedModel: Model;
  vendorProduct: VendorProduct;
  vatRates: VatRate[];
  ownerships: Ownership[];
  organisations: Organisation[];
  addBrand = false;
  addModel = false;
  parts: Part[];
  goodIdentificationTypes: GoodIdentificationType[];
  productNumber: ProductNumber;

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    @Self() public allors: ContextService,
    public navigationService: NavigationService,
    public metaService: MetaService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, refresh, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();

          const add = !id;

          let pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            pull.VatRate(),
            pull.GoodIdentificationType(),
            pull.ProductCategory({ sort: new Sort(m.ProductCategory.Name) }),
            pull.Part({
              include: {
                Brand: x,
                Model: x
              },
              sort: new Sort(m.Part.Name),
            })
          ];

          if (!add) {
            pulls = [
              ...pulls,
              pull.Good({
                object: id,
                include: {
                  Part: {
                    Brand: x,
                    Model: x
                  },
                  PrimaryPhoto: x,
                  ProductCategories: x,
                  GoodIdentifications: x,
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
                },
              }),
            ];
          }

          return this.allors.context.load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {

        this.allors.context.reset();

        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.facility = internalOrganisation.DefaultFacility;

        this.good = loaded.objects.Good as Good;
        this.categories = loaded.collections.ProductCategories as ProductCategory[];
        this.parts = loaded.collections.Parts as Part[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];

        const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);
        const goodNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === 'b640630d-a556-4526-a2e5-60a84ab0db3f');

        if (add) {
          this.add = !(this.edit = false);

          this.good = this.allors.context.create('Good') as Good;
          this.good.VatRate = vatRateZero;

          this.productNumber = this.allors.context.create('ProductNumber') as ProductNumber;
          this.productNumber.GoodIdentificationType = goodNumberType;

          this.good.AddGoodIdentification(this.productNumber);

          this.vendorProduct = this.allors.context.create('VendorProduct') as VendorProduct;
          this.vendorProduct.Product = this.good;
          this.vendorProduct.InternalOrganisation = internalOrganisation;
        } else {

          this.edit = !(this.add = false);
          this.productNumber = this.good.GoodIdentifications.find(v => v.GoodIdentificationType === goodNumberType);
        }
      },
        (error: any) => {
          this.errorService.handle(error);
          this.navigationService.back();
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

    this.allors.context.save()
      .subscribe(() => {
        this.navigationService.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }
}
