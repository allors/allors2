import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "../../../../../angular";
import { Equals, Fetch, Like, Page, Path, PullRequest, Query, Sort, TreeNode } from "../../../../../domain";
import { Good, ProductQuote, SalesInvoice, SalesOrder, SalesOrderItem } from "../../../../../domain";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./salesOrderOverview.component.html",
})
export class SalesOrderOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Sales Order Overview";
  public quote: ProductQuote;
  public order: SalesOrder;
  public orderItems: SalesOrderItem[] = [];
  public goods: Good[] = [];
  public salesInvoice: SalesInvoice;

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public dialogService: TdDialogService,
    private snackBar: MdSnackBar,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open("items saved", "close", { duration: 1000 });
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public ngOnInit(): void {

    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.SalesOrderItem.Product }),
                ],
                roleType: m.SalesOrder.SalesOrderItems,
              }),
              new TreeNode({ roleType: m.SalesOrder.ShipToCustomer }),
              new TreeNode({ roleType: m.SalesOrder.CurrentObjectState }),
              new TreeNode({ roleType: m.SalesOrder.CreatedBy }),
              new TreeNode({ roleType: m.SalesOrder.LastModifiedBy }),
              new TreeNode({ roleType: m.SalesOrder.Quote }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.SalesOrderStatus.SalesOrderObjectState }),
                ],
                roleType: m.SalesOrder.OrderStatuses,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.SalesOrder.ShipToAddress,
              }),
            ],
            name: "order",
          }),
          new Fetch({
            id,
            name: "quote",
            path: new Path({ step: m.SalesOrder.Quote }),
          }),
        ];

        const salesInvoiceFetch: Fetch = new Fetch({
          id,
          name: "salesInvoice",
          path: new Path({ step: m.SalesOrder.SalesInvoicesWhereSalesOrder }),
        });

        if (id != null) {
          fetch.push(salesInvoiceFetch);
        }

        const query: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {
        this.goods = loaded.collections.goods as Good[];
        this.quote = loaded.objects.quote as ProductQuote;
        this.order = loaded.objects.order as SalesOrder;
        this.salesInvoice = loaded.objects.salesInvoice as SalesInvoice;

        if (this.order) {
          this.orderItems = this.order.SalesOrderItems;
        }
      },
      (error: any) => {
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

  public goBack(): void {
    window.history.back();
  }

  public deleteOrderItem(orderItem: SalesOrderItem): void {
    this.order.RemoveSalesOrderItem(orderItem);
  }

  public createInvoice(): void {
    this.scope.invoke(this.order.Complete)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open("Invoice successfully created.", "close", { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }
}
