import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import 'rxjs/add/observable/combineLatest';

import { MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, RequestForQuote, ProductQuote, QuoteItem } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort } from "@allors/framework";

@Component({
  template: `
<mat-toolbar>
  <div layout="row" layout-align="start center" flex>
    <button mat-icon-button tdLayoutManageListOpen [hideWhenOpened]="true" style="display: none">
          <mat-icon>arrow_back</mat-icon>
        </button>
    <span>{{title}}</span>
    <span flex></span>
    <button mat-icon-button><mat-icon>settings</mat-icon></button>
  </div>
</mat-toolbar>

<div body *ngIf="quote" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Product Quote</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.QuoteState" display="Name" label="Status"></a-mat-static>
        </div>
        <div layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.QuoteNumber"></a-mat-static>
        </div>
        <div *ngIf="quote.Request" layout="row" class="link" [routerLink]="['/orders/request/' + quote.Request.id]">
          <a-mat-static [object]="quote" [roleType]="m.Quote.Request" display="RequestNumber"></a-mat-static>
        </div>
        <div *ngIf="salesOrder" layout="row" class="link" [routerLink]="['/orders/salesOrder/' + salesOrder.id ]">
          <a-mat-static [object]="salesOrder" [roleType]="m.SalesOrder.OrderNumber"></a-mat-static>
        </div>
        <div *ngIf="quote.Receiver.objectType.name == 'Person'" layout="row" class="link" [routerLink]="['/relations/person/' + quote.Receiver.id ]">
          <a-mat-static [object]="quote" [roleType]="m.Quote.Receiver" display="displayName" label="To"></a-mat-static>
        </div>
        <div *ngIf="quote.Receiver.objectType.name == 'Organisation'" layout="row" class="link" [routerLink]="['/relations/organisation/' + quote.Receiver.id ]">
          <a-mat-static [object]="quote" [roleType]="m.Quote.Receiver" display="displayName" label="To"></a-mat-static>
        </div>
        <div *ngIf="quote.ContactPerson" layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.ContactPerson" class="link" [routerLink]="['/relations/person/' + quote.ContactPerson.id ]" display="displayName"></a-mat-static>
        </div>
        <div *ngIf="quote.FullfillContactMechanism" layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.FullfillContactMechanism" display="displayName" label="Reply to"></a-mat-static>
        </div>
        <div *ngIf="quote.Description" layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.Description"y></a-mat-static>
        </div>
        <div *ngIf="request?.Comment" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.Comment" label="Request Comment"></a-mat-static>
        </div>
        <div *ngIf="quote.Comment" layout="row">
          <a-mat-textarea [object]="quote" [roleType]="m.Quote.Comment" label="Quote Comment"></a-mat-textarea>
        </div>
        <div *ngIf="request?.InternalComment" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.InternalComment" label="Request Internal Comment"></a-mat-static>
        </div>
        <div *ngIf="quote.InternalComment" layout="row">
          <a-mat-textarea [object]="quote" [roleType]="m.Quote.InternalComment" label="Quote Internal Comment"></a-mat-textarea>
        </div>
        <div *ngIf="quote.ValidFromDate" layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.ValidFromDate"></a-mat-static>
        </div>
        <div *ngIf="quote.ValidThroughDate" layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.ValidThroughDate"></a-mat-static>
        </div>
        <div *ngIf="quote.IssueDate" layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.IssueDate"></a-mat-static>
        </div>
        <div layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.Price" label="Quote Value"></a-mat-static>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> created: {{ quote.CreationDate | date}} by {{ quote.CreatedBy.displayName}}</p>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> last modified: {{ quote.LastModifiedDate | date}} by {{ quote.LastModifiedBy.displayName}}</p>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button *ngIf="quote.CanWriteQuoteItems" mat-button [routerLink]="['/productQuote/' + quote.id]">Edit</button>
        <button mat-button [routerLink]="['/printproductquote/' + quote.id ]">Print</button>
        <button *ngIf="quote.CanExecuteOrder" mat-button (click)="Order()">Create Sales Order</button>
      </mat-card-actions>
    </mat-card>

    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Quote Items</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>
        <ng-template tdLoading="quote.QuoteItems">
          <mat-list class="will-load">
            <div class="mat-padding" *ngIf="quote.QuoteItems.length === 0" layout="row" layout-align="center center">
              <h3>No Items to display.</h3>
            </div>

            <ng-template let-quoteItem let-last="last" ngFor [ngForOf]="quote.QuoteItems">
              <mat-list-item>
                <h3 *ngIf="quoteItem.Product && quote.CanWriteQuoteItems" mat-line [routerLink]="['/productQuote/' + quote.id + '/item/' + quoteItem.id ]">{{quoteItem.Quantity}} * {{quoteItem.Product.Name}}</h3>
                <h3 *ngIf="quoteItem.Product && !quote.CanWriteQuoteItems" mat-line>{{quoteItem.Quantity}} * {{quoteItem.Product.Name}}</h3>
                <span>
                  <button mat-icon-button [mat-menu-trigger-for]="menu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu x-position="before" #menu="matMenu">
                    <a *ngIf="quote.CanWriteQuoteItems" [routerLink]="['/productQuote/' + quote.id + '/item/' + quoteItem.id ]" mat-menu-item>Edit</a>
                    <button  mat-menu-item (click)="deletequoteItem(quoteItem)" [disabled]="!quoteItem.CanExecuteDelete">Delete</button>
                  </mat-menu>
                </span>
              </mat-list-item>
              <mat-divider *ngIf="!last" mat-inset></mat-divider>
            </ng-template>
          </mat-list>
        </ng-template>
      </mat-card-content>
      <mat-divider></mat-divider>
      <mat-card-actions>
        <button *ngIf="quote.CanWriteQuoteItems" mat-button [routerLink]="['/productQuote/' + quote.id +'/item']">+ Add</button>
      </mat-card-actions>
    </mat-card>
  </div>

  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Status History</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <ng-template tdLoading="quote.QuoteStatuses">
          <mat-list class="will-load">

            <ng-template let-status let-last="last" ngFor [ngForOf]="quote.QuoteStatuses">
              <mat-list-item>
                <p mat-line class="mat-caption">{{ status.QuoteObjectState.Name }} </p>
                <p mat-line hide-gt-md class="mat-caption"> Start: {{ status.StartDateTime | date}} </p>

                <span hide-xs hide-sm hide-md flex-gt-xs="60" flex-xs="40" layout-gt-xs="row">
                  <div class="mat-caption tc-grey-500" flex-gt-xs="50"> Start: {{ status.StartDateTime | date }}</div>
                </span>

              </mat-list-item>
            </ng-template>
          </mat-list>
        </ng-template>
      </mat-card-content>

    </mat-card>
  </div>
</div>
`,
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
