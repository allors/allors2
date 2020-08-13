import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { Carrier } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, EditService, DeleteService, Sorter } from '@allors/angular/material/core';
import { And, Like } from '@allors/data/system';
import { TestScope, Filter, FilterDefinition } from '@allors/angular/core';
import { Action } from 'rxjs/internal/scheduler/Action';

interface Row extends TableRow {
  object: Carrier;
  name: string;
}

@Component({
  templateUrl: './carrier-list.component.html',
  providers: [ContextService]
})
export class CarrierListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Carriers';

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
    this.edit.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
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

    const { m, pull } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.Carrier.Name, parameter: 'name' }),
    ]);

    const filterDefinition = new FilterDefinition(predicate);
    this.filter = new Filter(filterDefinition);
    
    const sorter = new Sorter(
      {
        name: m.Carrier.Name,
      }
    );

    this.subscription = combineLatest([this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$])
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
            pull.Carrier({
              predicate,
              sort: sorter.create(sort),
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const objects = loaded.collections.Carriers as Carrier[];
        this.table.total = loaded.values.Carriers_total;
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
