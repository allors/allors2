import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';
import { Meta } from '@allors/meta/generated'
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService } from '@allors/angular/services/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import {
  Organisation,
  Good,
  ProductCategory,
  ProductType,
  VatRate,
  Ownership,
  ProductIdentificationType,
  ProductNumber,
  Settings,
} from '@allors/domain/generated';
import { Sort } from '@allors/data/system';
import { FetcherService, Filters } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { TestScope, SearchFactory } from '@allors/angular/core';

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
  ownerships: Ownership[];
  organisations: Organisation[];
  goodIdentificationTypes: ProductIdentificationType[];
  productNumber: ProductNumber;
  selectedCategories: ProductCategory[] = [];
  settings: Settings;
  goodNumberType: ProductIdentificationType;

  private subscription: Subscription;

  nonUnifiedPartsFilter: SearchFactory;

  constructor(
    @Self() public allors: ContextService,
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

    const { m, pull } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const pulls = [
            this.fetcher.locales,
            this.fetcher.internalOrganisation,
            this.fetcher.Settings,
            pull.ProductIdentificationType(),
            pull.ProductCategory({ sort: new Sort(m.ProductCategory.Name) }),
          ];

          this.nonUnifiedPartsFilter = Filters.nonUnifiedPartsFilter(m);

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.categories = loaded.collections.ProductCategories as ProductCategory[];
        this.goodIdentificationTypes = loaded.collections.ProductIdentificationTypes as ProductIdentificationType[];
        this.locales = loaded.collections.AdditionalLocales as Locale[];
        this.settings = loaded.objects.Settings as Settings;

        this.goodNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === 'b640630d-a556-4526-a2e5-60a84ab0db3f');

        this.good = this.allors.context.create('NonUnifiedGood') as Good;

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
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
