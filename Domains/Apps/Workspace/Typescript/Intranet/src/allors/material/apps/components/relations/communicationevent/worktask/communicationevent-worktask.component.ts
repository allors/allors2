import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription } from 'rxjs';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../../angular';
import { CommunicationEvent, InternalOrganisation, Person, Priority, Singleton, WorkEffortAssignment, WorkEffortPurpose, WorkEffortState, WorkTask } from '../../../../../../domain';
import { Fetch, PullRequest, TreeNode, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/StateService';
import { Title } from '../../../../../../../../node_modules/@angular/platform-browser';
import { combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './communicationevent-worktask.component.html',
})
export class CommunicationEventWorkTaskComponent implements OnInit, OnDestroy {

  public title = 'Work Task';
  public subTitle: string;

  public m: MetaDomain;

  public workTask: WorkTask;

  public workEffortStates: WorkEffortState[];
  public priorities: Priority[];
  public workEffortPurposes: WorkEffortPurpose[];
  public employees: Person[];
  public workEffortAssignments: WorkEffortAssignment[];
  public assignees: Person[] = [];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {

    const { m, pull } = this.dataService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const roleId: string = this.route.snapshot.paramMap.get('roleId');

          const pulls = [
            pull.CommunicationEvent({
              object: id,
              include: { CommunicationEventState: x }
            }),
            pull.WorkTask({
              object: roleId,
            }),
            pull.InternalOrganisation({
              object: id,
              include: { ActiveEmployees: x }
            }),
            pull.WorkEffortState({
              sort: new Sort(m.WorkEffortState.Name)
            }),
            pull.Priority({
              predicate: new Equals({ propertyType: m.Priority.IsActive, value: true }),
              sort: new Sort(m.Priority.Name),
            }),
            pull.WorkEffortPurpose({
              predicate: new Equals({ propertyType: m.WorkEffortPurpose.IsActive, value: true }),
              sort: new Sort( m.WorkEffortPurpose.Name),
            }),
            pull.WorkEffortAssignment()
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.subTitle = 'edit work task';
        this.workTask = loaded.objects.worktask as WorkTask;
        const communicationEvent: CommunicationEvent = loaded.objects.communicationEvent as CommunicationEvent;

        if (!this.workTask) {
          this.subTitle = 'add a new work task';
          this.workTask = this.scope.session.create('WorkTask') as WorkTask;
          communicationEvent.AddWorkEffort(this.workTask);
        }

        this.workEffortStates = loaded.collections.workEffortStates as WorkEffortState[];
        this.priorities = loaded.collections.priorities as Priority[];
        this.workEffortPurposes = loaded.collections.workEffortPurposes as WorkEffortPurpose[];
        const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
        this.employees = internalOrganisation.ActiveEmployees;
      },
        (error: any) => {
          this.errorService.handle(error);
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
    this.assignees.forEach((assignee: Person) => {
      const workEffortAssignment: WorkEffortAssignment = this.scope.session.create('WorkEffortAssignment') as WorkEffortAssignment;
      workEffortAssignment.Assignment = this.workTask;
      workEffortAssignment.Professional = assignee;
    });

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
