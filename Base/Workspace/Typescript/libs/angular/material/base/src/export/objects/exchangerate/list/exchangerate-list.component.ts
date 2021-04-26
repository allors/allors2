import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { format } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, EditService } from '@allors/angular/material/core';
import { ExchangeRate } from '@allors/domain/generated';

interface Row extends TableRow {
  object: ExchangeRate;
  validFrom: string;
  from: string;
  to: string;
  rate: string;
}

@Component({
  templateUrl: './exchangerate-list.component.html',
  providers: [ContextService],
})
export class ExchangeRateListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Exchange Rates';

  table: Table<Row>;

  edit: Action;
  delete: Action;

  private subscription: Subscription;
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

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'validFrom', sort: true }, 
        { name: 'from', sort: true }, 
        { name: 'to', sort: true }, 
        { name: 'rate' }, 
      ],
      actions: [this.edit, this.delete],
      defaultAction: this.edit,
      pageSize: 50,
      initialSort: 'validFrom',
      initialSortDirection: 'desc',
    });
  }

  ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.ExchangeRate.filter = m.ExchangeRate.filter ?? new Filter(m.ExchangeRate.filterDefinition);

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
            pull.ExchangeRate({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.ExchangeRate.sorter.create(sort) : null,
              include: {
                FromCurrency: x,
                ToCurrency: x,
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

        const objects = loaded.collections.ExchangeRates as ExchangeRate[];
        this.table.total = loaded.values.ExchangeRates_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            validFrom: format(new Date(v.ValidFrom), 'dd-MM-yyyy'),
            from: v.FromCurrency.Name,
            to: v.ToCurrency.Name,
            rate: v.Rate,
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
