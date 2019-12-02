import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Like, Equals, Contains, Exists } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, InternalOrganisationId, TestScope } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService } from '../../../..';

import { UnifiedGood, ProductCategory, Brand, Model, ProductIdentification } from '../../../../../domain';

import { ObjectService } from '../../../../../material/core/services/object';


interface Row extends TableRow {
  object: UnifiedGood;
  name: string;
  id: string;
  categories: string;
  qoh: string;
  photos: string;
}

@Component({
  templateUrl: './unifiedgood-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class UnifiedGoodListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Unified Goods';

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
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        { name: 'id' },
        { name: 'photos' },
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
      new Like({ roleType: m.UnifiedGood.Name, parameter: 'name' }),
      new Like({ roleType: m.UnifiedGood.Keywords, parameter: 'keyword' }),
      new Contains({ propertyType: m.UnifiedGood.ProductCategoriesWhereProduct, parameter: 'category' }),
      new Contains({ propertyType: m.UnifiedGood.ProductIdentifications, parameter: 'identification' }),
      new Equals({ propertyType: m.UnifiedGood.Brand, parameter: 'brand' }),
      new Equals({ propertyType: m.UnifiedGood.Model, parameter: 'model' }),
      new Exists({ propertyType: m.UnifiedGood.SalesDiscontinuationDate, parameter: 'discontinued' }),
      new Exists({ propertyType: m.UnifiedGood.Photos, parameter: 'photos' }),
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
        category: { search: categorySearch, display: (v: ProductCategory) => v && v.Name },
        identification: { search: idSearch, display: (v: ProductIdentification) => v && v.Identification },
        brand: { search: brandSearch, display: (v: Brand) => v && v.Name },
        model: { search: modelSearch, display: (v: Model) => v && v.Name },
      });

    const sorter = new Sorter(
      {
        name: [m.UnifiedGood.Name],
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
        }, [, , , , , ]),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.UnifiedGood({
              predicate,
              sort: sorter.create(sort),
              include: {
                Photos: x,
                PrimaryPhoto: x,
                ProductIdentifications: {
                  ProductIdentificationType: x
                }
              },
              parameters: this.filterService.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
            pull.UnifiedGood({
              predicate,
              sort: sorter.create(sort),
              fetch: {
                ProductCategoriesWhereProduct: {
                  include: {
                    Products: x,
                    PrimaryAncestors: x
                  }
                },
              }
            })
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
            id: v.ProductIdentifications
                .find(p => p.ProductIdentificationType.UniqueId === 'b640630d-a556-4526-a2e5-60a84ab0db3f' || p.ProductIdentificationType.UniqueId === '5735191a-cdc4-4563-96ef-dddc7b969ca6').Identification,
            categories: productCategories.filter(w => w.Products.includes(v)).map((w) => w.displayName).join(', '),
            qoh: v.QuantityOnHand,
            photos: v.Photos.length.toString(),
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
