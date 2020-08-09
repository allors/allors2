import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { format, formatDistance } from 'date-fns';

import {
  ContextService,
  TestScope,
  MetaService,
  RefreshService,
  Action,
  NavigationService,
  MediaService,
  Filter,
  FilterDefinition,
  SearchFactory,
  UserId,
  ActionTarget,
} from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, MethodService, AllorsMaterialDialogService } from '@allors/angular/material/core';
import { Person, Organisation, Quote, QuoteState, Party, SalesInvoice, PaymentApplication, Receipt, Disbursement, SalesInvoiceType, SalesInvoiceState, Product, SerialisedItem, SalesOrder, SalesOrderState } from '@allors/domain/generated';
import { And, Equals, ContainedIn, Extent } from '@allors/data/system';
import { InternalOrganisationId, FetcherService, PrintService } from '@allors/angular/base';
import { Meta } from '@allors/meta/generated';



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
  providers: [ContextService]
})
export class SalesOrderListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Sales Orders';

  m: Meta;

  table: Table<Row>;

  delete: Action;
  print: Action;
  ship: Action;
  invoice: Action;

  user: Person;
  internalOrganisation: Organisation;
  canCreate: boolean;

  private subscription: Subscription;
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,
    
    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public printService: PrintService,
    public methodService: MethodService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private internalOrganisationId: InternalOrganisationId,
    private userId: UserId,
    private fetcher: FetcherService,
    titleService: Title,
  ) {
    super();

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
      initialSort: 'number',
      initialSortDirection: 'desc',
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
        extent: new Extent({
          objectType: m.SalesOrderItem,
          predicate: new ContainedIn({
            propertyType: m.SalesOrderItem.Product,
            parameter: 'product'
          })
        })
      }),
      new ContainedIn({
        propertyType: m.SalesOrder.SalesOrderItems,
        extent: new Extent({
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

    const filterDefinition = new FilterDefinition(predicate, {
      active: { initialValue: true },
      state: { search: stateSearch, display: (v: SalesOrderState) => v && v.Name },
      shipTo: { search: partySearch, display: (v: Party) => v && v.PartyName },
      billTo: { search: partySearch, display: (v: Party) => v && v.PartyName },
      shipToEndCustomer: { search: partySearch, display: (v: Party) => v && v.PartyName },
      billToEndCustomer: { search: partySearch, display: (v: Party) => v && v.PartyName },
      product: { search: productSearch, display: (v: Product) => v && v.Name },
      serialisedItem: { search: serialisedItemSearch, display: (v: SerialisedItem) => v && v.ItemNumber },
    });
    this.filter = new Filter(filterDefinition);
    
    const sorter = new Sorter(
      {
        number: m.SalesOrder.SortableOrderNumber,
        customerReference: m.SalesOrder.CustomerReference,
        lastModifiedDate: m.SalesOrder.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$, this.internalOrganisationId.observable$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
            internalOrganisationId
          ];
        }, [, , , , ,]),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Person({
              object: this.userId.value
            }),
            pull.SalesOrder({
              predicate,
              sort: sorter.create(sort),
              include: {
                PrintDocument: {
                  Media: x
                },
                ShipToCustomer: x,
                SalesOrderState: x,
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

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.user = loaded.objects.Person as Person;

        this.canCreate = this.internalOrganisation.CanExecuteCreateSalesOrder;

        const requests = loaded.collections.SalesOrders as SalesOrder[];
        this.table.total = loaded.values.SalesOrders_total;
        this.table.data = requests.filter(v => v.CanReadOrderNumber).map((v) => {
          return {
            object: v,
            number: `${v.OrderNumber}`,
            shipToCustomer: v.ShipToCustomer && v.ShipToCustomer.displayName,
            state: `${v.SalesOrderState && v.SalesOrderState.Name}`,
            customerReference: `${v.Description || ''}`,
            lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date())
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
