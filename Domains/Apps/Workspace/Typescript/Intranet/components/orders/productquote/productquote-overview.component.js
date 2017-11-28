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
        templateUrl: "./productquote-overview.component.html",
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
