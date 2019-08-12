import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Meta } from '../../../../../meta';
import { combineLatest, Subscription } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';
import * as moment from 'moment';

import { AllorsFilterService, ContextService, MediaService, MetaService, RefreshService, Action, NavigationService, InternalOrganisationId, TestScope, SearchFactory, ActionTarget } from '../../../../../angular';
import { PurchaseInvoice, PurchaseInvoiceType, PaymentApplication, Disbursement, Receipt } from '../../../../../domain';
import { And, Like, PullRequest, Equals } from '../../../../../framework';
import { OverviewService, Sorter, TableRow, Table, DeleteService, PrintService, AllorsMaterialDialogService } from '../../../../../material';
import { MethodService } from '../../../../../material/core/services/actions';
import { ɵangular_packages_forms_forms_x } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

interface Row extends TableRow {
  object: PurchaseInvoice;
  number: string;
  type: string;
  billedFrom: string;
  state: string;
  reference: string;
  dueDate: string;
  totalExVat: number;
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
  reopen: Action;
  reject: Action;
  createSalesInvoice: Action;
  print: Action;
  setPaid: Action;

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
    public dialogService: AllorsMaterialDialogService,
    public snackBar: MatSnackBar,
    private internalOrganisationId: InternalOrganisationId,
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.m = this.metaService.m;

    this.approve = methodService.create(allors.context, this.m.PurchaseInvoice.Approve, { name: 'Approve' });
    this.reject = methodService.create(allors.context, this.m.PurchaseInvoice.Reject, { name: 'Reject' });
    this.cancel = methodService.create(allors.context, this.m.PurchaseInvoice.Cancel, { name: 'Cancel' });
    this.reopen = methodService.create(allors.context, this.m.PurchaseInvoice.Reopen, { name: 'Reopen' });
    this.createSalesInvoice = methodService.create(allors.context, this.m.PurchaseInvoice.CreateSalesInvoice, { name: 'Create Sales Invoice' });
    this.print = printService.print();

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
          const anyDisabled = (target as PurchaseInvoice[]).filter(v => !v.CanExecuteSetPaid);
          return target.length > 0 ? anyDisabled.length > 0 : true;
        } else {
          return !(target as PurchaseInvoice).CanExecuteSetPaid;
        }
      },
      execute: (target: PurchaseInvoice) => {

        const invoices = Array.isArray(target) ? target as PurchaseInvoice[] : [target as PurchaseInvoice];
        const targets = invoices.filter((v) => v.CanExecuteSetPaid);

        if (targets.length > 0) {
          dialogService
            .prompt({ title: `Set Payment Date`, placeholder: `Payment date`, promptType: `date` })
            .subscribe((paymentDate: string) => {
              if (paymentDate) {
                targets.forEach((purchaseInvoice) => {
                  const amountToPay = purchaseInvoice.TotalIncVat - purchaseInvoice.AmountPaid;

                  if (purchaseInvoice.PurchaseInvoiceType.UniqueId === 'd08f0309a4cb4ab78f753bb11dcf3783' ||
                    purchaseInvoice.PurchaseInvoiceType.UniqueId === '0187d92781f54d6a984758b674ad3e6a') {

                    const paymentApplication = this.allors.context.create('PaymentApplication') as PaymentApplication;
                    paymentApplication.Invoice = purchaseInvoice;
                    paymentApplication.AmountApplied = amountToPay;

                    // purchase invoice
                    if (purchaseInvoice.PurchaseInvoiceType.UniqueId === 'd08f0309a4cb4ab78f753bb11dcf3783') {
                      const disbursement = this.allors.context.create('Disbursement') as Disbursement;
                      disbursement.Amount = amountToPay;
                      disbursement.EffectiveDate = paymentDate;
                      disbursement.Sender = purchaseInvoice.BilledFrom;
                      disbursement.AddPaymentApplication(paymentApplication);
                    }

                    // purchase return
                    if (purchaseInvoice.PurchaseInvoiceType.UniqueId === '0187d92781f54d6a984758b674ad3e6a') {
                      const receipt = this.allors.context.create('Receipt') as Receipt;
                      receipt.Amount = amountToPay;
                      receipt.EffectiveDate = paymentDate;
                      receipt.Sender = purchaseInvoice.BilledFrom;
                      receipt.AddPaymentApplication(paymentApplication);
                    }
                  }
                });

                this.allors.context.save()
                  .subscribe(() => {
                    snackBar.open('Successfully set to fully paid.', 'close', { duration: 5000 });
                    refreshService.refresh();
                  });
              }
            });
        }
      },
      result: null
    };

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort: true },
        { name: 'type', sort: true },
        { name: 'billedFrom' },
        { name: 'state' },
        { name: 'reference', sort: true },
        { name: 'dueDate', sort: true },
        { name: 'totalExVat', sort: true },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [
        overviewService.overview(),
        this.delete,
        this.approve,
        this.cancel,
        this.reopen,
        this.reject,
        this.createSalesInvoice,
        this.print,
        this.setPaid
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'number'
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.PurchaseInvoice.BilledTo });
    const predicate = new And([
      new Like({ roleType: m.PurchaseInvoice.InvoiceNumber, parameter: 'number' }),
      new Equals({ propertyType: m.PurchaseInvoice.PurchaseInvoiceType, parameter: 'type' }),
      internalOrganisationPredicate
    ]);

    const typeSearch = new SearchFactory({
      objectType: m.PurchaseInvoiceType,
      roleTypes: [m.PurchaseInvoiceType.Name],
    });

    this.filterService.init(predicate,
      {
        type: { search: typeSearch, display: (v: PurchaseInvoiceType) => v && v.Name },
      });

    const sorter = new Sorter(
      {
        number: m.PurchaseInvoice.InvoiceNumber,
        type: m.PurchaseInvoice.PurchaseInvoiceType,
        reference: m.PurchaseInvoice.CustomerReference,
        dueDate: m.PurchaseInvoice.DueDate,
        totalExVat: m.PurchaseInvoice.TotalExVat,
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
                PurchaseInvoiceType: x,
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
            type: `${v.PurchaseInvoiceType && v.PurchaseInvoiceType.Name}`,
            billedFrom: v.BilledFrom.displayName,
            state: `${v.PurchaseInvoiceState && v.PurchaseInvoiceState.Name}`,
            reference: `${v.CustomerReference} - ${v.PurchaseInvoiceState.Name}`,
            dueDate: v.DueDate && moment(v.DueDate).format('MMM Do YY'),
            totalExVat: v.TotalExVat,
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
