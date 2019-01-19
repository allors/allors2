import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Router } from '@angular/router';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Good, InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, Product, QuoteItem, SalesOrder, SalesOrderItem, SerialisedInventoryItem, VatRate, VatRegime, SerialisedItemState, SerialisedItem, Part } from '../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap, map } from 'rxjs/operators';
import { CreateData, EditData, ObjectData } from '../../../../../material/base/services/object';
@Component({
  templateUrl: './salesorderitem-edit.component.html',
  providers: [ContextService]
})
export class SalesOrderItemEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;
  subTitle: string;
  order: SalesOrder;
  orderItem: SalesOrderItem;
  quoteItem: QuoteItem;
  goods: Good[];
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
  serialisedItem: SerialisedItem;
  serialisedItems: SerialisedItem[] = [];

  private previousProduct;
  private subscription: Subscription;
  part: Part;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<SalesOrderItemEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar,
    public stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;

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
                QuoteItem: x,
                DiscountAdjustment: x,
                SurchargeAdjustment: x,
                DerivedVatRate: x,
                VatRegime: {
                  VatRate: x,
                }
              }
            }),
            pull.VatRate(),
            pull.VatRegime(),
            pull.SerialisedItemState(),
            pull.Good({ sort: new Sort(m.Good.Name) }),
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
                object: this.data.associationId
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
        this.goods = loaded.collections.Goods as Good[];
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

          this.previousProduct = this.orderItem.Product;
          this.serialisedItem = this.orderItem.SerialisedItem;

          if (this.orderItem.InvoiceItemType === this.productItemType) {
            this.refreshSerialisedItems(this.orderItem.Product);
          }

          if (this.orderItem.DiscountAdjustment) {
            this.discount = this.orderItem.DiscountAdjustment.Amount;
          }

          if (this.orderItem.SurchargeAdjustment) {
            this.surcharge = this.orderItem.SurchargeAdjustment.Amount;
          }

          if (this.orderItem.CanWriteActualUnitPrice) {
            this.title = 'Edit Sales Order Item';
          } else {
            this.title = 'View Sales Order Item';
          }
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goodSelected(object: any) {
    if (object) {
      this.refreshSerialisedItems(object as Product);
    }
  }

  public serialisedItemSelected(serialisedItem: SerialisedItem): void {
    this.serialisedItem = this.part.SerialisedItems.find(v => v === serialisedItem);
  }

  public save(): void {

    this.onSave();

    this.allors.context.save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.orderItem.id,
          objectType: this.orderItem.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  private onSave() {

    if (this.orderItem.InvoiceItemType !== this.productItemType) {
      this.orderItem.QuantityOrdered = 1;
    }
  }

  private refreshSerialisedItems(good: Product): void {

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Good({
        object: good.id,
        fetch: {
          Part: {
            include: {
              SerialisedItems: x
            }
          }
        }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.part = loaded.objects.Part as Part;
        this.serialisedItems = this.part.SerialisedItems.filter(v => v.AvailableForSale === true);

        if (this.orderItem.Product !== this.previousProduct) {
          this.orderItem.SerialisedItem = null;
          this.serialisedItem = null;
          this.previousProduct = this.orderItem.Product;
        }

      }, this.errorService.handler);
  }
}
