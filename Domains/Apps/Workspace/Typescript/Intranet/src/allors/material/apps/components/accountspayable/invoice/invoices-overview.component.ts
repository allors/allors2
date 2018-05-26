import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../../angular';
import { InternalOrganisation, PurchaseInvoice, PurchaseInvoiceState } from '../../../../../domain';
import { And, ContainedIn, Equals, Like, Page, Predicate, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

interface SearchData {
  company: string;
  reference: string;
  invoiceNumber: string;
  state: string;
}

@Component({
  templateUrl: './invoices-overview.component.html',
})
export class InvoicesOverviewComponent implements OnDestroy {

  public searchForm: FormGroup;

  public title = 'Purchase Invoices';
  public data: PurchaseInvoice[];
  public filtered: PurchaseInvoice[];
  public total: number;

  public purchaseInvoiceStates: PurchaseInvoiceState[];
  public selectedPurchaseInvoiceState: PurchaseInvoiceState;
  public purchaseInvoiceState: PurchaseInvoiceState;

  public internalOrganisations: InternalOrganisation[];
  public selectedInternalOrganisation: InternalOrganisation;
  public billedFromInternalOrganisation: InternalOrganisation;

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
    
    
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle('Purchase Invoices');

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      supplier: [''],
      internalOrganisation: [''],
      invoiceNumber: [''],
      reference: [''],
      state: [''],
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
              name: 'purchaseinvoiceStates',
              objectType: m.PurchaseInvoiceState,
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
          this.purchaseInvoiceStates = loaded.collections.purchaseinvoiceStates as PurchaseInvoiceState[];
          this.purchaseInvoiceState = this.purchaseInvoiceStates.find((v: PurchaseInvoiceState) => v.Name === data.state);

          this.internalOrganisations = loaded.collections.internalOrganisations as InternalOrganisation[];
          this.billedFromInternalOrganisation = this.internalOrganisations.find(
            (v) => v.PartyName === data.internalOrganisation,
          );

          const predicate: And = new And();
          const predicates: Predicate[] = predicate.predicates;

          predicates.push(new Equals({ roleType: m.PurchaseInvoice.BilledTo, value: internalOrganisationId }));

          if (data.invoiceNumber) {
            const like: string = '%' + data.invoiceNumber + '%';
            predicates.push(new Like({ roleType: m.PurchaseInvoice.InvoiceNumber, value: like }));
          }

          if (data.supplier) {
            const partyQuery: Query = new Query({
              objectType: m.Party, predicate: new Like({
                roleType: m.Party.PartyName, value: data.supplier.replace('*', '%') + '%',
              }),
            });

            const containedIn: ContainedIn = new ContainedIn({ roleType: m.PurchaseInvoice.BilledFrom, query: partyQuery });
            predicates.push(containedIn);
          }

          if (data.internalOrganisation) {
            predicates.push(
              new Equals({
                roleType: m.PurchaseInvoice.BilledFrom,
                value: this.billedFromInternalOrganisation,
              }),
            );
          }

          if (data.reference) {
            const like: string = data.reference.replace('*', '%') + '%';
            predicates.push(new Like({ roleType: m.PurchaseInvoice.CustomerReference, value: like }));
          }

          if (data.state) {
            predicates.push(new Equals({ roleType: m.PurchaseInvoice.PurchaseInvoiceState, value: this.purchaseInvoiceState }));
          }

          const queries: Query[] = [new Query(
          {
            include: [
              new TreeNode({ roleType: m.PurchaseInvoice.BilledFrom }),
              new TreeNode({ roleType: m.PurchaseInvoice.BillToCustomer }),
              new TreeNode({ roleType: m.PurchaseInvoice.PurchaseInvoiceState }),
            ],
            name: 'invoices',
            objectType: m.PurchaseInvoice,
            page: new Page({ skip: 0, take }),
            predicate,
            sort: [new Sort({ roleType: m.PurchaseInvoice.InvoiceNumber, direction: 'Desc' })],
          })];

          return this.scope.load('Pull', new PullRequest({ queries }));
        });
      })
      .subscribe((loaded) => {
        this.data = loaded.collections.invoices as PurchaseInvoice[];
        this.total = loaded.values.invoices_total;
      },
      (error: any) => {
        this.errorService.handle(error);
        this.goBack();
      });
  }

  public goBack(): void {
    window.history.back();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public onView(invoice: PurchaseInvoice): void {
    this.router.navigate(['/accountspayable/invoices/' + invoice.id]);
  }

  private more(): void {
    this.page$.next(this.data.length + 50);
  }
}
