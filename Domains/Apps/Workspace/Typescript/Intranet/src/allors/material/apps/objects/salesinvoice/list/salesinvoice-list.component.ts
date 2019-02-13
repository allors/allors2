import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Meta } from '../../../../../meta';
import { Subscription, combineLatest } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';
import * as moment from 'moment';

import { AllorsFilterService, ErrorService, ContextService, NavigationService, MediaService, MetaService, RefreshService, Action } from '../../../../../angular';
import { SalesInvoice } from '../../../../../domain';
import { And, Like, PullRequest, Sort, Equals } from '../../../../../framework';
import { PrintService, Sorter, Table, TableRow, DeleteService, OverviewService, StateService } from '../../../../../material';
import { MethodService } from '../../../../../material/base/services/actions';

interface Row extends TableRow {
  object: SalesInvoice;
  number: string;
  billedTo: string;
  state: string;
  reference: string;
  description: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './salesinvoice-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class SalesInvoiceListComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  public title = 'Sales Invoices';

  table: Table<Row>;

  delete: Action;
  print: Action;
  send: Action;
  cancel: Action;
  writeOff: Action;
  copy: Action;
  credit: Action;
  reopen: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public methodService: MethodService,
    public printService: PrintService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    private stateService: StateService,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.m = this.metaService.m;

    this.print = printService.print();
    this.send = methodService.create(allors.context, this.m.SalesInvoice.Send, { name: 'Send' });
    this.cancel = methodService.create(allors.context, this.m.SalesInvoice.CancelInvoice, { name: 'Cancel' });
    this.writeOff = methodService.create(allors.context, this.m.SalesInvoice.WriteOff, { name: 'WriteOff' });
    this.copy = methodService.create(allors.context, this.m.SalesInvoice.Copy, { name: 'Copy' });
    this.credit = methodService.create(allors.context, this.m.SalesInvoice.Credit, { name: 'Credit' });
    this.reopen = methodService.create(allors.context, this.m.SalesInvoice.Reopen, { name: 'Reopen' });

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort: true },
        { name: 'billedTo' },
        { name: 'reference', sort: true },
        { name: 'state' },
        { name: 'description', sort: true },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [
        overviewService.overview(),
        this.delete,
        this.print,
        this.cancel,
        this.writeOff,
        this.copy,
        this.credit,
        this.reopen,
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.SalesInvoice.BilledFrom });
    const predicate = new And([
      new Like({ roleType: m.SalesInvoice.InvoiceNumber, parameter: 'number' }),
      internalOrganisationPredicate
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        number: m.SalesInvoice.InvoiceNumber,
        reference: m.SalesInvoice.CustomerReference,
        description: m.SalesInvoice.Description,
        lastModifiedDate: m.SalesInvoice.LastModifiedDate,
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
            pull.SalesInvoice({
              predicate,
              sort: sorter.create(sort),
              include: {
                PrintDocument: x,
                BillToCustomer: x,
                SalesInvoiceState: x,
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
        const salesInvoices = loaded.collections.SalesInvoices as SalesInvoice[];
        this.table.total = loaded.values.SalesInvoices_total;
        this.table.data = salesInvoices.map((v) => {
          return {
            object: v,
            number: v.InvoiceNumber,
            billedTo: v.BillToCustomer.displayName,
            state: `${v.SalesInvoiceState && v.SalesInvoiceState.Name}`,
            reference: `${v.CustomerReference} - ${v.SalesInvoiceState.Name}`,
            description: v.Description,
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
