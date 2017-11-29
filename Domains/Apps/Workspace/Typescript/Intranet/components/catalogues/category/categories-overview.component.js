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
const forms_1 = require("@angular/forms");
const material_1 = require("@angular/material");
const platform_browser_1 = require("@angular/platform-browser");
const router_1 = require("@angular/router");
const BehaviorSubject_1 = require("rxjs/BehaviorSubject");
const Observable_1 = require("rxjs/Observable");
require("rxjs/add/observable/combineLatest");
const core_2 = require("@covalent/core");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let CategoriesOverviewComponent = class CategoriesOverviewComponent {
    constructor(workspaceService, errorService, formBuilder, titleService, snackBar, router, dialogService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.formBuilder = formBuilder;
        this.titleService = titleService;
        this.snackBar = snackBar;
        this.router = router;
        this.dialogService = dialogService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Categories";
        this.titleService.setTitle("Categories");
        this.scope = this.workspaceService.createScope();
        this.refresh$ = new BehaviorSubject_1.BehaviorSubject(undefined);
        this.searchForm = this.formBuilder.group({
            name: [""],
        });
        this.page$ = new BehaviorSubject_1.BehaviorSubject(50);
        const search$ = this.searchForm.valueChanges
            .debounceTime(400)
            .distinctUntilChanged()
            .startWith({});
        const combined$ = Observable_1.Observable
            .combineLatest(search$, this.page$, this.refresh$)
            .scan(([previousData, previousTake, previousDate], [data, take, date]) => {
            return [
                data,
                data !== previousData ? 50 : take,
                date,
            ];
        }, []);
        this.subscription = combined$
            .switchMap(([data, take]) => {
            const m = this.workspaceService.metaPopulation.metaDomain;
            const predicate = new framework_1.And();
            const predicates = predicate.predicates;
            if (data.name) {
                const like = data.name.replace("*", "%") + "%";
                predicates.push(new framework_1.Like({ roleType: m.ProductCategory.Name, value: like }));
            }
            const query = [new framework_1.Query({
                    include: [
                        new framework_1.TreeNode({ roleType: m.ProductCategory.CategoryImage }),
                        new framework_1.TreeNode({ roleType: m.ProductCategory.LocalisedNames }),
                        new framework_1.TreeNode({ roleType: m.ProductCategory.LocalisedDescriptions }),
                    ],
                    name: "categories",
                    objectType: m.ProductCategory,
                    page: new framework_1.Page({ skip: 0, take }),
                    predicate,
                })];
            return this.scope.load("Pull", new framework_1.PullRequest({ query }));
        })
            .subscribe((loaded) => {
            this.data = loaded.collections.categories;
            this.total = loaded.values.categories_total;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    more() {
        this.page$.next(this.data.length + 50);
    }
    goBack() {
        window.history.back();
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
    delete(category) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this category?" })
            .afterClosed().subscribe((confirm) => {
            if (confirm) {
                // TODO: Logical, physical or workflow delete
            }
        });
    }
    onView(category) {
        this.router.navigate(["/category/" + category.id]);
    }
};
CategoriesOverviewComponent = __decorate([
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


<mat-card>
  <div class="pad-top-xs pad-left pad-right">
    <form novalidate [formGroup]="searchForm">
      <mat-input-container>
        <input fxFlex matInput placeholder="Name" formControlName="name">
        <mat-icon matSuffix>search</mat-icon>
      </mat-input-container>
    </form>
  </div>

  <mat-divider></mat-divider>
  <ng-template tdLoading="data">
    <mat-list class="will-load">
      <div class="mat-padding" *ngIf="data && data.length === 0" layout="row" layout-align="center center">
        <h3>No categories to display.</h3>
      </div>
      <ng-template let-category let-last="last" ngFor [ngForOf]="data">
        <mat-list-item>
          <mat-icon *ngIf="!category.CategoryImage" mat-list-avatar>photo_camera</mat-icon>
          <img *ngIf="category.CategoryImage" mat-list-avatar src="http://localhost:5000/Media/Download/{{category.CategoryImage.UniqueId}}?revision={{category.CategoryImage.Revision}}"
          />

          <h3 mat-line [routerLink]="'/category/' + [category.id]"> {{category.Name}}</h3>
          <p mat-line> {{category.Description}} </p>

          <span>
            <button mat-icon-button [mat-menu-trigger-for]="menu">
            <mat-icon>more_vert</mat-icon>
            </button>
            <mat-menu x-position="before" #menu="matMenu">
            <a [routerLink]="'/category/' + [category.id]" mat-menu-item>Edit</a>
            <button mat-menu-item (click)="delete(category)">Delete</button>
            </mat-menu>
          </span>
        </mat-list-item>
        <mat-divider *ngIf="!last" mat-inset></mat-divider>
      </ng-template>
    </mat-list>
  </ng-template>
</mat-card>

<mat-card body tdMediaToggle="gt-xs" [mediaClasses]="['push']" *ngIf="this.data && this.data.length !== total">
  <mat-card-content>
    <button mat-button (click)="more()">More</button> {{this.data?.length}}/{{total}}
  </mat-card-content>
</mat-card>

<a mat-fab color="accent" class="mat-fab-bottom-right fixed" [routerLink]="['/category']">
  <mat-icon>add</mat-icon>
</a>
`,
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        forms_1.FormBuilder,
        platform_browser_1.Title,
        material_1.MatSnackBar,
        router_1.Router,
        core_2.TdDialogService,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], CategoriesOverviewComponent);
exports.CategoriesOverviewComponent = CategoriesOverviewComponent;
