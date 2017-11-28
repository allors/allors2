import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Subscription } from "rxjs/Rx";

import { TdDialogService, TdMediaService } from "@covalent/core";

import { MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals } from "@allors/framework";

interface SearchData {
  name: string;
  description: string;
  state: string;
  priority: string;
  assignee: string;
}

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
})
export class WorkTasksOverviewComponent implements AfterViewInit, OnDestroy {

  public total: number;

  public title: string = "Work Tasks";

  public searchForm: FormGroup;

  public data: WorkEffortAssignment[];

  public workEffortStates: WorkEffortState[];
  public selectedWorkEffortState: WorkEffortState;
  public workEffortState: WorkEffortState;

  public priorities: Priority[];
  public selectedPriority: Priority;
  public priority: Priority;

  public assignees: Person[];
  public selectedAssignee: Person;
  public assignee: Person;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    private snackBarService: MatSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    titleService.setTitle(this.title);
    this.scope = this.workspaceService.createScope()
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      assignee: [""],
      description: [""],
      name: [""],
      priority: [""],
      state: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable
    .combineLatest(search$, this.page$, this.refresh$)
    .scan(([previousData, previousTake, previousDate]: [SearchData, number, Date], [data, take, date]: [SearchData, number, Date]): [SearchData, number, Date] => {
      return [
        data,
        data !== previousData ? 50 : take,
        date,
      ];
    }, [] as [SearchData, number, Date]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const objectStatesQuery: Query[] = [
          new Query(
            {
              name: "workEffortStates",
              objectType: m.WorkEffortState,
            }),
          new Query(
            {
              name: "priorities",
              objectType: m.Priority,
            }),
          new Query(
            {
              include: [
                new TreeNode({
                  nodes: [
                    new TreeNode({ roleType: m.InternalOrganisation.ActiveEmployees }),
                  ],
                  roleType: m.Singleton.InternalOrganisation,
                }),
              ],
              name: "singletons",
              objectType: m.Singleton,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: objectStatesQuery }))
          .switchMap((loaded: Loaded) => {
            this.workEffortStates = loaded.collections.workEffortStates as WorkEffortState[];
            this.workEffortState = this.workEffortStates.find((v: WorkEffortState) => v.Name === data.state);

            this.priorities = loaded.collections.priorities as Priority[];
            this.priority = this.priorities.find((v: Priority) => v.Name === data.priority);

            const singleton: Singleton = loaded.collections.singletons[0] as Singleton;
            this.assignees = singleton.InternalOrganisation.ActiveEmployees;
            this.assignee = this.assignees.find((v: Person) => v.displayName === data.assignee);

            const predicate: And = new And();
            const predicates: Predicate[] = predicate.predicates;

            if (data.name) {
              const like: string = "%" + data.name + "%";
              predicates.push(new Like({ roleType: m.WorkTask.Name, value: like }));
            }

            if (data.description) {
              const like: string = "%" + data.description + "%";
              predicates.push(new Like({ roleType: m.WorkTask.Description, value: like }));
            }

            if (data.state) {
              predicates.push(new Equals({ roleType: m.WorkTask.WorkEffortState, value: this.workEffortState }));
            }

            if (data.priority) {
              predicates.push(new Equals({ roleType: m.WorkTask.Priority, value: this.priority }));
            }

            const workTasksquery: Query = new Query(
              {
                name: "worktasks",
                objectType: m.WorkTask,
                predicate,
              });

            const assignmentPredicate: And = new And();
            const assignmentPredicates: Predicate[] = assignmentPredicate.predicates;
            assignmentPredicates.push(new ContainedIn({ roleType: m.WorkEffortAssignment.Assignment, query: workTasksquery }));

            if (data.assignee) {
              assignmentPredicates.push(new Equals({ roleType: m.WorkEffortAssignment.Professional, value: this.assignee }));
            }

            const assignmentsQuery: Query[] = [
              new Query(
                {
                  include: [
                    new TreeNode({ roleType: m.WorkEffortAssignment.Professional }),
                    new TreeNode({
                      nodes: [
                        new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                        new TreeNode({ roleType: m.WorkEffort.Priority }),
                      ],
                      roleType: m.WorkEffortAssignment.Assignment,
                    }),
                  ],
                  name: "workEffortAssignments",
                  objectType: m.WorkEffortAssignment,
                  page: new Page({ skip: 0, take }),
                  predicate: assignmentPredicate,
                }),
            ];

            return this.scope
              .load("Pull", new PullRequest({ query: assignmentsQuery }));
          });
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.data = loaded.collections.workEffortAssignments as WorkEffortAssignment[];
        this.total = loaded.values.workEffortAssignments_total;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  public more(): void {
    this.page$.next(this.data.length + 50);
  }

  public goBack(): void {
    window.history.back();
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

  public delete(worktask: WorkTask): void {
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

  public onView(person: Person): void {
    this.router.navigate(["/relations/person/" + person.id]);
  }
}
