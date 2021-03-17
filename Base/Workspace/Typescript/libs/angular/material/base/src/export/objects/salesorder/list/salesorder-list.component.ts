import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { formatDistance } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService, UserId } from '@allors/angular/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, MethodService } from '@allors/angular/material/core';
import { Person, Organisation, Party, Product, SerialisedItem, SalesOrder, SalesOrderState, SalesInvoiceState, ShipmentState } from '@allors/domain/generated';
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
  providers: [ContextService],
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
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
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
        { name: 'invoice' },
        { name: 'customerReference', sort: true },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [overviewService.overview(), this.print, this.delete, this.ship, this.invoice],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'number',
      initialSortDirection: 'desc',
    });
  }

  ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.SalesOrder.filter = m.SalesOrder.filter ?? new Filter(m.SalesOrder.filterDefinition);

    const internalOrganisationPredicate = new Equals({ propertyType: m.SalesOrder.TakenBy });
    const predicate = new And([
      internalOrganisationPredicate,
      this.filter.definition.predicate
    ]);

    this.subscription = combineLatest([
      this.refreshService.refresh$,
      this.filter.fields$,
      this.table.sort$,
      this.table.pager$,
      this.internalOrganisationId.observable$
    ])
      .pipe(
        scan(
          ([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
            pageEvent =
            previousRefresh !== refresh || filterFields !== previousFilterFields
              ? {
                  ...pageEvent,
                  pageIndex: 0,
                }
              : pageEvent;

          if (pageEvent.pageIndex === 0) {
            this.table.pageIndex = 0;
          }

          return [refresh, filterFields, sort, pageEvent, internalOrganisationId];
        }),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {
          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Person({
              object: this.userId.value,
            }),
            pull.SalesOrder({
              predicate,
              sort: sort ? m.SalesOrder.sorter.create(sort) : null,
              include: {
                PrintDocument: {
                  Media: x,
                },
                ShipToCustomer: x,
                SalesOrderState: x,
                SalesInvoicesWhereSalesOrder: x,
              },
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
          ];

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
        this.table.data = requests
          .filter((v) => v.CanReadOrderNumber)
          .map((v) => {
            return {
              object: v,
              number: `${v.OrderNumber}`,
              shipToCustomer: v.ShipToCustomer && v.ShipToCustomer.displayName,
              state: `${v.SalesOrderState && v.SalesOrderState.Name}`,
              invoice: v.SalesInvoicesWhereSalesOrder.map((w) => w.InvoiceNumber).join(', '),
              customerReference: `${v.Description || ''}`,
              lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date()),
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
