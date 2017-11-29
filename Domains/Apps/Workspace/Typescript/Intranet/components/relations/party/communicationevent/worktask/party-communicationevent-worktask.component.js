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
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let PartyCommunicationEventWorkTaskComponent = class PartyCommunicationEventWorkTaskComponent {
    constructor(workspaceService, errorService, router, route, snackBar, dialogService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.router = router;
        this.route = route;
        this.snackBar = snackBar;
        this.dialogService = dialogService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Work Task";
        this.assignees = [];
        this.scope = this.workspaceService.createScope();
        this.refresh$ = new BehaviorSubject_1.BehaviorSubject(undefined);
        this.m = this.workspaceService.metaPopulation.metaDomain;
    }
    ngOnInit() {
        const route$ = this.route.url;
        const combined$ = Observable_1.Observable.combineLatest(route$, this.refresh$);
        this.subscription = combined$
            .switchMap(([urlSegments, date]) => {
            const id = this.route.snapshot.paramMap.get("id");
            const roleId = this.route.snapshot.paramMap.get("roleId");
            const m = this.workspaceService.metaPopulation.metaDomain;
            const fetch = [
                new framework_1.Fetch({
                    id,
                    include: [new framework_1.TreeNode({ roleType: this.m.CommunicationEvent.CommunicationEventState })],
                    name: "communicationEvent",
                }),
                new framework_1.Fetch({
                    id: roleId,
                    name: "worktask",
                }),
            ];
            const query = [
                new framework_1.Query({
                    name: "workEffortStates",
                    objectType: this.m.WorkEffortState,
                }),
                new framework_1.Query({
                    name: "priorities",
                    objectType: this.m.Priority,
                }),
                new framework_1.Query({
                    name: "workEffortPurposes",
                    objectType: this.m.WorkEffortPurpose,
                }),
                new framework_1.Query({
                    name: "workEffortAssingments",
                    objectType: this.m.WorkEffortAssignment,
                }),
                new framework_1.Query({
                    include: [
                        new framework_1.TreeNode({
                            nodes: [
                                new framework_1.TreeNode({ roleType: m.InternalOrganisation.ActiveEmployees }),
                            ],
                            roleType: m.Singleton.InternalOrganisation,
                        }),
                    ],
                    name: "singletons",
                    objectType: this.m.Singleton,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch, query }));
        })
            .subscribe((loaded) => {
            this.subTitle = "edit work task";
            this.workTask = loaded.objects.worktask;
            const communicationEvent = loaded.objects.communicationEvent;
            if (!this.workTask) {
                this.subTitle = "add a new work task";
                this.workTask = this.scope.session.create("WorkTask");
                communicationEvent.AddWorkEffort(this.workTask);
            }
            this.workEffortStates = loaded.collections.workEffortStates;
            this.priorities = loaded.collections.priorities;
            this.workEffortPurposes = loaded.collections.workEffortPurposes;
            this.singleton = loaded.collections.singletons[0];
            this.employees = this.singleton.InternalOrganisation.ActiveEmployees;
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
    save() {
        this.assignees.forEach((assignee) => {
            const workEffortAssignment = this.scope.session.create("WorkEffortAssignment");
            workEffortAssignment.Assignment = this.workTask;
            workEffortAssignment.Professional = assignee;
        });
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
PartyCommunicationEventWorkTaskComponent = __decorate([
    core_1.Component({
        template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="workTask" (submit)="save()" class="pad">
    <div>
      <a-mat-select [object]="workTask" [roleType]="m.WorkTask.WorkEffortState" [options]="workEffortStates" display="Name"
        label="Status"></a-mat-select>
      <a-mat-input [object]="workTask" [roleType]="m.WorkTask.Name"></a-mat-input>
      <a-mat-textarea [object]="workTask" [roleType]="m.WorkTask.Description"></a-mat-textarea>
      <div class="mat-input-wrapper">
        <div class="mat-input-flex">
          <div class="mat-input-infix">
            <mat-select fxFlex name="assignment" [(ngModel)]="assignees" placeholder="Assign To" multiple="true">
              <mat-option *ngFor="let employee of employees" [value]="employee">{{ employee.displayName }}</mat-option>
            </mat-select>
          </div>
        </div>
      </div>
      <a-mat-select [object]="workTask" [roleType]="m.WorkTask.Priority" [options]="priorities" display="Name"></a-mat-select>
      <a-mat-select [object]="workTask" [roleType]="m.WorkTask.WorkEffortPurposes" [options]="workEffortPurposes" display="Name"
        label="Purpose(s)"></a-mat-select>
      <a-mat-input [object]="workTask" [roleType]="m.WorkTask.EstimatedHours"></a-mat-input>
      <a-mat-input [object]="workTask" [roleType]="m.WorkTask.ActualHours"></a-mat-input>
      <a-mat-slide-toggle [object]="communicationEvent" [roleType]="m.CommunicationEvent.SendNotification"></a-mat-slide-toggle>
      <a-mat-slide-toggle [object]="communicationEvent" [roleType]="m.CommunicationEvent.SendReminder"></a-mat-slide-toggle>
      <div fxLayout="column" fxLayout.gt-sm="row" fxLayoutGap.gt-sm="2rem" class="pad-bottom">
        <a-mat-datepicker [object]="workTask" [roleType]="m.WorkTask.ScheduledStart" [useTime]="true"></a-mat-datepicker>
        <a-mat-datepicker [object]="workTask" [roleType]="m.WorkTask.ScheduledCompletion" [useTime]="true"></a-mat-datepicker>
      </div>
      <div fxLayout="column" fxLayout.gt-sm="row" fxLayoutGap.gt-sm="2rem" class="pad-bottom">
        <a-mat-datepicker [object]="workTask" [roleType]="m.WorkTask.ActualStart" [useTime]="true"></a-mat-datepicker>
        <a-mat-datepicker [object]="workTask" [roleType]="m.WorkTask.ActualCompletion" [useTime]="true"></a-mat-datepicker>
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
        router_1.Router,
        router_1.ActivatedRoute,
        material_1.MatSnackBar,
        core_2.TdDialogService,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], PartyCommunicationEventWorkTaskComponent);
exports.PartyCommunicationEventWorkTaskComponent = PartyCommunicationEventWorkTaskComponent;
