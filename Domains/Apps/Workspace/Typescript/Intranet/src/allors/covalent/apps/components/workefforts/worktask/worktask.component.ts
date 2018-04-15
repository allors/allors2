import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit
} from "@angular/core";
import { ActivatedRoute, UrlSegment } from "@angular/router";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import {
  ErrorService,
  Filter,
  Loaded,
  Saved,
  Scope,
  WorkspaceService
} from "../../../../../angular";
import {
  InternalOrganisation,
  Party,
  Person,
  Priority,
  Singleton,
  WorkEffortAssignment,
  WorkEffortPurpose,
  WorkEffortState,
  WorkTask,
  WorkTaskSubject
} from "../../../../../domain";
import {
  Fetch,
  Path,
  PullRequest,
  Query,
  TreeNode
} from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";
import { Fetcher } from "../../Fetcher";

@Component({
  templateUrl: "./worktask.component.html"
})
export class WorkTaskEditComponent implements OnInit, OnDestroy {
  public title: string = "Work Task";
  public subTitle: string;

  public m: MetaDomain;

  public workTask: WorkTask;

  public workEffortStates: WorkEffortState[];
  public priorities: Priority[];
  public workEffortPurposes: WorkEffortPurpose[];
  public singleton: Singleton;
  public employees: Person[];
  public workEffortAssignments: WorkEffortAssignment[];
  public assignees: Person[] = [];
  public existingAssignees: Person[] = [];
  public organisationsFilter: Filter;
  public workTaskSubjects: WorkTaskSubject[];

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private stateService: StateService
  ) {
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.fetcher = new Fetcher(this.stateService, this.m);
    this.organisationsFilter = new Filter({
      scope: this.scope,
      objectType: this.m.Organisation,
      roleTypes: [this.m.Organisation.Name]
    });
  }

  public ngOnInit(): void {
    this.subscription = Observable.combineLatest(
      this.route.url,
      this.refresh$,
      this.stateService.internalOrganisationId$
    )
      .switchMap(([urlSegments, date, internalOrganisationId]) => {
        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          this.fetcher.internalOrganisation,
          new Fetch({
            id,
            include: [new TreeNode({ roleType: m.WorkTask.TaskSubject })],
            name: "worktask"
          })
        ];

        const query: Query[] = [
          new Query({
            name: "workEffortStates",
            objectType: this.m.WorkEffortState
          }),
          new Query({
            name: "priorities",
            objectType: this.m.Priority
          }),
          new Query({
            name: "workEffortPurposes",
            objectType: this.m.WorkEffortPurpose
          })
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }))
          .switchMap(loaded => {
            this.subTitle = "edit work task";
            this.workTask = loaded.objects.worktask as WorkTask;

            const addMode: boolean = !this.workTask;

            if (addMode) {
              this.subTitle = "add a new work task";
              this.workTask = this.scope.session.create("WorkTask") as WorkTask;
            }

            this.workEffortStates = loaded.collections
              .workEffortStates as WorkEffortState[];
            this.priorities = loaded.collections.priorities as Priority[];
            this.workEffortPurposes = loaded.collections
              .workEffortPurposes as WorkEffortPurpose[];
            const internalOrganisation: InternalOrganisation = loaded.objects
              .internalOrganisation as InternalOrganisation;
            this.employees = internalOrganisation.ActiveEmployees;

            const assignmentsFetch: Fetch[] = [
              new Fetch({
                id,
                name: "workEffortAssignments",
                path: new Path({
                  step: m.WorkEffort.WorkEffortAssignmentsWhereAssignment
                })
              })
            ];

            if (addMode) {
              return this.scope.load("Pull", new PullRequest({}));
            } else {
              return this.scope.load(
                "Pull",
                new PullRequest({ fetch: assignmentsFetch })
              );
            }
          });
      })
      .subscribe(
        loaded => {
          this.workEffortAssignments = loaded.collections
            .workEffortAssignments as WorkEffortAssignment[];

          if (this.workEffortAssignments) {
            this.assignees = this.workEffortAssignments.map(
              (v: WorkEffortAssignment) => v.Professional
            );
          }

          this.existingAssignees = this.assignees;
        },
        (error: any) => {
          this.errorService.message(error);
          this.goBack();
        }
      );
  }

  public customerSelected(customer: Party) {
    this.updateTaskSubject(customer);
  }

  private updateTaskSubject(customer: Party): void {

    const fetch: Fetch[] = [
      new Fetch({
        id: customer.id,
        name: "workTaskSubjects",
        path: new Path({ step: this.m.Party.WorkTaskSubjectsWhereOwner }),
      }),
    ];

    this.scope
      .load("Pull", new PullRequest({ fetch }))
      .subscribe((loaded) => {
        this.workTaskSubjects = loaded.collections.workTaskSubjects as WorkTaskSubject[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    if (this.assignees) {
      this.assignees.forEach((assignee: Person) => {
        if (this.existingAssignees.indexOf(assignee) < 0) {
          const workEffortAssignment: WorkEffortAssignment = this.scope.session.create(
            "WorkEffortAssignment"
          ) as WorkEffortAssignment;
          workEffortAssignment.Assignment = this.workTask;
          workEffortAssignment.Professional = assignee;
        }
      });
    }
    this.scope.save().subscribe(
      (saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      }
    );
  }

  public goBack(): void {
    window.history.back();
  }
}
