import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { Subscription } from "rxjs/Subscription";

import { MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask, WorkEffortPurpose } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals } from "@allors/framework";

@Component({
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
      <a-mat-select [object]="workTask" [roleType]="m.WorkTask.WorkEffortPurposes" [options]="workEffortPurposes" display="Name" label="Purpose(s)"></a-mat-select>
      <a-mat-input [object]="workTask" [roleType]="m.WorkTask.EstimatedHours"></a-mat-input>
      <a-mat-input [object]="workTask" [roleType]="m.WorkTask.ActualHours"></a-mat-input>
      <a-mat-datepicker [object]="workTask" [roleType]="m.WorkTask.ScheduledStart"></a-mat-datepicker>
      <a-mat-datepicker [object]="workTask" [roleType]="m.WorkTask.ScheduledCompletion"></a-mat-datepicker>
      <a-mat-datepicker [object]="workTask" [roleType]="m.WorkTask.ActualStart"></a-mat-datepicker>
      <a-mat-datepicker [object]="workTask" [roleType]="m.WorkTask.ActualCompletion"></a-mat-datepicker>
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
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope()
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");

        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

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

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }))
          .switchMap((loaded: Loaded) => {

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
      if (this.existingAssignees.indexOf(assignee) < 0) {
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
