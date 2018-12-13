import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Equals } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService } from '../../../../../angular';
import { Sorter, TableRow, Table, NavigateService, DeleteService, StateService } from '../../../..';

import { SalesOrder } from '../../../../../domain';

interface Row extends TableRow {
  object: SalesOrder;
  number: string;
  shipToCustomer: string;
  state: string;
  customerReference: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './salesorder-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class SalesOrdersOverviewComponent implements OnInit, OnDestroy {

  public title = 'Sales Orders';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigateService: NavigateService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private stateService: StateService,
    titleService: Title,
  ) {
    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number' },
        { name: 'shipToCustomer' },
        { name: 'state' },
        { name: 'customerReference' },
        'lastModifiedDate'
      ],
      actions: [
        navigateService.overview(),
        this.delete
      ],
      defaultAction: navigateService.overview(),
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.SalesOrder.TakenBy });
    const predicate = new And([
      // new Like({ roleType: m.Person.FirstName, parameter: 'firstName' }),
      internalOrganisationPredicate
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        lastModifiedDate: m.SalesOrder.LastModifiedDate,
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
            pull.SalesOrder({
              predicate,
              sort: sorter.create(sort),
              include: {
                ShipToCustomer: x,
                SalesOrderState: x,
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
        const requests = loaded.collections.SalesOrders as SalesOrder[];
        this.table.total = loaded.values.SalesOrders_total;
        this.table.data = requests.map((v) => {
          return {
            object: v,
            number: `${v.OrderNumber}`,
            shipToCustomer: v.ShipToCustomer && v.ShipToCustomer.displayName,
            state: `${v.SalesOrderState && v.SalesOrderState.Name}`,
            customerReference: `${v.Description || ''}`,
            lastModifiedDate: moment(v.LastModifiedDate).fromNow()
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
