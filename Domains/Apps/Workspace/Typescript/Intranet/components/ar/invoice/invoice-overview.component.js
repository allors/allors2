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
const Rx_1 = require("rxjs/Rx");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let InvoiceOverviewComponent = class InvoiceOverviewComponent {
    constructor(workspaceService, errorService, route, dialogService, snackBar, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.dialogService = dialogService;
        this.snackBar = snackBar;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Sales Invoice Overview";
        this.goods = [];
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
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
        const combined$ = Rx_1.Observable.combineLatest(route$, this.refresh$);
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
                                new framework_1.TreeNode({ roleType: m.SalesInvoiceItem.Product }),
                                new framework_1.TreeNode({ roleType: m.SalesInvoiceItem.SalesInvoiceItemType }),
                            ],
                            roleType: m.SalesInvoice.SalesInvoiceItems,
                        }),
                        new framework_1.TreeNode({ roleType: m.SalesInvoice.ContactPerson }),
                        new framework_1.TreeNode({ roleType: m.SalesInvoice.BillToCustomer }),
                        new framework_1.TreeNode({ roleType: m.SalesInvoice.SalesInvoiceState }),
                        new framework_1.TreeNode({ roleType: m.SalesInvoice.CreatedBy }),
                        new framework_1.TreeNode({ roleType: m.SalesInvoice.LastModifiedBy }),
                        new framework_1.TreeNode({ roleType: m.SalesInvoice.SalesOrder }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({
                                    nodes: [
                                        new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
                                    ],
                                    roleType: m.PostalAddress.PostalBoundary,
                                }),
                            ],
                            roleType: m.SalesInvoice.BillToContactMechanism,
                        }),
                    ],
                    name: "invoice",
                }),
                new framework_1.Fetch({
                    id,
                    name: "order",
                    path: new framework_1.Path({ step: m.SalesInvoice.SalesOrder }),
                }),
            ];
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
            this.order = loaded.objects.order;
            this.invoice = loaded.objects.invoice;
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
    deleteInvoiceItem(invoiceItem) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this item?" })
            .afterClosed()
            .subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(invoiceItem.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
};
InvoiceOverviewComponent = __decorate([
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

<div body *ngIf="invoice" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Sales Invoice</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div layout="row">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.SalesInvoiceState" display="Name" label="Status"></a-mat-static>
        </div>
        <div layout="row">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.InvoiceNumber"></a-mat-static>
        </div>
        <div *ngIf="invoice.SalesOrder" layout="row" class="link" [routerLink]="['/orders/salesOrder/' + invoice.SalesOrder.id ]">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.SalesOrder" display="OrderNumber"></a-mat-static>
        </div>
        <div layout="row" class="link" [routerLink]="['/relations/organisation/' + invoice.BillToCustomer.id ]">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.BillToCustomer" display="displayName" label="Customer"></a-mat-static>
        </div>
        <div *ngIf="invoice.ContactPerson" layout="row" class="link" [routerLink]="['/relations/person/' + invoice.ContactPerson.id ]">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.ContactPerson" display="displayName"></a-mat-static>
        </div>
        <div *ngIf="invoice.BillToContactMechanism" layout="row">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.BillToContactMechanism" display="displayName" label="Bill to"></a-mat-static>
        </div>
        <div *ngIf="invoice.Description" layout="row">
          <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.Description"></a-mat-static>
        </div>
        <div *ngIf="order?.Comment" layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.Comment" label="Order Comment"></a-mat-static>
        </div>
        <div *ngIf="invoice.Comment" layout="row">
          <a-mat-textarea [object]="invoice" [roleType]="m.SalesInvoice.Comment" label="Invoice Comment"></a-mat-textarea>
        </div>
        <div *ngIf="order?.InternalComment" layout="row">
          <a-mat-static [object]="order" [roleType]="m.SalesOrder.InternalComment" label="Order Internal Comment"></a-mat-static>
        </div>
        <div *ngIf="invoice.InternalComment" layout="row">
          <a-mat-textarea [object]="invoice" [roleType]="m.SalesInvoice.InternalComment" label="Invoice Internal Comment"></a-mat-textarea>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> created: {{ invoice.CreationDate | date}} by {{ invoice.CreatedBy.displayName}}</p>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> last modified: {{ invoice.LastModifiedDate | date}} by {{ invoice.LastModifiedBy.displayName}}</p>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button *ngIf="invoice.CanWriteSalesInvoiceItems" mat-button [routerLink]="['/invoice/' + invoice.id]">Edit</button>
        <button mat-button [routerLink]="['/printinvoice/' + invoice.id ]">Print</button>
      </mat-card-actions>
    </mat-card>

    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Invoice Items</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>
        <ng-template tdLoading="invoice.SalesInvoiceItems">
          <mat-list class="will-load">
            <div class="mat-padding" *ngIf="invoice.SalesInvoiceItems.length === 0" layout="row" layout-align="center center">
              <h3>No Items to display.</h3>
            </div>

            <ng-template let-invoiceItem let-last="last" ngFor [ngForOf]="invoice.SalesInvoiceItems">
              <mat-list-item>
                <h3 *ngIf="invoiceItem.Product && invoice.CanWriteSalesInvoiceItems" mat-line [routerLink]="['/invoice/' + invoice.id + '/item/' + invoiceItem.id ]">{{invoiceItem.Quantity}} * {{invoiceItem.Product.Name}}</h3>
                <h3 *ngIf="invoiceItem.Product && !invoice.CanWriteSalesInvoiceItems" mat-line>{{invoiceItem.Quantity}} * {{invoiceItem.Product.Name}}</h3>

                <h3 *ngIf="!invoiceItem.Product && invoice.CanWriteSalesInvoiceItems" mat-line [routerLink]="['/invoice/' + invoice.id + '/item/' + invoiceItem.id ]">{{invoiceItem.SalesInvoiceItemType.Name}}</h3>
                <h3 *ngIf="!invoiceItem.Product && !invoice.CanWriteSalesInvoiceItems" mat-line>{{invoiceItem.SalesInvoiceItemType.Name}}</h3>

                <h4 mat-line>Total ex VAT: {{invoiceItem.TotalExVat}}</h4>

                <span>
                  <button mat-icon-button [mat-menu-trigger-for]="menu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu x-position="before" #menu="matMenu">
                    <a *ngIf="invoice.CanWriteSalesInvoiceItems" [routerLink]="['/invoice/' + invoice.id + '/item/' + invoiceItem.id ]" mat-menu-item>Edit</a>
                    <button  mat-menu-item (click)="deleteInvoiceItem(invoiceItem)" [disabled]="!invoiceItem.CanExecuteDelete">Delete</button>
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
        <button *ngIf="invoice.CanWriteSalesInvoiceItems" mat-button [routerLink]="['/invoice/' + invoice.id +'/item']">+ Add</button>
      </mat-card-actions>
    </mat-card>
  </div>

  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Status History</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <ng-template tdLoading="invoice.InvoiceStatuses">
          <mat-list class="will-load">

            <ng-template let-status let-last="last" ngFor [ngForOf]="invoice.InvoiceStatuses">
              <mat-list-item>
                <p mat-line class="mat-caption">{{ status.SalesInvoiceObjectState.Name }} </p>
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
        core_2.TdDialogService,
        material_1.MatSnackBar,
        core_2.TdMediaService,
        core_1.ChangeDetectorRef])
], InvoiceOverviewComponent);
exports.InvoiceOverviewComponent = InvoiceOverviewComponent;
