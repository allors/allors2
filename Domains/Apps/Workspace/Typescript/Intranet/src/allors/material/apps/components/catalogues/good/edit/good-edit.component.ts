import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription, BehaviorSubject, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, Saved, x, Allors, NavigationService, NavigationActivatedRoute, Scope } from '../../../../../../angular';
import { Good, Facility, Locale, ProductCategory, ProductType, Organisation, Brand, Model, VendorProduct, VatRate, Ownership, InternalOrganisation, Part, GoodIdentificationType, GoodIdentification } from '../../../../../../domain';
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

  m: MetaDomain;
  scope: Scope;
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
  goodNumberIdentification: GoodIdentification;

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
                },
              }),
            ];
          }

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

          this.good = scope.session.create('Good') as Good;
          this.good.VatRate = vatRateZero;

          this.goodNumberIdentification = scope.session.create('GoodIdentification') as GoodIdentification;
          this.goodNumberIdentification.GoodIdentificationType = goodNumberType;

          this.good.AddGoodIdentification(this.goodNumberIdentification);

          this.vendorProduct = scope.session.create('VendorProduct') as VendorProduct;
          this.vendorProduct.Product = this.good;
          this.vendorProduct.InternalOrganisation = internalOrganisation;
        } else {
          this.edit = !(this.add = false);
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
    const { scope } = this.allors;

    scope
      .save()
      .subscribe(() => {
        this.navigationService.back();
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
