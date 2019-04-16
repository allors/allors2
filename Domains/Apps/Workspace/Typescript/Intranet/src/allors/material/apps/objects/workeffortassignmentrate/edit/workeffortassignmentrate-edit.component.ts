import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { Subscription, combineLatest } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { WorkEffortAssignmentRate, TimeFrequency, RateType, WorkEffort, WorkEffortPartyAssignment } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { CreateData } from '../../../../../material/base/services/object';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { SaveService } from 'src/allors/material/base/services/save';

@Component({
  templateUrl: './workeffortassignmentrate-edit.component.html',
  providers: [ContextService]
})
export class WorkEffortAssignmentRateEditComponent implements OnInit, OnDestroy {

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
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<WorkEffortAssignmentRateEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    ) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            pull.WorkEffortAssignmentRate({
              object: this.data.id,
              include: {
                RateType: x,
                Frequency: x,
                WorkEffortPartyAssignment: x,
                WorkEffort: x
              }
            }),
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
            pull.RateType({ sort: new Sort(this.m.RateType.Name) }),
            pull.TimeFrequency({ sort: new Sort(this.m.TimeFrequency.Name) }),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
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
        const hour = this.timeFrequencies.find((v) => v.UniqueId.toUpperCase() === 'DB14E5D5-5EAF-4EC8-B149-C558A28D99F5');

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
      .subscribe((saved: Saved) => {
        const data: IObject = {
          id: this.workEffortAssignmentRate.id,
          objectType: this.workEffortAssignmentRate.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
