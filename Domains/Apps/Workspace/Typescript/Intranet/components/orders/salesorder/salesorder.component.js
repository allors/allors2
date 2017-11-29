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
const BehaviorSubject_1 = require("rxjs/BehaviorSubject");
const Observable_1 = require("rxjs/Observable");
require("rxjs/add/observable/combineLatest");
const workspace_1 = require("@allors/workspace");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let SalesOrderEditComponent = class SalesOrderEditComponent {
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
        this.addShipToAddress = false;
        this.addPerson = false;
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.refresh$ = new BehaviorSubject_1.BehaviorSubject(undefined);
        this.peopleFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName] });
        this.organisationsFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name] });
        this.currenciesFilter = new base_angular_1.Filter({ scope: this.scope, objectType: this.m.Currency, roleTypes: [this.m.Currency.Name] });
    }
    get showOrganisations() {
        return !this.order.ShipToCustomer || this.order.ShipToCustomer instanceof (workspace_1.Organisation);
    }
    get showPeople() {
        return !this.order.ShipToCustomer || this.order.ShipToCustomer instanceof (workspace_1.Person);
    }
    ngOnInit() {
        const route$ = this.route.url;
        const combined$ = Observable_1.Observable.combineLatest(route$, this.refresh$);
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
                new framework_1.Query({
                    name: "vatRates",
                    objectType: m.VatRate,
                }),
                new framework_1.Query({
                    name: "vatRegimes",
                    objectType: m.VatRegime,
                }),
                new framework_1.Query({
                    include: [
                        new framework_1.TreeNode({ roleType: m.Store.ProcessFlow }),
                    ],
                    name: "stores",
                    objectType: m.Store,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ query: rolesQuery }))
                .switchMap((loaded) => {
                this.scope.session.reset();
                this.currencies = loaded.collections.currencies;
                this.vatRates = loaded.collections.vatRates;
                this.vatRegimes = loaded.collections.vatRegimes;
                this.stores = loaded.collections.stores;
                const organisationRoles = loaded.collections.organisationRoles;
                const oCustomerRole = organisationRoles.find((v) => v.Name === "Customer");
                const personRoles = loaded.collections.organisationRoles;
                const pCustomerRole = organisationRoles.find((v) => v.Name === "Customer");
                const query = [
                    new framework_1.Query({
                        name: "organisations",
                        objectType: this.m.Organisation,
                        predicate: new framework_1.Contains({ roleType: m.Organisation.OrganisationRoles, object: oCustomerRole }),
                    }),
                    new framework_1.Query({
                        name: "people",
                        objectType: this.m.Person,
                        predicate: new framework_1.Contains({ roleType: m.Person.PersonRoles, object: pCustomerRole }),
                    }),
                ];
                const fetch = [
                    new framework_1.Fetch({
                        id,
                        include: [
                            new framework_1.TreeNode({ roleType: m.SalesOrder.ShipToCustomer }),
                            new framework_1.TreeNode({ roleType: m.SalesOrder.ShipToAddress }),
                            new framework_1.TreeNode({ roleType: m.SalesOrder.SalesOrderState }),
                            new framework_1.TreeNode({ roleType: m.SalesOrder.BillToContactMechanism }),
                            new framework_1.TreeNode({ roleType: m.SalesOrder.Quote }),
                            new framework_1.TreeNode({
                                nodes: [new framework_1.TreeNode({ roleType: m.VatRegime.VatRate })],
                                roleType: m.SalesOrder.VatRegime,
                            }),
                        ],
                        name: "salesOrder",
                    }),
                ];
                return this.scope.load("Pull", new framework_1.PullRequest({ fetch, query }));
            });
        })
            .subscribe((loaded) => {
            this.order = loaded.objects.salesOrder;
            if (!this.order) {
                this.order = this.scope.session.create("SalesOrder");
                if (this.stores.length === 1) {
                    this.order.Store = this.stores[0];
                }
                this.title = "Add Sales Order";
            }
            else {
                this.title = "Sales Order " + this.order.OrderNumber;
            }
            if (this.order.ShipToCustomer) {
                this.shipToCustomerSelected(this.order.ShipToCustomer);
            }
            if (this.order.BillToCustomer) {
                this.billToCustomerSelected(this.order.BillToCustomer);
            }
            this.previousShipToCustomer = this.order.ShipToCustomer;
            this.previousBillToCustomer = this.order.BillToCustomer;
            this.organisations = loaded.collections.organisations;
            this.people = loaded.collections.parties;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    personCancelled() {
        this.addPerson = false;
    }
    personAdded(id) {
        this.addPerson = false;
        const contact = this.scope.session.get(id);
        const organisationContactRelationship = this.scope.session.create("OrganisationContactRelationship");
        organisationContactRelationship.Organisation = this.order.ShipToCustomer;
        organisationContactRelationship.Contact = contact;
        this.contacts.push(contact);
    }
    webAddressCancelled() {
        this.addWebAddress = false;
    }
    webAddressAdded(id) {
        this.addWebAddress = false;
        const partyContactMechanism = this.scope.session.get(id);
        this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
        this.order.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
    }
    emailAddressCancelled() {
        this.addEmailAddress = false;
    }
    emailAddressAdded(id) {
        this.addEmailAddress = false;
        const partyContactMechanism = this.scope.session.get(id);
        this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
        this.order.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
    }
    postalAddressCancelled() {
        this.addPostalAddress = false;
    }
    postalAddressAdded(id) {
        this.addPostalAddress = false;
        const partyContactMechanism = this.scope.session.get(id);
        this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
        this.order.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
    }
    teleCommunicationsNumberCancelled() {
        this.addTeleCommunicationsNumber = false;
    }
    teleCommunicationsNumberAdded(id) {
        this.addTeleCommunicationsNumber = false;
        const partyContactMechanism = this.scope.session.get(id);
        this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
        this.order.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
    }
    shipToAddressCancelled() {
        this.addShipToAddress = false;
    }
    shipToAddressAdded(id) {
        this.addShipToAddress = false;
        const partyContactMechanism = this.scope.session.get(id);
        this.ShipToAddresses.push(partyContactMechanism.ContactMechanism);
        this.order.ShipToCustomer.AddPartyContactMechanism(partyContactMechanism);
    }
    approve() {
        const submitFn = () => {
            this.scope.invoke(this.order.Approve)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully approved.", "close", { duration: 5000 });
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
            this.scope.invoke(this.order.Reject)
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
    reject() {
        const rejectFn = () => {
            this.scope.invoke(this.order.Reject)
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
    hold() {
        const holdFn = () => {
            this.scope.invoke(this.order.Hold)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully put on hold.", "close", { duration: 5000 });
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
    continue() {
        const continueFn = () => {
            this.scope.invoke(this.order.Continue)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully removed from hold.", "close", { duration: 5000 });
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
                        continueFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    continueFn();
                }
            });
        }
        else {
            continueFn();
        }
    }
    confirm() {
        const confirmFn = () => {
            this.scope.invoke(this.order.Confirm)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully confirmed.", "close", { duration: 5000 });
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
                        confirmFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    confirmFn();
                }
            });
        }
        else {
            confirmFn();
        }
    }
    finish() {
        const finishFn = () => {
            this.scope.invoke(this.order.Continue)
                .subscribe((invoked) => {
                this.refresh();
                this.snackBar.open("Successfully finished.", "close", { duration: 5000 });
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
                        finishFn();
                    }, (error) => {
                        this.errorService.dialog(error);
                    });
                }
                else {
                    finishFn();
                }
            });
        }
        else {
            finishFn();
        }
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
            this.router.navigate(["/orders/salesOrder/" + this.order.id]);
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
    shipToCustomerSelected(party) {
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
            if (this.order.ShipToCustomer !== this.previousShipToCustomer) {
                this.order.ShipToAddress = null;
                this.order.ContactPerson = null;
                this.previousShipToCustomer = this.order.ShipToCustomer;
            }
            if (this.order.ShipToCustomer !== null && this.order.BillToCustomer === null) {
                this.order.BillToCustomer = this.order.ShipToCustomer;
                this.billToCustomerSelected(this.order.BillToCustomer);
            }
            const partyContactMechanisms = loaded.collections.partyContactMechanisms;
            this.ShipToAddresses = partyContactMechanisms.filter((v) => v.ContactMechanism.objectType.name === "PostalAddress").map((v) => v.ContactMechanism);
            this.contacts = loaded.collections.currentContacts;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    billToCustomerSelected(party) {
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
        ];
        this.scope
            .load("Pull", new framework_1.PullRequest({ fetch }))
            .subscribe((loaded) => {
            if (this.order.BillToCustomer !== this.previousBillToCustomer) {
                this.order.BillToContactMechanism = null;
                this.previousBillToCustomer = this.order.BillToCustomer;
            }
            if (this.order.BillToCustomer !== null && this.order.ShipToCustomer === null) {
                this.order.ShipToCustomer = this.order.BillToCustomer;
                this.shipToCustomerSelected(this.order.ShipToCustomer);
            }
            const partyContactMechanisms = loaded.collections.partyContactMechanisms;
            this.billToContactMechanisms = partyContactMechanisms.map((v) => v.ContactMechanism);
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
SalesOrderEditComponent = __decorate([
    core_1.Component({
        template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="order" (submit)="save()">

    <div class="pad">
      <div *ngIf="order.SalesOrderState">
        <a-mat-static [object]="order" [roleType]="m.SalesOrder.SalesOrderState" display="Name" label="Status"></a-mat-static>
        <button *ngIf="order.CanExecuteCancel" mat-button type="button" (click)="cancel()">Cancel</button>
        <button *ngIf="order.CanExecuteApprove" mat-button type="button" (click)="approve()">Approve</button>
        <button *ngIf="order.CanExecuteReject" mat-button type="button" (click)="reject()">Reject</button>
        <button *ngIf="order.CanExecuteHold" mat-button type="button" (click)="hold()">Hold</button>
        <button *ngIf="order.CanExecuteContinue" mat-button type="button" (click)="continue()">Continue</button>
        <button *ngIf="order.CanExecuteConfirm" mat-button type="button" (click)="confirm()">Confirm</button>
      </div>

      <a-mat-static *ngIf="order.Quote" [object]="order" [roleType]="m.SalesOrder.Quote" display="QuoteNumber"></a-mat-static>

      <a-mat-select *ngIf="stores.length > 1" [object]="stores" [roleType]="m.SalesOrder.Store" [options]="stores" display="Name"></a-mat-select>

      <a-mat-autocomplete *ngIf="showOrganisations" [object]="order" [roleType]="m.SalesOrder.ShipToCustomer" [options]="organisations" [filter]="organisationsFilter.create()"
        display="Name" (onSelect)="shipToCustomerSelected($event)" label="Ship to organisation"></a-mat-autocomplete>
      <a-mat-autocomplete *ngIf="showPeople" [object]="order" [roleType]="m.SalesOrder.ShipToCustomer" [options]="people" [filter]="peopleFilter.create()"
        display="displayName" (onSelect)="shipToCustomerSelected($event)" label="Ship to private  person "></a-mat-autocomplete>

      <a-mat-select *ngIf="showOrganisations && !showPeople" [object]="order" [roleType]="m.SalesOrder.ContactPerson" [options]="contacts" display="displayName"></a-mat-select>
      <button *ngIf="showOrganisations && !showPeople" type="button" mat-icon-button (click)="addPerson = true"><mat-icon>add</mat-icon></button>
      <div *ngIf="showOrganisations && addPerson" style="background: lightblue" class="pad">
        <person-inline (cancelled)="personCancelled($event)" (saved)="personAdded($event)">
        </person-inline>
      </div>

        <a-mat-select [object]="order" [roleType]="m.SalesOrder.ShipToAddress" [options]="ShipToAddresses" display="displayName"></a-mat-select>
      <button *ngIf="order.ShipToCustomer" type="button" mat-button (click)="addShipToAddress = true">+Postal</button>

      <div *ngIf="addShipToAddress && order.ShipToCustomer" style="background: lightblue" class="pad">
        <party-contactmechanism-postaladdress [scope]="scope" (cancelled)="shipToAddressCancelled($event)" (saved)="shipToAddressAdded($event)">
        </party-contactmechanism-postaladdress>
      </div>

      <a-mat-autocomplete *ngIf="showOrganisations" [object]="order" [roleType]="m.SalesOrder.BillToCustomer" [options]="organisations" [filter]="organisationsFilter.create()"
        display="Name" (onSelect)="billToCustomerSelected($event)" label="Bill to organisation"></a-mat-autocomplete>
      <a-mat-autocomplete *ngIf="showPeople" [object]="order" [roleType]="m.SalesOrder.BillToCustomer" [options]="people" [filter]="peopleFilter.create()"
        display="displayName" (onSelect)="billToCustomerSelected($event)" label="Bill to person "></a-mat-autocomplete>
      <a-mat-select [object]="order" [roleType]="m.SalesOrder.BillToContactMechanism" [options]="billToContactMechanisms" display="displayName"></a-mat-select>
      <button *ngIf="order.BillToCustomer" type="button" mat-button (click)="addWebAddress = true">+Web</button>
      <button *ngIf="order.BillToCustomer" type="button" mat-button (click)="addEmailAddress = true">+Email</button>
      <button *ngIf="order.BillToCustomer" type="button" mat-button (click)="addPostalAddress = true">+Postal</button>
      <button *ngIf="order.BillToCustomer" type="button" mat-button (click)="addTeleCommunicationsNumber = true">+Telecom</button>

      <div *ngIf="addWebAddress && order.BillToCustomer" style="background: lightblue" class="pad">
        <party-contactmechanism-webaddress  [scope]="scope" (cancelled)="webAddressCancelled($event)" (saved)="webAddressAdded($event)">
        </party-contactmechanism-webaddress>
      </div>
      <div *ngIf="addEmailAddress && order.BillToCustomer" style="background: lightblue" class="pad">
        <party-contactmechanism-emailAddress  [scope]="scope" (cancelled)="emailAddressCancelled($event)" (saved)="emailAddressAdded($event)">
        </party-contactmechanism-emailAddress>
      </div>
      <div *ngIf="addPostalAddress && order.BillToCustomer" style="background: lightblue" class="pad">
        <party-contactmechanism-postaladdress [scope]="scope" (cancelled)="postalAddressCancelled($event)" (saved)="postalAddressAdded($event)">
        </party-contactmechanism-postaladdress>
      </div>
      <div *ngIf="addTeleCommunicationsNumber && order.BillToCustomer" style="background: lightblue" class="pad">
        <party-contactmechanism-telecommunicationsnumber [scope]="scope" (cancelled)="teleCommunicationsNumberCancelled($event)" (saved)="teleCommunicationsNumberAdded($event)">
        </party-contactmechanism-telecommunicationsnumber>
      </div>

      <a-mat-input [object]="order" [roleType]="m.SalesOrder.Description"></a-mat-input>
      <a-mat-select [object]="order" [roleType]="m.SalesOrder.VatRegime" [options]="vatRegimes" display="Name"></a-mat-select>
      <a-mat-static *ngIf="order.VatRegime" [object]="order.VatRegime" [roleType]="m.VatRegime.VatRate" display="Rate"></a-mat-static>
      <a-mat-static *ngIf="quote?.Comment" [object]="quote" [roleType]="m.Quote.Comment" label="Quote Comment"></a-mat-static>
      <a-mat-textarea [object]="order" [roleType]="m.SalesOrder.Comment" label="Order Comment"></a-mat-textarea>
      <a-mat-static *ngIf="quote?.InternalComment" [object]="quote" [roleType]="m.Quote.InternalComment" label="Quote Internal Comment"></a-mat-static>
      <a-mat-textarea [object]="order" [roleType]="m.SalesOrder.InternalComment" label="Order Internal Comment"></a-mat-textarea>
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
        router_1.Router,
        router_1.ActivatedRoute,
        material_1.MatSnackBar,
        core_2.TdDialogService,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], SalesOrderEditComponent);
exports.SalesOrderEditComponent = SalesOrderEditComponent;
