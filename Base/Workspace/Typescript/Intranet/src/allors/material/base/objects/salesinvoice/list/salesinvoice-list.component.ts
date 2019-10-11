import { Component, OnDestroy, Self, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Meta } from '../../../../../meta';
import { Subscription, combineLatest, Subject } from 'rxjs';
import { scan, switchMap } from 'rxjs/operators';
import * as moment from 'moment';

import { AllorsFilterService, ContextService, NavigationService, MediaService, MetaService, RefreshService, Action, SearchFactory, InternalOrganisationId, TestScope, Invoked, ActionTarget, UserId, FetcherService } from '../../../../../angular';
import { SalesInvoice, SalesInvoiceState, Party, Product, SerialisedItem, SalesInvoiceType, PaymentApplication, Disbursement, Receipt, User, Organisation, Person, UserGroup } from '../../../../../domain';
import { And, Like, PullRequest, Sort, Equals, ContainedIn, Filter } from '../../../../../framework';
import { PrintService, Sorter, Table, TableRow, DeleteService, OverviewService, AllorsMaterialDialogService } from '../../../../../material';
import { MethodService } from '../../../../../material/core/services/actions';
import { MatSnackBar } from '@angular/material/snack-bar';

interface Row extends TableRow {
  object: SalesInvoice;
  number: string;
  type: string;
  billedTo: string;
  state: string;
  reference: string;
  description: string;
  totalExVat: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './salesinvoice-list.component.html',
  providers: [ContextService, AllorsFilterService]
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
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.setPaid = {
      name: 'setaspaid',
      displayName: () => 'Set as Paid',
      description: () => '',
      disabled: (target: ActionTarget) => {
        if (Array.isArray(target)) {
          const anyDisabled = (target as SalesInvoice[]).filter(v => !v.CanExecuteSetPaid);
          return target.length > 0 ? anyDisabled.length > 0 : true;
        } else {
          return !(target as SalesInvoice).CanExecuteSetPaid;
        }
      },
      execute: (target: SalesInvoice) => {

        const invoices = Array.isArray(target) ? target as SalesInvoice[] : [target as SalesInvoice];
        const targets = invoices.filter((v) => v.CanExecuteSetPaid);

        if (targets.length > 0) {
          dialogService
            .prompt({ title: `Set Payment Date`, placeholder: `Payment date`, promptType: `date` })
            .subscribe((paymentDate: string) => {
              if (paymentDate) {
                targets.forEach((salesinvoice) => {
                  const amountToPay = parseFloat(salesinvoice.TotalIncVat) - parseFloat(salesinvoice.AmountPaid);

                  if (salesinvoice.SalesInvoiceType.UniqueId === '92411bf1-835e-41f8-80af-6611efce5b32' ||
                    salesinvoice.SalesInvoiceType.UniqueId === 'ef5b7c52-e782-416d-b46f-89c8c7a5c24d') {

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
        { name: 'billedTo' },
        { name: 'reference', sort: true },
        { name: 'state' },
        { name: 'description', sort: true },
        { name: 'totalExVat', sort: true },
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
        this.setPaid
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.SalesInvoice.BilledFrom });
    const predicate = new And([
      internalOrganisationPredicate,
      new Equals({ propertyType: m.SalesInvoice.SalesInvoiceType, parameter: 'type' }),
      new Equals({ propertyType: m.SalesInvoice.InvoiceNumber, parameter: 'number' }),
      new Equals({ propertyType: m.SalesInvoice.CustomerReference, parameter: 'customerReference' }),
      new Equals({ propertyType: m.SalesInvoice.SalesInvoiceState, parameter: 'state' }),
      new Equals({ propertyType: m.SalesInvoice.ShipToCustomer, parameter: 'shipTo' }),
      new Equals({ propertyType: m.SalesInvoice.BillToCustomer, parameter: 'billTo' }),
      new Equals({ propertyType: m.SalesInvoice.ShipToEndCustomer, parameter: 'shipToEndCustomer' }),
      new Equals({ propertyType: m.SalesInvoice.BillToEndCustomer, parameter: 'billToEndCustomer' }),
      new ContainedIn({
        propertyType: m.SalesInvoice.SalesInvoiceItems,
        extent: new Filter({
          objectType: m.SalesInvoiceItem,
          predicate: new ContainedIn({
            propertyType: m.SalesInvoiceItem.Product,
            parameter: 'product'
          })
        })
      }),
      new ContainedIn({
        propertyType: m.SalesInvoice.SalesInvoiceItems,
        extent: new Filter({
          objectType: m.SalesInvoiceItem,
          predicate: new ContainedIn({
            propertyType: m.SalesInvoiceItem.SerialisedItem,
            parameter: 'serialisedItem'
          })
        })
      })
    ]);

    const typeSearch = new SearchFactory({
      objectType: m.SalesInvoiceType,
      roleTypes: [m.SalesInvoiceType.Name],
    });

    const stateSearch = new SearchFactory({
      objectType: m.SalesInvoiceState,
      roleTypes: [m.SalesInvoiceState.Name],
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

    this.filterService.init(predicate, {
      active: { initialValue: true },
      type: { search: typeSearch, display: (v: SalesInvoiceType) => v && v.Name },
      state: { search: stateSearch, display: (v: SalesInvoiceState) => v && v.Name },
      shipTo: { search: partySearch, display: (v: Party) => v && v.PartyName },
      billTo: { search: partySearch, display: (v: Party) => v && v.PartyName },
      shipToEndCustomer: { search: partySearch, display: (v: Party) => v && v.PartyName },
      billToEndCustomer: { search: partySearch, display: (v: Party) => v && v.PartyName },
      product: { search: productSearch, display: (v: Product) => v && v.Name },
      serialisedItem: { search: serialisedItemSearch, display: (v: SerialisedItem) => v && v.ItemNumber },
    });

    const sorter = new Sorter(
      {
        number: m.SalesInvoice.InvoiceNumber,
        type: m.SalesInvoice.SalesInvoiceType,
        reference: m.SalesInvoice.CustomerReference,
        description: m.SalesInvoice.Description,
        lastModifiedDate: m.SalesInvoice.LastModifiedDate,
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
            this.fetcher.internalOrganisation,
            pull.Person({
              object: this.userId.value
            }),
            pull.UserGroup({
              include: { Members: x }
            }),
            pull.SalesInvoice({
              predicate,
              sort: sorter.create(sort),
              include: {
                PrintDocument: {
                  Media: x
                },
                BillToCustomer: x,
                SalesInvoiceState: x,
                SalesInvoiceType: x
              },
              parameters: this.filterService.parameters(filterFields),
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
        const localAdministrator = this.internalOrganisation.LocalAdministrators.includes(this.user);
        const userGroups = loaded.collections.UserGroups as UserGroup[];
        const administratorUserGroup = userGroups.find((v: UserGroup) => v.UniqueId === 'cdc04209-683b-429c-bed2-440851f430df');
        const administrator = administratorUserGroup.Members.includes(this.user);
        this.canCreate = localAdministrator || administrator;

        const salesInvoices = loaded.collections.SalesInvoices as SalesInvoice[];
        this.table.total = loaded.values.SalesInvoices_total;
        this.table.data = salesInvoices.map((v) => {
          return {
            object: v,
            number: v.InvoiceNumber,
            type: `${v.SalesInvoiceType && v.SalesInvoiceType.Name}`,
            billedTo: v.BillToCustomer.displayName,
            state: `${v.SalesInvoiceState && v.SalesInvoiceState.Name}`,
            reference: `${v.CustomerReference} - ${v.SalesInvoiceState.Name}`,
            description: v.Description,
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
