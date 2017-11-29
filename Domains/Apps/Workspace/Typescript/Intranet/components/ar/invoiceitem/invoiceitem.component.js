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
const workspace_1 = require("@allors/workspace");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let InvoiceItemEditComponent = class InvoiceItemEditComponent {
    constructor(workspaceService, errorService, router, route, snackBar, dialogService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.router = router;
        this.route = route;
        this.snackBar = snackBar;
        this.dialogService = dialogService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Edit Sales Invoice Item";
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.scope = this.workspaceService.createScope();
        this.refresh$ = new BehaviorSubject_1.BehaviorSubject(undefined);
        this.goodsFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Good, roleTypes: [this.m.Good.Name] });
    }
    ngOnInit() {
        const route$ = this.route.url;
        const combined$ = Observable_1.Observable.combineLatest(route$, this.refresh$);
        this.subscription = combined$
            .switchMap(([urlSegments, date]) => {
            const id = this.route.snapshot.paramMap.get("id");
            const itemId = this.route.snapshot.paramMap.get("itemId");
            const m = this.m;
            const fetch = [
                new framework_1.Fetch({
                    id,
                    name: "salesInvoice",
                }),
                new framework_1.Fetch({
                    id: itemId,
                    include: [
                        new framework_1.TreeNode({ roleType: m.SalesInvoiceItem.SalesInvoiceItemState }),
                        new framework_1.TreeNode({ roleType: m.SalesInvoiceItem.SalesOrderItem }),
                    ],
                    name: "invoiceItem",
                }),
            ];
            const query = [
                new framework_1.Query({
                    name: "goods",
                    objectType: m.Good,
                }),
                new framework_1.Query({
                    name: "salesInvoiceItemTypes",
                    objectType: m.SalesInvoiceItemType,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.invoice = loaded.objects.salesInvoice;
            this.invoiceItem = loaded.objects.invoiceItem;
            this.orderItem = loaded.objects.orderItem;
            this.goods = loaded.collections.goods;
            this.salesInvoiceItemTypes = loaded.collections.salesInvoiceItemTypes;
            this.productItemType = this.salesInvoiceItemTypes.find((v) => v.UniqueId.toUpperCase() === "0D07F778-2735-44CB-8354-FB887ADA42AD");
            if (!this.invoiceItem) {
                this.title = "Add invoice Item";
                this.invoiceItem = this.scope.session.create("SalesInvoiceItem");
                this.invoice.AddSalesInvoiceItem(this.invoiceItem);
            }
            else {
                if (this.invoiceItem.SalesInvoiceItemType === this.productItemType) {
                    this.goodSelected(this.invoiceItem.Product);
                }
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
    goodSelected(product) {
        this.invoiceItem.SalesInvoiceItemType = this.productItemType;
        const fetch = [
            new framework_1.Fetch({
                id: product.id,
                name: "inventoryItem",
                path: new framework_1.Path({ step: this.m.Good.InventoryItemsWhereGood }),
            }),
        ];
        this.scope
            .load("Pull", new framework_1.PullRequest({ fetch }))
            .subscribe((loaded) => {
            this.inventoryItems = loaded.collections.inventoryItem;
            if (this.inventoryItems[0] instanceof workspace_1.SerialisedInventoryItem) {
                this.serialisedInventoryItem = this.inventoryItems[0];
            }
            if (this.inventoryItems[0] instanceof workspace_1.NonSerialisedInventoryItem) {
                this.nonSerialisedInventoryItem = this.inventoryItems[0];
            }
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    save() {
        this.scope
            .save()
            .subscribe((saved) => {
            this.router.navigate(["/ar/invoice/" + this.invoice.id]);
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    refresh() {
        this.refresh$.next(new Date());
    }
    goBack() {
        window.history.back();
    }
};
InvoiceItemEditComponent = __decorate([
    core_1.Component({
        template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="invoiceItem" (submit)="save()">

    <div class="pad">
      <div *ngIf="invoiceItem.SalesInvoiceItemState">
        <a-mat-static [object]="invoiceItem" [roleType]="m.SalesInvoiceItem.SalesInvoiceItemState" display="Name" label="Status"></a-mat-static>
        <!-- <button *ngIf="orderItem.CanExecuteSubmit" mat-button type="button" (click)="submit()">Submit</button>
        <button *ngIf="orderItem.CanExecuteCancel" mat-button type="button" (click)="cancel()">Cancel</button> -->
      </div>

      <!-- <a-mat-static *ngIf="quoteItem.RequestItem" [object]="quoteItem" [roleType]="m.QuoteItem.RequestItem" [display]=""></a-mat-static> -->
      <a-mat-autocomplete *ngIf="!invoiceItem.SalesInvoiceItemType || invoiceItem.SalesInvoiceItemType === productItemType"
        [object]="invoiceItem" [roleType]="m.SalesInvoiceItem.Product" [options]="goods" display="Name"
        (onSelect)="goodSelected($event)" [filter]="goodsFilter.create()"></a-mat-autocomplete>
      <a-mat-select  [object]="invoiceItem" [roleType]="m.SalesInvoiceItem.SalesInvoiceItemType" [options]="salesInvoiceItemTypes" display="Name"></a-mat-select>
      <a-mat-input *ngIf="invoiceItem.Product" [object]="invoiceItem" [roleType]="m.SalesInvoiceItem.Quantity"></a-mat-input>
      <a-mat-input [object]="invoiceItem" [roleType]="m.SalesInvoiceItem.ActualUnitPrice"></a-mat-input>
      <a-mat-static [object]="invoiceItem" [roleType]="m.SalesInvoiceItem.UnitVat"></a-mat-static>
      <a-mat-static [object]="invoiceItem" [roleType]="m.SalesInvoiceItem.TotalIncVat"></a-mat-static>
      <a-mat-static *ngIf="serialisedInventoryItem?.ExpectedSalesPrice" [object]="serialisedInventoryItem" [roleType]="m.SerialisedInventoryItem.ExpectedSalesPrice"></a-mat-static>
      <br/>
      <a-mat-static *ngIf="orderItem?.Comment" [object]="orderItem" [roleType]="m.SalesOrderItem.Comment"></a-mat-static>
      <a-mat-textarea [object]="invoiceItem" [roleType]="m.SalesInvoiceItem.Comment"></a-mat-textarea>
      <a-mat-static *ngIf="orderItem?.InternalComment" [object]="orderItem" [roleType]="m.SalesOrderItem.InternalComment"></a-mat-static>
      <a-mat-textarea [object]="invoiceItem" [roleType]="m.SalesInvoiceItem.InternalComment"></a-mat-textarea>
      <br/>
      <a-mat-textarea [object]="invoiceItem" [roleType]="m.SalesInvoiceItem.Message"></a-mat-textarea>
    </div>

    <mat-divider></mat-divider>

    <mat-card-actions>
      <button mat-button color="primary" type="submit" [disabled]="!form.form.valid">SAVE</button>
      <button mat-button (click)="goBack()" type="button">CANCEL</button>
    </mat-card-actions>
  </form>
</td-layout-card-over>
`,
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.Router,
        router_1.ActivatedRoute,
        material_1.MatSnackBar,
        core_2.TdDialogService,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], InvoiceItemEditComponent);
exports.InvoiceItemEditComponent = InvoiceItemEditComponent;
