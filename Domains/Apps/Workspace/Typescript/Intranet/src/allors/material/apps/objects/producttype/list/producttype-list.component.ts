import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Equals, Like } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, StateService, EditService } from '../../../..';

import { ProductType } from '../../../../../domain';

interface Row extends TableRow {
  object: ProductType;
  name: string;
}

@Component({
  templateUrl: './producttype-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class ProductTypesOverviewComponent implements OnInit, OnDestroy {

  public title = 'Product Types';

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
      ],
      actions: [
        this.edit,
        this.delete
      ],
      defaultAction: this.edit,
      pageSize: 50,
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.ProductType.Name, parameter: 'name' }),
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        name: m.ProductType.Name,
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
        }, [, , ,]),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.ProductType({
              predicate,
              sort: sorter.create(sort),
              include: {
                SerialisedItemCharacteristicTypes: x,
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

        const objects = loaded.collections.ProductTypes as ProductType[];
        this.table.total = loaded.values.ProductTypes_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            name: `${v.Name}`
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
