import { Subscription, Subject } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Component, ChangeDetectionStrategy, Self, OnInit, OnDestroy } from '@angular/core';
import { CalendarEvent } from 'angular-calendar';
import { Title } from '@angular/platform-browser';

import { ContextService, MetaService, RefreshService, UserId } from '../../../../../allors/angular';
import { And, Equals, ContainedIn, Filter, PullRequest } from '../../../../../allors/framework';
import { Person, WorkEffortPartyAssignment, TimeEntry, WorkEffort } from '../../../../../allors/domain';

export interface WorkEffortModel {
  object: WorkEffort;
  name: string;
  timeEntries: TimeEntry[];
  openTimeEntries: TimeEntry[];
}
@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: 'timesheet-app.component.html',
  providers: [ContextService]
})
export class TimesheetAppComponent implements OnInit, OnDestroy {

  title = 'Timesheet';

  refresh: Subject<any> = new Subject();
  viewDate = new Date();
  events: CalendarEvent[] = [];

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private userId: UserId,
    titleService: Title) {

    titleService.setTitle(this.title);
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = this.refreshService.refresh$
      .pipe(
        switchMap(() => {

          const pulls = [
            pull.Person({
              object: this.userId.value
            }),
            pull.TimeEntry({
              predicate: new Equals({ propertyType: m.TimeEntry.Worker, object: this.userId.value }),
              include: {
                WorkEffort: x,
              }
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const timeEntries = loaded.collections.TimeEntries as TimeEntry[];

        this.events = timeEntries.map((v) => {
          return {
            title: v.WorkEffort.Name,
            start: new Date(v.FromDate),
            meta: {
              timeEntry: v
            },
          };
        });

        this.refresh.next();
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
