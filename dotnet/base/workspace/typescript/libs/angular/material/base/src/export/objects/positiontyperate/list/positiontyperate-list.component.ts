import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { format } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, EditService } from '@allors/angular/material/core';
import { PositionType, PositionTypeRate, RateType } from '@allors/domain/generated';
import { And, Equals, Contains } from '@allors/data/system';

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
    this.edit.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
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
    this.filter = m.PositionTypeRate.filter = m.PositionTypeRate.filter ?? new Filter(m.PositionTypeRate.filterDefinition);

    this.subscription = combineLatest([this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$])
      .pipe(
        scan(
          ([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
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

          return [refresh, filterFields, sort, pageEvent];
        }),
        switchMap(([, filterFields, sort, pageEvent]) => {
          const pulls = [
            pull.PositionTypeRate({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.PositionTypeRate.sorter.create(sort) : null,
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
