import {AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";
import { ErrorService, FilterFactory, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { Good, InventoryItem, InvoiceItemType, NonSerialisedInventoryItem, Product, PurchaseInvoice, PurchaseInvoiceItem, PurchaseOrderItem, SerialisedInventoryItem, VatRate, VatRegime } from "../../../../../domain";
import { Fetch, Path, PullRequest, Query, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";

@Component({
  templateUrl: "./invoiceitem.component.html",
})
export class InvoiceItemEditComponent
  implements OnInit, OnDestroy {
  public m: MetaDomain;

  public title: string = "Edit Purchase Invoice Item";
  public subTitle: string;
  public invoice: PurchaseInvoice;
  public invoiceItem: PurchaseInvoiceItem;
  public orderItem: PurchaseOrderItem;
  public inventoryItems: InventoryItem[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;
  public goods: Good[];
  public invoiceItemTypes: InvoiceItemType[];
  public productItemType: InvoiceItemType;

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
    public media: TdMediaService,
    public stateService: StateService,
    private changeDetectorRef: ChangeDetectorRef,
  ) {
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {
        const id: string = this.route.snapshot.paramMap.get("id");
        const itemId: string = this.route.snapshot.paramMap.get("itemId");
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            name: "PurchaseInvoice",
          }),
          new Fetch({
            id: itemId,
            include: [
              new TreeNode({
                roleType: m.PurchaseInvoiceItem.PurchaseInvoiceItemState,
              }),
              new TreeNode({ roleType: m.PurchaseInvoiceItem.PurchaseOrderItem }),
              new TreeNode({
                nodes: [new TreeNode({ roleType: m.VatRegime.VatRate })],
                roleType: m.PurchaseInvoiceItem.VatRegime,
              }),
            ],
            name: "invoiceItem",
          }),
        ];

        const queries: Query[] = [
          new Query({
            name: "goods",
            objectType: m.Good,
          }),
          new Query({
            name: "invoiceItemTypes",
            objectType: m.InvoiceItemType,
          }),
          new Query({
            name: "vatRates",
            objectType: m.VatRate,
          }),
          new Query({
            name: "vatRegimes",
            objectType: m.VatRegime,
          }),
        ];

        return this.scope.load("Pull", new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {
          this.scope.session.reset();

          this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;
          this.invoiceItem = loaded.objects.invoiceItem as PurchaseInvoiceItem;
          this.orderItem = loaded.objects.orderItem as PurchaseOrderItem;
          this.goods = loaded.collections.goods as Good[];
          this.vatRates = loaded.collections.vatRates as VatRate[];
          this.vatRegimes = loaded.collections.vatRegimes as VatRegime[];
          this.invoiceItemTypes = loaded.collections.invoiceItemTypes as InvoiceItemType[];
          this.productItemType = this.invoiceItemTypes.find(
            (v: InvoiceItemType) => v.UniqueId.toUpperCase() === "0D07F778-2735-44CB-8354-FB887ADA42AD",
          );

          if (!this.invoiceItem) {
            this.title = "Add invoice Item";
            this.invoiceItem = this.scope.session.create("PurchaseInvoiceItem") as PurchaseInvoiceItem;
            this.invoice.AddPurchaseInvoiceItem(this.invoiceItem);
          } else {
            if (
              this.invoiceItem.InvoiceItemType === this.productItemType
            ) {
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

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goodSelected(product: Product): void {
    this.invoiceItem.InvoiceItemType = this.productItemType;

    const fetches: Fetch[] = [
      new Fetch({
        id: product.id,
        name: "inventoryItem",
        path: new Path({ step: this.m.Good.InventoryItemsWhereGood }),
      }),
    ];

    this.scope.load("Pull", new PullRequest({ fetches })).subscribe(
      (loaded) => {
        this.inventoryItems = loaded.collections
          .inventoryItem as InventoryItem[];
        if (this.inventoryItems[0].objectType.name === "SerialisedInventoryItem") {
          this.serialisedInventoryItem = this
            .inventoryItems[0] as SerialisedInventoryItem;
        }
        if (this.inventoryItems[0].objectType.name === "NonSerialisedInventoryItem") {
          this.nonSerialisedInventoryItem = this
            .inventoryItems[0] as NonSerialisedInventoryItem;
        }
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public save(): void {
    this.scope.save().subscribe(
      (saved: Saved) => {
        this.router.navigate(["/accountspayable/invoice/" + this.invoice.id]);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      },
    );
  }

  public update(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.refresh();
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
