import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../angular";
import { Good, SalesInvoice, SalesOrder, ProductQuote, SalesOrderItem, ProcessFlow } from "../../../../domain";
import { Fetch, Path, PullRequest, Query, TreeNode } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

@Component({
  templateUrl: "./salesorder-overview.component.html",
})
export class SalesOrderOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Sales Order Overview";
  public quote: ProductQuote;
  public order: SalesOrder;
  public orderItems: SalesOrderItem[] = [];
  public goods: Good[] = [];
  public salesInvoice: SalesInvoice;
  public processFlows: ProcessFlow[];
  public payFirst: ProcessFlow;

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private router: Router,
    public dialogService: TdDialogService,
    private snackBar: MatSnackBar,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
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

    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {
        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.SalesOrderItem.Product }),
                  new TreeNode({ roleType: m.SalesOrderItem.ItemType }),
                  new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemState }),
                ],
                roleType: m.SalesOrder.SalesOrderItems,
              }),
              new TreeNode({ roleType: m.SalesOrder.ContactPerson }),
              new TreeNode({ roleType: m.SalesOrder.ShipToCustomer }),
              new TreeNode({ roleType: m.SalesOrder.SalesOrderState }),
              new TreeNode({ roleType: m.SalesOrder.CreatedBy }),
              new TreeNode({ roleType: m.SalesOrder.LastModifiedBy }),
              new TreeNode({ roleType: m.SalesOrder.Quote }),
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
            new Query(
            {
              name: "processFlows",
              objectType: m.ProcessFlow,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {
        this.scope.session.reset();
        this.goods = loaded.collections.goods as Good[];
        this.order = loaded.objects.order as SalesOrder;
        this.salesInvoice = loaded.objects.salesInvoice as SalesInvoice;
        this.processFlows = loaded.collections.processFlows as ProcessFlow[];
        this.payFirst = this.processFlows.find((v: ProcessFlow) => v.UniqueId.toUpperCase() === "AB01CCC2-6480-4FC0-B20E-265AFD41FAE2");

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
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this item?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(orderItem.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public ship(): void {
    this.scope.invoke(this.order.Ship)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open("Customer Shipment successfully created.", "close", { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public createInvoice(): void {
    this.scope.invoke(this.order.Complete)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open("Invoice successfully created.", "close", { duration: 5000 });
        this.gotoInvoice();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public gotoInvoice(): void {

      const fetch: Fetch[] = [new Fetch({
        id: this.order.id,
        name: "invoices",
        path: new Path({ step: this.m.SalesOrder.SalesInvoicesWhereSalesOrder }),
      })];

      this.scope.load("Pull", new PullRequest({ fetch }))
        .subscribe((loaded: Loaded) => {
          const invoices = loaded.collections.invoices as SalesInvoice[];
          if (invoices.length === 1) {
            this.router.navigate(["/ar/invoice/" + invoices[0].id]);
          }
        },
        (error: any) => {
          this.errorService.message(error);
          this.goBack();
        });
    }
  }
