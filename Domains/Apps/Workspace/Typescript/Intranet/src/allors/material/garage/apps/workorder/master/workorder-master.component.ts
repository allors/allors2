import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { PullRequest, And, Equals, ContainedIn, Filter, Exists, Not } from '../../../../../framework';
import { AllorsFilterService, ContextService, NavigationService, RefreshService, MetaService, UserId } from '../../../../../angular';

import { WorkEffortPartyAssignment, WorkEffort, TimeEntry, Person } from '../../../../../domain';

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
    private userId: UserId,
    titleService: Title) {

    titleService.setTitle(this.title);
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = this.refreshService.refresh$
      .pipe(
        switchMap(() => {

          const predicate = new And([
            new Equals({ propertyType: m.WorkEffortPartyAssignment.Party, object: this.userId.value }),
            new ContainedIn({
              propertyType: m.WorkEffortPartyAssignment.Assignment,
              extent: new Filter({
                objectType: m.WorkEffort,
                // TODO: only in progress
              })
            })
          ]);

          const pulls = [
            pull.Person({
              object: this.userId.value
            }),
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

        const worker = loaded.objects.Person as Person;
        const workEffortPartyAssignments = loaded.collections.WorkEffortPartyAssignments as WorkEffortPartyAssignment[];
        const allTimeEntries = loaded.collections.ServiceEntries.filter((v) => v.objectType === m.TimeEntry) as TimeEntry[];

        this.workEfforts = workEffortPartyAssignments
          .filter((v) => v.CanReadAssignment && v.Assignment)
          .map((v) => v.Assignment)
          .filter((v, i, a) => a.indexOf(v) === i)
          .map((v) => {

            const timeEntries = allTimeEntries.filter((w) => w.Worker === worker && w.WorkEffort === v);
            const openTimeEntries = timeEntries.filter((w) => !w.ThroughDate);

            return {
              object: v,
              name: v.Name,
              started: new Date(v.ActualStart),
              routerLink: `./${v.id}`,
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

        this.runningWorkEfforts = this.workEfforts.filter((v) => v.timeEntries.length > 0 && v.openTimeEntries.length > 0);
        this.activeWorkEfforts = this.workEfforts.filter((v) => v.timeEntries.length > 0 && v.openTimeEntries.length === 0);
        this.assignedWorkEfforts = this.workEfforts.filter((v) => v.timeEntries.length === 0);
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
