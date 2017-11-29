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
let CommunicationEventOverviewComponent = class CommunicationEventOverviewComponent {
    constructor(workspaceService, errorService, route, dialogService, snackBar, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.dialogService = dialogService;
        this.snackBar = snackBar;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Communication Event overview";
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.refresh$ = new BehaviorSubject_1.BehaviorSubject(undefined);
    }
    get isEmail() {
        return this.communicationEventPrefetch instanceof (workspace_1.EmailCommunication);
    }
    get isMeeting() {
        return this.communicationEventPrefetch instanceof (workspace_1.FaceToFaceCommunication);
    }
    get isPhone() {
        return this.communicationEventPrefetch instanceof (workspace_1.PhoneCommunication);
    }
    get isLetter() {
        return this.communicationEvent instanceof (workspace_1.LetterCorrespondence);
    }
    ngOnInit() {
        const route$ = this.route.url;
        const combined$ = Observable_1.Observable.combineLatest(route$, this.refresh$);
        this.subscription = combined$
            .switchMap(([urlSegments, date]) => {
            const id = this.route.snapshot.paramMap.get("id");
            const roleId = this.route.snapshot.paramMap.get("roleId");
            const m = this.m;
            const fetch = [
                new framework_1.Fetch({
                    id: roleId,
                    name: "communicationEventPrefetch",
                }),
                new framework_1.Fetch({
                    name: "party",
                    id,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch }))
                .switchMap((loaded) => {
                this.communicationEventPrefetch = loaded.objects.communicationEventPrefetch;
                this.party = loaded.objects.party;
                const fetchEmail = [
                    new framework_1.Fetch({
                        id: roleId,
                        include: [
                            new framework_1.TreeNode({ roleType: m.EmailCommunication.Originator }),
                            new framework_1.TreeNode({ roleType: m.EmailCommunication.Addressees }),
                            new framework_1.TreeNode({ roleType: m.EmailCommunication.EmailTemplate }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.Priority }),
                                ],
                                roleType: m.CommunicationEvent.WorkEfforts,
                            }),
                        ],
                        name: "communicationEvent",
                    }),
                ];
                const fetchLetter = [
                    new framework_1.Fetch({
                        id: roleId,
                        include: [
                            new framework_1.TreeNode({ roleType: m.LetterCorrespondence.Originators }),
                            new framework_1.TreeNode({ roleType: m.LetterCorrespondence.Receivers }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.Priority }),
                                ],
                                roleType: m.CommunicationEvent.WorkEfforts,
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
                                roleType: m.LetterCorrespondence.PostalAddresses,
                            }),
                        ],
                        name: "communicationEvent",
                    }),
                ];
                const fetchMeeting = [
                    new framework_1.Fetch({
                        id: roleId,
                        include: [
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.Priority }),
                                ],
                                roleType: m.CommunicationEvent.WorkEfforts,
                            }),
                        ],
                        name: "communicationEvent",
                    }),
                ];
                const fetchPhone = [
                    new framework_1.Fetch({
                        id: roleId,
                        include: [
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.Priority }),
                                ],
                                roleType: m.CommunicationEvent.WorkEfforts,
                            }),
                        ],
                        name: "communicationEvent",
                    }),
                ];
                if (this.isEmail) {
                    return this.scope.load("Pull", new framework_1.PullRequest({ fetch: fetchEmail }));
                }
                if (this.isMeeting) {
                    return this.scope.load("Pull", new framework_1.PullRequest({ fetch: fetchMeeting }));
                }
                if (this.isLetter) {
                    return this.scope.load("Pull", new framework_1.PullRequest({ fetch: fetchLetter }));
                }
                if (this.isPhone) {
                    return this.scope.load("Pull", new framework_1.PullRequest({ fetch: fetchPhone }));
                }
            });
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.communicationEvent = loaded.objects.communicationEvent;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    deleteWorkEffort(worktask) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this work task?" })
            .afterClosed()
            .subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(worktask.Delete)
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
};
CommunicationEventOverviewComponent = __decorate([
    core_1.Component({
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
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        core_2.TdDialogService,
        material_1.MatSnackBar,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], CommunicationEventOverviewComponent);
exports.CommunicationEventOverviewComponent = CommunicationEventOverviewComponent;
