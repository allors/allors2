import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import {
  InventoryItem,
  Part,
  Facility,
  SerialisedInventoryItem,
  SerialisedItem,
  NonSerialisedInventoryItem,
  PurchaseOrderItem,
  SupplierOffering,
  Product,
  RequestItemState,
  RequestState,
  QuoteItemState,
  QuoteState,
  SalesOrderItemState,
  SalesOrderState,
  ShipmentItemState,
  ShipmentState,
  SalesOrderItem,
  SerialisedItemAvailability,
  Shipment,
  ShipmentItem,
  OrderShipment,
  PurchaseOrderState,
  Good,
} from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { Filters } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort, And, ContainedIn, Extent } from '@allors/data/system';
import { TestScope, SearchFactory } from '@allors/angular/core';

@Component({
  templateUrl: './shipmentitem-edit.component.html',
  providers: [ContextService],
})
export class ShipmentItemEditComponent extends TestScope implements OnInit, OnDestroy {
  readonly m: Meta;

  title: string;
  shipment: Shipment;
  shipmentItem: ShipmentItem;
  inventoryItems: InventoryItem[];
  serialisedInventoryItem: SerialisedInventoryItem;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  serialisedItems: SerialisedItem[] = [];
  sold: SerialisedItemAvailability;
  orderShipment: OrderShipment;
  salesOrderItems: SalesOrderItem[] = [];
  selectedSalesOrderItem: SalesOrderItem;
  selectedPurchaseOrderItem: PurchaseOrderItem;
  purchaseOrderItems: PurchaseOrderItem[] = [];
  supplierOfferings: SupplierOffering[];
  isCustomerShipment: boolean;
  isPurchaseShipment: boolean;
  isSerialized: boolean;
  supplierPartsFilter: SearchFactory;
  serialisedItemAvailabilities: SerialisedItemAvailability[];
  selectedFacility: Facility;
  addFacility = false;
  facilities: Facility[];
  goodIsSelected = false;
  partIsSelected = false;

  draftRequestItem: RequestItemState;
  submittedRequestItem: RequestItemState;
  anonymousRequest: RequestState;
  submittedRequest: RequestState;
  pendingCustomerRequest: RequestState;
  draftQuoteItem: QuoteItemState;
  submittedQuoteItem: QuoteItemState;
  approvedQuoteItem: QuoteItemState;
  awaitingApprovalQuoteItem: QuoteItemState;
  awaitingAcceptanceQuoteItem: QuoteItemState;
  acceptedQuoteItem: QuoteItemState;
  createdQuote: QuoteState;
  approvedQuote: QuoteState;
  acceptedQuote: QuoteState;
  awaitingAcceptanceQuote: QuoteState;
  provisionalOrderItem: SalesOrderItemState;
  requestsApprovalOrderItem: SalesOrderItemState;
  readyForPostingOrderItem: SalesOrderItemState;
  awaitingAcceptanceOrderItem: SalesOrderItemState;
  onHoldOrderItem: SalesOrderItemState;
  inProcessOrderItem: SalesOrderItemState;
  provisionalOrder: SalesOrderState;
  readyForPostingOrder: SalesOrderState;
  requestsApprovalOrder: SalesOrderState;
  awaitingAcceptanceOrder: SalesOrderState;
  inProcessOrder: SalesOrderState;
  onHoldOrder: SalesOrderState;
  createdShipmentItem: ShipmentItemState;
  pickingShipmentItem: ShipmentItemState;
  pickedShipmentItem: ShipmentItemState;
  packedShipmentItem: ShipmentItemState;
  createdShipment: ShipmentState;
  pickingShipment: ShipmentState;
  pickedShipment: ShipmentState;
  packedShipment: ShipmentState;
  onholdShipment: ShipmentState;

  private previousGood;
  private previousPart;
  private subscription: Subscription;

