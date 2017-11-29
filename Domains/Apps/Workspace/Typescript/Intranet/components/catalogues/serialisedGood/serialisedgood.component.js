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
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let SerialisedGoodComponent = class SerialisedGoodComponent {
    constructor(workspaceService, errorService, route, snackBar, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.snackBar = snackBar;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.manufacturersFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name] });
        this.suppliersFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name] });
        this.refresh$ = new BehaviorSubject_1.BehaviorSubject(undefined);
    }
    ngOnInit() {
        const route$ = this.route.url;
        const combined$ = Observable_1.Observable.combineLatest(route$, this.refresh$);
        this.subscription = combined$
            .switchMap(([urlSegments, date]) => {
            const id = this.route.snapshot.paramMap.get("id");
            const m = this.m;
            const fetch = [
                new framework_1.Fetch({
                    id,
                    include: [
                        new framework_1.TreeNode({ roleType: m.Good.PrimaryPhoto }),
                        new framework_1.TreeNode({ roleType: m.Good.Photos }),
                        new framework_1.TreeNode({ roleType: m.Good.LocalisedNames }),
                        new framework_1.TreeNode({ roleType: m.Good.LocalisedDescriptions }),
                        new framework_1.TreeNode({ roleType: m.Good.LocalisedComments }),
                        new framework_1.TreeNode({ roleType: m.Good.ProductCategories }),
                        new framework_1.TreeNode({ roleType: m.Good.InventoryItemKind }),
                        new framework_1.TreeNode({ roleType: m.Good.SuppliedBy }),
                        new framework_1.TreeNode({ roleType: m.Good.ManufacturedBy }),
                        new framework_1.TreeNode({ roleType: m.Good.StandardFeatures }),
                    ],
                    name: "good",
                }),
                new framework_1.Fetch({
                    id,
                    include: [
                        new framework_1.TreeNode({ roleType: m.SerialisedInventoryItem.Ownership }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({
                                    nodes: [new framework_1.TreeNode({ roleType: m.ProductCharacteristic.LocalisedNames })],
                                    roleType: m.ProductType.ProductCharacteristics,
                                }),
                            ],
                            roleType: m.InventoryItem.ProductType,
                        }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.ProductCharacteristicValue.ProductCharacteristic }),
                            ],
                            roleType: m.InventoryItem.ProductCharacteristicValues,
                        }),
                    ],
                    name: "inventoryItems",
                    path: new framework_1.Path({ step: this.m.Good.InventoryItemsWhereGood }),
                }),
            ];
            const query = [
                new framework_1.Query({
                    include: [
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.Locale.Language }),
                                new framework_1.TreeNode({ roleType: m.Locale.Country }),
                            ],
                            roleType: m.Singleton.Locales,
                        }),
                        new framework_1.TreeNode({
                            nodes: [new framework_1.TreeNode({ roleType: m.InternalOrganisation.DefaultFacility })],
                            roleType: m.Singleton.InternalOrganisation,
                        }),
                    ],
                    name: "singletons",
                    objectType: this.m.Singleton,
                }),
                new framework_1.Query({
                    name: "organisationRoles",
                    objectType: this.m.OrganisationRole,
                }),
                new framework_1.Query({
                    name: "categories",
                    objectType: this.m.ProductCategory,
                }),
                new framework_1.Query({
                    name: "productTypes",
                    objectType: this.m.ProductType,
                }),
                new framework_1.Query({
                    name: "vatRates",
                    objectType: this.m.VatRate,
                }),
                new framework_1.Query({
                    name: "brands",
                    objectType: this.m.Brand,
                    sort: [new framework_1.Sort({ roleType: m.Brand.Name, direction: "Asc" })],
                }),
                new framework_1.Query({
                    name: "ownerships",
                    objectType: this.m.Ownership,
                }),
                new framework_1.Query({
                    name: "inventoryItemKinds",
                    objectType: this.m.InventoryItemKind,
                }),
                new framework_1.Query({
                    name: "serialisedInventoryItemStates",
                    objectType: this.m.SerialisedInventoryItemState,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }))
                .switchMap((loaded) => {
                this.good = loaded.objects.good;
                this.categories = loaded.collections.categories;
                this.productTypes = loaded.collections.productTypes;
                this.vatRates = loaded.collections.vatRates;
                this.brands = loaded.collections.brands;
                this.ownerships = loaded.collections.ownerships;
                this.inventoryItemKinds = loaded.collections.inventoryItemKinds;
                this.serialisedInventoryItemStates = loaded.collections.serialisedInventoryItemStates;
                this.singleton = loaded.collections.singletons[0];
                this.facility = this.singleton.InternalOrganisation.DefaultFacility;
                this.locales = this.singleton.Locales;
                this.selectedLocaleName = this.singleton.DefaultLocale.Name;
                const vatRateZero = this.vatRates.find((v) => v.Rate === 0);
                const inventoryItemKindSerialised = this.inventoryItemKinds.find((v) => v.Name === "Serialised");
                if (this.good === undefined) {
                    this.good = this.scope.session.create("Good");
                    this.good.VatRate = vatRateZero;
                    this.good.Sku = "";
                    this.inventoryItem = this.scope.session.create("SerialisedInventoryItem");
                    this.good.InventoryItemKind = inventoryItemKindSerialised;
                    this.inventoryItem.Good = this.good;
                    this.inventoryItem.Facility = this.facility;
                }
                else {
                    this.inventoryItems = loaded.collections.inventoryItems;
                    this.inventoryItem = this.inventoryItems[0];
                }
                this.title = this.good.Name;
                this.subTitle = "Serialised";
                const organisationRoles = loaded.collections.organisationRoles;
                const manufacturerRole = organisationRoles.find((v) => v.Name === "Manufacturer");
                const supplierRole = organisationRoles.find((v) => v.Name === "Supplier");
                const Query2 = [
                    new framework_1.Query({
                        name: "manufacturers",
                        objectType: m.Organisation,
                        predicate: new framework_1.Contains({ roleType: m.Organisation.OrganisationRoles, object: manufacturerRole }),
                    }),
                    new framework_1.Query({
                        name: "suppliers",
                        objectType: m.Organisation,
                        predicate: new framework_1.Contains({ roleType: m.Organisation.OrganisationRoles, object: supplierRole }),
                    }),
                ];
                return this.scope.load("Pull", new framework_1.PullRequest({ query: Query2 }));
            });
        })
            .subscribe((loaded) => {
            this.manufacturers = loaded.collections.manufacturers;
            this.suppliers = loaded.collections.suppliers;
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
    imageSelected(id) {
        this.good.AddPhoto(this.good.PrimaryPhoto);
        this.update();
        this.snackBar.open("Good succesfully saved.", "close", { duration: 5000 });
    }
    save() {
        this.good.StandardFeatures.forEach((feature) => {
            this.good.RemoveStandardFeature(feature);
        });
        if (this.selectedBrand != null) {
            this.good.AddStandardFeature(this.selectedBrand);
        }
        if (this.selectedModel != null) {
            this.good.AddStandardFeature(this.selectedModel);
        }
        this.scope
            .save()
            .subscribe((saved) => {
            this.goBack();
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    update() {
        this.scope
            .save()
            .subscribe((saved) => {
            this.refresh();
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
    localisedName(productCharacteristic) {
        const localisedText = productCharacteristic.LocalisedNames.find((v) => v.Locale === this.locale);
        if (localisedText) {
            return localisedText.Text;
        }
        return productCharacteristic.Name;
    }
    setProductCharacteristicValues() {
        this.productCharacteristicValues = this.inventoryItem.ProductCharacteristicValues.filter((v) => v.Locale === this.locale);
    }
    get locale() {
        return this.locales.find((v) => v.Name === this.selectedLocaleName);
    }
    brandSelected(brand) {
        const fetch = [
            new framework_1.Fetch({
                id: brand.id,
                include: [new framework_1.TreeNode({ roleType: this.m.Brand.Models })],
                name: "selectedbrand",
            }),
        ];
        this.scope
            .load("Pull", new framework_1.PullRequest({ fetch }))
            .subscribe((loaded) => {
            const selectedbrand = loaded.objects.selectedbrand;
            this.models = selectedbrand.Models;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
};
SerialisedGoodComponent = __decorate([
    core_1.Component({
        template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">

    <form #form="ngForm" *ngIf="good" (submit)="save()">
      <div class="pad">
  <p>aantal {{good.Photos.length}}</p>
        <mat-tab-group>

          <mat-tab label="General">
            <a-mat-select [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.SerialisedInventoryItemState"
              [options]="serialisedInventoryItemStates" display="Name" label="Status"></a-mat-select>
            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.SerialNumber"></a-mat-input>
            <a-mat-localised-text [object]="good" [roleType]="m.Good.LocalisedNames" [locales]="locales" label="Name"></a-mat-localised-text>
            <a-mat-localised-text [object]="good" [roleType]="m.Good.LocalisedDescriptions" [locales]="locales" label="Description"></a-mat-localised-text>
            <a-mat-select [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.ProductType" [options]="productTypes"
              display="Name"></a-mat-select>
            <a-mat-select [object]="good" [roleType]="m.Good.ProductCategories" [options]="categories" display="Name"></a-mat-select>
            <a-mat-autocomplete [object]="good" [roleType]="m.Good.PrimaryProductCategory" [options]="categories" display="Name"></a-mat-autocomplete>

            <div class="mat-input-wrapper">
              <div class="mat-input-flex">
                <div class="mat-input-infix">
                  <mat-select fxFlex [(ngModel)]="selectedBrand" name="brandName" placeholder="Brand" multiple="false" (ngModelChange)="brandSelected($event)">
                    <mat-option>None</mat-option>
                    <mat-option *ngFor="let brand of brands" [value]="brand"> {{ brand.Name }} </mat-option>
                  </mat-select>
                </div>
              </div>
            </div>

            <div class="mat-input-wrapper">
              <div class="mat-input-flex">
                <div class="mat-input-infix">
                  <mat-select fxFlex [(ngModel)]="selectedModel" name="modelName" placeholder="Model" multiple="false">
                    <mat-option>None</mat-option>
                    <mat-option *ngFor="let model of models" [value]="model"> {{ model.Name }} </mat-option>
                  </mat-select>
                </div>
              </div>
            </div>

            <a-mat-select [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.Ownership" [options]="ownerships" display="Name"></a-mat-select>
            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.Owner"></a-mat-input>
            <a-mat-autocomplete [object]="good" [roleType]="m.Good.SuppliedBy" [filter]="suppliersFilter.create()" display="Name"></a-mat-autocomplete>
            <a-mat-autocomplete [object]="good" [roleType]="m.Good.ManufacturedBy" [filter]="manufacturersFilter.create()" display="Name"></a-mat-autocomplete>
            <a-mat-datepicker [object]="good" [roleType]="m.Good.SalesDiscontinuationDate"></a-mat-datepicker>
          </mat-tab>

          <mat-tab label="Characteristics">

            <mat-radio-group name="rg" [(ngModel)]="selectedLocaleName">
              <mat-radio-button *ngFor="let locale of locales" [value]="locale.Name">
                {{locale.Language.Name}}
              </mat-radio-button>
            </mat-radio-group>

            <mat-divider></mat-divider>

            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.AcquisitionYear"></a-mat-input>
            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.ManufacturingYear"></a-mat-input>
            <mat-input-container fxLayout="column" fxLayoutAlign="top stretch">
              <input fxFlex matInput [(ngModel)]="inventoryItem.age" name="age" placeholder="Age" readonly>
            </mat-input-container>
            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.LifeTime"></a-mat-input>
            <mat-input-container fxLayout="column" fxLayoutAlign="top stretch">
              <input fxFlex matInput [(ngModel)]="inventoryItem.yearsToGo" name="yearsToGo" placeholder="Years to go" readonly>
            </mat-input-container>
            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.DepreciationYears"></a-mat-input>

            <a-mat-textarea *ngFor="let productCharacteristicValue of productCharacteristicValues" [object]="productCharacteristicValue"
              [roleType]="m.ProductCharacteristicValue.Value" [label]="localisedName(productCharacteristicValue.ProductCharacteristic)"></a-mat-textarea>
          </mat-tab>

          <mat-tab label="Financial">
            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.ReplacementValue"></a-mat-input>
            <mat-input-container fxLayout="column" fxLayoutAlign="top stretch">
              <input fxFlex matInput [(ngModel)]="inventoryItem.goingConcern" name="goingConcern" placeholder="Going Concern" readonly>
            </mat-input-container>
            <mat-input-container fxLayout="column" fxLayoutAlign="top stretch">
              <input fxFlex matInput [(ngModel)]="inventoryItem.marketValue" name="marketValue" placeholder="Market Value" readonly>
            </mat-input-container>
            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.PurchasePrice"></a-mat-input>
            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.RefurbishCost"></a-mat-input>
            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.TransportCost"></a-mat-input>
            <mat-input-container fxLayout="column" fxLayoutAlign="top stretch">
              <input fxFlex matInput [(ngModel)]="inventoryItem.grossBookValue" name="grossBookValue" placeholder="Gross Book Value" readonly>
            </mat-input-container>
            <a-mat-input [object]="inventoryItem" [roleType]="m.SerialisedInventoryItem.ExpectedSalesPrice"></a-mat-input>
            <mat-input-container fxLayout="column" fxLayoutAlign="top stretch">
              <input fxFlex matInput [(ngModel)]="inventoryItem.expectedPosa" name="expectedPosa" placeholder="Expected Posa" readonly>
            </mat-input-container>
          </mat-tab>

          <mat-tab label="Comments">
            <a-mat-localised-text [object]="good" [roleType]="m.Good.LocalisedComments" [locales]="locales" label="Comment"></a-mat-localised-text>
            <a-mat-textarea [object]="good" [roleType]="m.Good.InternalComment"></a-mat-textarea>
            <a-mat-textarea [object]="good" [roleType]="m.Good.Keywords"></a-mat-textarea>
          </mat-tab>

          <mat-tab label="Images">
            <a-mat-media-upload [object]="good" [roleType]="m.Good.PrimaryPhoto" accept="image/*" (selected)="imageSelected($event)"></a-mat-media-upload>

            <ng-template let-photo let-last="last" ngFor [ngForOf]="good.Photos">
              <mat-list-item>
                <mat-card class="image-card">
                  <mat-card-header>
                    <mat-card-title>{{photo.FileName}}</mat-card-title>
                  </mat-card-header>
                  <img mat-card-image src="http://localhost:5000/Media/Download/{{photo.UniqueId}}?revision={{photo.Revision}}">
                </mat-card>
                </mat-list-item>
              <mat-divider *ngIf="!last" mat-inset></mat-divider>
            </ng-template>
          </mat-tab>

        </mat-tab-group>
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
        router_1.ActivatedRoute,
        material_1.MatSnackBar,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], SerialisedGoodComponent);
exports.SerialisedGoodComponent = SerialisedGoodComponent;
