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
const framework_1 = require("@allors/framework");
const base_angular_1 = require("@allors/base-angular");
const newgood_dialog_module_1 = require("../../catalogues/good/newgood-dialog.module");
let GoodsOverviewComponent = class GoodsOverviewComponent {
    constructor(workspaceService, errorService, formBuilder, titleService, dialog, snackBar, router, dialogService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.formBuilder = formBuilder;
        this.titleService = titleService;
        this.dialog = dialog;
        this.snackBar = snackBar;
        this.router = router;
        this.dialogService = dialogService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Products";
        this.titleService.setTitle("Products");
        this.scope = this.workspaceService.createScope();
        this.refresh$ = new BehaviorSubject_1.BehaviorSubject(undefined);
        this.chosenGood = "Serialised";
        this.searchForm = this.formBuilder.group({
            articleNumber: [""],
            brand: [""],
            inventoryItemKind: [""],
            keyword: [""],
            manufacturer: [""],
            model: [""],
            name: [""],
            owner: [""],
            ownership: [""],
            productCategory: [""],
            productType: [""],
            supplier: [""]
        });
        this.page$ = new BehaviorSubject_1.BehaviorSubject(50);
        const search$ = this.searchForm.valueChanges
            .debounceTime(400)
            .distinctUntilChanged()
            .startWith({});
        const combined$ = Observable_1.Observable.combineLatest(search$, this.page$, this.refresh$).scan(([previousData, previousTake, previousDate], [data, take, date]) => {
            return [data, data !== previousData ? 50 : take, date];
        }, []);
        const m = this.workspaceService.metaPopulation.metaDomain;
        this.subscription = combined$
            .switchMap(([data, take]) => {
            const rolesQuery = [
                new framework_1.Query({
                    name: "organisationRoles",
                    objectType: m.OrganisationRole
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ query: rolesQuery }))
                .switchMap((rolesLoaded) => {
                const organisationRoles = rolesLoaded
                    .collections.organisationRoles;
                const manufacturerRole = organisationRoles.find((v) => v.Name === "Manufacturer");
                const supplierRole = organisationRoles.find((v) => v.Name === "Supplier");
                const searchQuery = [
                    new framework_1.Query({
                        name: "brands",
                        objectType: m.Brand,
                    }),
                    new framework_1.Query({
                        name: "models",
                        objectType: m.Model,
                    }),
                    new framework_1.Query({
                        name: "inventoryItemKinds",
                        objectType: m.InventoryItemKind,
                    }),
                    new framework_1.Query({
                        name: "categories",
                        objectType: m.ProductCategory,
                    }),
                    new framework_1.Query({
                        name: "productTypes",
                        objectType: m.ProductType,
                    }),
                    new framework_1.Query({
                        name: "organisations",
                        objectType: m.ProductType,
                    }),
                    new framework_1.Query({
                        name: "ownerships",
                        objectType: m.Ownership,
                    }),
                    new framework_1.Query({
                        name: "manufacturers",
                        objectType: m.Organisation,
                        predicate: new framework_1.Contains({
                            roleType: m.Organisation.OrganisationRoles,
                            object: manufacturerRole,
                        }),
                        sort: [
                            new framework_1.Sort({ roleType: m.Organisation.Name, direction: "Asc" }),
                        ],
                    }),
                    new framework_1.Query({
                        name: "suppliers",
                        objectType: m.Organisation,
                        predicate: new framework_1.Contains({
                            roleType: m.Organisation.OrganisationRoles,
                            object: supplierRole,
                        }),
                        sort: [
                            new framework_1.Sort({ roleType: m.Organisation.Name, direction: "Asc" }),
                        ],
                    }),
                ];
                return this.scope
                    .load("Pull", new framework_1.PullRequest({ query: searchQuery }))
                    .switchMap((loaded) => {
                    this.brands = loaded.collections.brands;
                    this.brand = this.brands.find((v) => v.Name === data.brand);
                    this.models = loaded.collections.models;
                    this.model = this.models.find((v) => v.Name === data.model);
                    this.inventoryItemKinds = loaded.collections
                        .inventoryItemKinds;
                    this.inventoryItemKind = this.inventoryItemKinds.find((v) => v.Name === data.inventoryItemKind);
                    this.productCategories = loaded.collections
                        .categories;
                    this.productCategory = this.productCategories.find((v) => v.Name === data.productCategory);
                    this.productTypes = loaded.collections
                        .productTypes;
                    this.productType = this.productTypes.find((v) => v.Name === data.productType);
                    this.ownerships = loaded.collections.ownerships;
                    this.ownership = this.ownerships.find((v) => v.Name === data.ownership);
                    this.manufacturers = loaded.collections
                        .manufacturers;
                    this.manufacturer = this.manufacturers.find((v) => v.Name === data.manufacturer);
                    this.suppliers = loaded.collections.suppliers;
                    this.supplier = this.suppliers.find((v) => v.Name === data.supplier);
                    const goodsPredicate = new framework_1.And();
                    const goodsPredicates = goodsPredicate.predicates;
                    if (data.name) {
                        const like = data.name.replace("*", "%") + "%";
                        goodsPredicates.push(new framework_1.Like({ roleType: m.Good.Name, value: like }));
                    }
                    if (data.articleNumber) {
                        const like = data.articleNumber.replace("*", "%") + "%";
                        goodsPredicates.push(new framework_1.Like({ roleType: m.Good.ArticleNumber, value: like }));
                    }
                    if (data.keyword) {
                        const like = data.keyword.replace("*", "%") + "%";
                        goodsPredicates.push(new framework_1.Like({ roleType: m.Good.Keywords, value: like }));
                    }
                    if (data.brand) {
                        goodsPredicates.push(new framework_1.Contains({
                            roleType: m.Good.StandardFeatures,
                            object: this.brand,
                        }));
                    }
                    if (data.model) {
                        goodsPredicates.push(new framework_1.Contains({
                            roleType: m.Good.StandardFeatures,
                            object: this.model,
                        }));
                    }
                    if (data.productCategory) {
                        goodsPredicates.push(new framework_1.Contains({
                            roleType: m.Good.ProductCategories,
                            object: this.productCategory,
                        }));
                    }
                    if (data.inventoryItemKind) {
                        goodsPredicates.push(new framework_1.Equals({
                            roleType: m.Good.InventoryItemKind,
                            value: this.inventoryItemKind,
                        }));
                    }
                    if (data.manufacturer) {
                        goodsPredicates.push(new framework_1.Equals({
                            roleType: m.Good.ManufacturedBy,
                            value: this.manufacturer,
                        }));
                    }
                    if (data.supplier) {
                        goodsPredicates.push(new framework_1.Equals({
                            roleType: m.Good.SuppliedBy,
                            value: this.supplier,
                        }));
                    }
                    if (data.owner || data.ownership) {
                        const inventoryPredicate = new framework_1.And();
                        const inventoryPredicates = inventoryPredicate.predicates;
                        if (data.owner) {
                            const like = data.owner.replace("*", "%") + "%";
                            inventoryPredicates.push(new framework_1.Like({
                                roleType: m.SerialisedInventoryItem.Owner,
                                value: like,
                            }));
                        }
                        if (data.ownership) {
                            inventoryPredicates.push(new framework_1.Equals({
                                roleType: m.SerialisedInventoryItem.Ownership,
                                value: this.ownership,
                            }));
                        }
                        const serialisedInventoryQuery = new framework_1.Query({
                            objectType: m.SerialisedInventoryItem,
                            predicate: inventoryPredicate,
                        });
                        const containedIn = new framework_1.ContainedIn({
                            associationType: m.Good.InventoryItemsWhereGood,
                            query: serialisedInventoryQuery,
                        });
                        goodsPredicates.push(containedIn);
                    }
                    if (data.productType) {
                        const inventoryPredicate = new framework_1.And();
                        const inventoryPredicates = inventoryPredicate.predicates;
                        if (data.productType) {
                            inventoryPredicates.push(new framework_1.Equals({
                                roleType: m.InventoryItem.ProductType,
                                value: this.productType,
                            }));
                        }
                        const inventoryQuery = new framework_1.Query({
                            objectType: m.InventoryItem,
                            predicate: inventoryPredicate,
                        });
                        const containedIn = new framework_1.ContainedIn({
                            associationType: m.Good.InventoryItemsWhereGood,
                            query: inventoryQuery,
                        });
                        goodsPredicates.push(containedIn);
                    }
                    const goodsQuery = [
                        new framework_1.Query({
                            include: [
                                new framework_1.TreeNode({ roleType: m.Good.PrimaryPhoto }),
                                new framework_1.TreeNode({ roleType: m.Good.LocalisedNames }),
                                new framework_1.TreeNode({ roleType: m.Good.LocalisedDescriptions }),
                                new framework_1.TreeNode({ roleType: m.Good.PrimaryProductCategory }),
                            ],
                            name: "goods",
                            objectType: m.Good,
                            page: new framework_1.Page({ skip: 0, take }),
                            predicate: goodsPredicate,
                        }),
                    ];
                    return this.scope.load("Pull", new framework_1.PullRequest({ query: goodsQuery }));
                });
            });
        })
            .subscribe((loaded) => {
            this.data = loaded.collections.goods;
            this.total = loaded.values.goods_total;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    more() {
        this.page$.next(this.data.length + 50);
    }
    delete(good) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this product?" })
            .afterClosed()
            .subscribe((confirm) => {
            if (confirm) {
                // TODO: Logical, physical or workflow delete
            }
        });
    }
    addGood() {
        const dialogRef = this.dialog.open(newgood_dialog_module_1.NewGoodDialogComponent, {
            data: { chosenGood: this.chosenGood },
            height: "300px",
            width: "700px",
        });
        dialogRef.afterClosed().subscribe((answer) => {
            if (answer === "Serialised") {
                this.router.navigate(["/serialisedGood"]);
            }
            if (answer === "NonSerialised") {
                this.router.navigate(["/nonSerialisedGood"]);
            }
        });
    }
    serialisedGood(good) {
        return (good.InventoryItemKind ===
            this.inventoryItemKinds.find((v) => v.UniqueId.toUpperCase() === "2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE"));
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
    onView(good) {
        this.router.navigate(["/good/" + good.id]);
    }
};
GoodsOverviewComponent = __decorate([
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
    <div class="grid-8_xs-1">
        <mat-input-container class="col">
          <input matInput placeholder="Name" formControlName="name">
        </mat-input-container>
        <mat-input-container class="col">
          <input matInput placeholder="ArticleNr" formControlName="articleNumber">
        </mat-input-container>
        <mat-input-container class="col">
          <input matInput placeholder="Keyword" formControlName="keyword">
        </mat-input-container>
        <mat-input-container class="col">
          <input matInput placeholder="Owner" formControlName="owner">
          <mat-icon matSuffix>search</mat-icon>
        </mat-input-container>
        </div>
        <div class="grid-8_xs-1">
        <mat-input-container class="col">
          <mat-select formControlName="ownership" name="ownership" [(ngModel)]="selectedOwnership" placeholder="Ownership" >
          <mat-option>None</mat-option>
            <mat-option *ngFor="let ownership of ownerships" [value]="ownership.Name">{{ ownership.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="brand" name="brand" [(ngModel)]="selectedBrand" placeholder="Brand">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let brand of brands" [value]="brand.Name">{{ brand.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="model" name="model" [(ngModel)]="selectedModel" placeholder="Model">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let model of models" [value]="model.Name">{{ model.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="productCategory" name="productCategory" [(ngModel)]="selectedProductCategory" placeholder="Category">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let productCategory of productCategories" [value]="productCategory.Name">{{ productCategory.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="productType" name="productType" [(ngModel)]="selectedProductType" placeholder="Product Type">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let productType of productTypes" [value]="productType.Name">{{ productType.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="inventoryItemKind" name="inventoryItemKind" [(ngModel)]="selectedInventoryItemKind" placeholder="Inventory Kind">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let inventoryItemKind of inventoryItemKinds" [value]="inventoryItemKind.Name">{{ inventoryItemKind.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="manufacturer" name="manufacturer" [(ngModel)]="selectedManufacturer" placeholder="Manufacturer">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let manufacturer of manufacturers" [value]="manufacturer.Name">{{ manufacturer.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="supplier" name="supplier" [(ngModel)]="selectedSupplier" placeholder="Supplier">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let supplier of suppliers" [value]="supplier.Name">{{ supplier.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
      </div>
      </form>
  </div>

  <mat-divider></mat-divider>

  <ng-template tdLoading="data">
    <mat-list class="will-load">
      <div class="mat-padding" *ngIf="data && data.length === 0" layout="row" layout-align="center center">
        <h3>No products to display.</h3>
      </div>
      <ng-template let-good let-last="last" ngFor [ngForOf]="data">
        <mat-list-item>

          <mat-icon *ngIf="!good.PrimaryPhoto" mat-list-avatar>photo_camera</mat-icon>
          <img *ngIf="good.PrimaryPhoto" mat-list-avatar  src="http://localhost:5000/Media/Download/{{good.PrimaryPhoto.UniqueId}}?revision={{good.PrimaryPhoto.Revision}}"/>

          <h3 *ngIf="serialisedGood(good)" mat-line [routerLink]="'/serialisedGood/' + [good.id]"> {{good.Name}}</h3>
          <h3 *ngIf="!serialisedGood(good)" mat-line [routerLink]="'/nonSerialisedGood/' + [good.id]"> {{good.Name}}</h3>
          <p mat-line class="mat-caption">{{ good.Description }} </p>
          <p mat-line class="mat-caption">Primary category: {{ good.PrimaryProductCategory?.Name }} </p>

          <span>
                <button mat-icon-button [mat-menu-trigger-for]="menu">
                <mat-icon>more_vert</mat-icon>
                </button>
                <mat-menu x-position="before" #menu="matMenu">
                <a *ngIf="serialisedGood(good)" [routerLink]="'/serialisedGood/' + [good.id]" mat-menu-item>Edit</a>
                <a *ngIf="!serialisedGood(good)" [routerLink]="'/nonSerialisedGood/' + [good.id]" mat-menu-item>Edit</a>
                <button mat-menu-item (click)="delete(good)">Delete</button>
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

<span flex>
  <a mat-fab color="accent" class="mat-fab-bottom-right fixed" (click)="addGood()">
    <mat-icon>add</mat-icon>
  </a>
</span>
`
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        forms_1.FormBuilder,
        platform_browser_1.Title,
        material_1.MatDialog,
        material_1.MatSnackBar,
        router_1.Router,
        core_2.TdDialogService,
        core_2.TdMediaService,
        core_1.ChangeDetectorRef])
], GoodsOverviewComponent);
exports.GoodsOverviewComponent = GoodsOverviewComponent;
