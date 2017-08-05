import { Observable, BehaviorSubject, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService, TdDialogService } from '@covalent/core';

import { MetaDomain } from '../../../../../../meta/index';
import { PullRequest, PushResponse, Predicate, And, Or, Not, Equals, Like, Contains, Fetch, Path, Query, TreeNode, Sort, Page } from '../../../../../../domain';
import { CommunicationEvent, Person, Priority, Singleton, WorkEffortAssignment, WorkTask, WorkEffortObjectState, WorkEffortPurpose } from '../../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked } from '../../../../../../angular';

@Component({
  templateUrl: './form.component.html',
})
export class PartyCommunicationEventAddWorkTaskComponent implements OnInit, AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  title: string = 'Work Task';
  subTitle: string;

  m: MetaDomain;

  workTask: WorkTask;

  workEffortObjectStates: WorkEffortObjectState[];
  priorities: Priority[];
  workEffortPurposes: WorkEffortPurpose[];
  singleton: Singleton;
  employees: Person[];
  workEffortAssignments: WorkEffortAssignment[];
  assignees: Person[] = [];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MdSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.m = this.allors.meta;
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const roleId: string = this.route.snapshot.paramMap.get('roleId');

        const m: MetaDomain = this.allors.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'communicationEvent',
            id: id,
            include: [new TreeNode({ roleType: this.m.CommunicationEvent.CurrentObjectState })],
          }),
          new Fetch({
            name: 'worktask',
            id: roleId,
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'workEffortObjectStates',
              objectType: this.m.WorkEffortObjectState,
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
          new Query(
            {
              name: 'singletons',
              objectType: this.m.Singleton,
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
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {
        this.scope.session.reset();

        this.subTitle = 'edit work task';
        this.workTask = loaded.objects.worktask as WorkTask;
        let communicationEvent: CommunicationEvent = loaded.objects.communicationEvent as CommunicationEvent;

        if (!this.workTask) {
          this.subTitle = 'add a new work task';
          this.workTask = this.scope.session.create('WorkTask') as WorkTask;
          communicationEvent.AddWorkEffort(this.workTask);
        }

        this.workEffortObjectStates = loaded.collections.workEffortObjectStates as WorkEffortObjectState[];
        this.priorities = loaded.collections.priorities as Priority[];
        this.workEffortPurposes = loaded.collections.workEffortPurposes as WorkEffortPurpose[];
        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.employees = this.singleton.DefaultInternalOrganisation.Employees;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
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

  save(): void {
    this.assignees.forEach((assignee: Person) => {
      let workEffortAssignment: WorkEffortAssignment = this.scope.session.create('WorkEffortAssignment') as WorkEffortAssignment;
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

  refresh(): void {
    this.refresh$.next(new Date());
  }

  goBack(): void {
    window.history.back();
  }
}
