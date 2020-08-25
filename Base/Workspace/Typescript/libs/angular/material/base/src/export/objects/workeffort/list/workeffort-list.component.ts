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

    const internalOrganisationPredicate = new Equals({ propertyType: m.WorkEffort.TakenBy });
    const predicate = new And([
      internalOrganisationPredicate,
      new Equals({ propertyType: m.WorkEffort.WorkEffortState, parameter: 'state' }),
      new Equals({ propertyType: m.WorkEffort.Customer, parameter: 'customer' }),
      new Equals({ propertyType: m.WorkEffort.ExecutedBy, parameter: 'ExecutedBy' }),
      new Like({ roleType: m.WorkEffort.WorkEffortNumber, parameter: 'Number' }),
      new Like({ roleType: m.WorkEffort.Name, parameter: 'Name' }),
      new Like({ roleType: m.WorkEffort.Description, parameter: 'Description' }),
      new ContainedIn({
        propertyType: m.WorkEffort.WorkEffortPartyAssignmentsWhereAssignment,
        extent: new Extent({
          objectType: m.WorkEffortPartyAssignment,
          predicate: new Equals({ propertyType: m.WorkEffortPartyAssignment.Party, parameter: 'worker' }),
        }),
      }),
      new ContainedIn({
        propertyType: m.WorkEffort.WorkEffortFixedAssetAssignmentsWhereAssignment,
        extent: new Extent({
          objectType: m.WorkEffortFixedAssetAssignment,
          predicate: new Equals({ propertyType: m.WorkEffortFixedAssetAssignment.FixedAsset, parameter: 'equipment' }),
        }),
      }),
    ]);

    const stateSearch = new SearchFactory({
      objectType: m.WorkEffortState,
      roleTypes: [m.WorkEffortState.Name],
    });

    const partySearch = new SearchFactory({
      objectType: m.Party,
      roleTypes: [m.Party.PartyName],
    });

    const personSearch = new SearchFactory({
      objectType: m.Person,
      roleTypes: [m.Person.PartyName, m.Person.UserName],
    });

    const equipmentSearch = new SearchFactory({
      objectType: m.FixedAsset,
      roleTypes: [m.FixedAsset.SearchString],
    });

    const filterDefinition = new FilterDefinition(predicate, {
      state: { search: () => stateSearch, display: (v: WorkEffortState) => v && v.Name },
      customer: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      ExecutedBy: { search: () => partySearch, display: (v: Party) => v && v.PartyName },
      worker: { search: () => personSearch, display: (v: Person) => v && v.displayName },
      equipment: { search: () => equipmentSearch, display: (v: FixedAsset) => v && v.displayName },
    });
    this.filter = new Filter(filterDefinition);

    const sorter = new Sorter({
      number: [m.WorkEffort.SortableWorkEffortNumber],
      name: [m.WorkEffort.Name],
      description: [m.WorkEffort.Description],
      lastModifiedDate: m.Person.LastModifiedDate,
    });

    this.subscription = combineLatest(
      this.refreshService.refresh$,
      this.filter.fields$,
      this.table.sort$,
      this.table.pager$,
      this.internalOrganisationId.observable$
    )
      .pipe(
        scan(
          ([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
            return [
              refresh,
              filterFields,
              sort,
              previousRefresh !== refresh || filterFields !== previousFilterFields ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
              internalOrganisationId,
            ];
          },
          [, , , , ,]
        ),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {
          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            pull.WorkEffort({
              predicate,
              sort: sorter.create(sort),
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
