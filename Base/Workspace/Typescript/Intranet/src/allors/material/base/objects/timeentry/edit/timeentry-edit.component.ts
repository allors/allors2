import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { Subscription, combineLatest } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService, TestScope } from '../../../../../angular';
import { TimeEntry, TimeFrequency, TimeSheet, Party, WorkEffortPartyAssignment, WorkEffort, RateType, WorkEffortAssignmentRate, PartyRate } from '../../../../../domain';
import { PullRequest, Sort, IObject } from '../../../../../framework';
import { ObjectData } from '../../../../../material/core/services/object';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SaveService } from '../../../../../../allors/material';

@Component({
  templateUrl: './timeentry-edit.component.html',
  providers: [ContextService]
})
export class TimeEntryEditComponent extends TestScope implements OnInit, OnDestroy {

  title: string;
  subTitle: string;

  readonly m: Meta;

  frequencies: TimeFrequency[];

  private subscription: Subscription;
  timeEntry: TimeEntry;
  timeSheet: TimeSheet;
  workers: Party[];
  selectedWorker: Party;
  workEffort: WorkEffort;
  rateTypes: RateType[];
  workEffortAssignmentRates: WorkEffortAssignmentRate[];
  workEffortRate: WorkEffortAssignmentRate;
  partyRate: PartyRate;
  derivedBillingRate: number;
  customerRate: PartyRate;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<TimeEntryEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const workEffortPartyAssignmentPullName = `${this.m.WorkEffortPartyAssignment.name}`;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          let pulls = [
            pull.TimeEntry({
              object: this.data.id,
              include: {
                TimeFrequency: x,
                BillingFrequency: x
              }
            }),
            pull.TimeEntry({
              object: this.data.id,
              fetch: {
                WorkEffort: {
                  WorkEffortPartyAssignmentsWhereAssignment: {
                    include: {
                      Party: x
                    }
                  }
                }
              }
            }),
            pull.RateType({ sort: new Sort(this.m.RateType.Name) }),
            pull.TimeFrequency({ sort: new Sort(this.m.TimeFrequency.Name) }),
          ];

          if (isCreate) {
            pulls = [
              ...pulls,
              pull.WorkEffort({
                object: this.data.associationId
              }),
              pull.WorkEffort({
                name: workEffortPartyAssignmentPullName,
                object: this.data.associationId,
                fetch: {
                  WorkEffortPartyAssignmentsWhereAssignment: {
                    include: {
                      Party: x
                    }
                  }
                }
              }),
            ];
          }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.rateTypes = loaded.collections.RateTypes as RateType[];
        this.frequencies = loaded.collections.TimeFrequencies as TimeFrequency[];
        const hour = this.frequencies.find((v) => v.UniqueId === 'db14e5d55eaf4ec8b149c558a28d99f5');

        if (isCreate) {
          this.workEffort = loaded.objects.WorkEffort as WorkEffort;

          this.title = 'Add Time Entry';
          this.timeEntry = this.allors.context.create('TimeEntry') as TimeEntry;
          this.timeEntry.WorkEffort = this.workEffort;
          this.timeEntry.IsBillable = true;
          this.timeEntry.BillingFrequency = hour;
          this.timeEntry.TimeFrequency = hour;

          const workEffortPartyAssignments = loaded.collections[workEffortPartyAssignmentPullName] as WorkEffortPartyAssignment[];
          this.workers = Array.from(new Set(workEffortPartyAssignments.map(v => v.Party)).values());
        } else {
          this.timeEntry = loaded.objects.TimeEntry as TimeEntry;
          this.selectedWorker = this.timeEntry.Worker;
          this.workEffort = this.timeEntry.WorkEffort;

          const workEffortPartyAssignments = loaded.collections.WorkEffortPartyAssignments as WorkEffortPartyAssignment[];
          this.workers = Array.from(new Set(workEffortPartyAssignments.map(v => v.Party)).values());

          if (this.timeEntry.CanWriteAssignedAmountOfTime) {
            this.title = 'Edit Time Entry';
          } else {
            this.title = 'View Time Entry';
          }
        }

        if (!isCreate) {
          this.workerSelected(this.selectedWorker);
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

  public findBillingRate(parm: any): void {
    if (this.selectedWorker && this.timeEntry.RateType && this.timeEntry.FromDate) {
      this.workerSelected(this.selectedWorker);
    }
  }

  public workerSelected(party: Party): void {
    const { pull, tree, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          Person_TimeSheetWhereWorker: {
          }
        },
      }),
      pull.Party({
        object: party.id,
        fetch: {
          PartyRates: {
            include: {
              RateType: x,
              Frequency: x
            }
          }
        },
      }),
      pull.WorkEffort({
        object: this.data.associationId,
        fetch: {
          WorkEffortAssignmentRatesWhereWorkEffort: {
            include: {
              RateType: x,
              Frequency: x
            }
          }
        }
      }),
      pull.WorkEffort({
        name: 'customerRates',
        object: this.data.associationId,
        fetch: {
          Customer: {
            PartyRates: {
              include: {
                RateType: x,
                Frequency: x
              }
            }
          }
        }
      }),
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        this.timeSheet = loaded.objects.TimeSheet as TimeSheet;

        const partyRates = loaded.collections.PartyRates as PartyRate[];
        this.partyRate = partyRates.find(v => v.RateType === this.timeEntry.RateType && v.Frequency === this.timeEntry.BillingFrequency
          && v.FromDate <= this.timeEntry.FromDate
          && (v.ThroughDate === null || v.ThroughDate >= this.timeEntry.FromDate));

        const workEffortAssignmentRates = loaded.collections.WorkEffortAssignmentRates as WorkEffortAssignmentRate[];
        this.workEffortRate = workEffortAssignmentRates.find(v => v.RateType === this.timeEntry.RateType
          && v.Frequency === this.timeEntry.BillingFrequency
          && v.FromDate <= this.timeEntry.FromDate && (v.ThroughDate === null || v.ThroughDate >= this.timeEntry.FromDate));

        const customerRates = loaded.collections.customerRates as PartyRate[];
        this.customerRate = customerRates.find(v => v.RateType === this.timeEntry.RateType
          && v.Frequency === this.timeEntry.BillingFrequency
          && v.FromDate <= this.timeEntry.FromDate && (v.ThroughDate === null || v.ThroughDate >= this.timeEntry.FromDate));

        this.derivedBillingRate = this.workEffortRate && this.workEffortRate.Rate || this.customerRate && this.customerRate.Rate || this.partyRate && this.partyRate.Rate;
      });
  }

  public save(): void {

    if (!this.timeEntry.TimeSheetWhereTimeEntry) {
      this.timeSheet.AddTimeEntry(this.timeEntry);
    }

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        const data: IObject = {
          id: this.timeEntry.id,
          objectType: this.timeEntry.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
