import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Equals, Like } from '../../../../../framework';
import { MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, TestScope, FilterDefinition,  Filter } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, EditService } from '../../../..';

import { PositionType } from '../../../../../domain';

interface Row extends TableRow {
  object: PositionType;
  title: string;
  description: string;
}

@Component({
  templateUrl: './positiontype-list.component.html',
  providers: [ContextService]
})
export class PositionTypesOverviewComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Position Types';

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
        { name: 'title', sort: true },
        { name: 'description' },
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
      new Like({ roleType: m.PositionType.Title, parameter: 'title' }),
    ]);

    const filterDefinition = new FilterDefinition(predicate);
    this.filter = new Filter(filterDefinition);
    
    const sorter = new Sorter(
      {
        title: m.PositionType.Title,
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
            pull.PositionType({
              predicate,
              sort: sorter.create(sort),
              include: {
                PositionTypeRate: x,
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

        const objects = loaded.collections.PositionTypes as PositionType[];
        this.table.total = loaded.values.PositionTypes_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            title: v.Title,
            description: v.Description,
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
