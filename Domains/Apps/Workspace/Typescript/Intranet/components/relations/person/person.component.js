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
const platform_browser_1 = require("@angular/platform-browser");
const router_1 = require("@angular/router");
const core_2 = require("@covalent/core");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let PersonComponent = class PersonComponent {
    constructor(workspaceService, errorService, route, media, titleService, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.media = media;
        this.titleService = titleService;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Person";
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.titleService.setTitle(this.title);
    }
    ngOnInit() {
        this.subscription = this.route.url
            .switchMap((url) => {
            const id = this.route.snapshot.paramMap.get("id");
            const m = this.workspaceService.metaPopulation.metaDomain;
            const fetch = [
                new framework_1.Fetch({
                    id,
                    include: [
                        new framework_1.TreeNode({ roleType: m.Person.Picture }),
                    ],
                    name: "person",
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
                    name: "genders",
                    objectType: this.m.GenderType,
                }),
                new framework_1.Query({
                    name: "salutations",
                    objectType: this.m.Salutation,
                }),
                new framework_1.Query({
                    name: "roles",
                    objectType: this.m.PersonRole,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.subTitle = "edit person";
            this.person = loaded.objects.person;
            this.customerRelationships = loaded.collections.customerRelationships;
            if (!this.person) {
                this.subTitle = "add a new person";
                this.person = this.scope.session.create("Person");
            }
            this.locales = loaded.collections.locales;
            this.genders = loaded.collections.genders;
            this.salutations = loaded.collections.salutations;
            this.roles = loaded.collections.roles;
            this.customerRole = this.roles.find((v) => v.UniqueId.toUpperCase() === "B29444EF-0950-4D6F-AB3E-9C6DC44C050F");
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
        if (this.person.PersonRoles.indexOf(this.customerRole) > -1 && this.customerRelationships === undefined) {
            const customerRelationship = this.scope.session.create("CustomerRelationship");
            customerRelationship.Customer = this.person;
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
PersonComponent = __decorate([
    core_1.Component({
        template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="person" (submit)="save()">

    <div class="pad">
      <a-mat-media-upload [object]="person" [roleType]="m.Person.Picture" accept="image/*"></a-mat-media-upload>
      <a-mat-select [object]="person" [roleType]="m.Person.PersonRoles" [options]="roles" display="Name"></a-mat-select>
      <a-mat-input [object]="person" [roleType]="m.Person.FirstName"></a-mat-input>
      <a-mat-input [object]="person" [roleType]="m.Person.MiddleName"></a-mat-input>
      <a-mat-input [object]="person" [roleType]="m.Person.LastName"></a-mat-input>
      <a-mat-input [object]="person" [roleType]="m.Person.Comment"></a-mat-input>
      <a-mat-input [object]="person" [roleType]="m.Person.Function"></a-mat-input>
      <a-mat-select [object]="person" [roleType]="m.Person.Gender" [options]="genders" display="Name"></a-mat-select>
      <a-mat-select [object]="person" [roleType]="m.Person.Salutation" [options]="salutations" display="Name"></a-mat-select>
      <a-mat-select [object]="person" [roleType]="m.Person.Locale" [options]="locales" display="Name"></a-mat-select>
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
        platform_browser_1.Title,
        core_1.ChangeDetectorRef])
], PersonComponent);
exports.PersonComponent = PersonComponent;
