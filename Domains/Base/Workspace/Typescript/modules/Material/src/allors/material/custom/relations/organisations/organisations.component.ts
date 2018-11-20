import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { PageEvent, MatSnackBar } from '@angular/material';
import { Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';

import { Organisation } from '../../../../domain';
import { PullRequest, And, Like, Sort } from '../../../../framework';
import { SessionService, NavigationService, ActionTarget, AllorsFilterService, ErrorService, AllorsRefreshService } from '../../../../angular';
import { Table, Sorter } from '../../../../material';
import { DeleteService } from 'src/allors/material/base/actions/delete/delete.service';

interface Row extends ActionTarget {
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

  total: number;
  table: Table<Row>;

  private sort$: BehaviorSubject<Sort>;
  private pager$: BehaviorSubject<PageEvent>;
  private subscription: Subscription;

  constructor(
    @Self() public allors: SessionService,
    @Self() private filterService: AllorsFilterService,
    public refreshService: AllorsRefreshService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private titleService: Title) {

    this.sort$ = new BehaviorSubject<Sort>(undefined);
    this.pager$ = new BehaviorSubject<PageEvent>(Object.assign(new PageEvent(), { pageIndex: 0, pageSize: 50 }));

    this.title = 'Organisations';
    this.titleService.setTitle(this.title);

    this.table = new Table({
      selection: true,
      columns: ['name', 'owner'],
      actions: [
        {
          name: () => 'Details',
          handler: (target: ActionTarget) => {
            this.navigation.overview(target.object);
          }
        },
        deleteService.action(allors)
      ]
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

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.sort$, this.pager$)
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
        this.total = loaded.values.Organisations_total;
        const organisations = loaded.collections.Organisations as Organisation[];
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
