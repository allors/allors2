import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

import { ErrorService, Loaded, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { InternalOrganisation, PurchaseInvoice, PurchaseInvoiceState } from '../../../../../domain';
import { And, ContainedIn, Equals, Like, Predicate, PullRequest, Sort, Filter } from '../../../../../framework';
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

  public searchForm: FormGroup; public advancedSearch: boolean;

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
    private dataService: DataService,
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
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$ = combineLatest(search$, this.page$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousData, previousTake, previousDate, previousInternalOrganisationId], [data, take, date, internalOrganisationId]) => {
          return [
            data,
            data !== previousData ? 50 : take,
            date,
            internalOrganisationId,
          ];
        }, [])
      );

    const { m, pull } = this.dataService;

    this.subscription = combined$
      .pipe(
        switchMap(([data, take, , internalOrganisationId]) => {

          const pulls = [
            pull.PurchaseInvoiceItemState({
              sort: new Sort(m.PurchaseInvoiceItemState.Name)
            }),
            pull.InternalOrganisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: [
                new Sort(m.Organisation.PartyName),
              ],
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded: Loaded) => {
                this.purchaseInvoiceStates = loaded.collections.purchaseinvoiceStates as PurchaseInvoiceState[];
                this.purchaseInvoiceState = this.purchaseInvoiceStates.find((v: PurchaseInvoiceState) => v.Name === data.state);

                this.internalOrganisations = loaded.collections.internalOrganisations as InternalOrganisation[];
                this.billedFromInternalOrganisation = this.internalOrganisations.find(
                  (v) => v.PartyName === data.internalOrganisation,
                );

                const predicate: And = new And();
                const predicates: Predicate[] = predicate.operands;

                predicates.push(new Equals({ propertyType: m.PurchaseInvoice.BilledTo, value: internalOrganisationId }));

                if (data.invoiceNumber) {
                  const like: string = '%' + data.invoiceNumber + '%';
                  predicates.push(new Like({ roleType: m.PurchaseInvoice.InvoiceNumber, value: like }));
                }

                if (data.supplier) {
                  const containedIn: ContainedIn = new ContainedIn({
                    propertyType: m.PurchaseInvoice.BilledFrom, extent: new Filter({
                      objectType: m.Party,
                      predicate: new Like({
                        roleType: m.Party.PartyName, value: data.supplier.replace('*', '%') + '%',
                      })
                    })
                  });
                  predicates.push(containedIn);
                }

                if (data.internalOrganisation) {
                  predicates.push(
                    new Equals({
                      propertyType: m.PurchaseInvoice.BilledFrom,
                      value: this.billedFromInternalOrganisation,
                    }),
                  );
                }

                if (data.reference) {
                  const like: string = data.reference.replace('*', '%') + '%';
                  predicates.push(new Like({ roleType: m.PurchaseInvoice.CustomerReference, value: like }));
                }

                if (data.state) {
                  predicates.push(new Equals({ propertyType: m.PurchaseInvoice.PurchaseInvoiceState, value: this.purchaseInvoiceState }));
                }

                const queries = [
                  pull.PurchaseInvoice({
                    predicate,
                    include: {
                      BilledFrom: x,
                      BillToCustomer: x,
                      PurchaseInvoiceState: x
                    },
                    sort: [new Sort({ roleType: m.PurchaseInvoice.InvoiceNumber, descending: true })],
                    skip: 0, take
                  })];

                return this.scope.load('Pull', new PullRequest({ pulls: queries }));
              })
            );
        })
      )
      .subscribe((loaded) => {
        this.data = loaded.collections.PurchaseInvoice as PurchaseInvoice[];
        this.total = loaded.values.PurchaseInvoices_total;
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
