import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Like, ContainedIn, Filter, Equals } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService } from '../../../..';

import { SerialisedItem, SerialisedItemState, Ownership, Organisation, Party, Brand, Model } from '../../../../../domain';

import { ObjectService } from '../../../../../material/base/services/object';

interface Row extends TableRow {
  object: SerialisedItem;
  id: string;
  name: string;
  state: string;
  ownership: string;
  suppliedBy: string;
  ownedBy: string;
  rentedBy: string;
}

@Component({
  templateUrl: './serialiseditem-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class SerialisedItemListComponent implements OnInit, OnDestroy {

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
    private errorService: ErrorService,
    titleService: Title) {

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
        { name: 'state' },
        { name: 'ownership' },
        { name: 'suppliedBy' },
        { name: 'ownedBy' },
        { name: 'rentedBy' },
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
      new Equals({ propertyType: m.SerialisedItem.SerialisedItemState, parameter: 'state' }),
      new Equals({ propertyType: m.SerialisedItem.Ownership, parameter: 'ownership' }),
      new Equals({ propertyType: m.SerialisedItem.SuppliedBy, parameter: 'suppliedby' }),
      new Equals({ propertyType: m.SerialisedItem.RentedBy, parameter: 'rentedby' }),
      new Equals({ propertyType: m.SerialisedItem.OwnedBy, parameter: 'ownedby' }),
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
      })
    ]);

    const stateSearch = new SearchFactory({
      objectType: m.SerialisedItemState,
      roleTypes: [m.SerialisedItemState.Name],
      predicates: [new Equals({ propertyType: m.SerialisedItemState.IsActive, value: true })]
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

    this.filterService.init(predicate, {
      active: { initialValue: true },
      state: { search: stateSearch, display: (v: SerialisedItemState) => v.Name },
      ownership: { search: ownershipSearch, display: (v: Ownership) => v.Name },
      suppliedby: { search: supplierSearch, display: (v: Organisation) => v.Name },
      ownedby: { search: partySearch, display: (v: Party) => v.displayName },
      rentedby: { search: partySearch, display: (v: Party) => v.displayName },
      brand: { search: brandSearch, display: (v: Brand) => v.Name },
      model: { search: modelSearch, display: (v: Model) => v.Name },
  });

    const sorter = new Sorter(
      {
        id: [m.SerialisedItem.ItemNumber],
        name: [m.SerialisedItem.Name],
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
        }, []),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.SerialisedItem({
              predicate,
              sort: sorter.create(sort),
              include: {
                SerialisedItemState: x,
                Ownership: x,
                SuppliedBy: x,
                OwnedBy: x,
                RentedBy: x
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
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
            state: `${v.SerialisedItemState && v.SerialisedItemState.Name}`,
            ownership: `${v.Ownership && v.Ownership.Name}`,
            suppliedBy: v.SuppliedBy ? v.SuppliedBy.displayName : '',
            ownedBy: v.OwnedBy ? v.OwnedBy.displayName : '',
            rentedBy: v.RentedBy ? v.RentedBy.displayName : '',
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
