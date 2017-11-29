import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import 'rxjs/add/observable/combineLatest';

import { PostalAddress, MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask, WorkEffortPurpose, CommunicationEvent, Organisation, OrganisationContactRelationship, ContactMechanism, PersonRole, CustomerRelationship, Country, ProductCharacteristic, ProductQuote, RequestForQuote, Currency, Party, OrganisationRole, EmailCommunication, FaceToFaceCommunication, PhoneCommunication, LetterCorrespondence } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked, Filter } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals, Contains } from "@allors/framework";

@Component({
  template: `
<mat-toolbar>
  <div layout="row" layout-align="start center" flex>
    <button mat-icon-button tdLayoutManageListOpen [hideWhenOpened]="true" style="display: none">
          <mat-icon>arrow_back</mat-icon>
        </button>
    <span>{{title}}</span>
    <span flex></span>
    <button mat-icon-button><mat-icon>settings</mat-icon></button>
  </div>
</mat-toolbar>

<div body *ngIf="isEmail" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card *ngIf="communicationEvent" tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Email</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div class="pad">
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.CommunicationEventState" display="Name" label="Status"></a-mat-static>
          <a-mat-static *ngIf="communicationEvent.EventPurposes" [object]="communicationEvent" [roleType]="m.CommunicationEvent.EventPurposes" display="Name" label="Purposes"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.EmailCommunication.IncomingMail"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.EmailCommunication.Originator" display="displayName" label="From"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.EmailCommunication.Addressees" display="displayName" label="To"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.EmailCommunication.EmailTemplate" display="SubjectTemplate" label="Subject"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.EmailCommunication.EmailTemplate" display="BodyTemplate" label="Body"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ScheduledStart"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ScheduledEnd"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ActualStart"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ActualEnd"></a-mat-static>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button mat-button [routerLink]="'/party/' + this.party.id + '/communicationevent/emailcommunication/' + this.communicationEvent.id">Edit</button>
      </mat-card-actions>
    </mat-card>
  </div>
</div>

<div body *ngIf="isLetter" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card *ngIf="communicationEvent" tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Letter</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div class="pad">
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.CommunicationEventState" display="Name" label="Status"></a-mat-static>
          <a-mat-static *ngIf="communicationEvent.EventPurposes" [object]="communicationEvent" [roleType]="m.CommunicationEvent.EventPurposes" display="Name" label="Purposes"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.LetterCorrespondence.Originators" display="displayName" label="Sender"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.LetterCorrespondence.Receivers" display="displayName" label="Receiver"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.LetterCorrespondence.ContactMechanisms" display="displayName" label="Postal Address"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.LetterCorrespondence.Subject"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.LetterCorrespondence.Note"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.LetterCorrespondence.IncomingLetter"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ScheduledStart"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ScheduledEnd"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ActualStart"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ActualEnd"></a-mat-static>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button mat-button [routerLink]="'/party/' + this.party.id + '/communicationevent/lettercorrespondence/' + this.communicationEvent.id">Edit</button>
      </mat-card-actions>
    </mat-card>
  </div>
</div>

<div body *ngIf="isMeeting" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card *ngIf="communicationEvent" tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Meeting</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div class="pad">
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.CommunicationEventState" display="Name" label="Status"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.EventPurposes" display="Name" label="Purposes"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.FaceToFaceCommunication.Participants" display="PartyName"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.FaceToFaceCommunication.Location"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.FaceToFaceCommunication.Subject"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.FaceToFaceCommunication.Note"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ScheduledStart"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ScheduledEnd"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ActualStart"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ActualEnd"></a-mat-static>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button mat-button [routerLink]="'/party/' + this.party.id + '/communicationevent/facetofacecommunication/' + this.communicationEvent.id">Edit</button>
      </mat-card-actions>
    </mat-card>
  </div>
</div>

<div body *ngIf="isPhone" layout-gt-md="row">
  <div flex-gt-xs="50">
    <mat-card *ngIf="communicationEvent" tdMediaToggle="gt-xs" [mediaClasses]="['push']">
      <mat-card-title>Phone call</mat-card-title>
      <mat-divider></mat-divider>
      <mat-card-content>

        <div class="pad">
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.CommunicationEventState" display="Name" label="Status"></a-mat-static>
          <a-mat-static *ngIf="communicationEvent.EventPurposes" [object]="communicationEvent" [roleType]="m.CommunicationEvent.EventPurposes" display="Name" label="Purposes"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.PhoneCommunication.Subject"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.PhoneCommunication.Note"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.PhoneCommunication.IncomingCall"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.PhoneCommunication.LeftVoiceMail"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.PhoneCommunication.Callers" display="PartyName"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.PhoneCommunication.Receivers" display="PartyName"></a-mat-static>
          <a-mat-static *ngIf="!communicationEvent.IncomingCall && communicationEvent.ContactMechanisms" [object]="communicationEvent" [roleType]="m.PhoneCommunication.ContactMechanisms"
            display="displayName" label="Phone number"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ScheduledStart"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ScheduledEnd"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ActualStart"></a-mat-static>
          <a-mat-static [object]="communicationEvent" [roleType]="m.CommunicationEvent.ActualEnd"></a-mat-static>
        </div>

      </mat-card-content>

      <mat-divider></mat-divider>
      <mat-card-actions>
        <button mat-button [routerLink]="'/party/' + this.party.id + '/communicationevent/phonecommunication/' + this.communicationEvent.id">Edit</button>
      </mat-card-actions>
    </mat-card>
  </div>
</div>

<mat-card *ngIf="communicationEvent" tdMediaToggle="gt-xs" [mediaClasses]="['push']">
  <mat-card-title>Work Efforts</mat-card-title>

  <mat-divider></mat-divider>
  <mat-card-content>
    <ng-template tdLoading="communicationEvent.WorkEfforts">
      <mat-list class="will-load">
        <div class="mat-padding" *ngIf="communicationEvent.WorkEfforts.length === 0" layout="row" layout-align="center center">
          <h3>No work efforts to display.</h3>
        </div>

        <ng-template let-workEffort let-last="last" ngFor [ngForOf]="communicationEvent.WorkEfforts">
          <mat-list-item>
            <div mat-line class="mat-caption pointer" [routerLink]="['/worktask/' + workEffort.id ]">
              {{ workEffort.Name }}, {{ workEffort.WorkEffortState.Name }}
            </div>

            <p mat-line class="mat-caption">{{ workEffort.Description }} </p>
            <p *ngIf="workEffort.Priority" mat-line class="mat-caption">Priority {{ workEffort.Priority.Name }} </p>

            <span hide-xs hide-sm hide-md flex-gt-xs="60" flex-xs="40" layout-gt-xs="row">
              <div class="mat-caption tc-grey-500" flex-gt-xs="50"> Sched. Start: {{ workEffort.ScheduledStart | date}} </div>
              <div class="mat-caption tc-grey-500" flex-gt-xs="50"> Sched. Compl.: {{ workEffort.ScheduledCompletion | date }} </div>
              <div class="mat-caption tc-grey-500" flex-gt-xs="50"> Act. Start: {{ workEffort.ActualStart | date}} </div>
              <div class="mat-caption tc-grey-500" flex-gt-xs="50"> Act. Compl.: {{ workEffort.ActualCompletion | date }} </div>
            </span>

            <span>
              <button mat-icon-button [mat-menu-trigger-for]="menu">
                <mat-icon>more_vert</mat-icon>
              </button>
              <mat-menu x-position="before" #menu="matMenu">
                <a [routerLink]="['/worktask/' + workEffort.id]" mat-menu-item>Edit</a>
                <button  mat-menu-item (click)="deleteWorkEffort(workEffort)" [disabled]="!workEffort.CanExecuteDelete">Delete</button>
              </mat-menu>
          </span>

          </mat-list-item>

          <mat-divider *ngIf="!last" mat-inset></mat-divider>

        </ng-template>
      </mat-list>
    </ng-template>
  </mat-card-content>

  <mat-divider></mat-divider>
  <mat-card-actions>
    <button mat-button [routerLink]="['/communicationevent/' + communicationEvent.id +'/worktask']">+ Task</button>
  </mat-card-actions>
</mat-card>
`,
})
export class CommunicationEventOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string = "Communication Event overview";
  public m: MetaDomain;

  public communicationEventPrefetch: CommunicationEvent;
  public communicationEvent: CommunicationEvent;
  public  party: Party;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  get isEmail(): boolean {
    return this.communicationEventPrefetch instanceof (EmailCommunication);
  }

  get isMeeting(): boolean {
    return this.communicationEventPrefetch instanceof (FaceToFaceCommunication);
  }

  get isPhone(): boolean {
    return this.communicationEventPrefetch instanceof (PhoneCommunication);
  }

  get isLetter(): boolean {
    return this.communicationEvent instanceof (LetterCorrespondence);
  }

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: TdDialogService,
    private snackBar: MatSnackBar,

    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

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

        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id: roleId,
            name: "communicationEventPrefetch",
          }),
          new Fetch({
            name: "party",
            id,
          }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch }))
          .switchMap((loaded: Loaded) => {
            this.communicationEventPrefetch = loaded.objects.communicationEventPrefetch as CommunicationEvent;
            this.party = loaded.objects.party as Party;

            const fetchEmail: Fetch[] = [
              new Fetch({
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.EmailCommunication.Originator }),
                  new TreeNode({ roleType: m.EmailCommunication.Addressees }),
                  new TreeNode({ roleType: m.EmailCommunication.EmailTemplate }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
                ],
                name: "communicationEvent",
              }),
            ];

            const fetchLetter: Fetch[] = [
              new Fetch({
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.LetterCorrespondence.Originators }),
                  new TreeNode({ roleType: m.LetterCorrespondence.Receivers }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
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

            const fetchMeeting: Fetch[] = [
              new Fetch({
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
                ],
                name: "communicationEvent",
              }),
            ];

            const fetchPhone: Fetch[] = [
              new Fetch({
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
                ],
                name: "communicationEvent",
              }),
            ];

            if (this.isEmail) {
              return this.scope.load("Pull", new PullRequest({ fetch: fetchEmail }));
            }

            if (this.isMeeting) {
              return this.scope.load("Pull", new PullRequest({ fetch: fetchMeeting }));
            }

            if (this.isLetter) {
              return this.scope.load("Pull", new PullRequest({ fetch: fetchLetter }));
            }

            if (this.isPhone) {
              return this.scope.load("Pull", new PullRequest({ fetch: fetchPhone }));
            }
          });
      })
      .subscribe((loaded: Loaded) => {
        this.scope.session.reset();
        this.communicationEvent = loaded.objects.communicationEvent as CommunicationEvent;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public deleteWorkEffort(worktask: WorkTask): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this work task?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(worktask.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
