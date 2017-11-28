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
let RequestEditComponent = class RequestEditComponent {
    constructor(workspaceService, errorService, router, route, snackBar, dialogService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.router = router;
        this.route = route;
        this.snackBar = snackBar;
        this.dialogService = dialogService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.addEmailAddress = false;
        this.addPostalAddress = false;
        this.addTeleCommunicationsNumber = false;
        this.addWebAddress = false;
        this.addPerson = false;
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
        this.peopleFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName] });
        this.organisationsFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name] });
        this.currenciesFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Currency, roleTypes: [this.m.Currency.Name] });
    }
    get showOrganisations() {
        return !this.request.Originator || this.request.Originator instanceof (workspace_1.Organisation);
    }
    get showPeople() {
        return !this.request.Originator || this.request.Originator instanceof (workspace_1.Person);
    }
    ngOnInit() {
        const route$ = this.route.url;
        const combined$ = Rx_1.Observable.combineLatest(route$, this.refresh$);
        this.subscription = combined$
            .switchMap(([urlSegments, date]) => {
            const id = this.route.snapshot.paramMap.get("id");
            const m = this.m;
            const rolesQuery = [
                new framework_1.Query({
                    name: "organisationRoles",
                    objectType: m.OrganisationRole,
                }),
                new framework_1.Query({
                    name: "personRoles",
                    objectType: m.PersonRole,
                }),
                new framework_1.Query({
                    name: "currencies",
                    objectType: this.m.Currency,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ query: rolesQuery }))
                .switchMap((loaded) => {
                this.scope.session.reset();
                this.currencies = loaded.collections.currencies;
                const organisationRoles = loaded.collections.organisationRoles;
                const oCustomerRole = organisationRoles.find((v) => v.Name === "Customer");
                const personRoles = loaded.collections.organisationRoles;
                const pCustomerRole = organisationRoles.find((v) => v.Name === "Customer");
                const fetch = [
                    new framework_1.Fetch({
                        id,
                        include: [
                            new framework_1.TreeNode({ roleType: m.Request.Currency }),
                            new framework_1.TreeNode({ roleType: m.Request.Originator }),
                            new framework_1.TreeNode({ roleType: m.Request.RequestState }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({
                                        nodes: [
                                            new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
                                        ],
                                        roleType: m.PostalAddress.PostalBoundary,
                                    }),
                                ],
                                roleType: m.Request.FullfillContactMechanism,
                            })
                        ],
                        name: "requestForQuote",
                    }),
                ];
                const query = [
                    new framework_1.Query({
                        include: [new framework_1.TreeNode({ roleType: m.Party.CurrentContacts })],
                        name: "organisations",
                        objectType: this.m.Organisation,
                        predicate: new framework_1.Contains({ roleType: m.Organisation.OrganisationRoles, object: oCustomerRole }),
                    }),
                    new framework_1.Query({
                        name: "persons",
                        objectType: this.m.Person,
                        predicate: new framework_1.Contains({ roleType: m.Person.PersonRoles, object: pCustomerRole }),
                    }),
                ];
                return this.scope.load("Pull", new framework_1.PullRequest({ fetch, query }));
            });
        })
            .subscribe((loaded) => {
            this.request = loaded.objects.requestForQuote;
            if (!this.request) {
                this.request = this.scope.session.create("RequestForQuote");
                this.request.RequestDate = new Date();
                this.title = "Add Request";
            }
            else {
                this.title = "Request " + this.request.RequestNumber;
            }
            if (this.request.Originator) {
                this.originatorSelected(this.request.Originator);
                this.title = this.title + " from: " + this.request.Originator.PartyName;
            }
            this.previousOriginator = this.request.Originator;
            this.organisations = loaded.collections.organisations;
            this.people = loaded.collections.parties;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    submit() {
        const submitFn = () => {
            this.scope.invoke(this.request.Submit)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully submitted.", "close", { duration: 5000 });
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
                        submitFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    submitFn();
                }
            });
        }
        else {
            submitFn();
        }
    }
    cancel() {
        const cancelFn = () => {
            this.scope.invoke(this.request.Cancel)
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
    hold() {
        const holdFn = () => {
            this.scope.invoke(this.request.Hold)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully held.", "close", { duration: 5000 });
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
                        holdFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    holdFn();
                }
            });
        }
        else {
            holdFn();
        }
    }
    reject() {
        const rejectFn = () => {
            this.scope.invoke(this.request.Reject)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully rejected.", "close", { duration: 5000 });
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
                        rejectFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    rejectFn();
                }
            });
        }
        else {
            rejectFn();
        }
    }
    personCancelled() {
        this.addPerson = false;
    }
    personAdded(id) {
        this.addPerson = false;
        const contact = this.scope.session.get(id);
        const organisationContactRelationship = this.scope.session.create("OrganisationContactRelationship");
        organisationContactRelationship.Organisation = this.request.Originator;
        organisationContactRelationship.Contact = contact;
        this.contacts.push(contact);
    }
    webAddressCancelled() {
        this.addWebAddress = false;
    }
    webAddressAdded(id) {
        this.addWebAddress = false;
        const partyContactMechanism = this.scope.session.get(id);
        this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
        this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
    }
    emailAddressCancelled() {
        this.addEmailAddress = false;
    }
    emailAddressAdded(id) {
        this.addEmailAddress = false;
        const partyContactMechanism = this.scope.session.get(id);
        this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
        this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
    }
    postalAddressCancelled() {
        this.addPostalAddress = false;
    }
    postalAddressAdded(id) {
        this.addPostalAddress = false;
        const partyContactMechanism = this.scope.session.get(id);
        this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
        this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
    }
    teleCommunicationsNumberCancelled() {
        this.addTeleCommunicationsNumber = false;
    }
    teleCommunicationsNumberAdded(id) {
        this.addTeleCommunicationsNumber = false;
        const partyContactMechanism = this.scope.session.get(id);
        this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
        this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
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
            this.router.navigate(["/orders/request/" + this.request.id]);
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    originatorSelected(party) {
        const fetch = [
            new framework_1.Fetch({
                id: party.id,
                include: [
                    new framework_1.TreeNode({
                        nodes: [
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({ roleType: this.m.PostalBoundary.Country }),
                                ],
                                roleType: this.m.PostalAddress.PostalBoundary,
                            }),
                        ],
                        roleType: this.m.PartyContactMechanism.ContactMechanism,
                    }),
                ],
                name: "partyContactMechanisms",
                path: new framework_1.Path({ step: this.m.Party.CurrentPartyContactMechanisms }),
            }),
            new framework_1.Fetch({
                id: party.id,
                name: "currentContacts",
                path: new framework_1.Path({ step: this.m.Party.CurrentContacts }),
            }),
        ];
        this.scope
            .load("Pull", new framework_1.PullRequest({ fetch }))
            .subscribe((loaded) => {
            if (this.request.Originator !== this.previousOriginator) {
                this.request.ContactPerson = null;
                this.request.FullfillContactMechanism = null;
                this.previousOriginator = this.request.Originator;
            }
            const partyContactMechanisms = loaded.collections.partyContactMechanisms;
            this.contactMechanisms = partyContactMechanisms.map((v) => v.ContactMechanism);
            this.contacts = loaded.collections.currentContacts;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    refresh() {
        this.refresh$.next(new Date());
    }
    goBack() {
        window.history.back();
    }
};
RequestEditComponent = __decorate([
    core_1.Component({
        templateUrl: "./request.component.html",
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.Router,
        router_1.ActivatedRoute,
        material_1.MatSnackBar,
        core_2.TdDialogService,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], RequestEditComponent);
exports.RequestEditComponent = RequestEditComponent;
