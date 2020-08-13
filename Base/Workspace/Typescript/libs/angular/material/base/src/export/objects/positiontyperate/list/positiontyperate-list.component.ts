import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { format, formatDistance } from 'date-fns';

import {
  ContextService,
  TestScope,
  MetaService,
  RefreshService,
  Action,
  NavigationService,
  MediaService,
  Filter,
  FilterDefinition,
  SearchFactory,
} from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, ObjectService, EditService } from '@allors/angular/material/core';
import { Person, Organisation, Country, PositionType, PositionTypeRate, RateType } from '@allors/domain/generated';
import { And, Like, ContainedIn, Extent, Equals, Contains } from '@allors/data/system';

interface Row extends TableRow {
  object: PositionTypeRate;
  positionType: string;
  rateType: string;
  from: string;
  through: string;
  rate: string;
  frequency: string;
}

@Component({
  templateUrl: './positiontyperate-list.component.html',
  providers: [ContextService],
})
export class PositionTypeRatesOverviewComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Position Type Rates';

  table: Table<Row>;

  edit: Action;
  delete: Action;

  private subscription: Subscription;
  positionTypes: PositionType[];
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,

    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public editService: EditService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.edit = editService.edit();
    this.edit.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'positionType' },
        { name: 'rateType' },
        { name: 'from', sort },
        { name: 'through', sort },
        { name: 'rate', sort },
        { name: 'frequency' },
      ],
      actions: [this.edit, this.delete],
      defaultAction: this.edit,
      pageSize: 50,
    });
  }

  ngOnInit(): void {
    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Contains({ propertyType: m.PositionTypeRate.PositionTypesWherePositionTypeRate, parameter: 'positionType' }),
      new Equals({ propertyType: m.PositionTypeRate.RateType, parameter: 'rateType' }),
    ]);

    const positionTypeSearch = new SearchFactory({
      objectType: m.PositionType,
      roleTypes: [m.PositionType.Title],
    });

    const rateTypeSearch = new SearchFactory({
      objectType: m.RateType,
      roleTypes: [m.RateType.Name],
      predicates: [new Equals({ propertyType: m.RateType.IsActive, value: true })],
    });

    const filterDefinition = new FilterDefinition(predicate, {
      active: { initialValue: true },
      positionType: { search: positionTypeSearch, display: (v: PositionType) => v && v.Title },
      rateType: { search: rateTypeSearch, display: (v: RateType) => v && v.Name },
    });
    this.filter = new Filter(filterDefinition);

    const sorter = new Sorter({
      rate: m.PositionTypeRate.Rate,
      from: m.PositionTypeRate.FromDate,
      through: m.PositionTypeRate.ThroughDate,
    });

    this.subscription = combineLatest(this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$)
      .pipe(
        scan(
          ([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
            return [
              refresh,
              filterFields,
              sort,
              previousRefresh !== refresh || filterFields !== previousFilterFields ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
            ];
          },
          [, , , ,]
        ),
        switchMap(([, filterFields, sort, pageEvent]) => {
          const pulls = [
            pull.PositionTypeRate({
              predicate,
              sort: sorter.create(sort),
              include: {
                Frequency: x,
                RateType: x,
              },
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
            pull.PositionType({
              include: {
                PositionTypeRate: x,
              },
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.positionTypes = loaded.collections.PositionTypes as PositionType[];
        const objects = loaded.collections.PositionTypeRates as PositionTypeRate[];

        this.table.total = loaded.values.PositionTypeRates_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            positionType: this.positionTypes
              .filter((p) => p.PositionTypeRate === v)
              .map((p) => p.Title)
              .join(', '),
            rateType: v.RateType.Name,
            from: format(new Date(v.FromDate), 'dd-MM-yyyy'),
            through: v.ThroughDate !== null ? format(new Date(v.ThroughDate), 'dd-MM-yyyy') : '',
            rate: v.Rate,
            frequency: v.Frequency.Name,
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
