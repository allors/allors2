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
let SalesOrderOverviewComponent = class SalesOrderOverviewComponent {
    constructor(workspaceService, errorService, route, router, dialogService, snackBar, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.router = router;
        this.dialogService = dialogService;
        this.snackBar = snackBar;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Sales Order Overview";
        this.orderItems = [];
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
                                new framework_1.TreeNode({ roleType: m.SalesOrderItem.Product }),
                                new framework_1.TreeNode({ roleType: m.SalesOrderItem.ItemType }),
                                new framework_1.TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemState }),
                            ],
                            roleType: m.SalesOrder.SalesOrderItems,
                        }),
                        new framework_1.TreeNode({ roleType: m.SalesOrder.ContactPerson }),
                        new framework_1.TreeNode({ roleType: m.SalesOrder.ShipToCustomer }),
                        new framework_1.TreeNode({ roleType: m.SalesOrder.SalesOrderState }),
                        new framework_1.TreeNode({ roleType: m.SalesOrder.CreatedBy }),
                        new framework_1.TreeNode({ roleType: m.SalesOrder.LastModifiedBy }),
                        new framework_1.TreeNode({ roleType: m.SalesOrder.Quote }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({
                                    nodes: [
                                        new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
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
            const salesInvoiceFetch = new framework_1.Fetch({
                id,
                name: "salesInvoice",
                path: new framework_1.Path({ step: m.SalesOrder.SalesInvoicesWhereSalesOrder }),
            });
            if (id != null) {
                fetch.push(salesInvoiceFetch);
            }
            const query = [
                new framework_1.Query({
                    name: "goods",
                    objectType: m.Good,
                }),
                new framework_1.Query({
                    name: "processFlows",
                    objectType: m.ProcessFlow,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.goods = loaded.collections.goods;
            this.order = loaded.objects.order;
            this.salesInvoice = loaded.objects.salesInvoice;
            this.processFlows = loaded.collections.processFlows;
            this.payFirst = this.processFlows.find((v) => v.UniqueId.toUpperCase() === "AB01CCC2-6480-4FC0-B20E-265AFD41FAE2");
            if (this.order) {
                this.orderItems = this.order.SalesOrderItems;
            }
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
    deleteOrderItem(orderItem) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this item?" })
            .afterClosed()
            .subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(orderItem.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    createInvoice() {
        this.scope.invoke(this.order.Complete)
            .subscribe((invoked) => {
            this.goBack();
            this.snackBar.open("Invoice successfully created.", "close", { duration: 5000 });
            this.gotoInvoice();
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    gotoInvoice() {
        const fetch = [new framework_1.Fetch({
                id: this.order.id,
                name: "invoices",
                path: new framework_1.Path({ step: this.m.SalesOrder.SalesInvoicesWhereSalesOrder }),
            })];
        this.scope.load("Pull", new framework_1.PullRequest({ fetch }))
            .subscribe((loaded) => {
            const invoices = loaded.collections.invoices;
            if (invoices.length === 1) {
                this.router.navigate(["/ar/invoice/" + invoices[0].id]);
            }
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
};
SalesOrderOverviewComponent = __decorate([
    core_1.Component({
        template: `
<mat-toolbar>
  <div layout="row" layout-align="start center" flex>
    <button mat-icon-button tdLayoutManageListOpen [hideWhenOpened]="true" style="display: none">
      <mat-icon>arrow_back</mat-icon>
    </button>
    <span>{{title}}</span>
    <span flex></span>
    <button mat-icon-button>
      <mat-icon>settings</mat-icon>
    </button>
  </div>
</mat-toolbar>

<div body *ngIf="order" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Sales Order</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.SalesOrderState" display="Name" label="Status"></a-mat-static>
        </div>
        <div layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.OrderNumber"></a-mat-static>
        </div>
        <div *ngIf="order.Quote" layout="row" class="link" [routerLink]="['/orders/productQuote/' + order.Quote.id ]">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.Quote" display="QuoteNumber"></a-mat-static>
        </div>
        <div *ngIf="salesInvoice" layout="row" class="link" [routerLink]="['/ar/invoice/' + salesInvoice.id ]">
          <a-mat-static [object]="salesInvoice" [roleType]="m.SalesInvoice.InvoiceNumber"></a-mat-static>
        </div>
        <div *ngIf="order.ContactPerson" layout="row" class="link" [routerLink]="['/relations/person/' + order.ContactPerson.id ]">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.ContactPerson" display="displayName"></a-mat-static>
        </div>
        <div layout="row" class="link" [routerLink]="['/relations/organisation/' + order.ShipToCustomer.id ]">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.ShipToCustomer" display="displayName" label="Customer"></a-mat-static>
        </div>
        <div *ngIf="order.ShipToAddress" layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.ShipToAddress" display="displayName" label="Ship to"></a-mat-static>
        </div>
        <div *ngIf="order.Description" layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.Description"></a-mat-static>
        </div>
        <div *ngIf="quote?.Comment" layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.Comment" label="Quote Comment"></a-mat-static>
        </div>
        <div *ngIf="order.Comment" layout="row">
          <a-mat-textarea [object]="order" [roleType]="m.SalesOrder.Comment" label="Order Comment"></a-mat-textarea>
        </div>
        <div *ngIf="quote?.InternalComment" layout="row">
          <a-mat-static [object]="quote" [roleType]="m.Quote.InternalComment" label="Quote Internal Comment"></a-mat-static>
        </div>
        <div *ngIf="order.InternalComment" layout="row">
          <a-mat-textarea [object]="order" [roleType]="m.SalesOrder.InternalComment" label="Order Internal Comment"></a-mat-textarea>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> created: {{ order.CreationDate | date}} by {{ order.CreatedBy.displayName}}</p>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> last modified: {{ order.LastModifiedDate | date}} by {{ order.LastModifiedBy.displayName}}</p>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button *ngIf="order.CanWriteSalesOrderItems" mat-button [routerLink]="['/salesOrder/' + order.id]">Edit</button>
        <button mat-button [routerLink]="['/printsalesorder/' + order.id ]">Print</button>
        <button *ngIf="payFirst && order.CanExecuteComplete && order.ValidOrderItems.length > 0"
          mat-button (click)="createInvoice()">Create Invoice</button>
      </mat-card-actions>
    </mat-card>

    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Order Items</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>
        <ng-template tdLoading="order.SalesOrderItems">
          <mat-list class="will-load">
            <div class="mat-padding" *ngIf="order.SalesOrderItems.length === 0" layout="row" layout-align="center center">
              <h3>No Items to display.</h3>
            </div>

            <ng-template let-orderItem let-last="last" ngFor [ngForOf]="order.SalesOrderItems">
              <mat-list-item>
                <h3 *ngIf="orderItem.Product && order.CanWriteSalesOrderItems" mat-line [routerLink]="['/salesOrder/' + order.id + '/item/' + orderItem.id ]">{{orderItem.QuantityOrdered}} * {{orderItem.Product.Name}}</h3>
                <h3 *ngIf="orderItem.Product && !order.CanWriteSalesOrderItems" mat-line>{{orderItem.QuantityOrdered}} * {{orderItem.Product.Name}}</h3>

                <h3 *ngIf="!orderItem.Product && order.CanWriteSalesOrderItems" mat-line  [routerLink]="['/salesOrder/' + order.id + '/item/' + orderItem.id ]">{{orderItem.ItemType.Name}}</h3>
                <h3 *ngIf="!orderItem.Product && !order.CanWriteSalesOrderItems" mat-line>{{orderItem.ItemType.Name}}</h3>

                <h4 mat-line>Total ex VAT: {{orderItem.TotalExVat}}</h4>

                <span>
                  <button mat-icon-button [mat-menu-trigger-for]="menu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu x-position="before" #menu="matMenu">
                    <a *ngIf="order.CanWriteSalesOrderItems" [routerLink]="['/salesOrder/' + order.id + '/item/' + orderItem.id ]" mat-menu-item>Edit</a>
                    <button  mat-menu-item (click)="deleteOrderItem(orderItem)" [disabled]="!orderItem.CanExecuteDelete">Delete</button>
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
        <button *ngIf="order.CanWriteSalesOrderItems" mat-button [routerLink]="['/salesOrder/' + order.id +'/item']">+ Add</button>
      </mat-card-actions>
    </mat-card>
  </div>

  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Status History</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <ng-template tdLoading="order.OrderStatuses">
          <mat-list class="will-load">

            <ng-template let-status let-last="last" ngFor [ngForOf]="order.OrderStatuses">
              <mat-list-item>
                <p mat-line class="mat-caption">{{ status.SalesOrderObjectState.Name }} </p>
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
], SalesOrderOverviewComponent);
exports.SalesOrderOverviewComponent = SalesOrderOverviewComponent;
