import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { formatDistance } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService, UserId } from '@allors/angular/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, MethodService } from '@allors/angular/material/core';
import { Person, Organisation, Party, Part, SerialisedItem, PurchaseOrder, PurchaseOrderState } from '@allors/domain/generated';
import { And, ContainedIn, Extent, Equals } from '@allors/data/system';
import { InternalOrganisationId, FetcherService, PrintService } from '@allors/angular/base';

import { Meta } from '@allors/meta/generated';

interface Row extends TableRow {
  object: PurchaseOrder;
  number: string;
  supplier: string;
  state: string;
  shipmentState: string;
  customerReference: string;
  invoice: string;
  currency: string;
  totalExVat: string;
  totalIncVat: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './purchaseorder-list.component.html',
  providers: [ContextService],
})
export class PurchaseOrderListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Purchase Orders';

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

    this.print = printService.print();
    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.m = this.metaService.m;

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort: true },
        { name: 'supplier' },
        { name: 'state' },
        { name: 'shipmentState' },
        { name: 'customerReference', sort: true },
        { name: 'invoice' },
        { name: 'currency' },
        { name: 'totalExVat', sort: true },
        { name: 'totalIncVat', sort: true },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [overviewService.overview(), this.delete, this.print],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'number',
      initialSortDirection: 'desc',
    });
  }

  ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.PurchaseOrder.filter = m.PurchaseOrder.filter ?? new Filter(m.PurchaseOrder.filterDefinition);

    const internalOrganisationPredicate = new Equals({ propertyType: m.PurchaseOrder.OrderedBy });

    const predicate = new And([internalOrganisationPredicate, this.filter.definition.predicate]);

    this.subscription = combineLatest([
      this.refreshService.refresh$,
      this.filter.fields$,
      this.table.sort$,
      this.table.pager$,
      this.internalOrganisationId.observable$,
    ])
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
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
            pull.PurchaseOrder({
              predicate,
              sort: sort ? m.PurchaseOrder.sorter.create(sort) : null,
              include: {
                PrintDocument: {
                  Media: x,
                },
                TakenViaSupplier: x,
                PurchaseOrderState: x,
                PurchaseOrderShipmentState: x,
                PurchaseInvoicesWherePurchaseOrder: x,
                DerivedCurrency: x,
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

        this.canCreate = this.internalOrganisation.CanExecuteCreatePurchaseOrder;

        const orders = loaded.collections.PurchaseOrders as PurchaseOrder[];
        this.table.total = loaded.values.PurchaseOrders_total;
        this.table.data = orders
          .filter((v) => v.CanReadOrderNumber)
          .map((v) => {
            return {
              object: v,
              number: `${v.OrderNumber}`,
              supplier: v.TakenViaSupplier && v.TakenViaSupplier.displayName,
              state: `${v.PurchaseOrderState && v.PurchaseOrderState.Name}`,
              shipmentState: `${v.PurchaseOrderShipmentState && v.PurchaseOrderShipmentState.Name}`,
              customerReference: `${v.Description || ''}`,
              invoice: v.PurchaseInvoicesWherePurchaseOrder.map((w) => w.InvoiceNumber).join(', '),
              currency: `${v.DerivedCurrency && v.DerivedCurrency.IsoCode}`,
              totalExVat: v.TotalExVat,
              totalIncVat: v.TotalIncVat,
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
