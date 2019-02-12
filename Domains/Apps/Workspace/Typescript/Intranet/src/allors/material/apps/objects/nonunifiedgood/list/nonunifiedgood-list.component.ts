import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Like, Equals, ContainedIn, Filter, Contains, Exists } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, StateService } from '../../../..';

import { Good, ProductCategory, Brand, Model, ProductIdentification } from '../../../../../domain';

import { ObjectService } from '../../../../../material/base/services/object';


interface Row extends TableRow {
  object: Good;
  name: string;
  categories: string;
  qoh: number;
}

@Component({
  templateUrl: './nonunifiedgood-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class NonUnifiedGoodListComponent implements OnInit, OnDestroy {

  public title = 'Products';

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
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private stateService: StateService,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        { name: 'categories' },
        { name: 'qoh' },
      ],
      actions: [
        overviewService.overview(),
        this.delete
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.NonUnifiedGood.Name, parameter: 'name' }),
      new Like({ roleType: m.NonUnifiedGood.Keywords, parameter: 'keyword' }),
      new Contains({ propertyType: m.NonUnifiedGood.ProductCategoriesWhereProduct, parameter: 'category' }),
      new Contains({ propertyType: m.NonUnifiedGood.ProductIdentifications, parameter: 'identification' }),
      new Exists({ propertyType: m.NonUnifiedGood.SalesDiscontinuationDate, parameter: 'discontinued' }),
      new ContainedIn({
        propertyType: m.NonUnifiedGood.Part,
        extent: new Filter({
          objectType: m.Part,
          predicate: new Equals({
            propertyType: m.Part.Brand,
            parameter: 'brand'
          })
        })
      }),
      new ContainedIn({
        propertyType: m.NonUnifiedGood.Part,
        extent: new Filter({
          objectType: m.Part,
          predicate: new Equals({
            propertyType: m.Part.Model,
            parameter: 'model'
          })
        })
      })
    ]);

    const modelSearch = new SearchFactory({
      objectType: m.Model,
      roleTypes: [m.Model.Name],
    });

    const brandSearch = new SearchFactory({
      objectType: m.Brand,
      roleTypes: [m.Brand.Name],
    });

    const categorySearch = new SearchFactory({
      objectType: m.ProductCategory,
      roleTypes: [m.ProductCategory.Name],
    });

    const idSearch = new SearchFactory({
      objectType: m.ProductIdentification,
      roleTypes: [m.ProductIdentification.Identification],
    });

    this.filterService.init(predicate,
      {
        category: { search: categorySearch, display: (v: ProductCategory) => v.Name },
        identification: { search: idSearch, display: (v: ProductIdentification) => v.Identification },
        brand: { search: brandSearch, display: (v: Brand) => v.Name },
        model: { search: modelSearch, display: (v: Model) => v.Name },
      });

    const sorter = new Sorter(
      {
        name: [m.NonUnifiedGood.Name],
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent ]) => {

          const pulls = [
            pull.NonUnifiedGood({
              predicate,
              sort: sorter.create(sort),
              include: {
                Part: x,
                PrimaryPhoto: x,
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
            pull.NonUnifiedGood({
              predicate,
              sort: sorter.create(sort),
              fetch: {
                ProductCategoriesWhereProduct: {
                  include: {
                    Products: x,
                    Parents: x
                  }
                },
              }
            })
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const goods = loaded.collections.NonUnifiedGoods as Good[];
        const productCategories = loaded.collections.ProductCategories as ProductCategory[];

        this.table.total = loaded.values.NonUnifiedGoods_total;
        this.table.data = goods.map((v) => {
          return {
            object: v,
            name: v.Name,
            categories: productCategories.filter(w => w.Products.includes(v)).map((w) => w.Parents.length > 0 ? `${w.Parents.map((y) => y.Name).join(', ')}/${w.Name}` : w.Name).join(', '),
            qoh: v.Part.QuantityOnHand
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
