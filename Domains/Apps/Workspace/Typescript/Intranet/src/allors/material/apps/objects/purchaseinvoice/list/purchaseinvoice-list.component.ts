import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Meta } from '../../../../../meta';
import { combineLatest, Subscription } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';
import * as moment from 'moment';

import { AllorsFilterService, ContextService, MediaService, MetaService, RefreshService, Action, NavigationService, InternalOrganisationId, TestScope } from '../../../../../angular';
import { PurchaseInvoice } from '../../../../../domain';
import { And, Like, PullRequest, Equals } from '../../../../../framework';
import { OverviewService, Sorter, TableRow, Table, DeleteService, PrintService } from '../../../../../material';
import { MethodService } from '../../../../../material/base/services/actions';

interface Row extends TableRow {
  object: PurchaseInvoice;
  number: string;
  billedFrom: string;
  status: string;
  reference: string;
  lastModifiedDate: string;
}
@Component({
  templateUrl: './purchaseinvoice-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class PurchaseInvoiceListComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  public title = 'Purchase Invoices';

  table: Table<Row>;

  delete: Action;
  approve: Action;
  cancel: Action;
  finish: Action;
  createSalesInvoice: Action;
  print: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public methodService: MethodService,
    public printService: PrintService,
    public deleteService: DeleteService,
    public overviewService: OverviewService,
    public mediaService: MediaService,
    private internalOrganisationId: InternalOrganisationId,
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.m = this.metaService.m;

    this.approve = methodService.create(allors.context, this.m.PurchaseInvoice.Approve, { name: 'Approve' });
    this.cancel = methodService.create(allors.context, this.m.PurchaseInvoice.CancelInvoice, { name: 'Cancel' });
    this.finish = methodService.create(allors.context, this.m.PurchaseInvoice.Finish, { name: 'Finish' });
    this.createSalesInvoice = methodService.create(allors.context, this.m.PurchaseInvoice.CreateSalesInvoice, { name: 'Create Sales Invoice' });
    this.print = printService.print();
    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort: true },
        { name: 'billedFrom' },
        { name: 'state' },
        { name: 'reference', sort: true },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [
        overviewService.overview(),
        this.delete,
        this.approve,
        this.cancel,
        this.finish,
        this.createSalesInvoice,
        this.print
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.PurchaseInvoice.BilledTo });
    const predicate = new And([
      new Like({ roleType: m.PurchaseInvoice.InvoiceNumber, parameter: 'number' }),
      internalOrganisationPredicate
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        number: m.PurchaseInvoice.InvoiceNumber,
        reference: m.PurchaseInvoice.CustomerReference,
        lastModifiedDate: m.PurchaseInvoice.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$, this.internalOrganisationId.observable$)
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
            pull.PurchaseInvoice({
              predicate,
              sort: sorter.create(sort),
              include: {
                BilledFrom: x,
                BilledTo: x,
                PurchaseInvoiceState: x,
                PrintDocument: {
                  Media: x
                },
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
        const purchaseInvoices = loaded.collections.PurchaseInvoices as PurchaseInvoice[];
        this.table.total = loaded.values.PurchaseInvoices_total;
        this.table.data = purchaseInvoices.map((v) => {
          return {
            object: v,
            number: v.InvoiceNumber,
            billedFrom: v.BilledFrom.displayName,
            status: `${v.PurchaseInvoiceState && v.PurchaseInvoiceState.Name}`,
            reference: `${v.CustomerReference} - ${v.PurchaseInvoiceState.Name}`,
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
