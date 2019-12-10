import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, TestScope } from '../../../../../angular';
import { NonUnifiedGood, InventoryItem, NonSerialisedInventoryItem, Product, Shipment, ShipmentItem, SerialisedInventoryItem, SerialisedItem, Part, OrderShipment, SalesOrderItem, Good, SalesOrderItemState, SalesOrderState, SalesOrderItemShipmentState, SerialisedItemState } from '../../../../../domain';
import { PullRequest, IObject, Equals, Sort } from '../../../../../framework';
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
  serialisedItemStates: SerialisedItemState[];
  
  private previousGood;
  private subscription: Subscription;
  orderShipments: OrderShipment[];
  salesOrderItems: SalesOrderItem[];
  selectedSalesOrderItem: SalesOrderItem;

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

    const { pull, x, m } = this.metaService;

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
            pull.Shipment({
              object: this.data.associationId,
              include: {
                ShipToAddress: x
              }
            }),
            pull.Shipment({
              object: this.data.associationId,
              fetch: {
                ShipToAddress: {
                  SalesOrderItemsWhereShipToAddress: {
                    include: {
                      Product: x,
                      SerialisedItem: x,
                      SalesOrderWhereSalesOrderItem: {
                        SalesOrderState: x
                      }
                    }
                  }
                }
              }
            }),
            pull.SalesOrderState(),
            pull.SerialisedInventoryItemState(
              {
                predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
                sort: new Sort(m.SerialisedInventoryItemState.Name),
              }
            ),
          ];

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
        this.shipment = loaded.objects.Shipment as Shipment;
        this.serialisedItemStates = loaded.collections.SerialisedItemStates as SerialisedItemState[];

        const salesOrderStates = loaded.collections.SalesOrderStates as SalesOrderState[];
        const inProcess = salesOrderStates.find((v) => v.UniqueId === 'ddbb678e-9a66-4842-87fd-4e628cff0a75');

        const salesOrderItems = loaded.collections.SalesOrderItems as SalesOrderItem[];
        this.salesOrderItems = salesOrderItems.filter(v => v.SalesOrderWhereSalesOrderItem.SalesOrderState === inProcess && parseFloat(v.QuantityRequestsShipping) > 0);

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

    this.onSave();

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

  public salesOrderItemSelected(salesOrderItem: SalesOrderItem): void {

    this.shipmentItem.Good = salesOrderItem.Product as Good;
    this.shipmentItem.Quantity = salesOrderItem.QuantityRequestsShipping;
    this.shipmentItem.SerialisedItem = salesOrderItem.SerialisedItem;
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

  private onSave() {
    if (this.selectedSalesOrderItem) {
      const orderShipment = this.allors.context.create('OrderShipment') as OrderShipment;
      orderShipment.OrderItem = this.selectedSalesOrderItem;
      orderShipment.ShipmentItem = this.shipmentItem;
      orderShipment.Quantity = this.shipmentItem.Quantity;
    }
  }
}
