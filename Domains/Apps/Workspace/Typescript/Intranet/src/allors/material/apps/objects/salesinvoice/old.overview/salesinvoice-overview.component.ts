import {Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, MediaService, Saved, ContextService, MetaService } from '../../../../../angular';
import { Good, RepeatingSalesInvoice, SalesInvoice, SalesInvoiceItem, SalesOrder, SalesTerm } from '../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './salesinvoice-overview.component.html',
  providers: [ContextService]
})
export class SalesInvoiceOverviewComponent implements OnInit, OnDestroy {

  public m: Meta;
  public title = 'Sales Invoice Overview';
  public order: SalesOrder;
  public invoice: SalesInvoice;
  public repeatingInvoices: RepeatingSalesInvoice[];
  public repeatingInvoice: RepeatingSalesInvoice;
  public goods: Good[] = [];

  private subscription: Subscription;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    public mediaService: MediaService,
    private dialogService: AllorsMaterialDialogService) {

    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.m = this.metaService.m;
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('items saved', 'close', { duration: 1000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.SalesInvoice({
              object: id,
              include: {
                SalesInvoiceItems: {
                  Product: x,
                  InvoiceItemType: x,
                },
                SalesTerms: {
                  TermType: x,
                },
                BillToCustomer: x,
                BillToContactPerson: x,
                ShipToCustomer: x,
                ShipToContactPerson: x,
                ShipToEndCustomer: x,
                ShipToEndCustomerContactPerson: x,
                SalesInvoiceState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                SalesOrder: x,
                BillToContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x
                  }
                },
                ShipToAddress: {
                  PostalBoundary: {
                    Country: x
                  }
                },
                BillToEndCustomerContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x
                  }
                },
                ShipToEndCustomerAddress: {
                  PostalBoundary: {
                    Country: x
                  }
                }
              }
            }),
            pull.SalesInvoice({
              object: id,
              fetch: {
                SalesOrder: x
              }
            }),
            pull.Good(
              {
                sort: new Sort(m.Good.Name),
              }),
            pull.RepeatingSalesInvoice(
              {
                predicate: new Equals({ propertyType: m.RepeatingSalesInvoice.Source, value: id }),
                include: {
                  Frequency: x,
                  DayOfWeek: x
                }
              }),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })

      )
      .subscribe((loaded) => {
        this.allors.context.reset();
        this.goods = loaded.collections.goods as Good[];
        this.order = loaded.objects.order as SalesOrder;
        this.invoice = loaded.objects.invoice as SalesInvoice;
        this.repeatingInvoices = loaded.collections.repeatingInvoices as RepeatingSalesInvoice[];
        if (this.repeatingInvoices.length > 0) {
          this.repeatingInvoice = this.repeatingInvoices[0];
        } else {
          this.repeatingInvoice = undefined;
        }
      }, this.errorService.handler);
  }

  public print() {
    //
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
  }

  public send(): void {

    this.allors.context.invoke(this.invoice.Send)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully sent.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public cancel(): void {

    this.allors.context.invoke(this.invoice.CancelInvoice)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public writeOff(): void {

    this.allors.context.invoke(this.invoice.WriteOff)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully written off.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public reopen(): void {

    this.allors.context.invoke(this.invoice.Reopen)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully Reopened.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public credit(): void {

    this.allors.context.invoke(this.invoice.Credit)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully Credited.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public copy(): void {

    this.allors.context.invoke(this.invoice.Copy)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully copied.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public deleteInvoiceItem(invoiceItem: SalesInvoiceItem): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this item?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.context.invoke(invoiceItem.Delete)
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

  public deleteSalesTerm(salesTerm: SalesTerm): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this order term?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.context.invoke(salesTerm.Delete)
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
