import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, TestScope } from '../../../../../angular';
import { Product, RequestItem, UnitOfMeasure, Request, Part, SerialisedItem, Good, RequestItemState, RequestState, QuoteItemState, QuoteState, SalesOrderItemState, SalesOrderState } from '../../../../../domain';
import { PullRequest, Sort, Equals, IObject } from '../../../../../framework';
import { ObjectData, SaveService, FiltersService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { MatSnackBar } from '@angular/material';

@Component({
  templateUrl: './requestitem-edit.component.html',
  providers: [ContextService]
})
export class RequestItemEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  title: string;

  request: Request;
  requestItem: RequestItem;
  unitsOfMeasure: UnitOfMeasure[];
  parts: Part[];
  part: Part;
  serialisedItem: SerialisedItem;
  serialisedItems: SerialisedItem[] = [];

  private previousProduct;
  private subscription: Subscription;
  goods: Good[];

  draftRequestItem: RequestItemState;
  submittedRequestItem: RequestItemState;
  anonymousRequest: RequestState;
  submittedRequest: RequestState;
  pendingCustomerRequest: RequestState;
  draftQuoteItem: QuoteItemState;
  submittedQuoteItem: QuoteItemState;
  approvedQuoteItem: QuoteItemState;
  createdQuote: QuoteState;
  approvedQuote: QuoteState;
  provisionalOrderItem: SalesOrderItemState;
  requestsApprovalOrderItem: SalesOrderItemState;
  onHoldOrderItem: SalesOrderItemState;
  inProcessOrderItem: SalesOrderItemState;
  provisionalOrder: SalesOrderState;
  requestsApprovalOrder: SalesOrderState;
  onHoldOrder: SalesOrderState;
  inProcessOrder: SalesOrderState;
  createdOrderItem: SalesOrderItemState;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public filtersService: FiltersService,
    public dialogRef: MatDialogRef<RequestItemEditComponent>,
    public metaService: MetaService,
    private saveService: SaveService,
    public refreshService: RefreshService,
    public snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            pull.RequestItem({
              object: this.data.id,
              include: {
                RequestItemState: x,
                Product: x,
                SerialisedItem: x
              }
            }),
            pull.Good(
              {
                sort: new Sort(m.Good.Name),
              }
            ),
            pull.UnitOfMeasure({
              predicate: new Equals({ propertyType: m.UnitOfMeasure.IsActive, value: true }),
              sort: new Sort(m.UnitOfMeasure.Name)
            }),
            pull.RequestItemState(),
            pull.RequestState(),
            pull.QuoteItemState(),
            pull.QuoteState(),
            pull.SalesOrderItemState(),
            pull.SalesOrderState(),
          ];

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.Request({
                object: this.data.associationId
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

        this.requestItem = loaded.objects.RequestItem as RequestItem;
        this.goods = loaded.collections.Goods as Good[];
        this.unitsOfMeasure = loaded.collections.UnitsOfMeasure as UnitOfMeasure[];
        const piece = this.unitsOfMeasure.find((v: UnitOfMeasure) => v.UniqueId === 'f4bbdb52-3441-4768-92d4-729c6c5d6f1b');

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

        const quoteStates = loaded.collections.QuoteStates as QuoteState[];
        this.createdQuote = quoteStates.find((v: QuoteState) => v.UniqueId === 'b1565cd4-d01a-4623-bf19-8c816df96aa6');
        this.approvedQuote = quoteStates.find((v: QuoteState) => v.UniqueId === '675d6899-1ebb-4fdb-9dc9-b8aef0a135d2');

        const salesOrderItemStates = loaded.collections.SalesOrderItemStates as SalesOrderItemState[];
        this.createdOrderItem = salesOrderItemStates.find((v: SalesOrderItemState) => v.UniqueId === '5b0993b5-5784-4e8d-b1ad-93affac9a913');
        this.onHoldOrderItem = salesOrderItemStates.find((v: SalesOrderItemState) => v.UniqueId === '3b185d51-af4a-441e-be0d-f91cfcbdb5d8');
        this.inProcessOrderItem = salesOrderItemStates.find((v: SalesOrderItemState) => v.UniqueId === 'e08401f7-1deb-4b27-b0c5-8f034bffedb5');

        const salesOrderStates = loaded.collections.SalesOrderStates as SalesOrderState[];
        this.provisionalOrder = salesOrderStates.find((v: SalesOrderState) => v.UniqueId === '29abc67d-4be1-4af3-b993-64e9e36c3e6b');
        this.requestsApprovalOrder = salesOrderStates.find((v: SalesOrderState) => v.UniqueId === '6b6f6e25-4da1-455d-9c9f-21f2d4316d66');
        this.onHoldOrder = salesOrderStates.find((v: SalesOrderState) => v.UniqueId === 'f625fb7e-893e-4f68-ab7b-2bc29a644e5b');
        this.inProcessOrder = salesOrderStates.find((v: SalesOrderState) => v.UniqueId === 'ddbb678e-9a66-4842-87fd-4e628cff0a75');

        if (isCreate) {
          this.title = 'Create Request Item';
          this.request = loaded.objects.Request as Request;
          this.requestItem = this.allors.context.create('RequestItem') as RequestItem;
          this.requestItem.UnitOfMeasure = piece;
          this.request.AddRequestItem(this.requestItem);
        } else {

          if (this.requestItem.CanWriteQuantity) {
            this.title = 'Edit Request Item';
          } else {
            this.title = 'View Request Item';
          }

          if (this.requestItem.Product) {
            this.previousProduct = this.requestItem.Product;
            this.refreshSerialisedItems(this.requestItem.Product);
          } else {
            this.serialisedItems.push(this.requestItem.SerialisedItem);
          }
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goodSelected(product: Product): void {
    if (product) {
      this.refreshSerialisedItems(product);
    }
  }

  public serialisedItemSelected(serialisedItem: SerialisedItem): void {
    const onOtherRequestItem = serialisedItem.RequestItemsWhereSerialisedItem
      .find(v => (v.RequestItemState === this.draftRequestItem || v.RequestItemState === this.submittedRequestItem)
        && (v.RequestWhereRequestItem.RequestState === this.anonymousRequest || v.RequestWhereRequestItem.RequestState === this.submittedRequest || v.RequestWhereRequestItem.RequestState === this.pendingCustomerRequest));

    const onOtherQuoteItem = serialisedItem.QuoteItemsWhereSerialisedItem
      .find(v => (v.QuoteItemState === this.draftQuoteItem || v.QuoteItemState === this.submittedQuoteItem || v.QuoteItemState === this.approvedQuoteItem)
        && (v.QuoteWhereQuoteItem.QuoteState === this.createdQuote || v.QuoteWhereQuoteItem.QuoteState === this.approvedQuote));

    const onOtherOrderItem = serialisedItem.SalesOrderItemsWhereSerialisedItem
      .find(v => (v.SalesOrderItemState === this.createdOrderItem || v.SalesOrderItemState === this.onHoldOrderItem || v.SalesOrderItemState === this.inProcessOrderItem)
        && (v.SalesOrderWhereSalesOrderItem.SalesOrderState === this.provisionalOrder || v.SalesOrderWhereSalesOrderItem.SalesOrderState === this.requestsApprovalOrder) 
            || v.SalesOrderWhereSalesOrderItem.SalesOrderState === this.onHoldOrder || v.SalesOrderWhereSalesOrderItem.SalesOrderState === this.inProcessOrder);

    if (onOtherRequestItem) {
      this.snackBar.open(`Item already requested with ${onOtherRequestItem.RequestWhereRequestItem.RequestNumber}`, 'close');
    }

    if (onOtherQuoteItem) {
      this.snackBar.open(`Item already quoted with ${onOtherQuoteItem.QuoteWhereQuoteItem.QuoteNumber}`, 'close');
    }

    if (onOtherOrderItem) {
      this.snackBar.open(`Item already ordered with ${onOtherOrderItem.SalesOrderWhereSalesOrderItem.OrderNumber}`, 'close');
    }

    this.serialisedItem = this.part.SerialisedItems.find(v => v === serialisedItem);
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe(() => {
        const data: IObject = {
          id: this.requestItem.id,
          objectType: this.requestItem.objectType,
        };

        this.dialogRef.close(data);
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
            SerialisedItems: {
              include: {
                RequestItemsWhereSerialisedItem: {
                  RequestItemState: x,
                  RequestWhereRequestItem: {
                    RequestState: x
                  }
                },
                QuoteItemsWhereSerialisedItem: {
                  QuoteItemState: x,
                  QuoteWhereQuoteItem: {
                    QuoteState: x
                  }
                },
                SalesOrderItemsWhereSerialisedItem: {
                  SalesOrderItemState: x,
                  SalesOrderWhereSalesOrderItem: {
                    SalesOrderState: x
                  }
                }
              }
            }
          }
        }
      }),
      pull.UnifiedGood({
        object: product.id,
        include: {
          SerialisedItems: {
            RequestItemsWhereSerialisedItem: {
              RequestItemState: x,
              RequestWhereRequestItem: {
                RequestState: x
              }
            },
            QuoteItemsWhereSerialisedItem: {
              QuoteItemState: x,
              QuoteWhereQuoteItem: {
                QuoteState: x
              }
            },
            SalesOrderItemsWhereSerialisedItem: {
              SalesOrderItemState: x,
              SalesOrderWhereSalesOrderItem: {
                SalesOrderState: x
              }
            }
          }
        }
      })
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.part = loaded.objects.UnifiedGood as Part;
        if (this.part) {
          if (this.part.SerialisedItems) {
            this.serialisedItems = this.part.SerialisedItems.filter(v => v.AvailableForSale === true);
          } else {
            this.serialisedItems = [];
          }
        } else {
          this.serialisedItems = loaded.collections.SerialisedItems as SerialisedItem[];
        }

        if (this.requestItem.Product !== this.previousProduct) {
          this.requestItem.SerialisedItem = null;
          this.serialisedItem = null;
          this.previousProduct = this.requestItem.Product;
        }
      });
  }
}
