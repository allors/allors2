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
const forms_1 = require("@angular/forms");
const material_1 = require("@angular/material");
const platform_browser_1 = require("@angular/platform-browser");
const router_1 = require("@angular/router");
const Rx_1 = require("rxjs/Rx");
const core_2 = require("@covalent/core");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let WorkTasksOverviewComponent = class WorkTasksOverviewComponent {
    constructor(workspaceService, errorService, formBuilder, titleService, snackBar, router, dialogService, snackBarService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.formBuilder = formBuilder;
        this.titleService = titleService;
        this.snackBar = snackBar;
        this.router = router;
        this.dialogService = dialogService;
        this.snackBarService = snackBarService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Work Tasks";
        titleService.setTitle(this.title);
        this.scope = this.workspaceService.createScope();
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
        this.searchForm = this.formBuilder.group({
            assignee: [""],
            description: [""],
            name: [""],
            priority: [""],
            state: [""],
        });
        this.page$ = new Rx_1.BehaviorSubject(50);
        const search$ = this.searchForm.valueChanges
            .debounceTime(400)
            .distinctUntilChanged()
            .startWith({});
        const combined$ = Rx_1.Observable
            .combineLatest(search$, this.page$, this.refresh$)
            .scan(([previousData, previousTake, previousDate], [data, take, date]) => {
            return [
                data,
                data !== previousData ? 50 : take,
                date,
            ];
        }, []);
        this.subscription = combined$
            .switchMap(([data, take]) => {
            const m = this.workspaceService.metaPopulation.metaDomain;
            const objectStatesQuery = [
                new framework_1.Query({
                    name: "workEffortStates",
                    objectType: m.WorkEffortState,
                }),
                new framework_1.Query({
                    name: "priorities",
                    objectType: m.Priority,
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
                    objectType: m.Singleton,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ query: objectStatesQuery }))
                .switchMap((loaded) => {
                this.workEffortStates = loaded.collections.workEffortStates;
                this.workEffortState = this.workEffortStates.find((v) => v.Name === data.state);
                this.priorities = loaded.collections.priorities;
                this.priority = this.priorities.find((v) => v.Name === data.priority);
                const singleton = loaded.collections.singletons[0];
                this.assignees = singleton.InternalOrganisation.ActiveEmployees;
                this.assignee = this.assignees.find((v) => v.displayName === data.assignee);
                const predicate = new framework_1.And();
                const predicates = predicate.predicates;
                if (data.name) {
                    const like = "%" + data.name + "%";
                    predicates.push(new framework_1.Like({ roleType: m.WorkTask.Name, value: like }));
                }
                if (data.description) {
                    const like = "%" + data.description + "%";
                    predicates.push(new framework_1.Like({ roleType: m.WorkTask.Description, value: like }));
                }
                if (data.state) {
                    predicates.push(new framework_1.Equals({ roleType: m.WorkTask.WorkEffortState, value: this.workEffortState }));
                }
                if (data.priority) {
                    predicates.push(new framework_1.Equals({ roleType: m.WorkTask.Priority, value: this.priority }));
                }
                const workTasksquery = new framework_1.Query({
                    name: "worktasks",
                    objectType: m.WorkTask,
                    predicate,
                });
                const assignmentPredicate = new framework_1.And();
                const assignmentPredicates = assignmentPredicate.predicates;
                assignmentPredicates.push(new framework_1.ContainedIn({ roleType: m.WorkEffortAssignment.Assignment, query: workTasksquery }));
                if (data.assignee) {
                    assignmentPredicates.push(new framework_1.Equals({ roleType: m.WorkEffortAssignment.Professional, value: this.assignee }));
                }
                const assignmentsQuery = [
                    new framework_1.Query({
                        include: [
                            new framework_1.TreeNode({ roleType: m.WorkEffortAssignment.Professional }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.Priority }),
                                ],
                                roleType: m.WorkEffortAssignment.Assignment,
                            }),
                        ],
                        name: "workEffortAssignments",
                        objectType: m.WorkEffortAssignment,
                        page: new framework_1.Page({ skip: 0, take }),
                        predicate: assignmentPredicate,
                    }),
                ];
                return this.scope
                    .load("Pull", new framework_1.PullRequest({ query: assignmentsQuery }));
            });
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.data = loaded.collections.workEffortAssignments;
            this.total = loaded.values.workEffortAssignments_total;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    more() {
        this.page$.next(this.data.length + 50);
    }
    goBack() {
        window.history.back();
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
    delete(worktask) {
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
    onView(person) {
        this.router.navigate(["/relations/person/" + person.id]);
    }
};
WorkTasksOverviewComponent = __decorate([
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

<mat-card *ngIf="workEffortStates">
  <div class="pad-top-xs pad-left pad-right">
    <form novalidate [formGroup]="searchForm">
      <mat-input-container>
        <input fxFlex matInput placeholder="Name" formControlName="name">
      </mat-input-container>
      <mat-input-container>
        <input fxFlex matInput placeholder="Description" formControlName="description">
      </mat-input-container>
      <mat-select formControlName="state" name="state" [(ngModel)]="selectedWorkEffortState" placeholder="State">
        <mat-option>None</mat-option>
        <mat-option *ngFor="let objectState of workEffortStates" [value]="objectState.Name">{{ objectState.Name }}</mat-option>
      </mat-select>
      <mat-select formControlName="priority" name="priority" [(ngModel)]="selectedPriority" placeholder="Priority">
        <mat-option>None</mat-option>
        <mat-option *ngFor="let priority of priorities" [value]="priority.Name">{{ priority.Name }}</mat-option>
      </mat-select>
      <mat-select formControlName="assignee" name="assignee" [(ngModel)]="selectedAssignee" placeholder="Assignee">
        <mat-option>None</mat-option>
        <mat-option *ngFor="let assignee of assignees" [value]="assignee.displayName">{{ assignee.displayName }}</mat-option>
      </mat-select>
      <mat-icon matSuffix>search</mat-icon>
    </form>
  </div>

  <mat-divider></mat-divider>

  <mat-card-content>
    <ng-template tdLoading="data">
      <mat-list class="will-load">
        <div class="mat-padding" *ngIf="data && data.length === 0" layout="row" layout-align="center center">
          <h3>No work tasks to display.</h3>
        </div>
        <ng-template let-workEffortAssignment let-last="last" ngFor [ngForOf]="data">
          <mat-list-item>
            <span class="mat-list-text pointer" [routerLink]="['/worktask/' + workEffortAssignment.Assignment.id]">
              {{ workEffortAssignment.Assignment.Name }}, {{ workEffortAssignment.Assignment.WorkEffortState.Name }}

              <div mat-line class="mat-caption pointer" [routerLink]="['/relations/person/' + workEffortAssignment.Professional.id]">Assigned to: {{ workEffortAssignment.Professional.displayName }} </div>
              <p mat-line class="mat-caption">{{ workEffortAssignment.Assignment.Description }} </p>
              <p *ngIf="workEffortAssignment.Assignment.Priority" mat-line class="mat-caption">Priority {{ workEffortAssignment.Assignment.Priority.Name }} </p>

              <span hide-xs hide-sm hide-md flex-gt-xs="60" flex-xs="40" layout-gt-xs="row">
                <div class="mat-caption tc-grey-500" flex-gt-xs="50"> Sched. Start: {{ workEffortAssignment.Assignment.ScheduledStart | date}} </div>
                <div class="mat-caption tc-grey-500" flex-gt-xs="50"> Sched. Compl.: {{ workEffortAssignment.Assignment.ScheduledCompletion | date }} </div>
                <div class="mat-caption tc-grey-500" flex-gt-xs="50"> Act. Start: {{ workEffortAssignment.Assignment.ActualStart | date}} </div>
                <div class="mat-caption tc-grey-500" flex-gt-xs="50"> Act. Compl.: {{ workEffortAssignment.Assignment.ActualCompletion | date }} </div>
              </span>
            </span>

            <span>
              <button mat-icon-button [mat-menu-trigger-for]="menu">
                <mat-icon>more_vert</mat-icon>
              </button>
              <mat-menu x-position="before" #menu="matMenu">
                <a [routerLink]="['/worktask/' + workEffortAssignment.Assignment.id]" mat-menu-item>Edit</a>
                <button  mat-menu-item (click)="delete(workEffortAssignment.Assignment)" [disabled]="!workEffortAssignment.Assignment.CanExecuteDelete">Delete</button>
              </mat-menu>
            </span>

          </mat-list-item>
          <mat-divider *ngIf="!last" mat-inset></mat-divider>
        </ng-template>
      </mat-list>
    </ng-template>

  </mat-card-content>
</mat-card>

<mat-card body tdMediaToggle="gt-xs" [mediaClasses]="['push']" *ngIf="this.data && this.data.length !== total">
  <mat-card-content>
    <button mat-button (click)="more()">More</button> {{this.data?.length}}/{{total}}
  </mat-card-content>
</mat-card>

<a mat-fab color="accent" class="mat-fab-bottom-right fixed" [routerLink]="['/worktask']">
  <mat-icon>add</mat-icon>
</a>
`,
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        forms_1.FormBuilder,
        platform_browser_1.Title,
        material_1.MatSnackBar,
        router_1.Router,
        core_2.TdDialogService,
        material_1.MatSnackBar,
        core_2.TdMediaService,
        core_1.ChangeDetectorRef])
], WorkTasksOverviewComponent);
exports.WorkTasksOverviewComponent = WorkTasksOverviewComponent;
