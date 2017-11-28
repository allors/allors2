import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Filter, Loaded, Saved, Scope } from "@allors";
import { Fetch, Path, PullRequest, Query, TreeNode } from "@allors";
import { Good, InventoryItem, NonSerialisedInventoryItem, Product, SalesInvoice, SalesInvoiceItem, SalesInvoiceItemType, SalesOrderItem, SerialisedInventoryItem } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  templateUrl: "./invoiceitem.component.html",
})
export class InvoiceItemEditComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Edit Sales Invoice Item";
  public subTitle: string;
  public invoice: SalesInvoice;
  public invoiceItem: SalesInvoiceItem;
  public orderItem: SalesOrderItem;
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  public goods: Good[];
  public salesInvoiceItemTypes: SalesInvoiceItemType[];
  public productItemType: SalesInvoiceItemType;

  public goodsFilter: Filter;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.m = this.allorsService.meta;
    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.goodsFilter = new Filter({scope: this.scope, objectType: this.m.Good, roleTypes: [this.m.Good.Name]});
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const itemId: string = this.route.snapshot.paramMap.get("itemId");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            name: "salesInvoice",
          }),
          new Fetch({
            id: itemId,
            include: [
              new TreeNode({ roleType: m.SalesInvoiceItem.SalesInvoiceItemState }),
              new TreeNode({ roleType: m.SalesInvoiceItem.SalesOrderItem }),
            ],
            name: "invoiceItem",
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
            }),
          new Query(
            {
              name: "salesInvoiceItemTypes",
              objectType: m.SalesInvoiceItemType,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.invoiceItem = loaded.objects.invoiceItem as SalesInvoiceItem;
        this.orderItem = loaded.objects.orderItem as SalesOrderItem;
        this.goods = loaded.collections.goods as Good[];
        this.salesInvoiceItemTypes = loaded.collections.salesInvoiceItemTypes as SalesInvoiceItemType[];
        this.productItemType = this.salesInvoiceItemTypes.find((v: SalesInvoiceItemType) => v.UniqueId.toUpperCase() === "0D07F778-2735-44CB-8354-FB887ADA42AD");

        if (!this.invoiceItem) {
          this.title = "Add invoice Item";
          this.invoiceItem = this.scope.session.create("SalesInvoiceItem") as SalesInvoiceItem;
          this.invoice.AddSalesInvoiceItem(this.invoiceItem);
        } else {
          if (this.invoiceItem.SalesInvoiceItemType === this.productItemType) {
            this.goodSelected(this.invoiceItem.Product);
          }
        }
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goodSelected(product: Product): void {

    this.invoiceItem.SalesInvoiceItemType = this.productItemType;

    const fetch: Fetch[] = [
      new Fetch({
        id: product.id,
        name: "inventoryItem",
        path: new Path({ step: this.m.Good.InventoryItemsWhereGood }),
      }),
    ];

    this.scope
        .load("Pull", new PullRequest({ fetch }))
        .subscribe((loaded: Loaded) => {
          this.inventoryItems = loaded.collections.inventoryItem as InventoryItem[];
          if (this.inventoryItems[0] instanceof SerialisedInventoryItem) {
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

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(["/ar/invoice/" + this.invoice.id]);
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
}
