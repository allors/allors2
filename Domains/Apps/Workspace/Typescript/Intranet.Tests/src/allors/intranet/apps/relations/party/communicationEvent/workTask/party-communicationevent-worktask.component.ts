import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Loaded, Saved, Scope } from "@allors";
import { Fetch, PullRequest, Query, TreeNode } from "@allors";
import { CommunicationEvent, Person, Priority, Singleton, WorkEffortAssignment, WorkEffortPurpose, WorkEffortState, WorkTask } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  templateUrl: "./party-communicationevent-worktask.component.html",
})
export class PartyCommunicationEventWorkTaskComponent implements OnInit, AfterViewInit, OnDestroy {

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

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.m = this.allors.meta;
  }

  public ngOnInit(): void {

    const route$: Observable<UrlSegment[]> = this.route.url;
    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const roleId: string = this.route.snapshot.paramMap.get("roleId");

        const m: MetaDomain = this.allors.meta;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [new TreeNode({ roleType: this.m.CommunicationEvent.CommunicationEventState })],
            name: "communicationEvent",
          }),
          new Fetch({
            id: roleId,
            name: "worktask",
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
              name: "workEffortAssingments",
              objectType: this.m.WorkEffortAssignment,
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
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {
        this.subTitle = "edit work task";
        this.workTask = loaded.objects.worktask as WorkTask;
        const communicationEvent: CommunicationEvent = loaded.objects.communicationEvent as CommunicationEvent;

        if (!this.workTask) {
          this.subTitle = "add a new work task";
          this.workTask = this.scope.session.create("WorkTask") as WorkTask;
          communicationEvent.AddWorkEffort(this.workTask);
        }

        this.workEffortStates = loaded.collections.workEffortStates as WorkEffortState[];
        this.priorities = loaded.collections.priorities as Priority[];
        this.workEffortPurposes = loaded.collections.workEffortPurposes as WorkEffortPurpose[];
        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.employees = this.singleton.InternalOrganisation.ActiveEmployees;
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
      const workEffortAssignment: WorkEffortAssignment = this.scope.session.create("WorkEffortAssignment") as WorkEffortAssignment;
      workEffortAssignment.Assignment = this.workTask;
      workEffortAssignment.Professional = assignee;
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public  goBack(): void {
    window.history.back();
  }
}
