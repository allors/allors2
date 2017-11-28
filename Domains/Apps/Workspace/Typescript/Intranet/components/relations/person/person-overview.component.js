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
const platform_browser_1 = require("@angular/platform-browser");
const router_1 = require("@angular/router");
const core_2 = require("@covalent/core");
const Rx_1 = require("rxjs/Rx");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let PersonOverviewComponent = class PersonOverviewComponent {
    constructor(workspaceService, errorService, route, dialogService, snackBar, media, changeDetectorRef, titleService) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.dialogService = dialogService;
        this.snackBar = snackBar;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.titleService = titleService;
        this.title = "Person overview";
        this.contactMechanismsCollection = "Current";
        this.currentContactMechanisms = [];
        this.inactiveContactMechanisms = [];
        this.allContactMechanisms = [];
        titleService.setTitle(this.title);
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
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
            const fetch = [
                new framework_1.Fetch({
                    id,
                    include: [
                        new framework_1.TreeNode({ roleType: m.Party.Locale }),
                        new framework_1.TreeNode({ roleType: m.Person.PersonRoles }),
                        new framework_1.TreeNode({ roleType: m.Person.LastModifiedBy }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                                new framework_1.TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                            ],
                            roleType: m.Party.PartyContactMechanisms,
                        }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                                new framework_1.TreeNode({
                                    nodes: [
                                        new framework_1.TreeNode({
                                            nodes: [
                                                new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
                                            ],
                                            roleType: m.PostalAddress.PostalBoundary,
                                        }),
                                    ],
                                    roleType: m.PartyContactMechanism.ContactMechanism,
                                }),
                            ],
                            roleType: m.Party.PartyContactMechanisms,
                        }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                                new framework_1.TreeNode({
                                    nodes: [
                                        new framework_1.TreeNode({
                                            nodes: [
                                                new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
                                            ],
                                            roleType: m.PostalAddress.PostalBoundary,
                                        }),
                                    ],
                                    roleType: m.PartyContactMechanism.ContactMechanism,
                                }),
                            ],
                            roleType: m.Party.CurrentPartyContactMechanisms,
                        }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.PartyContactMechanism.ContactPurposes }),
                                new framework_1.TreeNode({
                                    nodes: [
                                        new framework_1.TreeNode({
                                            nodes: [
                                                new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
                                            ],
                                            roleType: m.PostalAddress.PostalBoundary,
                                        }),
                                    ],
                                    roleType: m.PartyContactMechanism.ContactMechanism,
                                }),
                            ],
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
                            roleType: m.Person.GeneralCorrespondence,
                        }),
                    ],
                    name: "person",
                }),
                new framework_1.Fetch({
                    id,
                    include: [
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.InvolvedParties }),
                    ],
                    name: "communicationEvents",
                    path: new framework_1.Path({ step: m.Party.CommunicationEventsWhereInvolvedParty }),
                }),
                new framework_1.Fetch({
                    id,
                    include: [
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                                new framework_1.TreeNode({ roleType: m.WorkEffort.Priority }),
                            ],
                            roleType: m.WorkEffortAssignment.Assignment,
                        }),
                    ],
                    name: "workEffortAssignments",
                    path: new framework_1.Path({ step: m.Person.WorkEffortAssignmentsWhereProfessional }),
                }),
                new framework_1.Fetch({
                    id,
                    include: [
                        new framework_1.TreeNode({ roleType: m.OrganisationContactRelationship.Organisation }),
                    ],
                    name: "organisationContactRelationships",
                    path: new framework_1.Path({ step: m.Person.OrganisationContactRelationshipsWhereContact }),
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
            this.person = loaded.objects.person;
            const organisationContactRelationships = loaded.collections.organisationContactRelationships;
            this.organisation = organisationContactRelationships.length > 0 ? organisationContactRelationships[0].Organisation : undefined;
            this.communicationEvents = loaded.collections.communicationEvents;
            this.workEffortAssignments = loaded.collections.workEffortAssignments;
            this.currentContactMechanisms = this.person.CurrentPartyContactMechanisms;
            this.inactiveContactMechanisms = this.person.InactivePartyContactMechanisms;
            this.allContactMechanisms = this.currentContactMechanisms.concat(this.inactiveContactMechanisms);
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
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
    delete(workEffort) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this work effort?" })
            .afterClosed()
            .subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(workEffort.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
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
    refresh() {
        this.refresh$.next(new Date());
    }
    goBack() {
        window.history.back();
    }
    checkType(obj) {
        return obj.objectType.name;
    }
};
PersonOverviewComponent = __decorate([
    core_1.Component({
        templateUrl: "./person-overview.component.html",
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        core_2.TdDialogService,
        material_1.MatSnackBar,
        core_2.TdMediaService,
        core_1.ChangeDetectorRef,
        platform_browser_1.Title])
], PersonOverviewComponent);
exports.PersonOverviewComponent = PersonOverviewComponent;
