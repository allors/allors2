import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { formatDistance } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { ObjectService } from '@allors/angular/material/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter } from '@allors/angular/material/core';
import { Brand, Model, UnifiedGood, ProductCategory, ProductIdentification } from '@allors/domain/generated';
import { And, Equals, Like, Contains, Exists } from '@allors/data/system';

interface Row extends TableRow {
  object: UnifiedGood;
  name: string;
  id: string;
  categories: string;
  qoh: string;
  photos: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './unifiedgood-list.component.html',
  providers: [ContextService],
})
export class UnifiedGoodListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Unified Goods';

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
    public navigation: NavigationService,
    public mediaService: MediaService,
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
        { name: 'name', sort: true },
        { name: 'id', sort: true },
        { name: 'photos' },
        { name: 'categories' },
        { name: 'qoh' },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [overviewService.overview(), this.delete],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.UnifiedGood.filter = m.UnifiedGood.filter ?? new Filter(m.UnifiedGood.filterDefinition);

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
            pull.UnifiedGood({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.UnifiedGood.sorter.create(sort) : null,
              include: {
                Photos: x,
                PrimaryPhoto: x,
                ProductIdentifications: {
                  ProductIdentificationType: x,
                },
              },
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
            pull.UnifiedGood({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.UnifiedGood.sorter.create(sort) : null,
              fetch: {
                ProductCategoriesWhereProduct: {
                  include: {
                    Products: x,
                    PrimaryAncestors: x,
                  },
                },
              },
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const goods = loaded.collections.UnifiedGoods as UnifiedGood[];
        const productCategories = loaded.collections.ProductCategories as ProductCategory[];

        this.table.total = loaded.values.UnifiedGoods_total;
        this.table.data = goods.map((v) => {
          return {
            object: v,
            name: v.Name,
            id: v.ProductNumber,
            categories: productCategories
              .filter((w) => w.Products.includes(v))
              .map((w) => w.DisplayName)
              .join(', '),
            qoh: v.QuantityOnHand,
            photos: v.Photos.length.toString(),
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
