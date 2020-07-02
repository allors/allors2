import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment/moment';

import { PullRequest, And, Like, ContainedIn, Filter, Equals } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, TestScope } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService } from '../../../..';

import { SerialisedItem, SerialisedItemState, Ownership, Organisation, Party, Brand, Model, SerialisedItemAvailability, ProductCategory, ProductType, UnifiedGood } from '../../../../../domain';

import { ObjectService } from '../../../../../material/core/services/object';

interface Row extends TableRow {
  object: SerialisedItem;
  id: string;
  name: string;
  categories: string;
  availability: string;
  ownership: string;
  suppliedBy: string;
  ownedBy: string;
  rentedBy: string;
}

@Component({
  templateUrl: './serialiseditem-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class SerialisedItemListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Serialised Assets';

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
        { name: 'id', sort: true },
        { name: 'name', sort: true },
        { name: 'categories', sort: true },
        { name: 'availability', sort: true },
        { name: 'ownership', sort: true },
        { name: 'suppliedBy', sort: true },
        { name: 'ownedBy', sort: true },
        { name: 'rentedBy', sort: true }
      ],
      actions: [
        overviewService.overview(),
        this.delete
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'id'
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.SerialisedItem.ItemNumber, parameter: 'id' }),
      new Like({ roleType: m.SerialisedItem.Name, parameter: 'name' }),
      new Like({ roleType: m.SerialisedItem.Keywords, parameter: 'keyword' }),
      new Equals({ propertyType: m.SerialisedItem.OnQuote, parameter: 'onQuote' }),
      new Equals({ propertyType: m.SerialisedItem.OnSalesOrder, parameter: 'onSalesOrder' }),
      new Equals({ propertyType: m.SerialisedItem.OnWorkEffort, parameter: 'onWorkEffort' }),
      new Equals({ propertyType: m.SerialisedItem.SerialisedItemAvailability, parameter: 'availability' }),
      new Equals({ propertyType: m.SerialisedItem.SerialisedItemState, parameter: 'state' }),
      new Equals({ propertyType: m.SerialisedItem.Ownership, parameter: 'ownership' }),
      new Equals({ propertyType: m.SerialisedItem.SuppliedBy, parameter: 'suppliedby' }),
      new Equals({ propertyType: m.SerialisedItem.RentedBy, parameter: 'rentedby' }),
      new Equals({ propertyType: m.SerialisedItem.OwnedBy, parameter: 'ownedby' }),
      new Like({ roleType: m.SerialisedItem.DisplayProductCategories, parameter: 'category' }),
      new ContainedIn({
        propertyType: m.SerialisedItem.PartWhereSerialisedItem,
        extent: new Filter({
          objectType: m.Part,
          predicate: new Equals({
            propertyType: m.Part.Brand,
            parameter: 'brand'
          })
        })
      }),
      new ContainedIn({
        propertyType: m.SerialisedItem.PartWhereSerialisedItem,
        extent: new Filter({
          objectType: m.Part,
          predicate: new Equals({
            propertyType: m.Part.Model,
            parameter: 'model'
          })
        })
      }),
      new ContainedIn({
        propertyType: m.SerialisedItem.PartWhereSerialisedItem,
        extent: new Filter({
          objectType: m.UnifiedGood,
          predicate: new ContainedIn({
            propertyType: m.UnifiedGood.ProductType,
            parameter: 'productType'
          })
        })
      })
    ]);

    const stateSearch = new SearchFactory({
      objectType: m.SerialisedItemState,
      roleTypes: [m.SerialisedItemState.Name],
      predicates: [new Equals({ propertyType: m.SerialisedItemState.IsActive, value: true })]
    });

    const availabilitySearch = new SearchFactory({
      objectType: m.SerialisedItemAvailability,
      roleTypes: [m.SerialisedItemAvailability.Name],
      predicates: [new Equals({ propertyType: m.SerialisedItemAvailability.IsActive, value: true })]
    });

    const ownershipSearch = new SearchFactory({
      objectType: m.Ownership,
      roleTypes: [m.Ownership.Name],
      predicates: [new Equals({ propertyType: m.Ownership.IsActive, value: true })]
    });

    const supplierSearch = new SearchFactory({
      objectType: m.Organisation,
      roleTypes: [m.Organisation.Name],
    });

    const partySearch = new SearchFactory({
      objectType: m.Party,
      roleTypes: [m.Party.PartyName],
    });

    const brandSearch = new SearchFactory({
      objectType: m.Brand,
      roleTypes: [m.Brand.Name],
    });

    const modelSearch = new SearchFactory({
      objectType: m.Model,
      roleTypes: [m.Model.Name],
    });

    const productTypeSearch = new SearchFactory({
      objectType: m.ProductType,
      roleTypes: [m.ProductType.Name],
    });

    this.filterService.init(predicate, {
      active: { initialValue: true },
      onQuote: { initialValue: true },
      onSalesOrder: { initialValue: true },
      onWorkEffort: { initialValue: true },
      state: { search: stateSearch, display: (v: SerialisedItemState) => v && v.Name },
      availability: { search: availabilitySearch, display: (v: SerialisedItemAvailability) => v && v.Name },
      ownership: { search: ownershipSearch, display: (v: Ownership) => v && v.Name },
      suppliedby: { search: supplierSearch, display: (v: Organisation) => v && v.Name },
      ownedby: { search: partySearch, display: (v: Party) => v && v.displayName },
      rentedby: { search: partySearch, display: (v: Party) => v && v.displayName },
      brand: { search: brandSearch, display: (v: Brand) => v && v.Name },
      model: { search: modelSearch, display: (v: Model) => v && v.Name },
      productType: { search: productTypeSearch, display: (v: ProductType) => v && v.Name },
    });

    const sorter = new Sorter(
      {
        id: [m.SerialisedItem.ItemNumber],
        categories: [m.SerialisedItem.DisplayProductCategories],
        name: [m.SerialisedItem.Name],
        availability: [m.SerialisedItem.SerialisedItemAvailabilityName],
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
          ];
        }, [, , , , ]),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.SerialisedItem({
              predicate,
              sort: sorter.create(sort),
              include: {
                SerialisedItemState: x,
                SerialisedItemAvailability: x,
                Ownership: x,
                SuppliedBy: x,
                OwnedBy: x,
                RentedBy: x
              },
              parameters: this.filterService.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const objects = loaded.collections.SerialisedItems as SerialisedItem[];

        this.table.total = loaded.values.SerialisedItems_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            id: v.ItemNumber,
            name: v.Name,
            categories: v.DisplayProductCategories,
            availability: v.SerialisedItemAvailabilityName,
            ownership: v.OwnershipByOwnershipName,
            suppliedBy: v.SuppliedByPartyName,
            ownedBy: v.OwnedByPartyName,
            rentedBy: v.RentedByPartyName,
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
