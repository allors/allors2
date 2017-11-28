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
let CatalogueComponent = class CatalogueComponent {
    constructor(workspaceService, errorService, route, snackBar, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.snackBar = snackBar;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
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
                        new framework_1.TreeNode({
                            nodes: [new framework_1.TreeNode({ roleType: m.LocalisedText.Locale })],
                            roleType: m.Catalogue.LocalisedNames,
                        }),
                        new framework_1.TreeNode({
                            nodes: [new framework_1.TreeNode({ roleType: m.LocalisedText.Locale })],
                            roleType: m.Catalogue.LocalisedDescriptions,
                        }),
                    ],
                    name: "catalogue",
                }),
            ];
            const query = [
                new framework_1.Query({
                    include: [
                        new framework_1.TreeNode({ roleType: m.Singleton.Locales }),
                    ],
                    name: "singletons",
                    objectType: this.m.Singleton,
                }),
                new framework_1.Query({
                    name: "categories",
                    objectType: this.m.ProductCategory,
                }),
                new framework_1.Query({
                    name: "catScopes",
                    objectType: this.m.CatScope,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.catalogue = loaded.objects.catalogue;
            if (!this.catalogue) {
                this.catalogue = this.scope.session.create("Catalogue");
            }
            this.title = this.catalogue.Name;
            this.singleton = loaded.collections.singletons[0];
            this.categories = loaded.collections.categories;
            this.catScopes = loaded.collections.catScopes;
            this.locales = this.singleton.Locales;
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
        this.update();
        this.snackBar.open("Catalogue succesfully saved.", "close", { duration: 5000 });
    }
    save() {
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
};
CatalogueComponent = __decorate([
    core_1.Component({
        template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="catalogue" (submit)="save()">

    <div body *ngIf="catalogue" layout-gt-md="row">
      <div flex-gt-xs="60">
        <div class="pad">
          <a-mat-media-upload [object]="catalogue" [roleType]="m.Catalogue.CatalogueImage" accept="image/*" (selected)="imageSelected($event)"></a-mat-media-upload>
          <a-mat-select [object]="catalogue" [roleType]="m.Catalogue.CatScope" [options]="catScopes" display="Name" label="Scope"></a-mat-select>
          <a-mat-localised-text [object]="catalogue" [roleType]="m.Catalogue.LocalisedNames" [locales]="locales" label="Name"></a-mat-localised-text>
          <a-mat-localised-text [object]="catalogue" [roleType]="m.Catalogue.LocalisedDescriptions" [locales]="locales" label="Description"></a-mat-localised-text>
          <a-mat-select [object]="catalogue" [roleType]="m.Catalogue.ProductCategories" [options]="categories" display="Name"></a-mat-select>
        </div>

        <mat-divider></mat-divider>

        <mat-card-actions>
          <button mat-button color="primary" type="submit" [disabled]="!form.form.valid">SAVE</button>
          <button mat-button (click)="goBack()" type="button">CANCEL</button>
        </mat-card-actions>
      </div>

      <div *ngIf="catalogue.CatalogueImage" flex-gt-xs="40">
        <mat-card class="image-card">
          <mat-card-header>
            <mat-card-title>{{catalogue.Name}}</mat-card-title>
            <mat-card-subtitle>{{catalogue.CatalogueImage.FileName}}</mat-card-subtitle>
          </mat-card-header>
          <img mat-card-image src="http://localhost:5000/Media/Download/{{catalogue.CatalogueImage.UniqueId}}?revision={{catalogue.CatalogueImage.Revision}}">
        </mat-card>
      </div>
    </div>

  </form>
</td-layout-card-over>
`,
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        material_1.MatSnackBar,
        core_2.TdMediaService,
        core_1.ChangeDetectorRef])
], CatalogueComponent);
exports.CatalogueComponent = CatalogueComponent;
