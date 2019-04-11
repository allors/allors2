import * as moment from 'moment';
import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { PullRequest, And, Equals } from '../../../../../framework';
import { AllorsFilterService, ContextService, NavigationService, RefreshService, MetaService, NavigationActivatedRoute } from '../../../../../angular';
import { StateService, SaveService } from '../../../..';

import { WorkEffort, TimeEntry, WorkEffortInventoryAssignment, TimeSheet, Person, RateType } from '../../../../../domain';
import { MatTableDataSource } from '@angular/material';
import { DataSource } from '@angular/cdk/table';

export interface TimeEntryModel {
  object: TimeEntry;
  worker: string;
  date: string;
  from: string;
  through: string;
  duration: string;
}

export interface PartModel {
  object: WorkEffortInventoryAssignment;
  name: string;
  location: string;
  quantity: number;
}

@Component({
  styleUrls: ['./workorder-detail.component.scss'],
  templateUrl: './workorder-detail.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class WorkerOrderDetailComponent implements OnInit, OnDestroy {

  title = 'Work Orders - Detail';

  private subscription: Subscription;

  timeEntryColumns: string[] = ['worker', 'date', 'from', 'through', 'duration'];
  timeEntryDataSource: DataSource<TimeEntryModel>;

  partColumns: string[] = ['name', 'location', 'quantity'];
  partDataSource: DataSource<PartModel>;

  rateTypes: RateType[];
  workEffort: WorkEffort;
  timeSheets: TimeSheet[];
  timeEntries: TimeEntry[];
  workEffortInventoryAssignments: WorkEffortInventoryAssignment[];

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    private route: ActivatedRoute,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    private stateService: StateService,
    private saveService: SaveService,
    titleService: Title) {

    titleService.setTitle(this.title);
  }

  get runningTimeEntry(): TimeEntry {
    if (this.timeEntries) {
      return this.timeEntries.find((v => v.Worker.id === this.stateService.userId && !v.ThroughDate));
    }
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const object = navRoute.id();

          const pulls = [
            pull.RateType({}),
            pull.WorkEffort({
              object,
              include: {
                Customer: x,
                ContactPerson: x,
                WorkEffortState: x
              }
            }),
            pull.TimeSheet({
              predicate: new Equals({ propertyType: m.TimeSheet.Worker, object: this.stateService.userId }),
              include: {
                Worker: x,
                TimeEntries: x,
              }
            }),
            pull.TimeEntry({
              predicate: new Equals({ propertyType: m.TimeEntry.WorkEffort, object }),
              include: {
                WorkEffort: x,
                Worker: x,
              }
            }),
            pull.WorkEffortInventoryAssignment({
              predicate: new Equals({ propertyType: m.WorkEffortInventoryAssignment.Assignment, object }),
              include: {
                Assignment: x,
                InventoryItem: {
                  Part: x,
                  Facility: x,
                },
              }
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.rateTypes = loaded.collections.RateTypes as RateType[];
        this.workEffort = loaded.objects.WorkEffort as WorkEffort;
        this.timeSheets = loaded.collections.TimeSheets as TimeSheet[];
        this.timeEntries = loaded.collections.TimeEntries as TimeEntry[];
        this.workEffortInventoryAssignments = loaded.collections.WorkEffortInventoryAssignments as WorkEffortInventoryAssignment[];

        // TimeEntries
        const timeEntryModels: TimeEntryModel[] = this.timeEntries.map((v) => {

          const fromDate = moment.utc(v.FromDate);

          let throughDate: moment.Moment;
          let diffDuration: moment.Duration;

          if (v.ThroughDate) {
            throughDate = moment.utc(v.ThroughDate);
            const diff = throughDate.diff(fromDate);
            diffDuration = moment.duration(diff);
          }

          return {
            object: v,
            worker: v.Worker.displayName,
            date: fromDate.format('ll'),
            from: fromDate.format('HH:mm'),
            through: throughDate ? throughDate.format('HH:mm') : '',
            duration: diffDuration ? diffDuration.humanize() : '',
          };
        });

        this.timeEntryDataSource = new MatTableDataSource<TimeEntryModel>(timeEntryModels);

        // Parts
        const partModels: PartModel[] = this.workEffortInventoryAssignments.map((v) => {

          return {
            object: v,
            name: v.InventoryItem.Part.Name,
            location: v.InventoryItem.Facility.Name,
            quantity: v.Quantity
          };
        });

        this.partDataSource = new MatTableDataSource<PartModel>(partModels);

      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  start() {
    const { m } = this.metaService;

    const timeEntry = this.allors.context.create(m.TimeEntry) as TimeEntry;
    timeEntry.WorkEffort = this.workEffort;
    timeEntry.FromDate = moment.utc().toISOString();
    timeEntry.RateType = this.rateTypes[0];

    const timeSheet = this.timeSheets[0];
    timeSheet.AddTimeEntry(timeEntry);

    this.save();
  }

  stop() {
    this.runningTimeEntry.ThroughDate = moment.utc().toISOString();

    this.save();
  }

  private save(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.refreshService.refresh();
      },
        this.saveService.errorHandler);
  }
}
