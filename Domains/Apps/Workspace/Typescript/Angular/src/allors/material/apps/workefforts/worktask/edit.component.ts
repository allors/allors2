import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../domain';
import { WorkTask, WorkEffortObjectState, Priority, WorkEffortPurpose } from '../../../../domain';
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
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {

        this.subTitle = 'edit work task';
        this.workTask = loaded.objects.worktask as WorkTask;

        if (!this.workTask) {
          this.subTitle = 'add a new work task';
          this.workTask = this.scope.session.create('WorkTask') as WorkTask;
        }

        this.workEffortObjectStates = loaded.collections.workEffortObjectStates as WorkEffortObjectState[];
        this.priorities = loaded.collections.priorities as Priority[];
        this.workEffortPurposes = loaded.collections.workEffortPurposes as WorkEffortPurpose[];
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
