import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy , OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import 'rxjs/add/observable/combineLatest';

import { MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask, WorkEffortPurpose, CommunicationEvent, Organisation, OrganisationContactRelationship, ContactMechanism, PersonRole, CustomerRelationship, EmailCommunication, Party, CommunicationEventPurpose, EmailTemplate, EmailAddress } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals } from "@allors/framework";

@Component({
  template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="communicationEvent" (submit)="save()">

    <div class="pad-left pad-right">
      <div *ngIf="communicationEvent.CommunicationEventState">
        <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.CommunicationEventState" display="Name" label="Status"></a-mat-static>
        <button *ngIf="communicationEvent.CanExecuteClose" mat-button type="button" (click)="close()">Close</button>
        <button *ngIf="communicationEvent.CanExecuteCancel" mat-button type="button" (click)="cancel()">Cancel</button>
        <button *ngIf="communicationEvent.CanExecuteReopen" mat-button type="button" (click)="reopen()">Reopen</button>
      </div>

      <a-mat-select [object]="communicationEvent" [roleType]="m.EmailCommunication.EventPurposes" [options]="purposes" display="Name"></a-mat-select>
      <a-mat-slide-toggle [object]="communicationEvent" [roleType]="m.EmailCommunication.IncomingMail"></a-mat-slide-toggle>

      <div fxLayout="row">
        <a-mat-select fxFlex [object]="communicationEvent" [roleType]="m.EmailCommunication.Originator" [options]="emailAddresses"
          display="displayName" label="From"></a-mat-select>
        <button *ngIf="communicationEvent.IncomingMail" type="button" mat-icon-button (click)="addOriginator = true"><mat-icon>add</mat-icon></button>
      </div>

      <div *ngIf="addOriginator" style="background: lightblue" class="pad">
        <party-contactmechanism-emailAddress [scope]="scope"(cancelled)="originatorCancelled($event)" (saved)="originatorAdded($event)">
        </party-contactmechanism-emailAddress>
      </div>

      <div fxLayout="row">
        <a-td-chips fxFlex [object]="communicationEvent" [roleType]="m.EmailCommunication.Addressees" [options]="emailAddresses"
          display="displayName" label="To"></a-td-chips>
        <button *ngIf="!communicationEvent.IncomingMail" type="button" mat-icon-button (click)="addAddressee = true"><mat-icon>add</mat-icon></button>
      </div>

      <div *ngIf="addAddressee" style="background: lightblue" class="pad">
        <party-contactmechanism-emailAddress [scope]="scope" (cancelled)="addresseeCancelled($event)" (saved)="addresseeAdded($event)">
        </party-contactmechanism-emailAddress>
      </div>

      <a-mat-input [object]="emailTemplate" [roleType]="m.EmailTemplate.SubjectTemplate" label="Subject "></a-mat-input>
      <a-mat-textarea [object]="emailTemplate" [roleType]="m.EmailTemplate.BodyTemplate" label="Body"></a-mat-textarea>
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
})
export class PartyCommunicationEventEmailCommunicationComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string = "Email Communication";
  public subTitle: string;

  public addOriginator: boolean = false;
  public addAddressee: boolean = false;

  public m: MetaDomain;

  public singleton: Singleton;
  public communicationEvent: EmailCommunication;
  public employees: Person[];
  public party: Party;
  public purposes: CommunicationEventPurpose[];
  public emailAddresses: ContactMechanism[] = [];
  public emailTemplate: EmailTemplate;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope()
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
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
            name: "party",
            id,
          }),
          new Fetch({
            id: roleId,
            include: [
              new TreeNode({ roleType: m.EmailCommunication.Originator }),
              new TreeNode({ roleType: m.EmailCommunication.Addressees }),
              new TreeNode({ roleType: m.EmailCommunication.EmailTemplate }),
              new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
              new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
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
                            new TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
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
        this.communicationEvent = loaded.objects.communicationEvent as EmailCommunication;

        if (!this.communicationEvent) {
          this.communicationEvent = this.scope.session.create("EmailCommunication") as EmailCommunication;
          this.emailTemplate = this.scope.session.create("EmailTemplate") as EmailTemplate;
          this.communicationEvent.EmailTemplate = this.emailTemplate;
          this.communicationEvent.Originator = this.party.GeneralEmail;
        }

        for (const employee of this.employees) {
          const employeeContactMechanisms: ContactMechanism[] = employee.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
          for (const contactMechanism of employeeContactMechanisms) {
            if (contactMechanism instanceof (EmailAddress)) {
              this.emailAddresses.push(contactMechanism);
            }
          }
        }

        const contactMechanisms: ContactMechanism[] = this.party.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        for (const contactMechanism of contactMechanisms) {
          if (contactMechanism instanceof (EmailAddress)) {
            this.emailAddresses.push(contactMechanism);
          }
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

  public originatorCancelled(): void {
    this.addOriginator = false;
  }

  public addresseeCancelled(): void {
    this.addAddressee = false;
  }

  public originatorAdded(id: string): void {
    this.addOriginator = false;

    const emailAddress: EmailAddress = this.scope.session.get(id) as EmailAddress;
    const partyContactMechanism: PartyContactMechanism = this.scope.session.create("PartyContactMechanism") as PartyContactMechanism;
    partyContactMechanism.ContactMechanism = emailAddress;
    this.party.AddPartyContactMechanism(partyContactMechanism);

    this.emailAddresses.push(emailAddress);
    this.communicationEvent.Originator = emailAddress;
  }

  public addresseeAdded(id: string): void {
    this.addAddressee = false;

    const emailAddress: EmailAddress = this.scope.session.get(id) as EmailAddress;
    const partyContactMechanism: PartyContactMechanism = this.scope.session.create("PartyContactMechanism") as PartyContactMechanism;
    partyContactMechanism.ContactMechanism = emailAddress;
    this.party.AddPartyContactMechanism(partyContactMechanism);

    this.emailAddresses.push(emailAddress);
    this.communicationEvent.AddAddressee(emailAddress);
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
