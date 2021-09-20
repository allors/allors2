import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';

import { TableRow, Table } from '@allors/angular/material/core';
import { Organisation } from '@allors/domain/generated';
import {  TestScope, Filter, Action } from '@allors/angular/core';
import { DeleteService, OverviewService } from '@allors/angular/material/core';
import { PullRequest } from '@allors/protocol/system';
import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';

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

  filter: Filter;

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
    this.delete.result.subscribe(() => {
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

    this.filter = m.Organisation.filter = m.Organisation.filter ?? new Filter(m.Organisation.filterDefinition);

    this.subscription = combineLatest([this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$])
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
              predicate: this.filter.definition.predicate,
              sort: sort ? m.Organisation.sorter.create(sort) : null,
              include: {
                Owner: x,
                Employees: x,
              },
              parameters: this.filter.parameters(filterFields),
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
