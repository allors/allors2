import { Subscription, Subject } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { Component, ChangeDetectionStrategy, Self, OnInit, OnDestroy } from '@angular/core';
import { CalendarEvent, CalendarEventTimesChangedEvent, CalendarView } from 'angular-calendar';
import { Title } from '@angular/platform-browser';

import { ContextService, MetaService, RefreshService, UserId, TestScope } from '../../../../angular';
import { Equals, PullRequest } from '../../../../framework';
import { TimeEntry, WorkEffort } from '../../../../domain';
import { ObjectService, SaveService } from '../../../../material';

import { TimeEntryData } from '../../objects/timeentry/edit/TimeEntryData';

import { colors } from './colors';

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
export class TimesheetAppComponent extends TestScope implements OnInit, OnDestroy {

  title = 'Timesheet';

  calendarRefresh$: Subject<any> = new Subject();
  viewDate = new Date();
  calendarView = CalendarView.Day;
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
    titleService: Title
  ) {
    super();

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
                WorkEffort: {
                  WorkEffortState: x
                },
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

          const workEffortState = v.WorkEffort.WorkEffortState;
          const editable = workEffortState.created || workEffortState.inProgress;

          const title = v.WorkEffort.Name;
          const start = new Date(v.FromDate);
          const end = v.ThroughDate ? new Date(v.ThroughDate) : null;
          const draggable = editable;
          const resizable = editable ? {
            beforeStart: true,
            afterEnd: true
          } : undefined;
          const meta: MetaType = {
            timeEntry: v
          };
          const color = editable ? !!end ? colors.yellow : colors.red : colors.blue;

          return { title, start, end, draggable, resizable, meta, color };
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
      this.objectService
        .edit(event.meta.timeEntry, this.createData)
        .subscribe((v) => this.refreshService.refresh());
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
