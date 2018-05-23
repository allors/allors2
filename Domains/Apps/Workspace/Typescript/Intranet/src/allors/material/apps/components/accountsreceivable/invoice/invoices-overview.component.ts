import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';



import { ErrorService, Loaded, PdfService, Scope, WorkspaceService } from '../../../../../angular';
import { InternalOrganisation, SalesInvoice, SalesInvoiceState } from '../../../../../domain';
import { And, ContainedIn, Equals, Like, Page, Predicate, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';

interface SearchData {
  internalOrganisation: string;
  company: string;
  reference: string;
  invoiceNumber: string;
  repeating: boolean;
  state: string;
  product: string;
}

@Component({
  templateUrl: './invoices-overview.component.html',
})
export class InvoicesOverviewComponent implements OnDestroy {

  public searchForm: FormGroup;

  public title = 'Sales Invoices';
  public data: SalesInvoice[];
  public filtered: SalesInvoice[];
  public total: number;

  public internalOrganisations: InternalOrganisation[];
  public selectedInternalOrganisation: InternalOrganisation;
  public billToInternalOrganisation: InternalOrganisation;

  public salesInvoiceStates: SalesInvoiceState[];
  public selectedSalesInvoiceState: SalesInvoiceState;
  public salesInvoiceState: SalesInvoiceState;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private router: Router,
    
    
    private changeDetectorRef: ChangeDetectorRef,
    public pdfService: PdfService,
    private stateService: StateService) {

    this.titleService.setTitle('Sales Invoices');

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      internalOrganisation: [''],
      company: [''],
      invoiceNumber: [''],
      reference: [''],
      repeating: [''],
      state: [''],
      product: [''],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$ = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$ = Observable.combineLatest(search$, this.page$, this.refresh$, this.stateService.internalOrganisationId$)
      .scan(([previousData, previousTake, previousDate, previousInternalOrganisationId], [data, take, date, internalOrganisationId]) => {
        return [
          data,
          data !== previousData ? 50 : take,
          date,
          internalOrganisationId,
        ];
      }, [] as [SearchData, number, Date, InternalOrganisation]);

    const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

    this.subscription = combined$
      .switchMap(([data, take, , internalOrganisationId]) => {

        const internalOrganisationsQuery: Query[] = [
          new Query(
            {
              name: 'salesinvoiceStates',
              objectType: m.SalesInvoiceState,
            }),
          new Query(
            {
              name: 'internalOrganisations',
              objectType: m.Organisation,
              predicate: new Equals({ roleType: m.Organisation.IsInternalOrganisation, value: true }),
            }),
        ];

        return this.scope
        .load('Pull', new PullRequest({ queries: internalOrganisationsQuery }))
        .switchMap((loaded: Loaded) => {
          this.salesInvoiceStates = loaded.collections.salesinvoiceStates as SalesInvoiceState[];
          this.salesInvoiceState = this.salesInvoiceStates.find((v: SalesInvoiceState) => v.Name === data.state);

          this.internalOrganisations = loaded.collections.internalOrganisations as InternalOrganisation[];
          this.billToInternalOrganisation = this.internalOrganisations.find(
            (v) => v.PartyName === data.internalOrganisation,
          );

          const predicate: And = new And();
          const predicates: Predicate[] = predicate.predicates;

          predicates.push(new Equals({ roleType: m.SalesInvoice.BilledFrom, value: internalOrganisationId }));

          if (data.invoiceNumber) {
            const like: string = '%' + data.invoiceNumber + '%';
            predicates.push(new Like({ roleType: m.SalesInvoice.InvoiceNumber, value: like }));
          }

          if (data.company) {
            const partyQuery: Query = new Query({
              objectType: m.Party, predicate: new Like({
                roleType: m.Party.PartyName, value: data.company.replace('*', '%') + '%',
              }),
            });

            const containedIn: ContainedIn = new ContainedIn({ roleType: m.SalesInvoice.BillToCustomer, query: partyQuery });
            predicates.push(containedIn);
          }

          if (data.product) {
            const productQuery: Query = new Query({
              objectType: m.Good,
              predicate: new Like({
                roleType: m.Good.Name, value: data.company.replace('*', '%') + '%',
              }),
            });

            const containedIn: ContainedIn = new ContainedIn({ roleType: m.SalesInvoiceItem.Product, query: productQuery });
            predicates.push(containedIn);
          }

          if (data.internalOrganisation) {
            predicates.push(
              new Equals({
                roleType: m.SalesInvoice.BillToCustomer,
                value: this.billToInternalOrganisation,
              }),
            );
          }

          if (data.reference) {
            const like: string = data.reference.replace('*', '%') + '%';
            predicates.push(new Like({ roleType: m.SalesInvoice.CustomerReference, value: like }));
          }

          if (data.repeating) {
            predicates.push(new Equals({ roleType: m.SalesInvoice.IsRepeatingInvoice, value: true }));
          }

          if (data.state) {
            predicates.push(new Equals({ roleType: m.SalesInvoice.SalesInvoiceState, value: this.salesInvoiceState }));
          }

          const queries: Query[] = [new Query(
            {
              include: [
                new TreeNode({ roleType: m.SalesInvoice.BillToCustomer }),
                new TreeNode({ roleType: m.SalesInvoice.SalesInvoiceState }),
              ],
              name: 'invoices',
              objectType: m.SalesInvoice,
              page: new Page({ skip: 0, take }),
              predicate,
              sort: [new Sort({ roleType: m.SalesInvoice.InvoiceNumber, direction: 'Desc' })],
            })];

          return this.scope.load('Pull', new PullRequest({ queries }));
        });
      })
      .subscribe((loaded) => {
        this.data = loaded.collections.invoices as SalesInvoice[];
        this.total = loaded.values.invoices_total;
      },
      (error: any) => {
        this.errorService.handle(error);
        this.goBack();
      });
  }

  public print(invoice: SalesInvoice) {
    this.pdfService.display(invoice);
  }

  public goBack(): void {
    window.history.back();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public onView(invoice: SalesInvoice): void {
    this.router.navigate(['/accountsreceivable/invoices/' + invoice.id]);
  }

  private more(): void {
    this.page$.next(this.data.length + 50);
  }
}
