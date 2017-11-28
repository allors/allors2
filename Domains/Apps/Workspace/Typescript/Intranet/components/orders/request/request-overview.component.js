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

<div body *ngIf="request" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Request for Quote</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.RequestState" display="Name" label="Status"></a-mat-static>
        </div>
        <div layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.RequestNumber"></a-mat-static>
        </div>
        <div *ngIf="quote" layout="row" class="link" [routerLink]="['/orders/productQuote/' + quote.id ]">
          <a-mat-static [object]="quote" [roleType]="m.Quote.QuoteNumber"></a-mat-static>
        </div>
        <div *ngIf="request.Originator?.objectType.name == 'Person'" layout="row" class="link" [routerLink]="['/relations/person/' + request.Originator.id ]">
          <a-mat-static [object]="request" [roleType]="m.Request.Originator" display="displayName" label="From"></a-mat-static>
        </div>
        <div *ngIf="request.Originator?.objectType.name == 'Organisation'" layout="row" class="link" [routerLink]="['/relations/organisation/' + request.Originator.id ]">
          <a-mat-static [object]="request" [roleType]="m.Request.Originator" display="displayName" label="From"></a-mat-static>
        </div>
        <div *ngIf="request.ContactPerson" layout="row" class="link" [routerLink]="['/relations/person/' + request.ContactPerson.id ]">
          <a-mat-static [object]="request" [roleType]="m.Request.ContactPerson" display="displayName"></a-mat-static>
        </div>
        <div *ngIf="request.FullfillContactMechanism" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.FullfillContactMechanism" display="displayName" label="Reply to"></a-mat-static>
        </div>
        <div *ngIf="request.EmailAddress" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.EmailAddress" display="displayName"></a-mat-static>
        </div>
        <div *ngIf="request.TelephoneNumber" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.TelephoneCountryCode"></a-mat-static>
          <a-mat-static [object]="request" [roleType]="m.Request.TelephoneNumber"></a-mat-static>
        </div>
        <div *ngIf="request.Description" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.Description"></a-mat-static>
        </div>
        <div *ngIf="request.Comment" layout="row">
          <a-mat-textarea [object]="request" [roleType]="m.Request.Comment"></a-mat-textarea>
        </div>
        <div *ngIf="request.InternalComment" layout="row">
          <a-mat-textarea [object]="request" [roleType]="m.Request.InternalComment"></a-mat-textarea>
        </div>
        <div *ngIf="request.RequestDate" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.RequestDate"></a-mat-static>
        </div>
        <div *ngIf="request.RequiredResponseDate" layout="row">
          <a-mat-static [object]="request" [roleType]="m.Request.RequiredResponseDate"></a-mat-static>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> created: {{ request.CreationDate | date}} by {{ request.CreatedBy.displayName}}</p>
        </div>
        <div layout="row">
          <p class="mat-caption tc-grey-500"> last modified: {{ request.LastModifiedDate | date}} by {{ request.LastModifiedBy.displayName}}</p>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button *ngIf="request.CanWriteRequestItems" mat-button [routerLink]="['/request/' + request.id ]">Edit</button>
        <button *ngIf="request.CanExecuteCreateQuote && request.RequestItems.length > 0" mat-button (click)="createQuote()">Create Quote</button>
      </mat-card-actions>
    </mat-card>

    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Request Items</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>
        <ng-template tdLoading="request.RequestItems">
          <mat-list class="will-load">
            <div class="mat-padding" *ngIf="request.RequestItems.length === 0" layout="row" layout-align="center center">
              <h3>No Items to display.</h3>
            </div>

            <ng-template let-requestItem let-last="last" ngFor [ngForOf]="request.RequestItems">
              <mat-list-item>
                <h3 *ngIf="requestItem.Product" mat-line [routerLink]="['/request/' + request.id + '/item/' + requestItem.id ]">{{requestItem.Quantity}} * {{requestItem.Product.Name}}</h3>
                <h4 *ngIf="serialisedInventoryItem?.ExpectedSalesPrice" mat-line>{{ serialisedInventoryItem.ExpectedSalesPrice }}</h4>
                <span>
                  <button mat-icon-button [mat-menu-trigger-for]="menu">
                    <mat-icon>more_vert</mat-icon>
                  </button>
                  <mat-menu x-position="before" #menu="matMenu">
                    <a [routerLink]="['/request/' + request.id + '/item/' + requestItem.id ]" mat-menu-item>Edit</a>
                    <button  mat-menu-item (click)="deleteRequestItem(requestItem)" [disabled]="!requestItem.CanExecuteDelete">Delete</button>
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
        <button *ngIf="request.CanWriteRequestItems" mat-button [routerLink]="['/request/' + request.id +'/item']">+ Add</button>
      </mat-card-actions>
    </mat-card>
  </div>

  <div flex-gt-xs="50">
    <mat-card tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Status History</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <ng-template tdLoading="request.RequestStatuses">
          <mat-list class="will-load">

            <ng-template let-status let-last="last" ngFor [ngForOf]="request.RequestStatuses">
              <mat-list-item>
                <p mat-line class="mat-caption">{{ status.RequestObjectState.Name }} </p>
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
        router_2.Router,
        core_2.TdDialogService,
        material_1.MatSnackBar,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], RequestOverviewComponent);
exports.RequestOverviewComponent = RequestOverviewComponent;
