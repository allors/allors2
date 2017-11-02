import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Loaded, Saved, Scope } from "@allors";
import { And, Equals, Fetch, Like, Not, Or, Page, Path, Predicate, PullRequest, PushResponse, Query, Sort, TreeNode } from "@allors";
import { CommunicationEvent, Person, Priority, Singleton, WorkEffortAssignment, WorkEffortPurpose, WorkEffortState, WorkTask } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  templateUrl: "./worktask.component.html",
})
export class WorkTaskEditComponent implements OnInit, AfterViewInit, OnDestroy {

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

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  public ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");

        const m: MetaDomain = this.allors.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: "worktask",
            id,
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "workEffortStates",
              objectType: this.m.WorkEffortState,
            }),
          new Query(
            {
              name: "priorities",
              objectType: this.m.Priority,
            }),
          new Query(
            {
              name: "workEffortPurposes",
              objectType: this.m.WorkEffortPurpose,
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
              objectType: this.m.Singleton,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }))
          .switchMap((loaded: Loaded) => {
            this.scope.session.reset();

            this.subTitle = "edit work task";
            this.workTask = loaded.objects.worktask as WorkTask;

            const addMode: boolean = !this.workTask;

            if (addMode) {
              this.subTitle = "add a new work task";
              this.workTask = this.scope.session.create("WorkTask") as WorkTask;
            }

            this.workEffortStates = loaded.collections.workEffortStates as WorkEffortState[];
            this.priorities = loaded.collections.priorities as Priority[];
            this.workEffortPurposes = loaded.collections.workEffortPurposes as WorkEffortPurpose[];
            this.singleton = loaded.collections.singletons[0] as Singleton;
            this.employees = this.singleton.InternalOrganisation.ActiveEmployees;

            const assignmentsFetch: Fetch[] = [
              new Fetch(
                {
                  name: "workEffortAssignments",
                  id,
                  path: new Path({ step: m.WorkEffort.WorkEffortAssignmentsWhereAssignment }),
                }),
            ];

            if (addMode) {
              return this.scope.load("Pull", new PullRequest({}));
            } else {
              return this.scope.load("Pull", new PullRequest({ fetch: assignmentsFetch }));
            }
          });
      })
      .subscribe((loaded: Loaded) => {
        this.workEffortAssignments = loaded.collections.workEffortAssignments as WorkEffortAssignment[];

        if (this.workEffortAssignments) {
          this.assignees = this.workEffortAssignments.map((v: WorkEffortAssignment) => v.Professional);
        }

        this.existingAssignees = this.assignees;
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

  public save(): void {
    this.assignees.forEach((assignee: Person) => {
      if (!this.existingAssignees.includes(assignee)) {
        const workEffortAssignment: WorkEffortAssignment = this.scope.session.create("WorkEffortAssignment") as WorkEffortAssignment;
        workEffortAssignment.Assignment = this.workTask;
        workEffortAssignment.Professional = assignee;
      }
    });

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public goBack(): void {
    window.history.back();
  }
}
