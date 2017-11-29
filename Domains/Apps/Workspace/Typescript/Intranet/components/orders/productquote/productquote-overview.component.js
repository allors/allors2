"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const material_1 = require("@angular/material");
const router_1 = require("@angular/router");
const core_2 = require("@covalent/core");
const BehaviorSubject_1 = require("rxjs/BehaviorSubject");
const Observable_1 = require("rxjs/Observable");
require("rxjs/add/observable/combineLatest");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let ProductQuoteOverviewComponent = class ProductQuoteOverviewComponent {
    constructor(workspaceService, errorService, route, router, dialogService, snackBar, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.router = router;
        this.dialogService = dialogService;
        this.snackBar = snackBar;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Quote Overview";
        this.goods = [];
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.refresh$ = new BehaviorSubject_1.BehaviorSubject(undefined);
    }
    refresh() {
        this.refresh$.next(new Date());
    }
    save() {
        this.scope
            .save()
            .subscribe((saved) => {
            this.snackBar.open("items saved", "close", { duration: 1000 });
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    ngOnInit() {
        const route$ = this.route.url;
        const combined$ = Observable_1.Observable.combineLatest(route$, this.refresh$);
        this.subscription = combined$
            .switchMap(([urlSegments, date]) => {
            const id = this.route.snapshot.paramMap.get("id");
            const m = this.m;
            const fetch = [
                new framework_1.Fetch({
                    id,
                    include: [
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.QuoteItem.Product }),
                            ],
                            roleType: m.Quote.QuoteItems,
                        }),
                        new framework_1.TreeNode({ roleType: m.Quote.Receiver }),
                        new framework_1.TreeNode({ roleType: m.Quote.ContactPerson }),
                        new framework_1.TreeNode({ roleType: m.Quote.QuoteState }),
                        new framework_1.TreeNode({ roleType: m.Quote.CreatedBy }),
                        new framework_1.TreeNode({ roleType: m.Quote.LastModifiedBy }),
                        new framework_1.TreeNode({ roleType: m.Quote.Request }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({
                                    nodes: [
                                        new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
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
            const salesOrderFetch = new framework_1.Fetch({
                id,
                name: "salesOrder",
                path: new framework_1.Path({ step: m.ProductQuote.SalesOrderWhereQuote }),
            });
            if (id != null) {
                fetch.push(salesOrderFetch);
            }
            const query = [
                new framework_1.Query({
                    name: "goods",
                    objectType: m.Good,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.goods = loaded.collections.goods;
            this.quote = loaded.objects.quote;
            this.salesOrder = loaded.objects.salesOrder;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    ngAfterViewInit() {
        this.media.broadcast();
        this.changeDetectorRef.detectChanges();
    }
    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }
    goBack() {
        window.history.back();
    }
    deleteQuoteItem(quoteItem) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this item?" })
            .afterClosed()
            .subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(quoteItem.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    Order() {
        this.scope.invoke(this.quote.Order)
            .subscribe((invoked) => {
            this.goBack();
            this.snackBar.open("Order successfully created.", "close", { duration: 5000 });
            this.gotoOrder();
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    gotoOrder() {
        const fetch = [new framework_1.Fetch({
                id: this.quote.id,
                name: "order",
                path: new framework_1.Path({ step: this.m.ProductQuote.SalesOrderWhereQuote }),
            })];
        this.scope.load("Pull", new framework_1.PullRequest({ fetch }))
            .subscribe((loaded) => {
            const order = loaded.objects.order;
            this.router.navigate(["/orders/salesOrder/" + order.id]);
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
};
ProductQuoteOverviewComponent = __decorate([
    core_1.Component({
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
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        router_1.Router,
        core_2.TdDialogService,
        material_1.MatSnackBar,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], ProductQuoteOverviewComponent);
exports.ProductQuoteOverviewComponent = ProductQuoteOverviewComponent;
