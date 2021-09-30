import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { format, formatDistance } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService, UserId } from '@allors/angular/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action, ActionTarget } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, MethodService } from '@allors/angular/material/core';
import {
  Person,
  Organisation,
  Party,
  SalesInvoice,
  PaymentApplication,
  Receipt,
  Disbursement,
  SalesInvoiceType,
  SalesInvoiceState,
  Product,
  SerialisedItem,
} from '@allors/domain/generated';
import { And, Equals, ContainedIn, Extent } from '@allors/data/system';
import { InternalOrganisationId, FetcherService, PrintService } from '@allors/angular/base';

import { MatSnackBar } from '@angular/material/snack-bar';
import { Meta } from '@allors/meta/generated';
import { AllorsMaterialDialogService } from '@allors/angular/material/services/core';

interface Row extends TableRow {
  object: SalesInvoice;
  number: string;
  type: string;
  billedTo: string;
  state: string;
  invoiceDate: string;
  dueDate: string;
  description: string;
  currency: string
  totalExVat: string;
  grandTotal: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './salesinvoice-list.component.html',
  providers: [ContextService],
})
export class SalesInvoiceListComponent extends TestScope implements OnInit, OnDestroy {
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
  setPaid: Action;

  user: Person;
  internalOrganisation: Organisation;
  canCreate: boolean;

  private subscription: Subscription;
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,

    public metaService: MetaService,
    public methodService: MethodService,
    public printService: PrintService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    public refreshService: RefreshService,
    public dialogService: AllorsMaterialDialogService,
    public snackBar: MatSnackBar,
    private internalOrganisationId: InternalOrganisationId,
    private userId: UserId,
    private fetcher: FetcherService,
    titleService: Title
  ) {
    super();

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
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.setPaid = {
      name: 'setaspaid',
      displayName: () => 'Set as Paid',
      description: () => '',
      disabled: (target: ActionTarget) => {
        if (Array.isArray(target)) {
          const anyDisabled = (target as SalesInvoice[]).filter((v) => !v.CanExecuteSetPaid);
          return target.length > 0 ? anyDisabled.length > 0 : true;
        } else {
          return !(target as SalesInvoice).CanExecuteSetPaid;
        }
      },
      execute: (target: SalesInvoice) => {
        const invoices = Array.isArray(target) ? (target as SalesInvoice[]) : [target as SalesInvoice];
        const targets = invoices.filter((v) => v.CanExecuteSetPaid);

        if (targets.length > 0) {
          dialogService
            .prompt({ title: `Set Payment Date`, placeholder: `Payment date`, promptType: `date` })
            .subscribe((paymentDate: string) => {
              if (paymentDate) {
                targets.forEach((salesinvoice) => {
                  const amountToPay = parseFloat(salesinvoice.TotalIncVat) - parseFloat(salesinvoice.AmountPaid);

                  if (
                    salesinvoice.SalesInvoiceType.UniqueId === '92411bf1-835e-41f8-80af-6611efce5b32' ||
                    salesinvoice.SalesInvoiceType.UniqueId === 'ef5b7c52-e782-416d-b46f-89c8c7a5c24d'
                  ) {
                    const paymentApplication = this.allors.context.create('PaymentApplication') as PaymentApplication;
                    paymentApplication.Invoice = salesinvoice;
                    paymentApplication.AmountApplied = amountToPay.toString();

                    // sales invoice
                    if (salesinvoice.SalesInvoiceType.UniqueId === '92411bf1-835e-41f8-80af-6611efce5b32') {
                      const receipt = this.allors.context.create('Receipt') as Receipt;
                      receipt.Amount = amountToPay.toString();
                      receipt.EffectiveDate = paymentDate;
                      receipt.Sender = salesinvoice.BilledFrom;
                      receipt.AddPaymentApplication(paymentApplication);
                    }

                    // credit note
                    if (salesinvoice.SalesInvoiceType.UniqueId === 'ef5b7c52-e782-416d-b46f-89c8c7a5c24d') {
                      const disbursement = this.allors.context.create('Disbursement') as Disbursement;
                      disbursement.Amount = amountToPay.toString();
                      disbursement.EffectiveDate = paymentDate;
                      disbursement.Sender = salesinvoice.BilledFrom;
                      disbursement.AddPaymentApplication(paymentApplication);
                    }
                  }
                });

                this.allors.context.save().subscribe(() => {
                  snackBar.open('Successfully set to fully paid.', 'close', { duration: 5000 });
                  refreshService.refresh();
                });
              }
            });
        }
      },
      result: null,
    };

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort: true },
        { name: 'type', sort: true },
        { name: 'billedTo' },
        { name: 'invoiceDate', sort: true },
        { name: 'dueDate', sort: true },
        { name: 'state' },
        { name: 'description', sort: true },
        { name: 'currency' },
        { name: 'totalExVat', sort: true },
        { name: 'grandTotal', sort: true },
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
        this.setPaid,
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'number',
      initialSortDirection: 'desc',
    });
  }
  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.SalesInvoice.filter = m.SalesInvoice.filter ?? new Filter(m.SalesInvoice.filterDefinition);

    const internalOrganisationPredicate = new Equals({ propertyType: m.SalesInvoice.BilledFrom });
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
            pull.SalesInvoice({
              predicate,
              sort: sort ? m.SalesInvoice.sorter.create(sort) : null,
              include: {
                PrintDocument: {
                  Media: x,
                },
                BillToCustomer: x,
                SalesInvoiceState: x,
                SalesInvoiceType: x,
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

        this.canCreate = this.internalOrganisation.CanExecuteCreateSalesInvoice;

        const salesInvoices = loaded.collections.SalesInvoices as SalesInvoice[];
        this.table.total = loaded.values.SalesInvoices_total;
        this.table.data = salesInvoices
          .filter((v) => v.CanReadInvoiceNumber)
          .map((v) => {
            return {
              object: v,
              number: v.InvoiceNumber,
              type: `${v.SalesInvoiceType && v.SalesInvoiceType.Name}`,
              billedTo: v.BillToCustomer && v.BillToCustomer.displayName,
              state: `${v.SalesInvoiceState && v.SalesInvoiceState.Name}`,
              invoiceDate: format(new Date(v.InvoiceDate), 'dd-MM-yyyy'),
              dueDate: `${v.DueDate && format(new Date(v.DueDate), 'dd-MM-yyyy')}`,
              description: v.Description,
              currency: `${v.DerivedCurrency && v.DerivedCurrency.IsoCode}`,
              totalExVat: v.TotalExVat,
              grandTotal: v.GrandTotal,
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
