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
let NonSerialisedGoodComponent = class NonSerialisedGoodComponent {
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
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
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
                        new framework_1.TreeNode({
                            roleType: m.InventoryItem.InventoryItemVariances,
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
                    name: "varianceReasons",
                    objectType: this.m.VarianceReason,
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
                    name: "inventoryItemKinds",
                    objectType: this.m.InventoryItemKind,
                }),
                new framework_1.Query({
                    name: "nonSerialisedInventoryItemStates",
                    objectType: this.m.NonSerialisedInventoryItemState,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }))
                .switchMap((loaded) => {
                this.good = loaded.objects.good;
                this.categories = loaded.collections.categories;
                this.productTypes = loaded.collections.productTypes;
                this.varianceReasons = loaded.collections.varianceReasons;
                this.vatRates = loaded.collections.vatRates;
                this.brands = loaded.collections.brands;
                this.inventoryItemKinds = loaded.collections.inventoryItemKinds;
                this.inventoryItemObjectStates = loaded.collections.nonSerialisedInventoryItemStates;
                this.singleton = loaded.collections.singletons[0];
                this.facility = this.singleton.InternalOrganisation.DefaultFacility;
                this.locales = this.singleton.Locales;
                this.selectedLocaleName = this.singleton.DefaultLocale.Name;
                const vatRateZero = this.vatRates.find((v) => v.Rate === 0);
                const inventoryItemKindNonSerialised = this.inventoryItemKinds.find((v) => v.Name === "Non serialised");
                if (this.good === undefined) {
                    this.good = this.scope.session.create("Good");
                    this.good.VatRate = vatRateZero;
                    this.good.Sku = "";
                    this.inventoryItem = this.scope.session.create("NonSerialisedInventoryItem");
                    this.good.InventoryItemKind = inventoryItemKindNonSerialised;
                    this.inventoryItem.Good = this.good;
                    this.inventoryItem.Facility = this.facility;
                }
                else {
                    this.inventoryItems = loaded.collections.inventoryItems;
                    this.inventoryItem = this.inventoryItems[0];
                }
                this.title = this.good.Name;
                this.subTitle = "Non Serialised";
                this.actualQuantityOnHand = this.good.QuantityOnHand;
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
    imageSelected(id) {
        this.good.AddPhoto(this.good.PrimaryPhoto);
        this.update();
        this.snackBar.open("Good succesfully saved.", "close", { duration: 5000 });
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
        if (this.actualQuantityOnHand !== this.good.QuantityOnHand) {
            const reason = this.varianceReasons.find((v) => v.Name === "Unknown");
            const inventoryItemVariance = this.scope.session.create("InventoryItemVariance");
            inventoryItemVariance.Quantity = this.actualQuantityOnHand - this.good.QuantityOnHand;
            inventoryItemVariance.Reason = reason;
            this.inventoryItem.AddInventoryItemVariance(inventoryItemVariance);
        }
        this.scope
            .save()
            .subscribe((saved) => {
            this.goBack();
        }, (error) => {
            this.errorService.dialog(error);
        });
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
NonSerialisedGoodComponent = __decorate([
    core_1.Component({
        templateUrl: "./nonserialisedgood.component.html",
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        material_1.MatSnackBar,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], NonSerialisedGoodComponent);
exports.NonSerialisedGoodComponent = NonSerialisedGoodComponent;
