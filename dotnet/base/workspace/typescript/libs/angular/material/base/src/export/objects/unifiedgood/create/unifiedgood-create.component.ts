import { Component, OnDestroy, OnInit, Self, Optional, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService } from '@allors/angular/services/core';
import { PullRequest } from '@allors/protocol/system';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import {
  ProductType,
  ProductIdentificationType,
  Settings,
  InventoryItemKind,
  Good,
  ProductNumber,
  UnifiedGood,
} from '@allors/domain/generated';
import { Sort } from '@allors/data/system';
import { FetcherService } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { Meta } from '@allors/meta/generated';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './unifiedgood-create.component.html',
  providers: [ContextService],
})
export class UnifiedGoodCreateComponent extends TestScope implements OnInit, OnDestroy {
  readonly m: Meta;
  good: Good;

  public title = 'Add Unified Good';

  productTypes: ProductType[];
  inventoryItemKinds: InventoryItemKind[];
  goodIdentificationTypes: ProductIdentificationType[];
  productNumber: ProductNumber;
  settings: Settings;
  goodNumberType: ProductIdentificationType;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<UnifiedGoodCreateComponent>,
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
            this.fetcher.Settings,
            pull.InventoryItemKind(),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }),
            pull.ProductIdentificationType(),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.inventoryItemKinds = loaded.collections.InventoryItemKinds as InventoryItemKind[];
        this.productTypes = loaded.collections.ProductTypes as ProductType[];
        this.goodIdentificationTypes = loaded.collections.ProductIdentificationTypes as ProductIdentificationType[];
        this.settings = loaded.objects.Settings as Settings;

        this.goodNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === 'b640630d-a556-4526-a2e5-60a84ab0db3f');

        this.good = this.allors.context.create('UnifiedGood') as UnifiedGood;

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
    this.allors.context.save().subscribe(() => {
      const data: IObject = {
        id: this.good.id,
        objectType: this.good.objectType,
      };

      this.dialogRef.close(data);
      this.refreshService.refresh();
    }, this.saveService.errorHandler);
  }
}
