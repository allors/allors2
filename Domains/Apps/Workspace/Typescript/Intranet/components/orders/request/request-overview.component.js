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
const router_2 = require("@angular/router");
const core_2 = require("@covalent/core");
const Rx_1 = require("rxjs/Rx");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let RequestOverviewComponent = class RequestOverviewComponent {
    constructor(workspaceService, errorService, route, router, dialogService, snackBar, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.router = router;
        this.dialogService = dialogService;
        this.snackBar = snackBar;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Requests Overview";
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
    }
    refresh() {
        this.refresh$.next(new Date());
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
                                new framework_1.TreeNode({ roleType: m.RequestItem.Product }),
                            ],
                            roleType: m.Request.RequestItems,
                        }),
                        new framework_1.TreeNode({ roleType: m.Request.RequestItems }),
                        new framework_1.TreeNode({ roleType: m.Request.Originator }),
                        new framework_1.TreeNode({ roleType: m.Request.ContactPerson }),
                        new framework_1.TreeNode({ roleType: m.Request.RequestState }),
                        new framework_1.TreeNode({ roleType: m.Request.Currency }),
                        new framework_1.TreeNode({ roleType: m.Request.CreatedBy }),
                        new framework_1.TreeNode({ roleType: m.Request.LastModifiedBy }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({
                                    nodes: [
                                        new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
                                    ],
                                    roleType: m.PostalAddress.PostalBoundary,
                                }),
                            ],
                            roleType: m.Request.FullfillContactMechanism,
                        }),
                    ],
                    name: "request",
                }),
            ];
            const quoteFetch = new framework_1.Fetch({
                id,
                name: "quote",
                path: new framework_1.Path({ step: m.RequestForQuote.QuoteWhereRequest }),
            });
            if (id != null) {
                fetch.push(quoteFetch);
            }
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch }));
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.request = loaded.objects.request;
            this.quote = loaded.objects.quote;
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
    createQuote() {
        this.scope.invoke(this.request.CreateQuote)
            .subscribe((invoked) => {
            this.snackBar.open("Quote successfully created.", "close", { duration: 5000 });
            this.gotoQuote();
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    deleteRequestItem(requestItem) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this item?" })
            .afterClosed()
            .subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(requestItem.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    gotoQuote() {
        const fetch = [new framework_1.Fetch({
                id: this.request.id,
                name: "quote",
                path: new framework_1.Path({ step: this.m.RequestForQuote.QuoteWhereRequest }),
            })];
        this.scope.load("Pull", new framework_1.PullRequest({ fetch }))
            .subscribe((loaded) => {
            const quote = loaded.objects.quote;
            this.router.navigate(["/orders/productQuote/" + quote.id]);
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
};
RequestOverviewComponent = __decorate([
    core_1.Component({
        templateUrl: "./request-overview.component.html",
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        router_2.Router,
        core_2.TdDialogService,
        material_1.MatSnackBar,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], RequestOverviewComponent);
exports.RequestOverviewComponent = RequestOverviewComponent;
