import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, TestScope } from '../../../../../angular';
import { NonUnifiedGood, InventoryItem, NonSerialisedInventoryItem, Product, Shipment, ShipmentItem, SerialisedInventoryItem, SerialisedItem, Part, OrderShipment } from '../../../../../domain';
import { PullRequest, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { ObjectData, SaveService, FiltersService } from '../../../../../material';

@Component({
  templateUrl: './shipmentitem-edit.component.html',
  providers: [ContextService]
})
export class ShipmentItemEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  shipment: Shipment;
  shipmentItem: ShipmentItem;
  goods: NonUnifiedGood[];
  inventoryItems: InventoryItem[];
  serialisedInventoryItem: SerialisedInventoryItem;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  part: Part;
  serialisedItem: SerialisedItem;
  serialisedItems: SerialisedItem[] = [];

  private previousGood;
  private subscription: Subscription;
  orderShipments: OrderShipment[];

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public filtersService: FiltersService,
    public dialogRef: MatDialogRef<ShipmentItemEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    public snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            pull.ShipmentItem({
              object: this.data.id,
              include: {
                SyncedShipment: x,
                Good: x,
                Part: x,
                SerialisedItem: x,
                ShipmentItemState: x
              }
            }),
            pull.ShipmentItem({
              object: this.data.id,
              fetch: {
                OrderShipmentsWhereShipmentItem: x
              }
            }),
          ];

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.Shipment({
                object: this.data.associationId,
              })
            );
          }

          return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.orderShipments = loaded.collections.OrderShipments as OrderShipment[];
        this.goods = loaded.collections.NonUnifiedGoods as NonUnifiedGood[];

        if (isCreate) {
          this.title = 'Add Shipment Item';
          this.shipmentItem = this.allors.context.create('ShipmentItem') as ShipmentItem;
          this.shipment.AddShipmentItem(this.shipmentItem);

        } else {
          this.shipmentItem = loaded.objects.ShipmentItem as ShipmentItem;

          if (this.shipmentItem.Good) {
            this.previousGood = this.shipmentItem.Good;

          }

          if (this.shipmentItem.CanWriteQuantity) {
            this.title = 'Edit Shipment Order Item';
          } else {
            this.title = 'View Shipment Order Item';
          }
        }
      });
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
          id: this.shipmentItem.id,
          objectType: this.shipmentItem.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }

  public goodSelected(product: Product): void {
    if (product) {
      this.refreshSerialisedItems(product);
    }
  }

  public serialisedItemSelected(serialisedItem: SerialisedItem): void {

    this.serialisedItem = this.part.SerialisedItems.find(v => v === serialisedItem);
    this.shipmentItem.Quantity = '1';
  }

  public update(): void {
    const { context } = this.allors;

    context
      .save()
      .subscribe(() => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }

  private refreshSerialisedItems(product: Product): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.NonUnifiedGood({
        object: product.id,
        fetch: {
          Part: {
            include: {
              SerialisedItems: x
            }
          }
        }
      }),
      pull.UnifiedGood({
        object: product.id,
        include: {
          SerialisedItems: x
        }
      })
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        this.part = (loaded.objects.UnifiedGood || loaded.objects.Part) as Part;
        this.serialisedItems = this.part.SerialisedItems.filter(v => v.AvailableForSale === true);

        if (this.shipmentItem.Good !== this.previousGood) {
          this.shipmentItem.SerialisedItem = null;
          this.serialisedItem = null;
          this.previousGood = this.shipmentItem.Good;
        }
      });
  }
}
