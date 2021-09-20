import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Saved } from '@allors/angular/services/core';
import { TimeFrequency, RateType, WorkEffortAssignmentRate, WorkEffort, WorkEffortPartyAssignment } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { IObject } from '@allors/domain/system';
import { Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';


@Component({
  templateUrl: './workeffortassignmentrate-edit.component.html',
  providers: [ContextService]
})
export class WorkEffortAssignmentRateEditComponent extends TestScope implements OnInit, OnDestroy {

  title: string;
  subTitle: string;

  readonly m: Meta;

  workEffortAssignmentRate: WorkEffortAssignmentRate;
  workEffort: WorkEffort;
  workEffortPartyAssignments: WorkEffortPartyAssignment[];
  timeFrequencies: TimeFrequency[];
  rateTypes: RateType[];

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<WorkEffortAssignmentRateEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            pull.RateType({ sort: new Sort(this.m.RateType.Name) }),
            pull.TimeFrequency({ sort: new Sort(this.m.TimeFrequency.Name) }),
          ];

          if (!isCreate) {
            pulls.push(
              pull.WorkEffortAssignmentRate({
                object: this.data.id,
                include: {
                  RateType: x,
                  Frequency: x,
                  WorkEffortPartyAssignment: x,
                  WorkEffort: x
                }
              }),
            );
          }

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.WorkEffort({
                object: this.data.associationId,
                fetch: {
                  WorkEffortPartyAssignmentsWhereAssignment:
                  {
                    include: {
                      Party: x
                    }
                  }
                }
              }),
              pull.WorkEffort({
                object: this.data.associationId,
              }),
            );
          }

          return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.workEffort = loaded.objects.WorkEffort as WorkEffort;
        this.workEffortPartyAssignments = loaded.collections.WorkEffortPartyAssignments as WorkEffortPartyAssignment[];
        this.rateTypes = loaded.collections.RateTypes as RateType[];
        this.timeFrequencies = loaded.collections.TimeFrequencies as TimeFrequency[];
        const hour = this.timeFrequencies.find((v) => v.UniqueId === 'db14e5d5-5eaf-4ec8-b149-c558a28d99f5');

        if (isCreate) {
          this.title = 'Add Rate';
          this.workEffortAssignmentRate = this.allors.context.create('WorkEffortAssignmentRate') as WorkEffortAssignmentRate;
          this.workEffortAssignmentRate.WorkEffort = this.workEffort;
          this.workEffortAssignmentRate.Frequency = hour;
        } else {
          this.workEffortAssignmentRate = loaded.objects.WorkEffortAssignmentRate as WorkEffortAssignmentRate;

          if (this.workEffortAssignmentRate.CanWriteRate) {
            this.title = 'Edit Rate';
          } else {
            this.title = 'View Rate';
          }
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public setDirty(): void {
    this.allors.context.session.hasChanges = true;
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe(() => {
        const data: IObject = {
          id: this.workEffortAssignmentRate.id,
          objectType: this.workEffortAssignmentRate.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
