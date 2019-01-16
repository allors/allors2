import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Location } from '@angular/common';
import { PageEvent, MatSnackBar } from '@angular/material';

import { BehaviorSubject, combineLatest, Subscription } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';
import * as moment from 'moment';

import { AllorsFilterService, ErrorService, ContextService, MediaService, MetaService, RefreshService, Action } from '../../../../../angular';
import { PurchaseInvoice } from '../../../../../domain';
import { And, Like, PullRequest, Sort } from '../../../../../framework';
import { OverviewService, AllorsMaterialDialogService, Sorter, TableRow, Table, DeleteService, PrintService, StateService } from '../../../../../material';

interface Row extends TableRow {
  object: PurchaseInvoice;
  number: string;
  billedFrom: string;
  billedTo: string;
  reference: string;
  lastModifiedDate: string;
}
@Component({
  templateUrl: './purchaseinvoice-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class PurchaseInvoiceListComponent implements OnInit, OnDestroy {

  public title = 'Purchase Invoices';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public printService: PrintService,
    public deleteService: DeleteService,
    public overviewService: OverviewService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private location: Location,
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
        { name: 'billedFrom' },
        { name: 'billedTo' },
        { name: 'reference' },
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
      new Like({ roleType: m.PurchaseInvoice.InvoiceNumber, parameter: 'number' }),
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        number: m.PurchaseInvoice.InvoiceNumber,
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
            pull.PurchaseInvoice({
              predicate,
              sort: sorter.create(sort),
              include: {
                BilledFrom: x,
                BilledTo: x,
                PurchaseInvoiceState: x,
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
        this.table.total = loaded.values.SalesOrders_total;
        this.table.data = purchaseInvoices.map((v) => {
          return {
            object: v,
            number: v.InvoiceNumber,
            billedFrom: v.BilledFrom.displayName,
            billedTo: v.BilledTo.displayName,
            reference: `${v.CustomerReference} - ${v.PurchaseInvoiceState.Name}`,
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
