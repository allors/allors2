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
const workspace_1 = require("@allors/workspace");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let OrganisationOverviewComponent = class OrganisationOverviewComponent {
    constructor(workspaceService, errorService, route, dialogService, snackBar, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.dialogService = dialogService;
        this.snackBar = snackBar;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Organisation overview";
        this.contactsCollection = "Current";
        this.currentContactRelationships = [];
        this.inactiveContactRelationships = [];
        this.allContactRelationships = [];
        this.contactMechanismsCollection = "Current";
        this.currentContactMechanisms = [];
        this.inactiveContactMechanisms = [];
        this.allContactMechanisms = [];
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
    }
    get contactRelationships() {
        switch (this.contactsCollection) {
            case "Current":
                return this.currentContactRelationships;
            case "Inactive":
                return this.inactiveContactRelationships;
            case "All":
            default:
                return this.allContactRelationships;
        }
    }
    get contactMechanisms() {
        switch (this.contactMechanismsCollection) {
            case "Current":
                return this.currentContactMechanisms;
            case "Inactive":
                return this.inactiveContactMechanisms;
            case "All":
            default:
                return this.allContactMechanisms;
        }
    }
    ngOnInit() {
        const route$ = this.route.url;
        const combined$ = Rx_1.Observable.combineLatest(route$, this.refresh$);
        this.subscription = combined$
            .switchMap(([urlSegments, date]) => {
            const id = this.route.snapshot.paramMap.get("id");
            const m = this.m;
            const organisationContactRelationshipTreeNodes = [
                new framework_1.TreeNode({ roleType: m.OrganisationContactRelationship.ContactKinds }),
                new framework_1.TreeNode({
                    nodes: [
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({
                                    nodes: [
                                        new framework_1.TreeNode({ roleType: m.ContactMechanism.ContactMechanismType }),
                                    ],
                                    roleType: m.PartyContactMechanism.ContactMechanism,
                                }),
                            ],
                            roleType: m.Person.PartyContactMechanisms,
                        }),
                    ],
                    roleType: m.OrganisationContactRelationship.Contact,
                }),
            ];
            const partyContactMechanismTreeNodes = [
                new framework_1.TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                new framework_1.TreeNode({
                    nodes: [
                        new framework_1.TreeNode({ roleType: m.ContactMechanism.ContactMechanismType }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
                            ],
                            roleType: m.PostalAddress.PostalBoundary,
                        }),
                    ],
                    roleType: m.PartyContactMechanism.ContactMechanism,
                }),
            ];
            const fetch = [
                new framework_1.Fetch({
                    id,
                    include: [
                        new framework_1.TreeNode({ roleType: m.Party.Locale }),
                        new framework_1.TreeNode({ roleType: m.Organisation.IndustryClassifications }),
                        new framework_1.TreeNode({ roleType: m.Organisation.OrganisationRoles }),
                        new framework_1.TreeNode({ roleType: m.Organisation.OrganisationClassifications }),
                        new framework_1.TreeNode({ roleType: m.Organisation.LastModifiedBy }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({
                                    nodes: partyContactMechanismTreeNodes,
                                    roleType: m.Person.PartyContactMechanisms,
                                }),
                            ],
                            roleType: m.Party.CurrentContacts,
                        }),
                        new framework_1.TreeNode({
                            nodes: organisationContactRelationshipTreeNodes,
                            roleType: m.Party.CurrentOrganisationContactRelationships,
                        }),
                        new framework_1.TreeNode({
                            nodes: organisationContactRelationshipTreeNodes,
                            roleType: m.Party.InactiveOrganisationContactRelationships,
                        }),
                        new framework_1.TreeNode({
                            nodes: partyContactMechanismTreeNodes,
                            roleType: m.Party.PartyContactMechanisms,
                        }),
                        new framework_1.TreeNode({
                            nodes: partyContactMechanismTreeNodes,
                            roleType: m.Party.CurrentPartyContactMechanisms,
                        }),
                        new framework_1.TreeNode({
                            nodes: partyContactMechanismTreeNodes,
                            roleType: m.Party.InactivePartyContactMechanisms,
                        }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({
                                    nodes: [
                                        new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
                                    ],
                                    roleType: m.PostalAddress.PostalBoundary,
                                }),
                            ],
                            roleType: m.Organisation.GeneralCorrespondence,
                        }),
                    ],
                    name: "organisation",
                }),
                new framework_1.Fetch({
                    id,
                    include: [
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                    ],
                    name: "communicationEvents",
                    path: new framework_1.Path({ step: m.Party.CommunicationEventsWhereInvolvedParty }),
                }),
            ];
            const query = [
                new framework_1.Query({
                    name: "countries",
                    objectType: m.Country,
                }),
                new framework_1.Query({
                    name: "genders",
                    objectType: m.GenderType,
                }),
                new framework_1.Query({
                    name: "salutations",
                    objectType: m.Salutation,
                }),
                new framework_1.Query({
                    name: "organisationContactKinds",
                    objectType: m.OrganisationContactKind,
                }),
                new framework_1.Query({
                    name: "contactMechanismPurposes",
                    objectType: m.ContactMechanismPurpose,
                }),
                new framework_1.Query({
                    name: "internalOrganisation",
                    objectType: m.InternalOrganisation,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.organisation = loaded.objects.organisation;
            this.communicationEvents = loaded.collections.communicationEvents;
            this.currentContactRelationships = this.organisation.CurrentOrganisationContactRelationships;
            this.inactiveContactRelationships = this.organisation.InactiveOrganisationContactRelationships;
            this.allContactRelationships = this.currentContactRelationships.concat(this.inactiveContactRelationships);
            this.currentContactMechanisms = this.organisation.CurrentPartyContactMechanisms;
            this.inactiveContactMechanisms = this.organisation.InactivePartyContactMechanisms;
            this.allContactMechanisms = this.currentContactMechanisms.concat(this.inactiveContactMechanisms);
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
    removeContact(organisationContactRelationship) {
        organisationContactRelationship.ThroughDate = new Date();
        this.scope
            .save()
            .subscribe((saved) => {
            this.scope.session.reset();
            this.refresh();
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    activateContact(organisationContactRelationship) {
        organisationContactRelationship.ThroughDate = undefined;
        this.scope
            .save()
            .subscribe((saved) => {
            this.scope.session.reset();
            this.refresh();
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    deleteContact(person) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this?" })
            .afterClosed().subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(person.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    removeContactMechanism(partyContactMechanism) {
        partyContactMechanism.ThroughDate = new Date();
        this.scope
            .save()
            .subscribe((saved) => {
            this.scope.session.reset();
            this.refresh();
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    activateContactMechanism(partyContactMechanism) {
        partyContactMechanism.ThroughDate = undefined;
        this.scope
            .save()
            .subscribe((saved) => {
            this.scope.session.reset();
            this.refresh();
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    deleteContactMechanism(contactMechanism) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this?" })
            .afterClosed().subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(contactMechanism.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    cancelCommunication(communicationEvent) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to cancel this?" })
            .afterClosed().subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(communicationEvent.Cancel)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    closeCommunication(communicationEvent) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to close this?" })
            .afterClosed().subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(communicationEvent.Close)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully closed.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    reopenCommunication(communicationEvent) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to reopen this?" })
            .afterClosed().subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(communicationEvent.Reopen)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully reopened.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    deleteCommunication(communicationEvent) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this?" })
            .afterClosed().subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(communicationEvent.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    goBack() {
        window.history.back();
    }
    refresh() {
        this.refresh$.next(new Date());
    }
    isPhone(contactMechanism) {
        if (contactMechanism instanceof workspace_1.TelecommunicationsNumber) {
            const phoneCommunication = contactMechanism;
            return phoneCommunication.ContactMechanismType && phoneCommunication.ContactMechanismType.Name === "Phone";
        }
    }
    checkType(obj) {
        return obj.objectType.name;
    }
};
OrganisationOverviewComponent = __decorate([
    core_1.Component({
        templateUrl: "./organisation-overview.component.html",
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        core_2.TdDialogService,
        material_1.MatSnackBar,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], OrganisationOverviewComponent);
exports.OrganisationOverviewComponent = OrganisationOverviewComponent;
