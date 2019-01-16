import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';
import * as moment from 'moment';

import { AllorsFilterService, ErrorService, ContextService, NavigationService, MediaService, MetaService, RefreshService, Action } from '../../../../../angular';
import { SalesInvoice } from '../../../../../domain';
import { And, Like, PullRequest, Sort } from '../../../../../framework';
import { PrintService, Sorter, Table, TableRow, DeleteService, OverviewService, StateService } from '../../../../../material';

interface Row extends TableRow {
  object: SalesInvoice;
  number: string;
  billedTo: string;
  reference: string;
  description: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './salesinvoice-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class SalesInvoiceListComponent implements OnInit, OnDestroy {

  public title = 'Sales Invoices';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
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

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number' },
        { name: 'billedTo' },
        { name: 'reference' },
        { name: 'description' },
        'lastModifiedDate'
      ],
      actions: [
        overviewService.overview(),
        this.printService.print(),
        this.delete
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.SalesInvoice.InvoiceNumber, parameter: 'number' }),
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        number: m.SalesInvoice.InvoiceNumber,
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
        switchMap(([, filterFields, sort, pageEvent]) => {

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
        this.table.total = loaded.values.SalesOrders_total;
        this.table.data = salesInvoices.map((v) => {
          return {
            object: v,
            number: v.InvoiceNumber,
            billedTo: v.BillToCustomer.displayName,
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
