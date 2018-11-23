import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';

import { Organisation } from '../../../../domain';
import { PullRequest, And, Like } from '../../../../framework';
import { SessionService, NavigationService, AllorsFilterService, ErrorService, AllorsRefreshService, Action } from '../../../../angular';
import { Table, TableRow, Sorter } from '../../../../material';

import { NavigateService, DeleteService } from '../../../../material';

interface Row extends TableRow {
  object: Organisation;
  name: string;
  owner: string;
}

@Component({
  templateUrl: './organisations.component.html',
  providers: [SessionService, AllorsFilterService]
})
export class OrganisationsComponent implements OnInit, OnDestroy {

  title: string;

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: SessionService,
    @Self() private filterService: AllorsFilterService,
    public refreshService: AllorsRefreshService,
    public navigateService: NavigateService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private titleService: Title) {

    this.title = 'Organisations';
    this.titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        'owner'
      ],
      actions: [
        navigateService.overview(),
        this.delete
      ],
    });
  }

  public ngOnInit(): void {

    const { x, m, pull } = this.allors;

    const predicate = new And([
      new Like({ roleType: m.Organisation.Name, parameter: 'name' }),
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        name: m.Organisation.Name,
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
        switchMap(([refresh, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.Organisation({
              predicate,
              sort: sorter.create(sort),
              include: {
                Owner: x,
                Employees: x,
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();
        const organisations = loaded.collections.Organisations as Organisation[];

        this.table.total = loaded.values.Organisations_total;
        this.table.data = organisations.map((v) => {
          return {
            object: v,
            name: v.Name,
            owner: v.Owner && v.Owner.UserName
          };
        });

      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
