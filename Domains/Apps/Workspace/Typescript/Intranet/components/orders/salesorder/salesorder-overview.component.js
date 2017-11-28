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
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
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
        templateUrl: "./salesorder-overview.component.html",
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
