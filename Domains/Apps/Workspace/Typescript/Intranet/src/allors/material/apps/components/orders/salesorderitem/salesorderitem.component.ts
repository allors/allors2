import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Field, FilterFactory, Invoked, Loaded, Saved, Scope, WorkspaceService } from '../../../../../angular';
import { Good, InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, Product, QuoteItem, SalesOrder, SalesOrderItem, SerialisedInventoryItem, SerialisedInventoryItemState, VatRate, VatRegime } from '../../../../../domain';
import { And, ContainedIn, Equals, Fetch, Path, PullRequest, Query, TreeNode, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { NewGoodDialogComponent } from '../../catalogues';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './salesorderitem.component.html',
})
export class SalesOrderItemEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string;
  public subTitle: string;
  public order: SalesOrder;
  public orderItem: SalesOrderItem;
  public quoteItem: QuoteItem;
  public goods: Good[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];
  public discount: number;
  public surcharge: number;
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  public serialisedInventoryItemStates: SerialisedInventoryItemState[];
  public invoiceItemTypes: InvoiceItemType[];
  public productItemType: InvoiceItemType;

  public scope: Scope;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    public stateService: StateService,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const itemId: string = this.route.snapshot.paramMap.get('itemId');
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            name: 'salesOrder',
          }),
          new Fetch({
            id: itemId,
            include: [
              new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemState }),
              new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemShipmentState }),
              new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemInvoiceState }),
              new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemPaymentState }),
              new TreeNode({ roleType: m.SalesOrderItem.ReservedFromNonSerialisedInventoryItem }),
              new TreeNode({ roleType: m.SalesOrderItem.ReservedFromSerialisedInventoryItem }),
              new TreeNode({ roleType: m.SalesOrderItem.NewSerialisedInventoryItemState }),
              new TreeNode({ roleType: m.SalesOrderItem.QuoteItem }),
              new TreeNode({ roleType: m.SalesOrderItem.DiscountAdjustment }),
              new TreeNode({ roleType: m.SalesOrderItem.SurchargeAdjustment }),
              new TreeNode({ roleType: m.SalesOrderItem.DerivedVatRate }),
              new TreeNode({
                nodes: [new TreeNode({ roleType: m.VatRegime.VatRate })],
                roleType: m.SalesOrderItem.VatRegime,
              }),
            ],
            name: 'orderItem',
          }),
        ];

        const queries: Query[] = [
          new Query(m.VatRate),
          new Query(m.VatRegime),
          new Query(
            {
              name: 'goods',
              objectType: m.Good,
              sort: [
                new Sort({ roleType: m.Good.Name, direction: 'Asc' }),
              ],
            }),
          new Query(
            {
              name: 'invoiceItemTypes',
              objectType: m.InvoiceItemType,
              predicate: new Equals({ roleType: m.InvoiceItemType.IsActive, value: true }),
              sort: [
                new Sort({ roleType: m.InvoiceItemType.Name, direction: 'Asc' }),
              ],
            }),
          new Query(
            {
              name: 'serialisedInventoryItemStates',
              objectType: m.SerialisedInventoryItemState,
              predicate: new Equals({ roleType: m.SerialisedInventoryItemState.IsActive, value: true }),
              sort: [
                new Sort({ roleType: m.SerialisedInventoryItemState.Name, direction: 'Asc' }),
              ],
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {
        this.scope.session.reset();

        this.order = loaded.objects.salesOrder as SalesOrder;
        this.orderItem = loaded.objects.orderItem as SalesOrderItem;
        this.quoteItem = loaded.objects.quoteItem as QuoteItem;
        this.goods = loaded.collections.goods as Good[];
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.serialisedInventoryItemStates = loaded.collections.serialisedInventoryItemStates as SerialisedInventoryItemState[];
        this.invoiceItemTypes = loaded.collections.invoiceItemTypes as InvoiceItemType[];
        this.productItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId.toUpperCase() === '0D07F778-2735-44CB-8354-FB887ADA42AD');

        if (!this.orderItem) {
          this.title = 'Add Order Item';
          this.orderItem = this.scope.session.create('SalesOrderItem') as SalesOrderItem;
          this.order.AddSalesOrderItem(this.orderItem);
        } else {

          if (this.orderItem.CanWriteActualUnitPrice) {
            this.title = 'Edit Sales Order Item';
          } else {
            this.title = 'View Sales Order Item';
          }

          if (this.orderItem.InvoiceItemType === this.productItemType) {
            this.refreshInventory(this.orderItem.Product);
          }

          if (this.orderItem.DiscountAdjustment) {
            this.discount = this.orderItem.DiscountAdjustment.Amount;
          }

          if (this.orderItem.SurchargeAdjustment) {
            this.surcharge = this.orderItem.SurchargeAdjustment.Amount;
          }
        }
      },
        (error: Error) => {
          this.errorService.handle(error);
          this.goBack();
        },
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goodSelected(object: any) {
    if (object) {
      this.refreshInventory(object as Product);
    }
  }

  public save(): void {

    // if (this.discount !== 0) {
    //   const discountAdjustment = this.scope.session.create("DiscountAdjustment") as DiscountAdjustment;
    //   discountAdjustment.Amount = this.discount;
    //   this.orderItem.DiscountAdjustment = discountAdjustment;
    // }

    // if (this.surcharge !== 0) {
    //   const surchargeAdjustment = this.scope.session.create("SurchargeAdjustment") as SurchargeAdjustment;
    //   surchargeAdjustment.Amount = this.surcharge;
    //   this.orderItem.SurchargeAdjustment = surchargeAdjustment;
    // }

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(['/orders/salesOrder/' + this.order.id]);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public update(): void {
    const isNew = this.orderItem.isNew;

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('Successfully saved.', 'close', { duration: 5000 });
        if (isNew) {
          this.router.navigate(['/salesOrder/' + this.order.id + '/item/' + this.orderItem.id]);
        } else {
          this.refresh();
        }
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.orderItem.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
        this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public reject(): void {
    const rejectFn: () => void = () => {
      this.scope.invoke(this.orderItem.Reject)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully reejcted.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
        this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                rejectFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            rejectFn();
          }
        });
    } else {
      rejectFn();
    }
  }

  private refreshInventory(product: Product): void {

    const fetches: Fetch[] = [
      new Fetch({
        id: product.id,
        name: 'inventoryItem',
        path: new Path({ step: this.m.Good.InventoryItemsWhereGood }),
      }),
    ];

    this.scope
      .load('Pull', new PullRequest({ fetches }))
      .subscribe((loaded) => {
        this.inventoryItems = loaded.collections.inventoryItem as InventoryItem[];
        if (this.inventoryItems[0].objectType.name === 'SerialisedInventoryItem') {
          this.orderItem.QuantityOrdered = 1;
          this.serialisedInventoryItem = this.inventoryItems[0] as SerialisedInventoryItem;
        }
        if (this.inventoryItems[0].objectType.name === 'NonSerialisedInventoryItem') {
          this.nonSerialisedInventoryItem = this.inventoryItems[0] as NonSerialisedInventoryItem;
        }
      },
        (error: Error) => {
          this.errorService.handle(error);
          this.goBack();
        },
    );
  }
}
