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
const router_1 = require("@angular/router");
const core_2 = require("@covalent/core");
const Rx_1 = require("rxjs/Rx");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let OrganisationComponent = class OrganisationComponent {
    constructor(workspaceService, errorService, route, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Organisation";
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
            const fetch = [
                new framework_1.Fetch({
                    name: "organisation",
                    id,
                }),
                new framework_1.Fetch({
                    name: "customerRelationships",
                    id,
                    path: new framework_1.Path({ step: this.m.Organisation.CustomerRelationshipsWhereCustomer }),
                }),
            ];
            const query = [
                new framework_1.Query({
                    name: "locales",
                    objectType: this.m.Locale,
                }),
                new framework_1.Query({
                    name: "roles",
                    objectType: this.m.OrganisationRole,
                }),
                new framework_1.Query({
                    name: "classifications",
                    objectType: this.m.CustomOrganisationClassification,
                }),
                new framework_1.Query({
                    name: "industries",
                    objectType: this.m.IndustryClassification,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.subTitle = "edit organisation";
            this.organisation = loaded.objects.organisation;
            this.customerRelationships = loaded.collections.customerRelationships;
            if (!this.organisation) {
                this.subTitle = "add a new organisation";
                this.organisation = this.scope.session.create("Organisation");
            }
            this.locales = loaded.collections.locales;
            this.classifications = loaded.collections.classifications;
            this.industries = loaded.collections.industries;
            this.roles = loaded.collections.roles;
            this.customerRole = this.roles.find((v) => v.UniqueId.toUpperCase() === "8B5E0CEE-4C98-42F1-8F18-3638FBA943A0");
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
    save() {
        if (this.organisation.OrganisationRoles.indexOf(this.customerRole) > -1 && this.customerRelationships === undefined) {
            const customerRelationship = this.scope.session.create("CustomerRelationship");
            customerRelationship.Customer = this.organisation;
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
};
OrganisationComponent = __decorate([
    core_1.Component({
        template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="organisation" (submit)="save()">

    <div class="pad">
      <a-mat-media-upload [object]="organisation" [roleType]="m.Organisation.LogoImage" accept="image/*"></a-mat-media-upload>
      <a-mat-select [object]="organisation" [roleType]="m.Organisation.OrganisationRoles" [options]="roles" display="Name"></a-mat-select>
      <a-mat-select [object]="organisation" [roleType]="m.Organisation.OrganisationClassifications" [options]="classifications" display="Name" label="Classification"></a-mat-select>
      <a-mat-select [object]="organisation" [roleType]="m.Organisation.IndustryClassifications" [options]="industries" display="Name" label="Industry"></a-mat-select>
      <a-mat-input [object]="organisation" [roleType]="m.Organisation.Name"></a-mat-input>
      <a-mat-input [object]="organisation" [roleType]="m.Organisation.Comment"></a-mat-input>
      <a-mat-select [object]="organisation" [roleType]="m.Organisation.Locale" [options]="locales" display="Name"></a-mat-select>
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
        core_2.TdMediaService,
        core_1.ChangeDetectorRef])
], OrganisationComponent);
exports.OrganisationComponent = OrganisationComponent;
