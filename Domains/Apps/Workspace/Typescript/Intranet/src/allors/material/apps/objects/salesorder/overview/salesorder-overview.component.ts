import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatDialog, MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, Loaded, MediaService, Saved, SessionService } from '../../../../../angular';
import { BillingProcess, Good, ProductQuote, SalesInvoice, SalesOrder, SalesOrderItem, SalesTerm, SerialisedInventoryItemState } from '../../../../../domain';
import { Fetch, PullRequest, TreeNode, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './salesorder-overview.component.html',
  providers: [SessionService]
})
export class SalesOrderOverviewComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public title = 'Sales Order Overview';
  public quote: ProductQuote;
  public order: SalesOrder;
  public orderItems: SalesOrderItem[] = [];
  public goods: Good[] = [];
  public salesInvoice: SalesInvoice;
  public billingProcesses: BillingProcess[];
  public billingForOrderItems: BillingProcess;
  public selectedSerialisedInventoryState: string;
  public inventoryItemStates: SerialisedInventoryItemState[];
  private subscription: Subscription;

  private refresh$: BehaviorSubject<Date>;

  constructor(
    @Self() private allors: SessionService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    public mediaService: MediaService,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {
    this.allors
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('items saved', 'close', { duration: 1000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {
          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.SalesOrder({
              object: id,
              include: {
                SalesOrderItems: {
                  Product: x,
                  InvoiceItemType: x,
                  SalesOrderItemState: x,
                  SalesOrderItemShipmentState: x,
                  SalesOrderItemPaymentState: x,
                  SalesOrderItemInvoiceState: x,
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
                BillToEndCustomer: x,
                BillToEndCustomerContactPerson: x,
                SalesOrderState: x,
                SalesOrderShipmentState: x,
                SalesOrderInvoiceState: x,
                SalesOrderPaymentState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                Quote: x,
                ShipToAddress: {
                  PostalBoundary: {
                    Country: x,
                  }
                },
                BillToEndCustomerContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
                },
                ShipToEndCustomerAddress: {
                  PostalBoundary: {
                    Country: x,
                  }
                },
                BillToContactMechanism: {
                  PostalAddress_PostalBoundary: {
                    Country: x,
                  }
                }
              }
            }),
            pull.Good({ sort: new Sort(m.Good.Name) }),
            pull.BillingProcess({ sort: new Sort(m.BillingProcess.Name) }),
            pull.SerialisedInventoryItemState({
              predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
              sort: new Sort(m.SerialisedInventoryItemState.Name)
            })
          ];

          const salesInvoiceFetch = pull.SalesOrder({
            object: id,
            fetch: { SalesInvoicesWhereSalesOrder: x }
          });

          if (id != null) {
            pulls.push(salesInvoiceFetch);
          }

          return this.allors
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();
        this.goods = loaded.collections.goods as Good[];
        this.order = loaded.objects.order as SalesOrder;
        this.salesInvoice = loaded.objects.salesInvoice as SalesInvoice;
        this.inventoryItemStates = loaded.collections.serialisedInventoryItemStates as SerialisedInventoryItemState[];
        this.billingProcesses = loaded.collections.billingProcesses as BillingProcess[];
        this.billingForOrderItems = this.billingProcesses.find((v: BillingProcess) => v.UniqueId.toUpperCase() === 'AB01CCC2-6480-4FC0-B20E-265AFD41FAE2');

        if (this.order) {
          this.orderItems = this.order.SalesOrderItems;
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
  }

  public print() {
    // this.pdfService.display(this.order);
  }

  public approve(): void {

    this.allors.invoke(this.order.Approve)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public cancel(): void {

    this.allors.invoke(this.order.Reject)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public reject(): void {

    this.allors.invoke(this.order.Reject)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public hold(): void {

    this.allors.invoke(this.order.Hold)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully put on hold.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public continue(): void {

    this.allors.invoke(this.order.Continue)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully removed from hold.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public confirm(): void {

    this.allors.invoke(this.order.Confirm)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully confirmed.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public finish(): void {

    this.allors.invoke(this.order.Continue)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully finished.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public cancelOrderItem(orderItem: SalesOrderItem): void {

    this.allors.invoke(orderItem.Cancel)
      .subscribe((invoked: Invoked) => {
        this.snackBar.open('Order Item successfully cancelled.', 'close', { duration: 5000 });
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public rejectOrderItem(orderItem: SalesOrderItem): void {

    this.allors.invoke(orderItem.Reject)
      .subscribe((invoked: Invoked) => {
        this.snackBar.open('Order Item successfully rejected.', 'close', { duration: 5000 });
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public deleteOrderItem(orderItem: SalesOrderItem): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this item?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(orderItem.Delete)
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
          this.allors.invoke(salesTerm.Delete)
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

  public ship(): void {

    this.allors.invoke(this.order.Ship)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open('Customer Shipment successfully created.', 'close', { duration: 5000 });
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public createInvoice(): void {

    this.allors.invoke(this.order.Invoice)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open('Invoice successfully created.', 'close', { duration: 5000 });
        this.gotoInvoice();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public gotoInvoice(): void {

    const { pull, x } = this.allors;

    const pulls = [
      pull.SalesOrder({
        object: this.order,
        fetch: { SalesInvoicesWhereSalesOrder: x }
      })
    ];

    this.allors.load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        const invoices = loaded.collections.invoices as SalesInvoice[];
        if (invoices.length === 1) {
          this.router.navigate(['/accountsreceivable/invoice/' + invoices[0].id]);
        }
      }, this.errorService.handler);
  }
}
