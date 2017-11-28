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
const party_contactmechanism_telecommunicationsnumber_component_1 = require("./party-contactmechanism-telecommunicationsnumber.component");
let PartyContactMechanismTelecommunicationsNumberAddComponent = class PartyContactMechanismTelecommunicationsNumberAddComponent {
    constructor(workspaceService, errorService, route, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Telecommunications Number";
        this.subTitle = "add a telecommunications number";
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
    }
    ngOnInit() {
        this.subscription = this.route.url
            .switchMap((url) => {
            const id = this.route.snapshot.paramMap.get("id");
            const m = this.m;
            const fetch = [
                new framework_1.Fetch({
                    name: "party",
                    id,
                    include: [
                        new framework_1.TreeNode({
                            roleType: m.Party.PartyContactMechanisms,
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                                new framework_1.TreeNode({
                                    roleType: m.PartyContactMechanism.ContactMechanism,
                                    nodes: [new framework_1.TreeNode({ roleType: m.ContactMechanism.ContactMechanismType })],
                                }),
                            ],
                        }),
                    ],
                }),
            ];
            const query = [
                new framework_1.Query({
                    name: "contactMechanismPurposes",
                    objectType: this.m.ContactMechanismPurpose,
                }),
                new framework_1.Query({
                    name: "contactMechanismTypes",
                    objectType: this.m.ContactMechanismType,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes;
            this.contactMechanismTypes = loaded.collections.contactMechanismTypes;
            const phone = this.contactMechanismTypes.find((v) => v.Name === "Phone");
            this.party = loaded.objects.party;
            if (!this.contactMechanism) {
                this.contactMechanism = this.scope.session.create("TelecommunicationsNumber");
                this.contactMechanism.ContactMechanismType = phone;
            }
            this.partyContactMechanism = this.scope.session.create("PartyContactMechanism");
            this.partyContactMechanism.ContactMechanism = this.contactMechanism;
            this.partyContactMechanism.UseAsDefault = true;
            this.party.AddPartyContactMechanism(this.partyContactMechanism);
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
PartyContactMechanismTelecommunicationsNumberAddComponent = __decorate([
    core_1.Component({
        template: party_contactmechanism_telecommunicationsnumber_component_1.template,
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], PartyContactMechanismTelecommunicationsNumberAddComponent);
exports.PartyContactMechanismTelecommunicationsNumberAddComponent = PartyContactMechanismTelecommunicationsNumberAddComponent;
