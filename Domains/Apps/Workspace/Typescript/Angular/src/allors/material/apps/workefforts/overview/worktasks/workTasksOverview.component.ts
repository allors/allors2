import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { TdDialogService, TdLoadingService, TdMediaService } from "@covalent/core";

import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "../../../../../angular";
import { And, ContainedIn, Contains, Equals, Like, Not, Or, Page, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../../domain";
import { Person, Priority, Singleton, WorkEffortAssignment, WorkEffortObjectState, WorkTask } from "../../../../../domain";
import { MetaDomain } from "../../../../../meta/index";

interface SearchData {
  name: string;
  description: string;
  state: string;
  priority: string;
  assignee: string;
}

@Component({
  templateUrl: "./workTasksOverview.component.html",
})
export class WorkTasksOverviewComponent implements AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;
  total: number;

  title: string = "Work Tasks";

  searchForm: FormGroup;

  data: WorkEffortAssignment[];

  workEffortObjectStates: WorkEffortObjectState[];
  selectedWorkEffortObjectState: WorkEffortObjectState;
  workEffortObjectState: WorkEffortObjectState;

  priorities: Priority[];
  selectedPriority: Priority;
  priority: Priority;

  assignees: Person[];
  selectedAssignee: Person;
  assignee: Person;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MdSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    private snackBarService: MdSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    titleService.setTitle(this.title);
    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [""],
      description: [""],
      state: [""],
      priority: [""],
      assignee: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable.combineLatest(search$, this.page$)
      .scan(([previousData, previousTake]: [SearchData, number], [data, take]: [SearchData, number]): [SearchData, number] => {
        return [
          data,
          data !== previousData ? 50 : take,
        ];
      }, [] as [SearchData, number]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.allors.meta;

        const objectStatesQuery: Query[] = [
          new Query(
            {
              name: "workEffortObjectStates",
              objectType: m.WorkEffortObjectState,
            }),
          new Query(
            {
              name: "priorities",
              objectType: m.Priority,
            }),
          new Query(
            {
              name: "singletons",
              objectType: m.Singleton,
              include: [
                new TreeNode({
                  roleType: m.Singleton.DefaultInternalOrganisation,
                  nodes: [
                    new TreeNode({ roleType: m.InternalOrganisation.Employees }),
                  ],
                }),
              ],
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: objectStatesQuery }))
          .switchMap((loaded: Loaded) => {
            this.workEffortObjectStates = loaded.collections.workEffortObjectStates as WorkEffortObjectState[];
            this.workEffortObjectState = this.workEffortObjectStates.find((v: WorkEffortObjectState) => v.Name === data.state);

            this.priorities = loaded.collections.priorities as Priority[];
            this.priority = this.priorities.find((v: Priority) => v.Name === data.priority);

            const singleton: Singleton = loaded.collections.singletons[0] as Singleton;
            this.assignees = singleton.DefaultInternalOrganisation.Employees;
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
              predicates.push(new Equals({ roleType: m.WorkTask.CurrentObjectState, value: this.workEffortObjectState }));
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
                  name: "workEffortAssignments",
                  objectType: m.WorkEffortAssignment,
                  predicate: assignmentPredicate,
                  page: new Page({ skip: 0, take }),
                  include: [
                    new TreeNode({ roleType: m.WorkEffortAssignment.Professional }),
                    new TreeNode({
                      roleType: m.WorkEffortAssignment.Assignment,
                      nodes: [
                        new TreeNode({ roleType: m.WorkEffort.CurrentObjectState }),
                        new TreeNode({ roleType: m.WorkEffort.Priority }),
                      ],
                    }),
                  ],
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

  more(): void {
    this.page$.next(this.data.length + 50);
  }

  goBack(): void {
    window.history.back();
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  refresh(): void {
    this.refresh$.next(new Date());
  }

  delete(worktask: WorkTask): void {
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

  onView(person: Person): void {
    this.router.navigate(["/relations/person/" + person.id ]);
  }
}
