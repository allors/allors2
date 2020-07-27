import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';

import { Organisation } from '../../../../domain';
import { PullRequest, And, Like } from '../../../../framework';
import { ContextService, NavigationService, RefreshService, Action, MetaService, TestScope, FilterBuilder } from '../../../../angular';
import { Table, TableRow } from '../../../../material';

import { DeleteService, OverviewService } from '../../../../material';

interface Row extends TableRow {
  object: Organisation;
  name: string | null;
  owner: string | null;
}

@Component({
  templateUrl: './organisations.component.html',
  providers: [ContextService],
})
export class OrganisationsComponent extends TestScope implements OnInit, OnDestroy {
  title = 'Organisations';

  table: Table<Row>;

  overview: Action;
  delete: Action;

  filterBuilder: FilterBuilder;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public deleteService: DeleteService,
    public overviewService: OverviewService,
    private titleService: Title,
  ) {
    super();

    this.titleService.setTitle(this.title);

    this.overview = overviewService.overview();
    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [{ name: 'name', sort: true }, 'owner'],
      actions: [this.overview, this.delete],
      defaultAction: this.overview,
    });
  }

  public ngOnInit(): void {
    const { x, m, pull } = this.metaService;
    this.filterBuilder = m.Organisation.filterBuilder;

    this.subscription = combineLatest([this.refreshService.refresh$, this.filterBuilder.filterFields$, this.table.sort$, this.table.pager$])
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => [
          refresh,
          filterFields,
          sort,
          previousRefresh !== refresh || filterFields !== previousFilterFields ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
        ]),
        switchMap(([, filterFields, sort, pageEvent]) => {
          const pulls = [
            pull.Organisation({
              predicate: this.filterBuilder.predicate,
              sort: sort ? m.Organisation.sorter.create(sort) : null,
              include: {
                Owner: x,
                Employees: x,
              },
              parameters: this.filterBuilder.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        }),
      )
      .subscribe((loaded) => {
        this.allors.context.reset();
        const organisations = loaded.collections.Organisations as Organisation[];
        this.table.total = loaded.values.Organisations_total;
        this.table.data = organisations.map((v) => {
          return {
            object: v,
            name: v.Name,
            owner: v.Owner?.UserName ?? null,
          };
        });
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
