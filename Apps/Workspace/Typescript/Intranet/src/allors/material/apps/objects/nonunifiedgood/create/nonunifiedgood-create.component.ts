import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { ContextService, NavigationService, MetaService, RefreshService, FetcherService, TestScope } from '../../../../../angular';
import { Locale, ProductCategory, ProductType, Organisation, VatRate, Ownership, Part, ProductIdentificationType, ProductNumber, Settings } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { SaveService } from '../../../..';
import { ObjectData } from '../../../../../../allors/material/base/services/object';
import { Good } from '../../../../../../allors/domain/generated';

@Component({
  templateUrl: './nonunifiedgood-create.component.html',
  providers: [ContextService]
})
export class NonUnifiedGoodCreateComponent extends TestScope implements OnInit, OnDestroy {

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
  goodIdentificationTypes: ProductIdentificationType[];
  productNumber: ProductNumber;
  selectedCategories: ProductCategory[] = [];
  settings: Settings;
  goodNumberType: ProductIdentificationType;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<NonUnifiedGoodCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    public navigationService: NavigationService,
    private saveService: SaveService,
    private fetcher: FetcherService
  ) {

    super();

    this.m = this.metaService.m;
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
            pull.ProductIdentificationType(),
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
        this.goodIdentificationTypes = loaded.collections.ProductIdentificationTypes as ProductIdentificationType[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.settings = loaded.objects.Settings as Settings;

        const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);
        this.goodNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === 'b640630da5564526a2e560a84ab0db3f');

        this.good = this.allors.context.create('NonUnifiedGood') as Good;
        this.good.VatRate = vatRateZero;

        if (!this.settings.UseProductNumberCounter) {
          this.productNumber = this.allors.context.create('ProductNumber') as ProductNumber;
          this.productNumber.ProductIdentificationType = this.goodNumberType;

          this.good.AddProductIdentification(this.productNumber);
        }
      });
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
        const data: IObject = {
          id: this.good.id,
          objectType: this.good.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
