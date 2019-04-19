import { Subscription, Subject } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Component, ChangeDetectionStrategy, Self, OnInit, OnDestroy } from '@angular/core';
import { CalendarEvent, CalendarEventTimesChangedEvent } from 'angular-calendar';
import { Title } from '@angular/platform-browser';

import { ContextService, MetaService, RefreshService, UserId } from '../../../../angular';
import { Equals, PullRequest } from '../../../../framework';
import { TimeEntry, WorkEffort } from '../../../../domain';
import { ObjectService, SaveService } from '../../../../material';

import { TimeEntryData } from '../../objects/timeentry/edit/TimeEntryData';

interface MetaType {
  timeEntry: TimeEntry;
}

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

  calendarRefresh$: Subject<any> = new Subject();
  viewDate = new Date();
  events: CalendarEvent<MetaType>[] = [];

  get createData(): TimeEntryData {
    return {
      workerId: this.userId.value,
    };
  }

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private objectService: ObjectService,
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
            end: v.ThroughDate ? new Date(v.ThroughDate) : null,
            draggable: true,
            resizable: {
              beforeStart: true,
              afterEnd: true
            },
            meta: {
              timeEntry: v
            },
          };
        });

        this.calendarRefresh$.next();
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  eventClicked({ event }: { event: CalendarEvent }): void {
    if (event.meta) {
      this.objectService.edit(event.meta.timeEntry, this.createData);
    }
  }

  eventTimesChanged({ event: { meta: { timeEntry } }, newStart, newEnd }: CalendarEventTimesChangedEvent<MetaType>) {

    timeEntry.FromDate = newStart.toISOString();
    timeEntry.ThroughDate = newEnd.toISOString();
    this.save();

  }

  private save() {

    this.allors.context.save()
      .subscribe(() => {
        this.refreshService.refresh();
      },
        this.saveService.errorHandler
      );
  }
}
