import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subscription } from "rxjs/Rx";

import { MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, RequestForQuote, ProductQuote, QuoteItem } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort } from "@allors/framework";

@Component({
  templateUrl: "./productquote-overview.component.html",
})
export class ProductQuoteOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Quote Overview";
  public request: RequestForQuote;
  public quote: ProductQuote;
  public goods: Good[] = [];
  public salesOrder: SalesOrder;

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

    this.scope = this.workspaceService.createScope()
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
                  new TreeNode({ roleType: m.QuoteItem.Product }),
                ],
                roleType: m.Quote.QuoteItems,
              }),
              new TreeNode({ roleType: m.Quote.Receiver }),
              new TreeNode({ roleType: m.Quote.ContactPerson }),
              new TreeNode({ roleType: m.Quote.QuoteState }),
              new TreeNode({ roleType: m.Quote.CreatedBy }),
              new TreeNode({ roleType: m.Quote.LastModifiedBy }),
              new TreeNode({ roleType: m.Quote.Request }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.Quote.FullfillContactMechanism,
              }),
            ],
            name: "quote",
          }),
        ];

        const salesOrderFetch: Fetch = new Fetch({
          id,
          name: "salesOrder",
          path: new Path({ step: m.ProductQuote.SalesOrderWhereQuote }),
        });

        if (id != null) {
          fetch.push(salesOrderFetch);
        }

        const query: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {
        this.scope.session.reset();
        this.goods = loaded.collections.goods as Good[];
        this.quote = loaded.objects.quote as ProductQuote;
        this.salesOrder = loaded.objects.salesOrder as SalesOrder;
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

  public deleteQuoteItem(quoteItem: QuoteItem): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this item?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(quoteItem.Delete)
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

  public Order(): void {
    this.scope.invoke(this.quote.Order)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open("Order successfully created.", "close", { duration: 5000 });
        this.gotoOrder();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public gotoOrder(): void {

    const fetch: Fetch[] = [new Fetch({
      id: this.quote.id,
      name: "order",
      path: new Path({ step: this.m.ProductQuote.SalesOrderWhereQuote }),
    })];

    this.scope.load("Pull", new PullRequest({ fetch }))
      .subscribe((loaded: Loaded) => {
        const order = loaded.objects.order as SalesOrder;
        this.router.navigate(["/orders/salesOrder/" + order.id]);
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }
}
