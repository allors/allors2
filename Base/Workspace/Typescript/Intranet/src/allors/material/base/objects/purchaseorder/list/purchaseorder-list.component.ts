import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Meta } from '../../../../../meta';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment/moment';

import { PullRequest, And, Equals, Filter, ContainedIn } from '../../../../../framework';
import { MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, InternalOrganisationId, TestScope, UserId, FetcherService, FilterBuilder } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, PrintService } from '../../../..';

import { PurchaseOrder, Party, PurchaseOrderState, Product, SerialisedItem, Organisation, Person, UserGroup, Part } from '../../../../../domain';
import { MethodService } from '../../../../../material/core/services/actions';

interface Row extends TableRow {
  object: PurchaseOrder;
  number: string;
  supplier: string;
  state: string;
  shipmentState: string;
  customerReference: string;
  invoice: string;
  totalExVat: string;
  totalIncVat: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './purchaseorder-list.component.html',
  providers: [ContextService]
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
  filterBuilder: FilterBuilder;

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

    this.print = printService.print();
    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
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
        { name: 'invoice'},
        { name: 'totalExVat', sort: true },
        { name: 'totalIncVat', sort: true },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [
        overviewService.overview(),
        this.delete,
        this.print,
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'number',
      initialSortDirection: 'desc',
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.PurchaseOrder.OrderedBy });
    const supplierPredicate = new Equals({ propertyType: m.SupplierRelationship.InternalOrganisation });

    const predicate = new And([
      internalOrganisationPredicate,
      new Equals({ propertyType: m.PurchaseOrder.OrderNumber, parameter: 'number' }),
      new Equals({ propertyType: m.PurchaseOrder.CustomerReference, parameter: 'customerReference' }),
      new Equals({ propertyType: m.PurchaseOrder.PurchaseOrderState, parameter: 'state' }),
      new Equals({ propertyType: m.PurchaseOrder.TakenViaSupplier, parameter: 'supplier' }),
      new ContainedIn({
        propertyType: m.PurchaseOrder.PurchaseOrderItems,
        extent: new Filter({
          objectType: m.PurchaseOrderItem,
          predicate: new ContainedIn({
            propertyType: m.PurchaseOrderItem.Part,
            parameter: 'sparePart'
          })
        })
      }),
      new ContainedIn({
        propertyType: m.PurchaseOrder.PurchaseOrderItems,
        extent: new Filter({
          objectType: m.PurchaseOrderItem,
          predicate: new ContainedIn({
            propertyType: m.PurchaseOrderItem.SerialisedItem,
            parameter: 'serialisedItem'
          })
        })
      })
    ]);

    const stateSearch = new SearchFactory({
      objectType: m.PurchaseOrderState,
      roleTypes: [m.PurchaseOrderState.Name],
    });

    const supplierSearch = new SearchFactory({
      objectType: m.Organisation,
      predicates: [
        new ContainedIn({
          propertyType: m.Organisation.SupplierRelationshipsWhereSupplier,
          extent: new Filter({
            objectType: m.SupplierRelationship,
            predicate: supplierPredicate,
          })
        })],
      roleTypes: [m.Organisation.PartyName],
    });

    const partSearch = new SearchFactory({
      objectType: m.NonUnifiedPart,
      roleTypes: [m.NonUnifiedPart.Name, m.NonUnifiedPart.SearchString],
    });

    const serialisedItemSearch = new SearchFactory({
      objectType: m.SerialisedItem,
      roleTypes: [m.SerialisedItem.ItemNumber],
    });

    this.filterBuilder = new FilterBuilder(predicate, {
      active: { initialValue: true },
      state: { search: stateSearch, display: (v: PurchaseOrderState) => v && v.Name },
      supplier: { search: supplierSearch, display: (v: Party) => v && v.PartyName },
      sparePart: { search: partSearch, display: (v: Part) => v && v.Name },
      serialisedItem: { search: serialisedItemSearch, display: (v: SerialisedItem) => v && v.ItemNumber },
    });

    const sorter = new Sorter(
      {
        number: m.PurchaseOrder.SortableOrderNumber,
        customerReference: m.PurchaseOrder.CustomerReference,
        totalExVat: m.PurchaseOrder.TotalExVat,
        totalIncVat: m.PurchaseOrder.TotalIncVat,
        lastModifiedDate: m.PurchaseOrder.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterBuilder.filterFields$, this.table.sort$, this.table.pager$, this.internalOrganisationId.observable$)
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
            pull.PurchaseOrder({
              predicate,
              sort: sorter.create(sort),
              include: {
                PrintDocument: {
                  Media: x
                },
                TakenViaSupplier: x,
                PurchaseOrderState: x,
                PurchaseOrderShipmentState: x,
                PurchaseInvoicesWherePurchaseOrder: x,
              },
              parameters: this.filterBuilder.parameters(filterFields),
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

        this.canCreate = this.internalOrganisation.CanExecuteCreatePurchaseOrder;

        const orders = loaded.collections.PurchaseOrders as PurchaseOrder[];
        this.table.total = loaded.values.PurchaseOrders_total;
        this.table.data = orders.filter(v => v.CanReadOrderNumber).map((v) => {
          return {
            object: v,
            number: `${v.OrderNumber}`,
            supplier: v.TakenViaSupplier && v.TakenViaSupplier.displayName,
            state: `${v.PurchaseOrderState && v.PurchaseOrderState.Name}`,
            shipmentState: `${v.PurchaseOrderShipmentState && v.PurchaseOrderShipmentState.Name}`,
            customerReference: `${v.Description || ''}`,
            invoice: v.PurchaseInvoicesWherePurchaseOrder.map(w => w.InvoiceNumber).join(', '),
            totalExVat: v.TotalExVat,
            totalIncVat: v.TotalIncVat,
            lastModifiedDate: moment(v.LastModifiedDate).fromNow()
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
