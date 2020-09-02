import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { ObjectService } from '@allors/angular/material/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter } from '@allors/angular/material/core';
import { Good, ProductCategory, ProductIdentification, Brand, Model, NonUnifiedGood, UnifiedGood } from '@allors/domain/generated';
import { And, Like, Contains, Exists } from '@allors/data/system';

interface Row extends TableRow {
  object: Good;
  name: string;
  id: string;
  categories: string;
  qoh: string;
}

@Component({
  templateUrl: './good-list.component.html',
  providers: [ContextService],
})
export class GoodListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Goods';

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
    titleService: Title,
  ) {
    super();

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [{ name: 'name', sort: true }, { name: 'id' }, { name: 'categories' }, { name: 'qoh' }],
      actions: [overviewService.overview(), this.delete],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.Good.filter = m.Good.filter ?? new Filter(m.Good.filterDefinition);

    this.subscription = combineLatest([this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$])
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
          [, , , , ,],
        ),
        switchMap(([, filterFields, sort, pageEvent]) => {
          const pulls = [
            pull.Good({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.Good.sorter.create(sort) : null,
              include: {
                NonUnifiedGood_Part: x,
                ProductIdentifications: {
                  ProductIdentificationType: x,
                },
              },
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
            pull.Good({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.Good.sorter.create(sort) : null,
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
        }),
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const goods = loaded.collections.Goods as Good[];
        const productCategories = loaded.collections.ProductCategories as ProductCategory[];

        this.table.total = loaded.values.NonUnifiedGoods_total;
        this.table.data = goods.map((v) => {
          return {
            object: v,
            name: v.Name,
            id: v.ProductIdentifications.find((p) => p.ProductIdentificationType.UniqueId === 'b640630d-a556-4526-a2e5-60a84ab0db3f')
              .Identification,
            categories: productCategories
              .filter((w) => w.Products.includes(v))
              .map((w) => w.displayName)
              .join(', '),
            // qoh: v.Part && v.Part.QuantityOnHand
            qoh: ((v as NonUnifiedGood).Part && (v as NonUnifiedGood).Part.QuantityOnHand) || (v as UnifiedGood).QuantityOnHand,
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
