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
const workspace_1 = require("@allors/workspace");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let SalesOrderItemEditComponent = class SalesOrderItemEditComponent {
    constructor(workspaceService, errorService, router, route, snackBar, dialogService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.router = router;
        this.route = route;
        this.snackBar = snackBar;
        this.dialogService = dialogService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Edit Sales Order Item";
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.scope = this.workspaceService.createScope();
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
        this.goodsFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Good, roleTypes: [this.m.Good.Name] });
    }
    ngOnInit() {
        const route$ = this.route.url;
        const combined$ = Rx_1.Observable.combineLatest(route$, this.refresh$);
        this.subscription = combined$
            .switchMap(([urlSegments, date]) => {
            const id = this.route.snapshot.paramMap.get("id");
            const itemId = this.route.snapshot.paramMap.get("itemId");
            const m = this.m;
            const fetch = [
                new framework_1.Fetch({
                    id,
                    name: "salesOrder",
                }),
                new framework_1.Fetch({
                    id: itemId,
                    include: [
                        new framework_1.TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemState }),
                        new framework_1.TreeNode({ roleType: m.SalesOrderItem.QuoteItem }),
                        new framework_1.TreeNode({ roleType: m.SalesOrderItem.DiscountAdjustment }),
                        new framework_1.TreeNode({ roleType: m.SalesOrderItem.SurchargeAdjustment }),
                        new framework_1.TreeNode({ roleType: m.SalesOrderItem.DerivedVatRate }),
                        new framework_1.TreeNode({
                            nodes: [new framework_1.TreeNode({ roleType: m.VatRegime.VatRate })],
                            roleType: m.SalesOrderItem.VatRegime,
                        }),
                    ],
                    name: "orderItem",
                }),
            ];
            const query = [
                new framework_1.Query({
                    name: "goods",
                    objectType: m.Good,
                }),
                new framework_1.Query({
                    name: "vatRates",
                    objectType: m.VatRate,
                }),
                new framework_1.Query({
                    name: "vatRegimes",
                    objectType: m.VatRegime,
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
            this.order = loaded.objects.salesOrder;
            this.orderItem = loaded.objects.orderItem;
            this.quoteItem = loaded.objects.quoteItem;
            this.goods = loaded.collections.goods;
            this.vatRates = loaded.collections.vatRates;
            this.vatRegimes = loaded.collections.vatRegimes;
            this.salesInvoiceItemTypes = loaded.collections.salesInvoiceItemTypes;
            this.productItemType = this.salesInvoiceItemTypes.find((v) => v.UniqueId.toUpperCase() === "0D07F778-2735-44CB-8354-FB887ADA42AD");
            if (!this.orderItem) {
                this.title = "Add Order Item";
                this.orderItem = this.scope.session.create("SalesOrderItem");
                this.order.AddSalesOrderItem(this.orderItem);
            }
            else {
                if (this.orderItem.ItemType === this.productItemType) {
                    this.goodSelected(this.orderItem.Product);
                }
                if (this.orderItem.DiscountAdjustment) {
                    this.discount = this.orderItem.DiscountAdjustment.Amount;
                }
                if (this.orderItem.SurchargeAdjustment) {
                    this.surcharge = this.orderItem.SurchargeAdjustment.Amount;
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
        this.orderItem.ItemType = this.productItemType;
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
        // if (this.discount !== 0) {
        //   const discountAdjustment = this.scope.session.create("DiscountAdjustment") as DiscountAdjustment;
        //   discountAdjustment.Amount = this.discount;
        //   this.orderItem.DiscountAdjustment = discountAdjustment;
        // }
        // if (this.surcharge !== 0) {
        //   const surchargeAdjustment = this.scope.session.create("SurchargeAdjustment") as SurchargeAdjustment;
        //   surchargeAdjustment.Amount = this.surcharge;
        //   this.orderItem.SurchargeAdjustment = surchargeAdjustment;
        // }
        this.scope
            .save()
            .subscribe((saved) => {
            this.router.navigate(["/orders/salesOrder/" + this.order.id]);
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
SalesOrderItemEditComponent = __decorate([
    core_1.Component({
        templateUrl: "./salesorderitem.component.html",
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.Router,
        router_1.ActivatedRoute,
        material_1.MatSnackBar,
        core_2.TdDialogService,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], SalesOrderItemEditComponent);
exports.SalesOrderItemEditComponent = SalesOrderItemEditComponent;
