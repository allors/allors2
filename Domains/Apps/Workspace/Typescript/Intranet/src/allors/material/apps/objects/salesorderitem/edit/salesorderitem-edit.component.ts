import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { NonUnifiedGood, InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, Product, QuoteItem, SalesOrder, SalesOrderItem, SerialisedInventoryItem, VatRate, VatRegime, SerialisedItemState, SerialisedItem, Part } from '../../../../../domain';
import { Equals, PullRequest, Sort, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { CreateData, SaveService, FiltersService } from '../../../../../material';

@Component({
  templateUrl: './salesorderitem-edit.component.html',
  providers: [ContextService]
})
export class SalesOrderItemEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  order: SalesOrder;
  orderItem: SalesOrderItem;
  quoteItem: QuoteItem;
  goods: NonUnifiedGood[];
  vatRates: VatRate[];
  vatRegimes: VatRegime[];
  discount: number;
  surcharge: number;
  inventoryItems: InventoryItem[];
  serialisedInventoryItem: SerialisedInventoryItem;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  serialisedItemStates: SerialisedItemState[];
  invoiceItemTypes: InvoiceItemType[];
  productItemType: InvoiceItemType;
  part: Part;
  serialisedItem: SerialisedItem;
  serialisedItems: SerialisedItem[] = [];

  private previousProduct;
  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public filtersService: FiltersService,
    public dialogRef: MatDialogRef<SalesOrderItemEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            pull.SalesOrderItem({
              object: this.data.id,
              include: {
                SalesOrderItemState: x,
                SalesOrderItemShipmentState: x,
                SalesOrderItemInvoiceState: x,
                SalesOrderItemPaymentState: x,
                ReservedFromNonSerialisedInventoryItem: x,
                ReservedFromSerialisedInventoryItem: x,
                NewSerialisedItemState: x,
                Product: x,
                SerialisedItem: x,
                QuoteItem: x,
                DiscountAdjustment: x,
                SurchargeAdjustment: x,
                VatRate: x,
                VatRegime: {
                  VatRate: x,
                }
              }
            }),
            pull.SalesOrderItem({
              object: this.data.id,
              fetch: {
                SalesOrderWhereSalesOrderItem: {
                  include: {
                    VatRegime: x
                  }
                }
              }
            }),
            pull.VatRate(),
            pull.VatRegime(),
            pull.SerialisedItemState(),
            pull.NonUnifiedGood({ sort: new Sort(m.NonUnifiedGood.Name) }),
            pull.InvoiceItemType({
              predicate: new Equals({ propertyType: m.InvoiceItemType.IsActive, value: true }),
              sort: new Sort(m.InvoiceItemType.Name),
            }),
            pull.SerialisedInventoryItemState(
              {
                predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
                sort: new Sort(m.SerialisedInventoryItemState.Name),
              }
            ),
          ];

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.SalesOrder({
                object: this.data.associationId,
                include: {
                  VatRegime: x
                }
              })
            );
          }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.order = loaded.objects.SalesOrder as SalesOrder;
        this.quoteItem = loaded.objects.QuoteItem as QuoteItem;
        this.goods = loaded.collections.NonUnifiedGoods as NonUnifiedGood[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.serialisedItemStates = loaded.collections.SerialisedItemStates as SerialisedItemState[];
        this.invoiceItemTypes = loaded.collections.InvoiceItemTypes as InvoiceItemType[];
        this.productItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId.toUpperCase() === '0D07F778-2735-44CB-8354-FB887ADA42AD');

        if (isCreate) {
          this.title = 'Add Order Item';
          this.orderItem = this.allors.context.create('SalesOrderItem') as SalesOrderItem;
          this.order.AddSalesOrderItem(this.orderItem);

        } else {
          this.orderItem = loaded.objects.SalesOrderItem as SalesOrderItem;

          if (this.orderItem.Product) {
            this.previousProduct = this.orderItem.Product;

            if (this.orderItem.InvoiceItemType === this.productItemType) {
              this.refreshSerialisedItems(this.orderItem.Product);
            }
          } else {
            this.serialisedItems.push(this.orderItem.SerialisedItem);
          }

          if (this.orderItem.DiscountAdjustment) {
            this.discount = this.orderItem.DiscountAdjustment.Amount;
          }

          if (this.orderItem.SurchargeAdjustment) {
            this.surcharge = this.orderItem.SurchargeAdjustment.Amount;
          }

          if (this.orderItem.CanWriteAssignedUnitPrice) {
            this.title = 'Edit Sales Order Item';
          } else {
            this.title = 'View Sales Order Item';
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
          id: this.orderItem.id,
          objectType: this.orderItem.objectType,
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
  }

  private onSave() {

    if (this.orderItem.InvoiceItemType !== this.productItemType) {
      this.orderItem.QuantityOrdered = 1;
    }
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
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        this.part = (loaded.objects.UnifiedGood || loaded.objects.Part) as Part;
        this.serialisedItems = this.part.SerialisedItems.filter(v => v.AvailableForSale === true);

        if (this.orderItem.Product !== this.previousProduct) {
          this.orderItem.SerialisedItem = null;
          this.serialisedItem = null;
          this.previousProduct = this.orderItem.Product;
        }

      });
  }
}
