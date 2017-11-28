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
let PartyCommunicationEventEmailCommunicationComponent = class PartyCommunicationEventEmailCommunicationComponent {
    constructor(workspaceService, errorService, route, snackBar, dialogService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.snackBar = snackBar;
        this.dialogService = dialogService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Email Communication";
        this.addOriginator = false;
        this.addAddressee = false;
        this.emailAddresses = [];
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
            const roleId = this.route.snapshot.paramMap.get("roleId");
            const m = this.workspaceService.metaPopulation.metaDomain;
            const fetch = [
                new framework_1.Fetch({
                    name: "party",
                    id,
                }),
                new framework_1.Fetch({
                    id: roleId,
                    include: [
                        new framework_1.TreeNode({ roleType: m.EmailCommunication.Originator }),
                        new framework_1.TreeNode({ roleType: m.EmailCommunication.Addressees }),
                        new framework_1.TreeNode({ roleType: m.EmailCommunication.EmailTemplate }),
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                    ],
                    name: "communicationEvent",
                }),
            ];
            const query = [
                new framework_1.Query({
                    include: [
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({
                                    nodes: [
                                        new framework_1.TreeNode({
                                            nodes: [
                                                new framework_1.TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                                            ],
                                            roleType: m.Party.CurrentPartyContactMechanisms,
                                        }),
                                    ],
                                    roleType: m.InternalOrganisation.ActiveEmployees,
                                }),
                            ],
                            roleType: m.Singleton.InternalOrganisation,
                        }),
                    ],
                    name: "singletons",
                    objectType: this.m.Singleton,
                }),
                new framework_1.Query({
                    name: "purposes",
                    objectType: this.m.CommunicationEventPurpose,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.party = loaded.objects.party;
            this.singleton = loaded.collections.singletons[0];
            this.employees = this.singleton.InternalOrganisation.ActiveEmployees;
            this.purposes = loaded.collections.purposes;
            this.communicationEvent = loaded.objects.communicationEvent;
            if (!this.communicationEvent) {
                this.communicationEvent = this.scope.session.create("EmailCommunication");
                this.emailTemplate = this.scope.session.create("EmailTemplate");
                this.communicationEvent.EmailTemplate = this.emailTemplate;
                this.communicationEvent.Originator = this.party.GeneralEmail;
            }
            for (const employee of this.employees) {
                const employeeContactMechanisms = employee.CurrentPartyContactMechanisms.map((v) => v.ContactMechanism);
                for (const contactMechanism of employeeContactMechanisms) {
                    if (contactMechanism instanceof (workspace_1.EmailAddress)) {
                        this.emailAddresses.push(contactMechanism);
                    }
                }
            }
            const contactMechanisms = this.party.CurrentPartyContactMechanisms.map((v) => v.ContactMechanism);
            for (const contactMechanism of contactMechanisms) {
                if (contactMechanism instanceof (workspace_1.EmailAddress)) {
                    this.emailAddresses.push(contactMechanism);
                }
            }
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
    originatorCancelled() {
        this.addOriginator = false;
    }
    addresseeCancelled() {
        this.addAddressee = false;
    }
    originatorAdded(id) {
        this.addOriginator = false;
        const emailAddress = this.scope.session.get(id);
        const partyContactMechanism = this.scope.session.create("PartyContactMechanism");
        partyContactMechanism.ContactMechanism = emailAddress;
        this.party.AddPartyContactMechanism(partyContactMechanism);
        this.emailAddresses.push(emailAddress);
        this.communicationEvent.Originator = emailAddress;
    }
    addresseeAdded(id) {
        this.addAddressee = false;
        const emailAddress = this.scope.session.get(id);
        const partyContactMechanism = this.scope.session.create("PartyContactMechanism");
        partyContactMechanism.ContactMechanism = emailAddress;
        this.party.AddPartyContactMechanism(partyContactMechanism);
        this.emailAddresses.push(emailAddress);
        this.communicationEvent.AddAddressee(emailAddress);
    }
    cancel() {
        const cancelFn = () => {
            this.scope.invoke(this.communicationEvent.Cancel)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
            }, (error) => {
                this.errorService.dialog(error);
            });
        };
        if (this.scope.session.hasChanges) {
            this.dialogService
                .openConfirm({ message: "Save changes?" })
                .afterClosed().subscribe((confirm) => {
                if (confirm) {
                    this.scope
                        .save()
                        .subscribe((saved) => {
                        this.scope.session.reset();
                        cancelFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    cancelFn();
                }
            });
        }
        else {
            cancelFn();
        }
    }
    close() {
        const cancelFn = () => {
            this.scope.invoke(this.communicationEvent.Close)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully closed.", "close", { duration: 5000 });
            }, (error) => {
                this.errorService.dialog(error);
            });
        };
        if (this.scope.session.hasChanges) {
            this.dialogService
                .openConfirm({ message: "Save changes?" })
                .afterClosed().subscribe((confirm) => {
                if (confirm) {
                    this.scope
                        .save()
                        .subscribe((saved) => {
                        this.scope.session.reset();
                        cancelFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    cancelFn();
                }
            });
        }
        else {
            cancelFn();
        }
    }
    reopen() {
        const cancelFn = () => {
            this.scope.invoke(this.communicationEvent.Reopen)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully reopened.", "close", { duration: 5000 });
            }, (error) => {
                this.errorService.dialog(error);
            });
        };
        if (this.scope.session.hasChanges) {
            this.dialogService
                .openConfirm({ message: "Save changes?" })
                .afterClosed().subscribe((confirm) => {
                if (confirm) {
                    this.scope
                        .save()
                        .subscribe((saved) => {
                        this.scope.session.reset();
                        cancelFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    cancelFn();
                }
            });
        }
        else {
            cancelFn();
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
    refresh() {
        this.refresh$.next(new Date());
    }
    goBack() {
        window.history.back();
    }
};
PartyCommunicationEventEmailCommunicationComponent = __decorate([
    core_1.Component({
        templateUrl: "./party-communicationevent-emailcommunication.component.html",
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        material_1.MatSnackBar,
        core_2.TdDialogService,
        core_2.TdMediaService,
        core_1.ChangeDetectorRef])
], PartyCommunicationEventEmailCommunicationComponent);
exports.PartyCommunicationEventEmailCommunicationComponent = PartyCommunicationEventEmailCommunicationComponent;
