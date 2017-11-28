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
let PartyCommunicationEventPhoneCommunicationComponent = class PartyCommunicationEventPhoneCommunicationComponent {
    constructor(workspaceService, errorService, route, snackBar, dialogService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.snackBar = snackBar;
        this.dialogService = dialogService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Phone Communication";
        this.addCaller = false;
        this.addReceiver = false;
        this.addPhoneNumber = false;
        this.contacts = [];
        this.phonenumbers = [];
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
    }
    get PartyIsOrganisation() {
        return this.party instanceof (workspace_1.Organisation);
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
                    id,
                    include: [
                        new framework_1.TreeNode({ roleType: m.Party.CurrentContacts }),
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                            ],
                            roleType: m.Party.CurrentPartyContactMechanisms,
                        }),
                    ],
                    name: "party",
                }),
                new framework_1.Fetch({
                    id: roleId,
                    include: [
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                        new framework_1.TreeNode({ roleType: m.CommunicationEvent.ToParties }),
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
            this.singleton = loaded.collections.singletons[0];
            this.employees = this.singleton.InternalOrganisation.ActiveEmployees;
            this.purposes = loaded.collections.purposes;
            this.party = loaded.objects.party;
            this.communicationEvent = loaded.objects.communicationEvent;
            if (!this.communicationEvent) {
                this.communicationEvent = this.scope.session.create("PhoneCommunication");
                this.communicationEvent.AddCaller(this.party);
            }
            const contactMechanisms = this.party.CurrentPartyContactMechanisms.map((v) => v.ContactMechanism);
            for (const contactMechanism of contactMechanisms) {
                if (contactMechanism instanceof (workspace_1.TelecommunicationsNumber)) {
                    this.phonenumbers.push(contactMechanism);
                }
            }
            this.contacts.push(this.party);
            if (this.employees.length > 0) {
                this.contacts = this.contacts.concat(this.employees);
            }
            if (this.party.CurrentContacts.length > 0) {
                this.contacts = this.contacts.concat(this.party.CurrentContacts);
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
    phoneNumberCancelled() {
        this.addPhoneNumber = false;
    }
    callerCancelled() {
        this.addCaller = false;
    }
    receiverCancelled() {
        this.addReceiver = false;
    }
    phoneNumberAdded(id) {
        this.addPhoneNumber = false;
        const telecommunicationsNumber = this.scope.session.get(id);
        const partyContactMechanism = this.scope.session.create("PartyContactMechanism");
        partyContactMechanism.ContactMechanism = telecommunicationsNumber;
        this.party.AddPartyContactMechanism(partyContactMechanism);
        this.phonenumbers.push(telecommunicationsNumber);
        this.communicationEvent.AddContactMechanism(telecommunicationsNumber);
    }
    callerAdded(id) {
        this.addCaller = false;
        const caller = this.scope.session.get(id);
        const relationShip = this.scope.session.create("OrganisationContactRelationship");
        relationShip.Contact = caller;
        relationShip.Organisation = this.party;
        this.communicationEvent.AddCaller(caller);
    }
    receiverAdded(id) {
        this.addReceiver = false;
        const receiver = this.scope.session.get(id);
        const relationShip = this.scope.session.create("OrganisationContactRelationship");
        relationShip.Contact = receiver;
        relationShip.Organisation = this.party;
        this.communicationEvent.AddReceiver(receiver);
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
PartyCommunicationEventPhoneCommunicationComponent = __decorate([
    core_1.Component({
        template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="communicationEvent" (submit)="save()">

    <div class="pad">

      <div *ngIf="communicationEvent.CommunicationEventState">
        <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.CommunicationEventState" display="Name" label="Status"></a-mat-static>
        <button *ngIf="communicationEvent.CanExecuteClose" mat-button type="button" (click)="close()">Close</button>
        <button *ngIf="communicationEvent.CanExecuteCancel" mat-button type="button" (click)="cancel()">Cancel</button>
        <button *ngIf="communicationEvent.CanExecuteReopen" mat-button type="button" (click)="reopen()">Reopen</button>
      </div>

      <a-mat-input [object]="communicationEvent" [roleType]="m.PhoneCommunication.Subject"></a-mat-input>
      <a-mat-textarea [object]="communicationEvent" [roleType]="m.PhoneCommunication.Note"></a-mat-textarea>

      <a-mat-slide-toggle [object]="communicationEvent" [roleType]="m.PhoneCommunication.IncomingCall"></a-mat-slide-toggle>
      <a-mat-slide-toggle [object]="communicationEvent" [roleType]="m.PhoneCommunication.LeftVoiceMail"></a-mat-slide-toggle>

      <a-mat-select [object]="communicationEvent" [roleType]="m.PhoneCommunication.EventPurposes" [options]="purposes" display="Name"></a-mat-select>

      <div fxLayout.gt-md="row" fxLayoutGap.gt-md="2rem">
        <a-td-chips fxFlex.gt-md [object]="communicationEvent" [roleType]="m.PhoneCommunication.Callers" [options]="contacts" display="PartyName"></a-td-chips>
        <a-td-chips fxFlex.gt-md [object]="communicationEvent" [roleType]="m.PhoneCommunication.Receivers" [options]="contacts" display="PartyName"></a-td-chips>
      </div>

      <a-mat-select *ngIf="!communicationEvent.IncomingCall" [object]="communicationEvent" [roleType]="m.PhoneCommunication.ContactMechanisms"
        [options]="phonenumbers" display="displayName" label="Phone number"></a-mat-select>

      <a-mat-slide-toggle [object]="communicationEvent" [roleType]="m.CommunicationEvent.SendNotification"></a-mat-slide-toggle>
      <a-mat-slide-toggle [object]="communicationEvent" [roleType]="m.CommunicationEvent.SendReminder"></a-mat-slide-toggle>
      <div fxLayout="column" fxLayout.gt-sm="row" fxLayoutGap.gt-sm="2rem" class="pad-bottom">
        <a-mat-datepicker [object]="communicationEvent" [roleType]="m.CommunicationEvent.ScheduledStart" [useTime]="true"></a-mat-datepicker>
        <a-mat-datepicker [object]="communicationEvent" [roleType]="m.CommunicationEvent.ScheduledEnd" [useTime]="true"></a-mat-datepicker>
      </div>
      <div fxLayout="column" fxLayout.gt-sm="row" fxLayoutGap.gt-sm="2rem" class="pad-bottom">
        <a-mat-datepicker [object]="communicationEvent" [roleType]="m.CommunicationEvent.ActualStart" [useTime]="true"></a-mat-datepicker>
        <a-mat-datepicker [object]="communicationEvent" [roleType]="m.CommunicationEvent.ActualEnd" [useTime]="true"></a-mat-datepicker>
      </div>

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
        material_1.MatSnackBar,
        core_2.TdDialogService,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], PartyCommunicationEventPhoneCommunicationComponent);
exports.PartyCommunicationEventPhoneCommunicationComponent = PartyCommunicationEventPhoneCommunicationComponent;
