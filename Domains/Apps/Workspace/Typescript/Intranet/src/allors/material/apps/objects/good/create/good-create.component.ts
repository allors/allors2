import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription, BehaviorSubject, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { ErrorService, ContextService, NavigationService, NavigationActivatedRoute, MetaService, RefreshService } from '../../../../../angular';
import { Good, Facility, Locale, ProductCategory, ProductType, Organisation, Brand, Model, VendorProduct, VatRate, Ownership, InternalOrganisation, Part, GoodIdentificationType, ProductNumber, Settings } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { Fetcher } from '../../Fetcher';
import { StateService } from '../../../..';
import { CreateData, ObjectData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './good-create.component.html',
  providers: [ContextService]
})
export class GoodCreateComponent implements OnInit, OnDestroy {

  m: Meta;
  good: Good;

  public title = 'Add Good';

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
  selectedCategories: ProductCategory[] = [];

  private subscription: Subscription;
  private fetcher: Fetcher;
  settings: Settings;
  goodNumberType: GoodIdentificationType;

  constructor(
    @Self() private allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<GoodCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    public navigationService: NavigationService,
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
            this.fetcher.Settings,
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

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.facility = internalOrganisation.DefaultFacility;

        this.categories = loaded.collections.ProductCategories as ProductCategory[];
        this.parts = loaded.collections.Parts as Part[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.settings = loaded.objects.Settings as Settings;

        const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);
        this.goodNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === 'b640630d-a556-4526-a2e5-60a84ab0db3f');

        this.good = this.allors.context.create('Good') as Good;
        this.good.VatRate = vatRateZero;

        if (!this.settings.UseGlobalProductNumber) {
          this.productNumber = this.allors.context.create('ProductNumber') as ProductNumber;
          this.productNumber.GoodIdentificationType = this.goodNumberType;

          this.good.AddGoodIdentification(this.productNumber);
        }

        this.vendorProduct = this.allors.context.create('VendorProduct') as VendorProduct;
        this.vendorProduct.Product = this.good;
        this.vendorProduct.InternalOrganisation = internalOrganisation;
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
    this.refreshService.refresh();
  }

  public save(): void {

    this.selectedCategories.forEach((category: ProductCategory) => {
      category.AddProduct(this.good);
    });

    this.allors.context.save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.good.id,
          objectType: this.good.objectType,
        };

        this.dialogRef.close(data);
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
