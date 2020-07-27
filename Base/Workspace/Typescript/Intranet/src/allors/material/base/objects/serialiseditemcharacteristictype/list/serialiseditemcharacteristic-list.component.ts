import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Equals, Like } from '../../../../../framework';
import { MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, TestScope, FilterBuilder } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, EditService } from '../../../..';

import { SerialisedItemCharacteristicType, UnitOfMeasure, IUnitOfMeasure } from '../../../../../domain';

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
  filterBuilder: FilterBuilder;

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

    this.filterBuilder = new FilterBuilder(predicate,
      {
        active: { initialValue: true },
        uom: { search: uomSearch, display: (v: IUnitOfMeasure) => v && v.Name },
      });

    const sorter = new Sorter(
      {
        name: m.SerialisedItemCharacteristicType.Name,
        uom: m.UnitOfMeasure.Name,
        active: m.SerialisedItemCharacteristicType.IsActive
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterBuilder.filterFields$, this.table.sort$, this.table.pager$)
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
              parameters: this.filterBuilder.parameters(filterFields),
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
