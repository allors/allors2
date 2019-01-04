import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Equals, Like } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, StateService, EditService } from '../../../..';

import { SerialisedItemCharacteristicType } from '../../../../../domain';

interface Row extends TableRow {
  object: SerialisedItemCharacteristicType;
  name: string;
  uom: string;
  active: boolean;
}

@Component({
  templateUrl: './serialiseditemcharacteristic-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class SerialisedItemCharacteristicListComponent implements OnInit, OnDestroy {

  public title = 'Product Characteristics';

  table: Table<Row>;

  edit: Action;
  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public editService: EditService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private stateService: StateService,
    titleService: Title,
  ) {
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
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        name: m.SerialisedItemCharacteristicType.Name,
        uom: m.UnitOfMeasure.Name,
        active: m.SerialisedItemCharacteristicType.IsActive
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
            pull.SerialisedItemCharacteristicType({
              predicate,
              sort: sorter.create(sort),
              include: {
                UnitOfMeasure: x,
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

        const objects = loaded.collections.SerialisedItemCharacteristicTypes as SerialisedItemCharacteristicType[];
        this.table.total = loaded.values.SerialisedItemCharacteristicTypes_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            name: `${v.Name}`,
            uom: `${v.UnitOfMeasure && v.UnitOfMeasure.Name}`,
            active: v.IsActive,
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
