import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy , OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../../../angular";
import { Brand, Catalogue, CatScope, ContactMechanism, Currency, Facility, Good, InventoryItemKind, InventoryItemVariance,
  Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationContactRelationship,
  OrganisationRole, Party, PartyContactMechanism, Person, ProcessFlow, ProductCategory, ProductCharacteristic,
  ProductCharacteristicValue, ProductFeature, ProductQuote, ProductType, QuoteItem, RequestForQuote, SalesInvoice, SalesInvoiceItem, SalesOrder, SalesOrderItem,
  Singleton, VarianceReason, VatRate, VatRegime, LetterCorrespondence, CommunicationEventPurpose, PostalAddress } from "../../../../../../domain";
import { And, ContainedIn, Contains, Fetch, Like, Page, Path, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../../../framework";
import { MetaDomain } from "../../../../../../meta";

@Component({
  template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="communicationEvent" (submit)="save()">

    <div class="pad">

      <div>
        <div *ngIf="communicationEvent.CommunicationEventState">
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.CommunicationEventState" display="Name" label="Status"></a-mat-static>
          <button *ngIf="communicationEvent.CanExecuteClose" mat-button type="button" (click)="close()">Close</button>
          <button *ngIf="communicationEvent.CanExecuteCancel" mat-button type="button" (click)="cancel()">Cancel</button>
          <button *ngIf="communicationEvent.CanExecuteReopen" mat-button type="button" (click)="reopen()">Reopen</button>
        </div>

        <a-mat-select [object]="communicationEvent" [roleType]="m.EmailCommunication.EventPurposes" [options]="purposes" display="Name"></a-mat-select>

        <div fxLayout="row">
          <a-td-chips fxFlex [object]="communicationEvent" [roleType]="m.LetterCorrespondence.Originators" [options]="contacts" display="displayName"
            label="Sender"></a-td-chips>
          <button *ngIf="communicationEvent.IncomingLetter && PartyIsOrganisation" type="button" mat-icon-button (click)="addSender = true"><mat-icon>add</mat-icon></button>
        </div>

        <div *ngIf="addSender" style="background: lightblue" class="pad">
          <person-inline (cancelled)="senderCancelled($event)" (saved)="senderAdded($event)">
          </person-inline>
        </div>

        <div fxLayout="row">
          <a-td-chips fxFlex [object]="communicationEvent" [roleType]="m.LetterCorrespondence.Receivers" [options]="contacts" display="displayName"
            label="Receiver"></a-td-chips>
          <button *ngIf="!communicationEvent.IncomingLetter && PartyIsOrganisation" type="button" mat-icon-button (click)="addReceiver = true"><mat-icon>add</mat-icon></button>
        </div>

        <div *ngIf="addReceiver" style="background: lightblue" class="pad">
          <person-inline (cancelled)="receiverCancelled($event)" (saved)="receiverAdded($event)">
          </person-inline>
        </div>

        <div fxLayout="row">
          <a-mat-select fxFlex [object]="communicationEvent" [roleType]="m.LetterCorrespondence.ContactMechanisms" [options]="postalAddresses"
            display="displayName" label="Postal Address"></a-mat-select>
          <button type="button" mat-icon-button (click)="addAddress = true"><mat-icon>add</mat-icon></button>
        </div>

        <div *ngIf="addAddress" style="background: lightblue" class="pad">
          <party-contactmechanism-postaladdress [scope]="scope" (cancelled)="addressCancelled($event)" (saved)="addressAdded($event)">
          </party-contactmechanism-postaladdress>
        </div>

        <a-mat-input [object]="communicationEvent" [roleType]="m.LetterCorrespondence.Subject"></a-mat-input>
        <a-mat-textarea [object]="communicationEvent" [roleType]="m.LetterCorrespondence.Note"></a-mat-textarea>
        <a-mat-slide-toggle [object]="communicationEvent" [roleType]="m.LetterCorrespondence.IncomingLetter"></a-mat-slide-toggle>
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

    </div>

    <mat-divider></mat-divider>

    <mat-card-actions>
      <button mat-button color="primary" type="submit" [disabled]="!form.form.valid">SAVE</button>
      <button mat-button (click)="goBack()" type="button">CANCEL</button>
    </mat-card-actions>

  </form>
</td-layout-card-over>
`,
})
export class PartyCommunicationEventLetterCorrespondenceComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string = "Letter Correspondence";
  public subTitle: string;

  public addSender: boolean = false;
  public addReceiver: boolean = false;
  public addAddress: boolean = false;

  public m: MetaDomain;

  public singleton: Singleton;
  public communicationEvent: LetterCorrespondence;
  public employees: Person[];
  public contacts: Party[] = [];
  public party: Party;
  public purposes: CommunicationEventPurpose[];
  public postalAddresses: ContactMechanism[] = [];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope()
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  get PartyIsOrganisation(): boolean {
    return this.party instanceof (Organisation);
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const roleId: string = this.route.snapshot.paramMap.get("roleId");

        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Party.CurrentContacts }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({
                        nodes: [
                          new TreeNode({ roleType: m.PostalBoundary.Country }),
                        ],
                        roleType: m.PostalAddress.PostalBoundary,
                      }),
                    ],
                    roleType: m.PartyContactMechanism.ContactMechanism,
                  }),
                ],
                roleType: m.Party.CurrentPartyContactMechanisms,
              }),
            ],
            name: "party",
          }),
          new Fetch({
            id: roleId,
            include: [
              new TreeNode({ roleType: m.LetterCorrespondence.Originators }),
              new TreeNode({ roleType: m.LetterCorrespondence.Receivers }),
              new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
              new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.LetterCorrespondence.PostalAddresses,
              }),
            ],
            name: "communicationEvent",
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              include: [
                new TreeNode({
                  nodes: [
                    new TreeNode({
                      nodes: [
                        new TreeNode({
                          nodes: [
                            new TreeNode({
                              nodes: [
                                new TreeNode({
                                  nodes: [
                                    new TreeNode({ roleType: m.PostalBoundary.Country }),
                                  ],
                                  roleType: m.PostalAddress.PostalBoundary,
                                }),
                              ],
                              roleType: m.PartyContactMechanism.ContactMechanism,
                            }),
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
          new Query(
            {
              name: "purposes",
              objectType: this.m.CommunicationEventPurpose,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.party = loaded.objects.party as Party;
        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.employees = this.singleton.InternalOrganisation.ActiveEmployees;
        this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];
        this.communicationEvent = loaded.objects.communicationEvent as LetterCorrespondence;

        if (!this.communicationEvent) {
          this.communicationEvent = this.scope.session.create("LetterCorrespondence") as LetterCorrespondence;
          this.communicationEvent.AddOriginator(this.party);
        }

        for (const employee of this.employees) {
          const employeeContactMechanisms: ContactMechanism[] = employee.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
          for (const contactMechanism of employeeContactMechanisms) {
            if (contactMechanism instanceof (PostalAddress)) {
              this.postalAddresses.push(contactMechanism);
            }
          }
        }

        const contactMechanisms: ContactMechanism[] = this.party.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        for (const contactMechanism of contactMechanisms) {
          if (contactMechanism instanceof (PostalAddress)) {
            this.postalAddresses.push(contactMechanism);
          }
        }

        this.contacts.push(this.party);
        if (this.employees.length > 0) {
          this.contacts = this.contacts.concat(this.employees);
        }

        if (this.party.CurrentContacts.length > 0) {
          this.contacts = this.contacts.concat(this.party.CurrentContacts);
        }
    },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public senderCancelled(): void {
    this.addSender = false;
  }

  public receiverCancelled(): void {
    this.addReceiver = false;
  }

  public addressCancelled(): void {
    this.addAddress = false;
  }

  public senderAdded(id: string): void {
    this.addSender = false;

    const sender: Person = this.scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    relationShip.Contact = sender;
    relationShip.Organisation = this.party as Organisation;

    this.communicationEvent.AddOriginator(sender);
  }

  public receiverAdded(id: string): void {
    this.addReceiver = false;

    const receiver: Person = this.scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    relationShip.Contact = receiver;
    relationShip.Organisation = this.party as Organisation;

    this.communicationEvent.AddReceiver(receiver);
  }

  public addressAdded(id: string): void {
    this.addAddress = false;

    const postalAddress: PostalAddress = this.scope.session.get(id) as PostalAddress;
    const partyContactMechanism: PartyContactMechanism = this.scope.session.create("PartyContactMechanism") as PartyContactMechanism;
    partyContactMechanism.ContactMechanism = postalAddress;
    this.party.AddPartyContactMechanism(partyContactMechanism);

    this.postalAddresses.push(postalAddress);
    this.communicationEvent.AddPostalAddress(postalAddress);
  }

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public close(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Close)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully closed.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public reopen(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Reopen)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully reopened.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
