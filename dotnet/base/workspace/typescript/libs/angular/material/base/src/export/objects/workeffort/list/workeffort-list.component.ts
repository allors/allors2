import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { formatDistance } from 'date-fns';

import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter } from '@allors/angular/material/core';
import { Party, WorkEffort, WorkEffortState, Person, FixedAsset } from '@allors/domain/generated';
import { And, Equals, ContainedIn, Extent, Like } from '@allors/data/system';
import { InternalOrganisationId, PrintService } from '@allors/angular/base';
import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { ObjectService } from '@allors/angular/material/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';

interface Row extends TableRow {
  object: WorkEffort;
  number: string;
  name: string;
  type: string;
  state: string;
  customer: string;
  equipment: string;
  worker: string;
  executedBy: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './workeffort-list.component.html',
  providers: [ContextService],
})
export class WorkEffortListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Work Orders';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,

    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public printService: PrintService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private internalOrganisationId: InternalOrganisationId,
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort: true },
        { name: 'name', sort: true },
        { name: 'type', sort: false },
        { name: 'state' },
        { name: 'customer' },
        { name: 'executedBy' },
        { name: 'equipment' },
        { name: 'worker' },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [overviewService.overview(), this.printService.print(), this.delete],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'number',
      initialSortDirection: 'desc',
    });
  }

  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.WorkEffort.filter = m.WorkEffort.filter ?? new Filter(m.WorkEffort.filterDefinition);

    const internalOrganisationPredicate = new Equals({ propertyType: m.WorkEffort.TakenBy });
    const predicate = new And([
      internalOrganisationPredicate,
      this.filter.definition.predicate
    ]);

    this.subscription = combineLatest([
      this.refreshService.refresh$,
      this.filter.fields$,
      this.table.sort$,
      this.table.pager$,
      this.internalOrganisationId.observable$
    ])
      .pipe(
        scan(
          ([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
            pageEvent =
            previousRefresh !== refresh || filterFields !== previousFilterFields
              ? {
                  ...pageEvent,
                  pageIndex: 0,
                }
              : pageEvent;

          if (pageEvent.pageIndex === 0) {
            this.table.pageIndex = 0;
          }

          return [refresh, filterFields, sort, pageEvent, internalOrganisationId];
        }),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {
          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            pull.WorkEffort({
              predicate,
              sort: sort ? m.WorkEffort.sorter.create(sort) : null,
              include: {
                Customer: x,
                ExecutedBy: x,
                PrintDocument: {
                  Media: x,
                },
                WorkEffortState: x,
                WorkEffortPurposes: x,
                WorkEffortFixedAssetAssignmentsWhereAssignment: {
                  FixedAsset: x,
                },
                WorkEffortPartyAssignmentsWhereAssignment: {
                  Party: x,
                },
              },
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();
        const workEfforts = loaded.collections.WorkEfforts as WorkEffort[];
        this.table.total = loaded.values.WorkTasks_total;
        this.table.data = workEfforts
          .filter((v) => v.CanReadWorkEffortNumber)
          .map((v) => {
            return {
              object: v,
              number: v.WorkEffortNumber,
              name: v.Name,
              type: v.objectType.name,
              state: v.WorkEffortState ? v.WorkEffortState.Name : '',
              customer: v.Customer ? v.Customer.displayName : '',
              executedBy: v.ExecutedBy ? v.ExecutedBy.displayName : '',
              equipment: v.WorkEffortFixedAssetAssignmentsWhereAssignment
                ? v.WorkEffortFixedAssetAssignmentsWhereAssignment.map((w) => w.FixedAsset.displayName).join(', ')
                : '',
              worker: v.WorkEffortPartyAssignmentsWhereAssignment
                ? v.WorkEffortPartyAssignmentsWhereAssignment.map((w) => w.Party.displayName).join(', ')
                : '',
              lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date()),
            } as Row;
          });
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
