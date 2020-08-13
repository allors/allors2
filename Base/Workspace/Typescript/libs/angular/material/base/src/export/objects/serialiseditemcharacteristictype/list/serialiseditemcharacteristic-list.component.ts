import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

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
} from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, ObjectService, EditService } from '@allors/angular/material/core';
import { Organisation, Party, SerialisedItem, SerialisedItemState, SerialisedItemAvailability, Ownership, Brand, Model, ProductType, SerialisedItemCharacteristicType, IUnitOfMeasure } from '@allors/domain/generated';
import { And, Equals, ContainedIn, Extent, Like } from '@allors/data/system';

interface Row extends TableRow {
  object: SerialisedItemCharacteristicType;
  name: string;
  uom: string;
  active: boolean;
}

@Component({
  templateUrl: './serialiseditemcharacteristic-list.component.html',
  providers: [ContextService]
})
export class SerialisedItemCharacteristicListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Product Characteristics';

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
    titleService: Title,
  ) {
    super();

    titleService.setTitle(this.title);

    this.edit = editService.edit();
    this.edit.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        { name: 'uom', sort: true },
        { name: 'active', sort: true }
      ],
      actions: [
        this.edit,
        this.delete
      ],
      defaultAction: this.edit
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.SerialisedItemCharacteristicType.Name, parameter: 'name' }),
      new Equals({ propertyType: m.SerialisedItemCharacteristicType.IsActive, parameter: 'active' }),
      new Equals({ propertyType: m.SerialisedItemCharacteristicType.UnitOfMeasure, parameter: 'uom' })
    ]);

    const uomSearch = new SearchFactory({
      objectType: m.IUnitOfMeasure,
      roleTypes: [m.IUnitOfMeasure.Name],
      predicates: [new Equals({ propertyType: m.IUnitOfMeasure.IsActive, value: true })]
    });

    const filterDefinition = new FilterDefinition(predicate,
      {
        active: { initialValue: true },
        uom: { search: uomSearch, display: (v: IUnitOfMeasure) => v && v.Name },
      });
      this.filter = new Filter(filterDefinition);
      
    const sorter = new Sorter(
      {
        name: m.SerialisedItemCharacteristicType.Name,
        uom: m.UnitOfMeasure.Name,
        active: m.SerialisedItemCharacteristicType.IsActive
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
        }, [, , , ,]),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.SerialisedItemCharacteristicType({
              predicate,
              sort: sorter.create(sort),
              include: {
                UnitOfMeasure: x,
              },
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const objects = loaded.collections.SerialisedItemCharacteristicTypes as SerialisedItemCharacteristicType[];
        this.table.total = loaded.values.SerialisedItemCharacteristicTypes_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            name: `${v.Name}`,
            uom: v.UnitOfMeasure ? v.UnitOfMeasure.Name : '',
            active: v.IsActive,
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
