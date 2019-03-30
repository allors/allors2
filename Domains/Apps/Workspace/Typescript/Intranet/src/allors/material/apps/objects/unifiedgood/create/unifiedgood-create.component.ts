import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { ErrorService, ContextService, NavigationService, MetaService, RefreshService } from '../../../../../angular';
import { ProductType, VatRate, ProductIdentificationType, ProductNumber, Settings, UnifiedGood, Good, InventoryItemKind } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { CreateData } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { Fetcher } from '../../Fetcher';
import { StateService } from '../../../..';

@Component({
  templateUrl: './unifiedgood-create.component.html',
  providers: [ContextService]
})
export class UnifiedGoodCreateComponent implements OnInit, OnDestroy {

  readonly m: Meta;
  good: Good;

  public title = 'Add Unified Good';

  productTypes: ProductType[];
  inventoryItemKinds: InventoryItemKind[];
  vatRates: VatRate[];
  goodIdentificationTypes: ProductIdentificationType[];
  productNumber: ProductNumber;
  settings: Settings;
  goodNumberType: ProductIdentificationType;

  private subscription: Subscription;
  private fetcher: Fetcher;

  constructor(
    @Self() private allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<UnifiedGoodCreateComponent>,
    public metaService: MetaService,
    private refreshService: RefreshService,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const pulls = [
            this.fetcher.Settings,
            pull.InventoryItemKind(),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }),
            pull.VatRate(),
            pull.ProductIdentificationType(),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.inventoryItemKinds = loaded.collections.InventoryItemKinds as InventoryItemKind[];
        this.productTypes = loaded.collections.ProductTypes as ProductType[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.goodIdentificationTypes = loaded.collections.ProductIdentificationTypes as ProductIdentificationType[];
        this.settings = loaded.objects.Settings as Settings;

        const vatRateZero = this.vatRates.find((v: VatRate) => v.Rate === 0);
        this.goodNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === 'b640630d-a556-4526-a2e5-60a84ab0db3f');

        this.good = this.allors.context.create('UnifiedGood') as UnifiedGood;
        this.good.VatRate = vatRateZero;

        if (!this.settings.UseProductNumberCounter) {
          this.productNumber = this.allors.context.create('ProductNumber') as ProductNumber;
          this.productNumber.ProductIdentificationType = this.goodNumberType;

          this.good.AddProductIdentification(this.productNumber);
        }
      }, this.errorService.handler);
    }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
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
