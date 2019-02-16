import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Meta } from '../../../../../meta';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Equals, Filter, ContainedIn } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, StateService, PrintService } from '../../../..';

import { SalesOrder, Party, SalesOrderState, SerialisedItem, Product } from '../../../../../domain';
import { MethodService } from '../../../../../material/base/services/actions';

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
export class SalesOrderListComponent implements OnInit, OnDestroy {

  public title = 'Sales Orders';

  m: Meta;

  table: Table<Row>;

  delete: Action;
  print: Action;
  ship: Action;
  invoice: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public printService: PrintService,
    public methodService: MethodService,
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

    this.m = this.metaService.m;

    this.print = printService.print();
    this.ship = methodService.create(allors.context, this.m.SalesOrder.Ship, { name: 'Ship' });
    this.invoice = methodService.create(allors.context, this.m.SalesOrder.Invoice, { name: 'Invoice' });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort: true },
        { name: 'shipToCustomer' },
        { name: 'state' },
        { name: 'customerReference', sort: true },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [
        overviewService.overview(),
        this.print,
        this.delete,
        this.ship,
        this.invoice
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.SalesOrder.TakenBy });
    const predicate = new And([
      internalOrganisationPredicate,
      new Equals({ propertyType: m.SalesOrder.OrderNumber, parameter: 'number' }),
      new Equals({ propertyType: m.SalesOrder.CustomerReference, parameter: 'customerReference' }),
      new Equals({ propertyType: m.SalesOrder.SalesOrderState, parameter: 'state' }),
      new Equals({ propertyType: m.SalesOrder.ShipToCustomer, parameter: 'shipTo' }),
      new Equals({ propertyType: m.SalesOrder.BillToCustomer, parameter: 'billTo' }),
      new Equals({ propertyType: m.SalesOrder.ShipToEndCustomer, parameter: 'shipToEndCustomer' }),
      new Equals({ propertyType: m.SalesOrder.BillToEndCustomer, parameter: 'billToEndCustomer' }),
      new ContainedIn({
        propertyType: m.SalesOrder.SalesOrderItems,
        extent: new Filter({
          objectType: m.SalesOrderItem,
          predicate: new ContainedIn({
            propertyType: m.SalesOrderItem.Product,
            parameter: 'product'
          })
        })
      }),
      new ContainedIn({
        propertyType: m.SalesOrder.SalesOrderItems,
        extent: new Filter({
          objectType: m.SalesOrderItem,
          predicate: new ContainedIn({
            propertyType: m.SalesOrderItem.SerialisedItem,
            parameter: 'serialisedItem'
          })
        })
      })
    ]);

    const stateSearch = new SearchFactory({
      objectType: m.SalesOrderState,
      roleTypes: [m.SalesOrderState.Name],
    });

    const partySearch = new SearchFactory({
      objectType: m.Party,
      roleTypes: [m.Party.PartyName],
    });

    const productSearch = new SearchFactory({
      objectType: m.Product,
      roleTypes: [m.Product.Name],
    });

    const serialisedItemSearch = new SearchFactory({
      objectType: m.SerialisedItem,
      roleTypes: [m.SerialisedItem.ItemNumber],
    });

    this.filterService.init(predicate, {
      active: { initialValue: true },
      state: { search: stateSearch, display: (v: SalesOrderState) => v.Name },
      shipTo: { search: partySearch, display: (v: Party) => v.PartyName },
      billTo: { search: partySearch, display: (v: Party) => v.PartyName },
      shipToEndCustomer: { search: partySearch, display: (v: Party) => v.PartyName },
      billToEndCustomer: { search: partySearch, display: (v: Party) => v.PartyName },
      product: { search: productSearch, display: (v: Product) => v.Name },
      serialisedItem: { search: serialisedItemSearch, display: (v: SerialisedItem) => v.ItemNumber },
    });

    const sorter = new Sorter(
      {
        number: m.SalesOrder.OrderNumber,
        customerReference: m.SalesOrder.CustomerReference,
        lastModifiedDate: m.SalesOrder.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
            internalOrganisationId
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            pull.SalesOrder({
              predicate,
              sort: sorter.create(sort),
              include: {
                PrintDocument: x,
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
