import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, EditService } from '@allors/angular/material/core';
import { ProductType } from '@allors/domain/generated';
import { And, Like } from '@allors/data/system';

interface Row extends TableRow {
  object: ProductType;
  name: string;
}

@Component({
  templateUrl: './producttype-list.component.html',
  providers: [ContextService],
})
export class ProductTypesOverviewComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Product Types';

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
      columns: [{ name: 'name', sort: true }],
      actions: [this.edit, this.delete],
      defaultAction: this.edit,
      pageSize: 50,
    });
  }

  ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.ProductType.filter = m.ProductType.filter ?? new Filter(m.ProductType.filterDefinition);

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
            pull.ProductType({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.ProductType.sorter.create(sort) : null,
              include: {
                SerialisedItemCharacteristicTypes: x,
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

        const objects = loaded.collections.ProductTypes as ProductType[];
        this.table.total = loaded.values.ProductTypes_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            name: `${v.Name}`,
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
