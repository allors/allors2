import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Like } from '../../../../../framework';
import { WorkEffort } from '../../../../../domain';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, PrintService, ObjectService } from '../../../../../material';

interface Row extends TableRow {
  object: WorkEffort;
  number: string;
  name: string;
  type: string;
  state: string;
  description: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './workeffort-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class WorkEffortListComponent implements OnInit, OnDestroy {

  public title = 'Work Efforts';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public printService: PrintService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort: true },
        { name: 'name', sort: true },
        { name: 'type', sort: false },
        { name: 'state', sort: true },
        { name: 'description', sort: true },
        'lastModifiedDate'
      ],
      actions: [
        overviewService.overview(),
        this.printService.print(),
        this.delete
      ],
      defaultAction: overviewService.overview(),
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.WorkEffort.WorkEffortNumber, parameter: 'Number' }),
      new Like({ roleType: m.WorkEffort.Name, parameter: 'Name' }),
      new Like({ roleType: m.WorkEffort.Description, parameter: 'Description' }),
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        number: [m.WorkEffort.WorkEffortNumber],
        name: [m.WorkEffort.Name],
        state: [m.WorkEffortState.Name],
        description: [m.WorkEffort.Description],
        lastModifiedDate: m.Person.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.WorkEffort({
              predicate,
              sort: sorter.create(sort),
              include: {
                PrintDocument: x,
                WorkEffortState: x,
                WorkEffortPurposes: x,
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();
        const workEfforts = loaded.collections.WorkEfforts as WorkEffort[];
        this.table.total = loaded.values.WorkTasks_total;
        this.table.data = workEfforts.map((v) => {
          return {
            object: v,
            number: v.WorkEffortNumber,
            name: v.Name,
            type: v.objectType.name,
            description: v.Description,
            lastModifiedDate: moment(v.LastModifiedDate).fromNow()
          } as Row;
        });
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
