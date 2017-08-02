import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Predicate, And, Or, Not, Equals, Like, TreeNode, Sort, Page } from '../../../../domain';
import { CommunicationEvent, Person, Priority, Singleton, WorkEffortAssignment, WorkTask, WorkEffortObjectState, WorkEffortPurpose } from '../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved } from '../../../../angular';

@Component({
  templateUrl: './form.component.html',
})
export class WorkTaskEditComponent implements OnInit, AfterViewInit, OnDestroy {

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
  existingAssignees: Person[] = [];

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');

        const m: MetaDomain = this.allors.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'worktask',
            id: id,
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

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }))
          .switchMap((loaded: Loaded) => {
            this.scope.session.reset();

            this.subTitle = 'edit work task';
            this.workTask = loaded.objects.worktask as WorkTask;

            const addMode: boolean = !this.workTask;

            if (addMode) {
              this.subTitle = 'add a new work task';
              this.workTask = this.scope.session.create('WorkTask') as WorkTask;
            }

            this.workEffortObjectStates = loaded.collections.workEffortObjectStates as WorkEffortObjectState[];
            this.priorities = loaded.collections.priorities as Priority[];
            this.workEffortPurposes = loaded.collections.workEffortPurposes as WorkEffortPurpose[];
            this.singleton = loaded.collections.singletons[0] as Singleton;
            this.employees = this.singleton.DefaultInternalOrganisation.Employees;

            const assignmentsFetch: Fetch[] = [
              new Fetch(
                {
                  name: 'workEffortAssignments',
                  id: id,
                  path: new Path({ step: m.WorkEffort.WorkEffortAssignmentsWhereAssignment }),
                }),
            ];

            if (addMode) {
              return this.scope.load('Pull', new PullRequest({}));
            } else {
              return this.scope.load('Pull', new PullRequest({ fetch: assignmentsFetch }));
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
      if (!this.existingAssignees.includes(assignee)) {
        let workEffortAssignment: WorkEffortAssignment = this.scope.session.create('WorkEffortAssignment') as WorkEffortAssignment;
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

  goBack(): void {
    window.history.back();
  }
}