  goodsFilter: SearchFactory;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
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
            pull.SerialisedItemAvailability(),
            pull.SerialisedInventoryItemState({
              predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
              sort: new Sort(m.SerialisedInventoryItemState.Name),
            }),
            pull.RequestItemState(),
            pull.RequestState(),
            pull.QuoteItemState(),
            pull.QuoteState(),
            pull.SalesOrderItemState(),
            pull.SalesOrderState(),
            pull.PurchaseOrderItemState(),
            pull.PurchaseOrderState(),
            pull.ShipmentItemState(),
            pull.ShipmentState(),
            pull.Facility(),
          ];

          if (!isCreate) {
            pulls.push(
              pull.ShipmentItem({
                object: this.data.id,
                include: {
                  SyncedShipment: x,
                  Good: {
                    UnifiedGood_InventoryItemKind: x,
                  },
                  Part: {
                    InventoryItemKind: x,
                  },
                  SerialisedItem: x,
                  ShipmentItemState: x,
                  StoredInFacility: x,
                },
              }),
              pull.ShipmentItem({
                object: this.data.id,
                fetch: {
                  OrderShipmentsWhereShipmentItem: {
                    include: {
                      OrderItem: x,
                    },
                  },
                },
              }),
            );
          }

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.Shipment({
                object: this.data.associationId,
                include: {
                  ShipToAddress: x,
                },
              }),
              pull.Shipment({
                object: this.data.associationId,
                fetch: {
                  ShipToAddress: {
                    SalesOrderItemsWhereDerivedShipToAddress: {
                      include: {
                        Product: x,
                        SerialisedItem: x,
                        SalesOrderWhereSalesOrderItem: {
                          SalesOrderState: x,
                        },
                      },
                    },
                  },
                },
              }),
              pull.Shipment({
                object: this.data.associationId,
                fetch: {
                  ShipFromParty: {
                    PurchaseOrdersWhereTakenViaSupplier: {
                      include: {
                        PurchaseOrderItems: {
                          Part: x,
                          SerialisedItem: x,
                          PurchaseOrderItemState: x,
                          PurchaseOrderWherePurchaseOrderItem: {
                            PurchaseOrderState: x,
                          },
                        },
                      },
                    },
                  },
                },
              }),
            );
          }

          this.goodsFilter = Filters.goodsFilter(m);

          return this.allors.context.load(new PullRequest({ pulls })).pipe(map((loaded) => ({ loaded, isCreate })));
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.shipmentItem = loaded.objects.ShipmentItem as ShipmentItem;
        this.shipment = (loaded.objects.Shipment as Shipment) || this.shipmentItem.SyncedShipment;
        this.isCustomerShipment = this.shipment.objectType === this.metaService.m.CustomerShipment;
        this.isPurchaseShipment = this.shipment.objectType === this.metaService.m.PurchaseShipment;

        this.facilities = loaded.collections.Facilities as Facility[];
        this.serialisedItemAvailabilities = loaded.collections.SerialisedItemAvailabilities as SerialisedItemAvailability[];
        this.sold = this.serialisedItemAvailabilities.find(
          (v: SerialisedItemAvailability) => v.UniqueId === '9bdc0a55-4e3c-4604-b054-2441a551aa1c'
        );

        const salesOrderStates = loaded.collections.SalesOrderStates as SalesOrderState[];
        const inProcess = salesOrderStates.find((v) => v.UniqueId === 'ddbb678e-9a66-4842-87fd-4e628cff0a75');

        this.provisionalOrder = salesOrderStates.find((v: SalesOrderState) => v.UniqueId === '29abc67d-4be1-4af3-b993-64e9e36c3e6b');
        this.readyForPostingOrder = salesOrderStates.find((v: SalesOrderState) => v.UniqueId === 'e8e7c70b-e920-4f70-96d4-a689518f602c');
        this.requestsApprovalOrder = salesOrderStates.find((v: SalesOrderState) => v.UniqueId === '6b6f6e25-4da1-455d-9c9f-21f2d4316d66');
        this.awaitingAcceptanceOrder = salesOrderStates.find((v: SalesOrderState) => v.UniqueId === '37d344e7-5962-425c-86a9-24bf1e098448');
        this.inProcessOrder = salesOrderStates.find((v: SalesOrderState) => v.UniqueId === 'ddbb678e-9a66-4842-87fd-4e628cff0a75');
        this.onHoldOrder = salesOrderStates.find((v: SalesOrderState) => v.UniqueId === 'f625fb7e-893e-4f68-ab7b-2bc29a644e5b');

        const salesOrderItemStates = loaded.collections.SalesOrderItemStates as SalesOrderItemState[];
        this.provisionalOrderItem = salesOrderItemStates.find(
          (v: SalesOrderItemState) => v.UniqueId === '5b0993b5-5784-4e8d-b1ad-93affac9a913'
        );
        this.readyForPostingOrderItem = salesOrderItemStates.find(
          (v: SalesOrderItemState) => v.UniqueId === '6e4f9535-a7ce-483f-9fbd-c9fd331d355e'
        );
        this.requestsApprovalOrderItem = salesOrderItemStates.find(
          (v: SalesOrderItemState) => v.UniqueId === '8d3a4a0a-ed27-4478-baff-ece591068712'
        );
        this.awaitingAcceptanceOrderItem = salesOrderItemStates.find(
          (v: SalesOrderItemState) => v.UniqueId === 'd3965e9b-764d-4787-87b4-82cb2acb0878'
        );
        this.inProcessOrderItem = salesOrderItemStates.find(
          (v: SalesOrderItemState) => v.UniqueId === 'e08401f7-1deb-4b27-b0c5-8f034bffedb5'
        );
        this.onHoldOrderItem = salesOrderItemStates.find((v: SalesOrderItemState) => v.UniqueId === '3b185d51-af4a-441e-be0d-f91cfcbdb5d8');

        const requestItemStates = loaded.collections.RequestItemStates as RequestItemState[];
        this.draftRequestItem = requestItemStates.find((v: RequestItemState) => v.UniqueId === 'b173dfbe-9421-4697-8ffb-e46afc724490');
        this.submittedRequestItem = requestItemStates.find((v: RequestItemState) => v.UniqueId === 'b118c185-de34-4131-be1f-e6162c1dea4b');

        const requestStates = loaded.collections.RequestStates as RequestState[];
        this.anonymousRequest = requestStates.find((v: RequestState) => v.UniqueId === '2f054949-e30c-4954-9a3c-191559de8315');
        this.submittedRequest = requestStates.find((v: RequestState) => v.UniqueId === 'db03407d-bcb1-433a-b4e9-26cea9a71bfd');
        this.pendingCustomerRequest = requestStates.find((v: RequestState) => v.UniqueId === '671fda2f-5aa6-4ea5-b5d6-c914f0911690');

        const quoteItemStates = loaded.collections.QuoteItemStates as QuoteItemState[];
        this.draftQuoteItem = quoteItemStates.find((v: QuoteItemState) => v.UniqueId === '84ad17a3-10f7-4fdb-b76a-41bdb1edb0e6');
        this.submittedQuoteItem = quoteItemStates.find((v: QuoteItemState) => v.UniqueId === 'e511ea2d-6eb9-428d-a982-b097938a8ff8');
        this.approvedQuoteItem = quoteItemStates.find((v: QuoteItemState) => v.UniqueId === '3335810c-9e26-4604-b272-d18b831e79e0');
        this.awaitingApprovalQuoteItem = quoteItemStates.find((v: QuoteItemState) => v.UniqueId === '76155bb7-53a3-4175-b026-74274a337820');
        this.awaitingAcceptanceQuoteItem = quoteItemStates.find(
          (v: QuoteItemState) => v.UniqueId === 'e0982b61-deb1-47cb-851b-c182f03326a1'
        );
        this.acceptedQuoteItem = quoteItemStates.find((v: QuoteItemState) => v.UniqueId === '6e56c9f1-7bea-4ced-a724-67e4221a5993');

        const quoteStates = loaded.collections.QuoteStates as QuoteState[];
        this.createdQuote = quoteStates.find((v: QuoteState) => v.UniqueId === 'b1565cd4-d01a-4623-bf19-8c816df96aa6');
        this.approvedQuote = quoteStates.find((v: QuoteState) => v.UniqueId === '675d6899-1ebb-4fdb-9dc9-b8aef0a135d2');
        this.awaitingAcceptanceQuote = quoteStates.find((v: QuoteState) => v.UniqueId === '324beb70-937f-4c4d-a7e9-2e3063c88a62');
        this.acceptedQuote = quoteStates.find((v: QuoteState) => v.UniqueId === '3943f87c-f098-49c8-89ba-12047c826777');

        const shipmentItemStates = loaded.collections.ShipmentItemStates as ShipmentItemState[];
        this.createdShipmentItem = shipmentItemStates.find((v: ShipmentItemState) => v.UniqueId === 'e05818b1-2660-4879-b5a8-8ca96f324f7b');
        this.pickingShipmentItem = shipmentItemStates.find((v: ShipmentItemState) => v.UniqueId === 'f9043add-e106-4646-8b02-6b10efbb2e87');
        this.pickedShipmentItem = shipmentItemStates.find((v: ShipmentItemState) => v.UniqueId === 'a8e2014f-c4cb-4a6f-8ccf-0875e439d1f3');
        this.packedShipmentItem = shipmentItemStates.find((v: ShipmentItemState) => v.UniqueId === '91853258-c875-4f85-bd84-ef1ebd2e5930');

        const shipmentStates = loaded.collections.ShipmentStates as ShipmentState[];
        this.createdShipment = shipmentStates.find((v: ShipmentState) => v.UniqueId === '854ad6a0-b2d1-4b92-8c3d-e9e72dd19afd');
        this.pickingShipment = shipmentStates.find((v: ShipmentState) => v.UniqueId === '1d76de65-4de4-494d-8677-653b4d62aa42');
        this.pickedShipment = shipmentStates.find((v: ShipmentState) => v.UniqueId === 'c63c5d25-f139-490f-86d1-2e9e51f5c0a5');
        this.packedShipment = shipmentStates.find((v: ShipmentState) => v.UniqueId === 'dcabe845-a6f2-49d9-bbae-06fb47012a21');
        this.onholdShipment = shipmentStates.find((v: ShipmentState) => v.UniqueId === '268cb9a7-6965-47e8-af89-8f915242c23d');

        const purchaseOrderStates = loaded.collections.PurchaseOrderStates as PurchaseOrderState[];
        const purchaseOrderinProcess = purchaseOrderStates.find((v) => v.UniqueId === '7752f5c5-b19b-4339-a937-0bad768142a8');

        const salesOrderItems = loaded.collections.SalesOrderItems as SalesOrderItem[];
        if (salesOrderItems) {
          this.salesOrderItems = salesOrderItems.filter(
            (v) => v.SalesOrderWhereSalesOrderItem.SalesOrderState === inProcess && parseFloat(v.QuantityRequestsShipping) > 0
          );
        }

        const purchaseOrderItems = loaded.collections.PurchaseOrderItems as PurchaseOrderItem[];
        if (purchaseOrderItems) {
          this.purchaseOrderItems = purchaseOrderItems.filter(
            (v) => v.PurchaseOrderWherePurchaseOrderItem.PurchaseOrderState === purchaseOrderinProcess
          );
        }

        if (this.isPurchaseShipment) {
          this.supplierPartsFilter = new SearchFactory({
            objectType: this.m.Part,
            roleTypes: [this.m.Part.Name, this.m.Part.SearchString],
            post: (predicate: And) => {
              predicate.operands.push(
                new ContainedIn({
                  propertyType: this.m.Part.SupplierOfferingsWherePart,
                  extent: new Extent({
                    objectType: this.m.SupplierOffering,
                    predicate: new Equals({ propertyType: m.SupplierOffering.Supplier, object: this.shipment.ShipFromParty }),
                  }),
                })
              );
            },
          });
        }

        if (isCreate) {
          this.title = 'Add Shipment Item';
          this.shipmentItem = this.allors.context.create('ShipmentItem') as ShipmentItem;
          this.selectedFacility = this.shipment.ShipToFacility;

          this.shipment.AddShipmentItem(this.shipmentItem);
        } else {
          // This UI does not allow shipmentitem to be combination from multiple order items
          const orderShipments = loaded.collections.OrderShipments as OrderShipment[];
          if (orderShipments && orderShipments.length > 0) {
            this.orderShipment = orderShipments[0];
            if (this.orderShipment.OrderItem) {
              if (this.isCustomerShipment) {
                this.selectedSalesOrderItem = this.orderShipment.OrderItem as SalesOrderItem;
                this.salesOrderItems.push(this.selectedSalesOrderItem);
              }

              if (this.isPurchaseShipment) {
                this.selectedPurchaseOrderItem = this.orderShipment.OrderItem as PurchaseOrderItem;
                this.purchaseOrderItems.push(this.selectedPurchaseOrderItem);
              }
            }
          }

          if (this.shipmentItem.Good) {
            this.goodIsSelected = true;
            this.partIsSelected = false;
            this.previousGood = this.shipmentItem.Good;
            this.loadProduct(this.shipmentItem.Good);
          }

          if (this.shipmentItem.Part) {
            this.partIsSelected = true;
            this.goodIsSelected = false;
            this.previousPart = this.shipmentItem.Part;
            this.loadPart(this.shipmentItem.Part);
          }

          this.selectedFacility = this.shipmentItem.StoredInFacility;
          this.serialisedItems.push(this.shipmentItem.SerialisedItem);

          if (this.shipmentItem.CanWriteQuantity) {
            this.title = 'Edit Shipment Item';
          } else {
            this.title = 'View Shipment Item';
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

    this.allors.context.save().subscribe(() => {
      const data: IObject = {
        id: this.shipmentItem.id,
        objectType: this.shipmentItem.objectType,
      };

      this.dialogRef.close(data);
      this.refreshService.refresh();
    }, this.saveService.errorHandler);
  }

  public salesOrderItemSelected(obj: ISessionObject): void {
    if (obj) {
      const salesOrderItem = obj as SalesOrderItem;
      this.shipmentItem.Good = salesOrderItem.Product as Good;
      this.shipmentItem.Quantity = salesOrderItem.QuantityRequestsShipping;
      this.shipmentItem.SerialisedItem = salesOrderItem.SerialisedItem;
    }
  }

  public purchaseOrderItemSelected(obj: ISessionObject): void {
    if (obj) {
      const purchaseOrderItem = obj as PurchaseOrderItem;
      this.shipmentItem.Part = purchaseOrderItem.Part as Part;
      this.shipmentItem.Quantity = purchaseOrderItem.QuantityOrdered;
      this.shipmentItem.SerialisedItem = purchaseOrderItem.SerialisedItem;
    }
  }

  public goodSelected(product: ISessionObject): void {
    this.goodIsSelected = true;
    this.partIsSelected = false;

    if (product) {
      this.loadProduct(product as Product);
    }
  }

  public partSelected(part: ISessionObject): void {
    this.partIsSelected = true;
    this.goodIsSelected = false;

    if (part) {
      this.loadPart(part as Part);
    }
  }

  public serialisedItemSelected(obj: ISessionObject): void {
    if (obj) {
      const serialisedItem = obj as SerialisedItem;
      const onRequestItem = serialisedItem.RequestItemsWhereSerialisedItem.find(
        (v) =>
          (v.RequestItemState === this.draftRequestItem || v.RequestItemState === this.submittedRequestItem) &&
          (v.RequestWhereRequestItem.RequestState === this.anonymousRequest ||
            v.RequestWhereRequestItem.RequestState === this.submittedRequest ||
            v.RequestWhereRequestItem.RequestState === this.pendingCustomerRequest)
      );

      const onQuoteItem = serialisedItem.QuoteItemsWhereSerialisedItem.find(
        (v) =>
          (v.QuoteItemState === this.draftQuoteItem ||
            v.QuoteItemState === this.submittedQuoteItem ||
            v.QuoteItemState === this.approvedQuoteItem ||
            v.QuoteItemState === this.awaitingApprovalQuoteItem ||
            v.QuoteItemState === this.awaitingAcceptanceQuoteItem ||
            v.QuoteItemState === this.acceptedQuoteItem) &&
          (v.QuoteWhereQuoteItem.QuoteState === this.createdQuote ||
            v.QuoteWhereQuoteItem.QuoteState === this.approvedQuote ||
            v.QuoteWhereQuoteItem.QuoteState === this.awaitingAcceptanceQuote ||
            v.QuoteWhereQuoteItem.QuoteState === this.acceptedQuote)
      );

      const onOrderItem = serialisedItem.SalesOrderItemsWhereSerialisedItem.find(
        (v) =>
          (v.SalesOrderItemState === this.provisionalOrderItem ||
            v.SalesOrderItemState === this.readyForPostingOrderItem ||
            v.SalesOrderItemState === this.requestsApprovalOrderItem ||
            v.SalesOrderItemState === this.awaitingAcceptanceOrderItem ||
            v.SalesOrderItemState === this.onHoldOrderItem ||
            v.SalesOrderItemState === this.inProcessOrderItem) &&
          (v.SalesOrderWhereSalesOrderItem.SalesOrderState === this.provisionalOrder ||
            v.SalesOrderWhereSalesOrderItem.SalesOrderState === this.readyForPostingOrder ||
            v.SalesOrderWhereSalesOrderItem.SalesOrderState === this.requestsApprovalOrder ||
            v.SalesOrderWhereSalesOrderItem.SalesOrderState === this.awaitingAcceptanceOrder ||
            v.SalesOrderWhereSalesOrderItem.SalesOrderState === this.onHoldOrder ||
            v.SalesOrderWhereSalesOrderItem.SalesOrderState === this.inProcessOrder)
      );

      const onOtherShipmentItem = serialisedItem.ShipmentItemsWhereSerialisedItem.find(
        (v) =>
          (v.ShipmentItemState === this.createdShipmentItem ||
            v.ShipmentItemState === this.pickingShipmentItem ||
            v.ShipmentItemState === this.pickedShipmentItem ||
            v.ShipmentItemState === this.packedShipmentItem) &&
          (v.ShipmentWhereShipmentItem?.ShipmentState === this.createdShipment ||
            v.ShipmentWhereShipmentItem?.ShipmentState === this.pickingShipment ||
            v.ShipmentWhereShipmentItem?.ShipmentState === this.pickingShipment ||
            v.ShipmentWhereShipmentItem?.ShipmentState === this.packedShipment ||
            v.ShipmentWhereShipmentItem?.ShipmentState === this.onholdShipment)
      );

      if (onRequestItem) {
        this.snackBar.open(`Item already requested with ${onRequestItem.RequestWhereRequestItem.RequestNumber}`, 'close');
      }

      if (onQuoteItem) {
        this.snackBar.open(`Item already quoted with ${onQuoteItem.QuoteWhereQuoteItem.QuoteNumber}`, 'close');
      }

      if (onOrderItem) {
        this.snackBar.open(`Item already ordered with ${onOrderItem.SalesOrderWhereSalesOrderItem.OrderNumber}`, 'close');
      }

      if (onOtherShipmentItem) {
        this.snackBar.open(`Item already shipped with ${onOtherShipmentItem.ShipmentWhereShipmentItem.ShipmentNumber}`, 'close');
      }

      this.shipmentItem.Quantity = '1';
    }
  }

  public facilityAdded(facility: Facility): void {
    this.facilities.push(facility);
    this.selectedFacility = facility;

    this.allors.context.session.hasChanges = true;
  }

  private loadProduct(product: Product): void {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.NonUnifiedGood({
        object: product.id,
        fetch: {
          Part: {
            include: {
              InventoryItemKind: x,
              SerialisedItems: {
                RequestItemsWhereSerialisedItem: {
                  RequestItemState: x,
                  RequestWhereRequestItem: {
                    RequestState: x,
                  },
                },
                QuoteItemsWhereSerialisedItem: {
                  QuoteItemState: x,
                  QuoteWhereQuoteItem: {
                    QuoteState: x,
                  },
                },
                SalesOrderItemsWhereSerialisedItem: {
                  SalesOrderItemState: x,
                  SalesOrderWhereSalesOrderItem: {
                    SalesOrderState: x,
                  },
                },
                ShipmentItemsWhereSerialisedItem: {
                  ShipmentItemState: x,
                  ShipmentWhereShipmentItem: {
                    ShipmentState: x,
                  },
                },
              },
            },
          },
        },
      }),
      pull.UnifiedGood({
        object: product.id,
        include: {
          InventoryItemKind: x,
          SerialisedItems: {
            SerialisedInventoryItemsWhereSerialisedItem: x,
            RequestItemsWhereSerialisedItem: {
              RequestItemState: x,
              RequestWhereRequestItem: {
                RequestState: x,
              },
            },
            QuoteItemsWhereSerialisedItem: {
              QuoteItemState: x,
              QuoteWhereQuoteItem: {
                QuoteState: x,
              },
            },
            SalesOrderItemsWhereSerialisedItem: {
              SalesOrderItemState: x,
              SalesOrderWhereSalesOrderItem: {
                SalesOrderState: x,
              },
            },
            ShipmentItemsWhereSerialisedItem: {
              ShipmentItemState: x,
              ShipmentWhereShipmentItem: {
                ShipmentState: x,
              },
            },
          },
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      const part = (loaded.objects.UnifiedGood || loaded.objects.Part) as Part;

      this.isSerialized = part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';

      if (this.isCustomerShipment) {
        this.serialisedItems = part.SerialisedItems.filter((v) => v.AvailableForSale === true);
      } else {
        this.serialisedItems = part.SerialisedItems.filter((v) => v.SerialisedInventoryItemsWhereSerialisedItem.length === 0);
      }

      if (this.shipmentItem.Good !== this.previousGood) {
        this.shipmentItem.SerialisedItem = null;
        this.previousGood = this.shipmentItem.Good;
      }
    });
  }

  private loadPart(part: Part): void {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.NonUnifiedPart({
        object: part.id,
        fetch: {
          InventoryItemsWherePart: {
            include: {
              Facility: x,
            },
          },
        },
      }),
      pull.NonUnifiedPart({
        object: part.id,
        include: {
          InventoryItemKind: x,
        },
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe((loaded) => {
      this.isSerialized = part.InventoryItemKind.UniqueId === '2596e2dd-3f5d-4588-a4a2-167d6fbe3fae';
      this.inventoryItems = loaded.collections.InventoryItems as InventoryItem[];

      if (this.shipmentItem.Part !== this.previousPart) {
        this.shipmentItem.SerialisedItem = null;
        this.previousPart = this.shipmentItem.Part;
      }
    });
  }

  private onSave() {
    if (this.selectedSalesOrderItem) {
      if (this.orderShipment === undefined) {
        this.orderShipment = this.allors.context.create('OrderShipment') as OrderShipment;
      }

      this.orderShipment.OrderItem = this.selectedSalesOrderItem;
      this.orderShipment.ShipmentItem = this.shipmentItem;
      this.orderShipment.Quantity = this.shipmentItem.Quantity;
    }

    if (this.isPurchaseShipment) {
      this.shipmentItem.StoredInFacility = this.selectedFacility;
    }
  }
}
