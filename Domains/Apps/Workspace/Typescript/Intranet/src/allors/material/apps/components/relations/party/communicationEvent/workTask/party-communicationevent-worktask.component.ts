import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';


import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from '../../../../../../../angular';
import { CommunicationEvent, InternalOrganisation, Person, Priority, Singleton, WorkEffortAssignment, WorkEffortPurpose, WorkEffortState, WorkTask } from '../../../../../../../domain';
import { Fetch, PullRequest, Query, TreeNode, Sort } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';
import { StateService } from '../../../../../services/StateService';

@Component({
  templateUrl: './party-communicationevent-worktask.component.html',
})
export class PartyCommunicationEventWorkTaskComponent implements OnInit, OnDestroy {

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
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const roleId: string = this.route.snapshot.paramMap.get('roleId');

        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            include: [new TreeNode({ roleType: this.m.CommunicationEvent.CommunicationEventState })],
            name: 'communicationEvent',
          }),
          new Fetch({
            id: roleId,
            name: 'worktask',
          }),
          new Fetch({
            id: internalOrganisationId,
            include: [
              new TreeNode({
                roleType: m.InternalOrganisation.ActiveEmployees }),
            ],
            name: 'internalOrganisation',
          }),
        ];

        const queries: Query[] = [
          new Query(
            {
              name: 'workEffortStates',
              objectType: this.m.WorkEffortState,
            }),
          new Query(
            {
              name: 'priorities',
              objectType: this.m.Priority,
            }),
          new Query(
            {
              name: 'workEffortPurposes',
              objectType: this.m.WorkEffortPurpose,
            }),
          new Query(
            {
              name: 'workEffortAssingments',
              objectType: this.m.WorkEffortAssignment,
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
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
        const internalOrganisation = loaded.objects.internalOrganisationId as InternalOrganisation;
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

  public  goBack(): void {
    window.history.back();
  }
}
