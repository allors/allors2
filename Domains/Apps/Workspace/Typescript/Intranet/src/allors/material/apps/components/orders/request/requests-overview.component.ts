import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Like, Equals } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, SessionService, NavigationService, Action, AllorsRefreshService } from '../../../../../angular';
import { Sorter, TableRow, Table, NavigateService, DeleteService, StateService } from '../../../../../material';

import { Request } from '../../../../../domain';

interface Row extends TableRow {
  object: Request;
  originator: string;
  description: string;
  responseRequired: Date;
  lastModifiedDate: Date;
}

@Component({
  templateUrl: './requests-overview.component.html',
  providers: [SessionService, AllorsFilterService]
})
export class RequestsOverviewComponent implements OnInit, OnDestroy {

  public title = 'Requests';

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
    public mediaService: MediaService,
    private errorService: ErrorService,
    private stateService: StateService,
    titleService: Title,
  ) {
    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'originator' },
        { name: 'description' },
        { name: 'responseRequired' },
        'lastModifiedDate'
      ],
      actions: [
        navigateService.overview(),
        this.delete
      ],
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.allors;

    const internalOrganisationPredicate = new Equals({ propertyType: m.Request.Recipient });
    const predicate = new And([
      // new Like({ roleType: m.Person.FirstName, parameter: 'firstName' }),
      internalOrganisationPredicate
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        lastModifiedDate: m.Person.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          internalOrganisationPredicate.value = internalOrganisationId;

          const pulls = [
            pull.Request({
              predicate,
              sort: sorter.create(sort),
              include: {
                Originator: x,
                RequestState: x,
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
        const requests = loaded.collections.Requests as Request[];
        this.table.total = loaded.values.Request_total;
        this.table.data = requests.map((v) => {
          return {
            object: v,
            originator: v.Originator.displayName,
            description: `${v.RequestNumber} ${v.Description} ${v.RequestState.Name}`,
            responseRequired: v.RequiredResponseDate,
            lastModifiedDate: v.LastModifiedDate
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
