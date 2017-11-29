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
let QuoteItemEditComponent = class QuoteItemEditComponent {
    constructor(workspaceService, errorService, router, route, snackBar, dialogService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.router = router;
        this.route = route;
        this.snackBar = snackBar;
        this.dialogService = dialogService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Edit Quote Item";
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
                    name: "productQuote",
                }),
                new framework_1.Fetch({
                    id: itemId,
                    include: [
                        new framework_1.TreeNode({ roleType: m.QuoteItem.QuoteItemState }),
                        new framework_1.TreeNode({ roleType: m.QuoteItem.RequestItem }),
                    ],
                    name: "quoteItem",
                }),
                new framework_1.Fetch({
                    id: itemId,
                    name: "requestItem",
                    path: new framework_1.Path({ step: m.QuoteItem.RequestItem }),
                }),
            ];
            const query = [
                new framework_1.Query({
                    name: "goods",
                    objectType: m.Good,
                    sort: [new framework_1.Sort({ roleType: m.Good.Name, direction: "Asc" })],
                }),
                new framework_1.Query({
                    name: "unitsOfMeasure",
                    objectType: m.UnitOfMeasure,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.quote = loaded.objects.productQuote;
            this.quoteItem = loaded.objects.quoteItem;
            this.requestItem = loaded.objects.requestItem;
            this.goods = loaded.collections.goods;
            this.unitsOfMeasure = loaded.collections.unitsOfMeasure;
            const piece = this.unitsOfMeasure.find((v) => v.UniqueId.toUpperCase() === "F4BBDB52-3441-4768-92D4-729C6C5D6F1B");
            if (!this.quoteItem) {
                this.title = "Add Quote Item";
                this.quoteItem = this.scope.session.create("QuoteItem");
                this.quoteItem.UnitOfMeasure = piece;
                this.quote.AddQuoteItem(this.quoteItem);
            }
            else {
                this.goodSelected(this.quoteItem.Product);
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
    submit() {
        const submitFn = () => {
            this.scope.invoke(this.quoteItem.Submit)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully submitted.", "close", { duration: 5000 });
            }, (error) => {
                this.errorService.dialog(error);
            });
        };
        if (this.scope.session.hasChanges) {
            this.dialogService
                .openConfirm({ message: "Save changes?" })
                .afterClosed().subscribe((confirm) => {
                if (confirm) {
                    this.scope
                        .save()
                        .subscribe((saved) => {
                        this.scope.session.reset();
                        submitFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    submitFn();
                }
            });
        }
        else {
            submitFn();
        }
    }
    cancel() {
        const cancelFn = () => {
            this.scope.invoke(this.quoteItem.Cancel)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
            }, (error) => {
                this.errorService.dialog(error);
            });
        };
        if (this.scope.session.hasChanges) {
            this.dialogService
                .openConfirm({ message: "Save changes?" })
                .afterClosed().subscribe((confirm) => {
                if (confirm) {
                    this.scope
                        .save()
                        .subscribe((saved) => {
                        this.scope.session.reset();
                        cancelFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    cancelFn();
                }
            });
        }
        else {
            cancelFn();
        }
    }
    save() {
        this.scope
            .save()
            .subscribe((saved) => {
            this.router.navigate(["/orders/productQuote/" + this.quote.id]);
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
QuoteItemEditComponent = __decorate([
    core_1.Component({
        template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="quoteItem" (submit)="save()">

    <div class="pad">
      <div *ngIf="quoteItem.QuoteItemState">
        <a-mat-static [object]="quoteItem" [roleType]="m.QuoteItem.QuoteItemState" display="Name" label="Status"></a-mat-static>
        <button *ngIf="quoteItem.CanExecuteSubmit" mat-button type="button" (click)="submit()">Submit</button>
        <button *ngIf="quoteItem.CanExecuteCancel" mat-button type="button" (click)="cancel()">Cancel</button>
      </div>

      <a-mat-autocomplete [object]="quoteItem" [roleType]="m.QuoteItem.Product" [options]="goods" display="Name" [filter]="goodsFilter.create()"
        (onSelect)="goodSelected($event)"></a-mat-autocomplete>
      <a-mat-input [object]="quoteItem" [roleType]="m.QuoteItem.Quantity"></a-mat-input>
      <a-mat-select [object]="quoteItem" [roleType]="m.QuoteItem.UnitOfMeasure" [options]="unitsOfMeasure" display="Name"></a-mat-select>
      <a-mat-input [object]="quoteItem" [roleType]="m.QuoteItem.UnitPrice"></a-mat-input>
      <a-mat-static *ngIf="requestItem" [object]="requestItem" [roleType]="m.RequestItem.MaximumAllowedPrice"></a-mat-static>
      <a-mat-static *ngIf="serialisedInventoryItem?.ExpectedSalesPrice" [object]="serialisedInventoryItem" [roleType]="m.SerialisedInventoryItem.ExpectedSalesPrice"></a-mat-static>
      <a-mat-datepicker [object]="quoteItem" [roleType]="m.QuoteItem.RequiredByDate" disabled="true"></a-mat-datepicker>
      <a-mat-datepicker [object]="quoteItem" [roleType]="m.QuoteItem.EstimatedDeliveryDate"></a-mat-datepicker>
      <a-mat-static *ngIf="requestItem?.Comment" [object]="requestItem" [roleType]="m.RequestItem.Comment"></a-mat-static>
      <a-mat-textarea [object]="quoteItem" [roleType]="m.QuoteItem.Comment"></a-mat-textarea>
      <a-mat-static *ngIf="requestItem?.InternalComment" [object]="requestItem" [roleType]="m.RequestItem.InternalComment"></a-mat-static>
      <a-mat-textarea [object]="quoteItem" [roleType]="m.QuoteItem.InternalComment"></a-mat-textarea>
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
], QuoteItemEditComponent);
exports.QuoteItemEditComponent = QuoteItemEditComponent;
