import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title, Meta } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { formatDistance } from 'date-fns';

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
  UserId,
} from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, ObjectService, EditService, MethodService } from '@allors/angular/material/core';
import { Organisation, Party, SerialisedItem, SerialisedItemState, SerialisedItemAvailability, Ownership, Brand, Model, ProductType, SerialisedItemCharacteristicType, IUnitOfMeasure, Shipment, ShipmentState, TaskAssignment, UnifiedGood, ProductCategory, ProductIdentification } from '@allors/domain/generated';
import { And, Equals, ContainedIn, Extent, Like, Or, Contains, Exists } from '@allors/data/system';
import { InternalOrganisationId } from '@allors/angular/base';




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
  providers: [ContextService]
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
    this.delete.result.subscribe((v) => {
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
      roleTypes: [m.ProductCategory.DisplayName],
    });

    const idSearch = new SearchFactory({
      objectType: m.ProductIdentification,
      roleTypes: [m.ProductIdentification.Identification],
    });

    const filterDefinition = new FilterDefinition(predicate,
      {
        category: { search: categorySearch, display: (v: ProductCategory) => v && v.DisplayName },
        identification: { search: idSearch, display: (v: ProductIdentification) => v && v.Identification },
        brand: { search: brandSearch, display: (v: Brand) => v && v.Name },
        model: { search: modelSearch, display: (v: Model) => v && v.Name },
      });
      this.filter = new Filter(filterDefinition);
      
    const sorter = new Sorter(
      {
        name: [m.UnifiedGood.Name],
        id: [m.UnifiedGood.ProductNumber],
        lastModifiedDate: m.UnifiedGood.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
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
              parameters: this.filter.parameters(filterFields),
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
            id: v.ProductNumber,
            categories: productCategories.filter(w => w.Products.includes(v)).map((w) => w.DisplayName).join(', '),
            qoh: v.QuantityOnHand,
            photos: v.Photos.length.toString(),
            lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date())
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
