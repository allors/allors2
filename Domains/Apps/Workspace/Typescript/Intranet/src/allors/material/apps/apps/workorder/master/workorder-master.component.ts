import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { PullRequest, And, Equals } from '../../../../../framework';
import { AllorsFilterService, ContextService, NavigationService, RefreshService, MetaService } from '../../../../../angular';
import { StateService } from '../../../..';

import { WorkEffortPartyAssignment, WorkEffort, TimeEntry } from '../../../../../domain';

export interface TimeEntryModel {
  object: TimeEntry;
}

export interface WorkEffortModel {
  object: WorkEffort;
  name: string;
  started: Date;
  routerLink: string;
  timeEntries: TimeEntryModel[];
  openTimeEntries: TimeEntryModel[];
}

@Component({
  templateUrl: './workorder-master.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class WorkerOrderMasterComponent implements OnInit, OnDestroy {

  title = 'Work Orders';

  workEfforts: WorkEffortModel[];

  runningWorkEfforts: WorkEffortModel[];
  activeWorkEfforts: WorkEffortModel[];
  assignedWorkEfforts: WorkEffortModel[];

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    private stateService: StateService,
    titleService: Title) {

    titleService.setTitle(this.title);
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = this.refreshService.refresh$
      .pipe(
        switchMap(() => {

          const predicate = new And([
            new Equals({ propertyType: m.WorkEffortPartyAssignment.Party, object: this.stateService.userId }),
          ]);

          const pulls = [
            pull.WorkEffortPartyAssignment({
              predicate,
              include: {
                Assignment: x,
                Party: x,
              }
            }),
            pull.WorkEffortPartyAssignment({
              predicate,
              fetch: {
                Assignment: {
                  ServiceEntriesWhereWorkEffort: {
                    include: {
                      WorkEffort: x,
                      TimeEntry_Worker: x
                    }
                  }
                }
              }
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const workEffortPartyAssignments = loaded.collections.WorkEffortPartyAssignments as WorkEffortPartyAssignment[];
        const allTimeEntries = loaded.collections.ServiceEntries.filter((v) => v.objectType === m.TimeEntry) as TimeEntry[];

        this.workEfforts = workEffortPartyAssignments
          .filter((v) => v.CanReadAssignment && v.Assignment)
          .map((v) => {
            const workEffort = v.Assignment;
            const timeEntries = allTimeEntries.filter((w) => v.Party === w.Worker && w.WorkEffort === workEffort);
            const openTimeEntries = timeEntries.filter((w) => !v.ThroughDate);

            return {
              object: workEffort,
              name: workEffort.Name,
              started: new Date(workEffort.ActualStart),
              routerLink: `./${workEffort.id}`,
              timeEntries: timeEntries.map((w) => {
                return {
                  object: w
                };
              }),
              openTimeEntries: openTimeEntries.map((w) => {
                return {
                  object: w
                };
              })
            };
          });

        this.activeWorkEfforts = this.workEfforts.filter((v) => v.timeEntries.length > 0 && v.openTimeEntries.length > 0);
        this.runningWorkEfforts = this.workEfforts.filter((v) => v.timeEntries.length > 0 && v.openTimeEntries.length === 0);
        this.assignedWorkEfforts = this.workEfforts.filter((v) => v.timeEntries.length === 0);
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
