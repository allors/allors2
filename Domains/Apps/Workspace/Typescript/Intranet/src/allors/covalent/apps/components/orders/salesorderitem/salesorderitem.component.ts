import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Field, Filter, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { Good, InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, Product, QuoteItem, SalesOrder, SalesOrderItem, SerialisedInventoryItem, SerialisedInventoryItemState, VatRate, VatRegime } from "../../../../../domain";
import { Fetch, Path, PullRequest, Query, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./salesorderitem.component.html",
})
export class SalesOrderItemEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Edit Sales Order Item";
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

  public goodsFilter: Filter;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.goodsFilter = new Filter({
      scope: this.scope,
      objectType: this.m.Good,
      roleTypes: [this.m.Good.Name]});
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const itemId: string = this.route.snapshot.paramMap.get("itemId");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            name: "salesOrder",
          }),
          new Fetch({
            id: itemId,
            include: [
              new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemState }),
              new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemShipmentState }),
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
            name: "orderItem",
          }),
        ];

        const query: Query[] = [
          new Query(m.Good),
          new Query(m.VatRate),
          new Query(m.VatRegime),
          new Query(m.InvoiceItemType),
          new Query(m.SerialisedInventoryItemState),
          ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded) => {

        this.order = loaded.objects.salesOrder as SalesOrder;
        this.orderItem = loaded.objects.orderItem as SalesOrderItem;
        this.quoteItem = loaded.objects.quoteItem as QuoteItem;
        this.goods = loaded.collections.GoodQuery as Good[];
        this.vatRates = loaded.collections.VatRateQuery as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimeQuery as VatRegime[];
        this.serialisedInventoryItemStates = loaded.collections.SerialisedInventoryItemStateQuery as SerialisedInventoryItemState[];
        this.invoiceItemTypes = loaded.collections.InvoiceItemTypeQuery as InvoiceItemType[];
        this.productItemType = this.invoiceItemTypes.find((v: InvoiceItemType) => v.UniqueId.toUpperCase() === "0D07F778-2735-44CB-8354-FB887ADA42AD");

        if (!this.orderItem) {
          this.title = "Add Order Item";
          this.orderItem = this.scope.session.create("SalesOrderItem") as SalesOrderItem;
          this.order.AddSalesOrderItem(this.orderItem);
        } else {
          if (this.orderItem.InvoiceItemType === this.productItemType) {
            this.update(this.orderItem.Product);
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
        this.errorService.message(error);
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
      this.update(object as Product);
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
        this.router.navigate(["/orders/salesOrder/" + this.order.id]);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }

  private update(product: Product): void {

    this.orderItem.InvoiceItemType = this.productItemType;

    const fetch: Fetch[] = [
      new Fetch({
        id: product.id,
        name: "inventoryItem",
        path: new Path({ step: this.m.Good.InventoryItemsWhereGood }),
      }),
    ];

    this.scope
        .load("Pull", new PullRequest({ fetch }))
        .subscribe((loaded) => {
          this.inventoryItems = loaded.collections.inventoryItem as InventoryItem[];
          if (this.inventoryItems[0] instanceof SerialisedInventoryItem) {
            this.orderItem.QuantityOrdered = 1;
            this.serialisedInventoryItem = this.inventoryItems[0] as SerialisedInventoryItem;
          }
          if (this.inventoryItems[0] instanceof NonSerialisedInventoryItem) {
            this.nonSerialisedInventoryItem = this.inventoryItems[0] as NonSerialisedInventoryItem;
          }
        },
        (error: Error) => {
          this.errorService.message(error);
          this.goBack();
        },
      );
  }
}
