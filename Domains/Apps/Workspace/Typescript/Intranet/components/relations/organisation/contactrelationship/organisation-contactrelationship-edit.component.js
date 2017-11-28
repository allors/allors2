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
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let OrganisationContactrelationshipEditComponent = class OrganisationContactrelationshipEditComponent {
    constructor(workspaceService, errorService, route, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Contact Relationship";
        this.subTitle = "add a new contact relationship";
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.peopleFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName] });
    }
    ngOnInit() {
        this.subscription = this.route.url
            .switchMap((url) => {
            const roleId = this.route.snapshot.paramMap.get("roleId");
            const m = this.m;
            const fetch = [
                new framework_1.Fetch({
                    name: "organisationContactRelationship",
                    id: roleId,
                    include: [
                        new framework_1.TreeNode({ roleType: m.OrganisationContactRelationship.ContactKinds }),
                    ],
                }),
            ];
            const query = [
                new framework_1.Query({
                    name: "organisationContactKinds",
                    objectType: this.m.OrganisationContactKind,
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
            this.organisationContactRelationship = loaded.objects.organisationContactRelationship;
            this.organisationContactKinds = loaded.collections.organisationContactKinds;
            this.roles = loaded.collections.roles;
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
OrganisationContactrelationshipEditComponent = __decorate([
    core_1.Component({
        templateUrl: "./organisation-contactrelationship.component.html",
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], OrganisationContactrelationshipEditComponent);
exports.OrganisationContactrelationshipEditComponent = OrganisationContactrelationshipEditComponent;
