import * as moment from 'moment';
import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { PullRequest, And, Equals } from '../../../../../framework';
import { AllorsFilterService, ContextService, NavigationService, RefreshService, MetaService, NavigationActivatedRoute } from '../../../../../angular';
import { StateService } from '../../../..';

import { WorkEffort, TimeEntry, WorkEffortInventoryAssignment } from '../../../../../domain';
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

@Component({
  templateUrl: './workorder-detail.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class WorkerOrderDetailComponent implements OnInit, OnDestroy {

  title = 'Work Orders - Detail';

  private subscription: Subscription;

  timeEntryColumns: string[] = ['worker', 'date', 'from', 'through', 'duration'];
  timeEntryDataSource: DataSource<TimeEntryModel>;

  workEffort: WorkEffort;
  timeEntries: TimeEntry[];
  workEffortInventoryAssignments: any[];

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    private route: ActivatedRoute,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    private stateService: StateService,
    titleService: Title) {

    titleService.setTitle(this.title);
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const object = navRoute.id();

          const pulls = [
            pull.WorkEffort({
              object,
              include: {
                Customer: x,
                ContactPerson: x,
                WorkEffortState: x
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
                InventoryItemTransactions: x,
              }
            }),
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.workEffort = loaded.objects.WorkEffort as WorkEffort;
        this.timeEntries = loaded.collections.TimeEntries as TimeEntry[];
        this.workEffortInventoryAssignments = loaded.collections.WorkEffortInventoryAssignments as WorkEffortInventoryAssignment[];

        const timeEntryModels: TimeEntryModel[] = this.timeEntries.map((v) => {

          const fromDate = moment.utc(v.FromDate);
          const throughDate = moment.utc(v.ThroughDate);
          const diff = throughDate.diff(fromDate);
          const diffDuration = moment.duration(diff);

          return {
            object: v,
            worker: v.Worker.displayName,
            date: fromDate.format('ll'),
            from: fromDate.format('HH:mm'),
            through: throughDate.format('HH:mm'),
            duration: diffDuration.humanize(),
          };
        });

        this.timeEntryDataSource = new MatTableDataSource<TimeEntryModel>(timeEntryModels);
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
