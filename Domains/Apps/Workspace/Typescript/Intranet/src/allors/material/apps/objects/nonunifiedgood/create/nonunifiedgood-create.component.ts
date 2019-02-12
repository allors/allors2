import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { ErrorService, ContextService, NavigationService, MetaService, RefreshService } from '../../../../../angular';
import { Good, Locale, ProductCategory, ProductType, Organisation, VatRate, Ownership, Part, GoodIdentificationType, ProductNumber, Settings } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { Fetcher } from '../../Fetcher';
import { StateService } from '../../../..';
import { CreateData, ObjectData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './nonunifiedgood-create.component.html',
  providers: [ContextService]
})
export class NonUnifiedGoodCreateComponent implements OnInit, OnDestroy {

  readonly m: Meta;
  good: Good;

  public title = 'Add Good';

  locales: Locale[];
  categories: ProductCategory[];
  productTypes: ProductType[];
  manufacturers: Organisation[];
  vatRates: VatRate[];
  ownerships: Ownership[];
  organisations: Organisation[];
  parts: Part[];
  goodIdentificationTypes: GoodIdentificationType[];
  productNumber: ProductNumber;
  selectedCategories: ProductCategory[] = [];
  settings: Settings;
  goodNumberType: GoodIdentificationType;

  private subscription: Subscription;
  private fetcher: Fetcher;

  constructor(
    @Self() private allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<NonUnifiedGoodCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

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

        if (!this.settings.UseProductNumberCounter) {
          this.productNumber = this.allors.context.create('ProductNumber') as ProductNumber;
          this.productNumber.GoodIdentificationType = this.goodNumberType;

          this.good.AddGoodIdentification(this.productNumber);
        }
      }, this.errorService.handler);
    }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
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
}
