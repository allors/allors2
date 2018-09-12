import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

import { ErrorService, Loaded, PdfService, Scope, WorkspaceService, Invoked, Saved, DataService, x } from '../../../../../angular';
import { InternalOrganisation, SalesInvoice, SalesInvoiceState } from '../../../../../domain';
import { And, ContainedIn, Equals, Like, Predicate, PullRequest, Sort, TreeNode, Filter } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { MatSnackBar } from '@angular/material';

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

  public searchForm: FormGroup; public advancedSearch: boolean;

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
  public readyForPostingState: SalesInvoiceState;

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
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: AllorsMaterialDialogService,
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
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({})
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
            pull.SalesInvoiceState({
              sort: new Sort(m.SalesInvoiceState.Name),
            }),
            pull.Organisation({
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
                this.salesInvoiceStates = loaded.collections.salesinvoiceStates as SalesInvoiceState[];
                this.salesInvoiceState = this.salesInvoiceStates.find((v: SalesInvoiceState) => v.Name === data.state);

                this.readyForPostingState = this.salesInvoiceStates.find((v: SalesInvoiceState) => v.UniqueId.toUpperCase() === '488F61FF-F474-44BA-9925-49AF7BCB0CD0');

                this.internalOrganisations = loaded.collections.internalOrganisations as InternalOrganisation[];
                this.billToInternalOrganisation = this.internalOrganisations.find(
                  (v) => v.PartyName === data.internalOrganisation,
                );

                const predicate: And = new And();
                const predicates: Predicate[] = predicate.operands;

                predicates.push(new Equals({ propertyType: m.SalesInvoice.BilledFrom, value: internalOrganisationId }));

                if (data.invoiceNumber) {
                  const like: string = '%' + data.invoiceNumber + '%';
                  predicates.push(new Like({ roleType: m.SalesInvoice.InvoiceNumber, value: like }));
                }

                if (data.company) {
                  const partyFilter = new Filter({
                    objectType: m.Party,
                    predicate: new Like({
                      roleType: m.Party.PartyName, value: data.company.replace('*', '%') + '%',
                    }),
                  });

                  const containedIn: ContainedIn = new ContainedIn({ propertyType: m.SalesInvoice.BillToCustomer, extent: partyFilter });
                  predicates.push(containedIn);
                }

                if (data.product) {
                  const productFilter = new Filter({
                    objectType: m.Good,
                    predicate: new Like({
                      roleType: m.Good.Name, value: data.company.replace('*', '%') + '%',
                    }),
                  });

                  const containedIn: ContainedIn = new ContainedIn({ propertyType: m.SalesInvoiceItem.Product, extent: productFilter });
                  predicates.push(containedIn);
                }

                if (data.internalOrganisation) {
                  predicates.push(
                    new Equals({
                      propertyType: m.SalesInvoice.BillToCustomer,
                      value: this.billToInternalOrganisation,
                    }),
                  );
                }

                if (data.reference) {
                  const like: string = data.reference.replace('*', '%') + '%';
                  predicates.push(new Like({ roleType: m.SalesInvoice.CustomerReference, value: like }));
                }

                if (data.repeating) {
                  predicates.push(new Equals({ propertyType: m.SalesInvoice.IsRepeatingInvoice, value: true }));
                }

                if (data.state) {
                  predicates.push(new Equals({ propertyType: m.SalesInvoice.SalesInvoiceState, value: this.salesInvoiceState }));
                }

                const queries = [
                  pull.SalesInvoice({
                    predicate,
                    include: {
                      BillToCustomer: x,
                      SalesInvoiceState: x,
                      SalesOrder: x,
                    },
                    sort: [new Sort(m.SalesInvoice.InvoiceNumber)],
                    skip: 0, take
                  })
                ];

                return this.scope.load('Pull', new PullRequest({ pulls: queries }));
              }));
        })
      )
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

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public send(invoice: SalesInvoice): void {
    const sendFn: () => void = () => {
      this.scope.invoke(invoice.Send)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully send.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                sendFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            sendFn();
          }
        });
    } else {
      sendFn();
    }
  }

  public cancel(invoice: SalesInvoice): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(invoice.CancelInvoice)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public writeOff(invoice: SalesInvoice): void {
    const writeOffFn: () => void = () => {
      this.scope.invoke(invoice.WriteOff)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully written off.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                writeOffFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            writeOffFn();
          }
        });
    } else {
      writeOffFn();
    }
  }

  public reopen(invoice: SalesInvoice): void {
    const reopenFn: () => void = () => {
      this.scope.invoke(invoice.Reopen)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                reopenFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            reopenFn();
          }
        });
    } else {
      reopenFn();
    }
  }

  public delete(invoice: SalesInvoice): void {
    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this invoice?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(invoice.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }
}
