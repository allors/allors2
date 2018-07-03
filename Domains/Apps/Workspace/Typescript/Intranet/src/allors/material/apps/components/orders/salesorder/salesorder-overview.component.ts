import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Invoked, Loaded, MediaService, PdfService, Saved, Scope, WorkspaceService, LayoutService } from '../../../../../angular';
import { BillingProcess, Good, ProductQuote, SalesInvoice, SalesOrder, SalesOrderItem, SalesTerm, SerialisedInventoryItemState} from '../../../../../domain';
import { Fetch, Path, PullRequest, Query, TreeNode, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './salesorder-overview.component.html',
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
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    public layout: LayoutService,
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    public mediaService: MediaService,
    public pdfService: PdfService,
    private dialogService: AllorsMaterialDialogService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('items saved', 'close', { duration: 1000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {
        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.SalesOrderItem.Product }),
                  new TreeNode({ roleType: m.SalesOrderItem.InvoiceItemType }),
                  new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemState }),
                  new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemShipmentState }),
                  new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemPaymentState }),
                  new TreeNode({ roleType: m.SalesOrderItem.SalesOrderItemInvoiceState }),
                ],
                roleType: m.SalesOrder.SalesOrderItems,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.SalesTerm.TermType }),
                ],
                roleType: m.SalesOrder.SalesTerms,
              }),
              new TreeNode({ roleType: m.SalesOrder.BillToCustomer }),
              new TreeNode({ roleType: m.SalesOrder.BillToContactPerson }),
              new TreeNode({ roleType: m.SalesOrder.ShipToCustomer }),
              new TreeNode({ roleType: m.SalesOrder.ShipToContactPerson }),
              new TreeNode({ roleType: m.SalesOrder.ShipToEndCustomer }),
              new TreeNode({ roleType: m.SalesOrder.ShipToEndCustomerContactPerson }),
              new TreeNode({ roleType: m.SalesOrder.BillToEndCustomer }),
              new TreeNode({ roleType: m.SalesOrder.BillToEndCustomerContactPerson }),
              new TreeNode({ roleType: m.SalesOrder.SalesOrderState }),
              new TreeNode({ roleType: m.SalesOrder.SalesOrderShipmentState }),
              new TreeNode({ roleType: m.SalesOrder.SalesOrderInvoiceState }),
              new TreeNode({ roleType: m.SalesOrder.SalesOrderPaymentState }),
              new TreeNode({ roleType: m.SalesOrder.CreatedBy }),
              new TreeNode({ roleType: m.SalesOrder.LastModifiedBy }),
              new TreeNode({ roleType: m.SalesOrder.Quote }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.SalesOrder.ShipToAddress,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.SalesOrder.BillToEndCustomerContactMechanism,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.SalesOrder.ShipToEndCustomerAddress,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.SalesOrder.BillToContactMechanism,
              }),
            ],
            name: 'order',
          }),
        ];

        const salesInvoiceFetch: Fetch = new Fetch({
          id,
          name: 'salesInvoice',
          path: new Path({ step: m.SalesOrder.SalesInvoicesWhereSalesOrder }),
        });

        if (id != null) {
          fetches.push(salesInvoiceFetch);
        }

        const queries: Query[] = [
          new Query(
            {
              name: 'goods',
              objectType: m.Good,
              sort: [
                new Sort({ roleType: m.Good.Name, direction: 'Asc' }),
              ],
            }),
          new Query(
            {
              name: 'billingProcesses',
              objectType: m.BillingProcess,
              sort: [
                new Sort({ roleType: m.BillingProcess.Name, direction: 'Asc' }),
              ],
            }),
          new Query(
            {
              name: 'serialisedInventoryItemStates',
              objectType: m.SerialisedInventoryItemState,
              sort: [
                new Sort({ roleType: m.SerialisedInventoryItemState.Name, direction: 'Asc' }),
              ],
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.goods = loaded.collections.goods as Good[];
        this.order = loaded.objects.order as SalesOrder;
        this.salesInvoice = loaded.objects.salesInvoice as SalesInvoice;
        this.inventoryItemStates = loaded.collections.serialisedInventoryItemStates as SerialisedInventoryItemState[];
        this.billingProcesses = loaded.collections.billingProcesses as BillingProcess[];
        this.billingForOrderItems = this.billingProcesses.find((v: BillingProcess) => v.UniqueId.toUpperCase() === 'AB01CCC2-6480-4FC0-B20E-265AFD41FAE2');

        if (this.order) {
          this.orderItems = this.order.SalesOrderItems;
        }
      },
      (error: any) => {
        this.errorService.handle(error);
        this.goBack();
      },
    );
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
    this.pdfService.display(this.order);
  }

  public approve(): void {
      this.scope.invoke(this.order.Approve)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public cancel(): void {
    this.scope.invoke(this.order.Reject)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public reject(): void {
    this.scope.invoke(this.order.Reject)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public hold(): void {
    this.scope.invoke(this.order.Hold)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully put on hold.', 'close', { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public continue(): void {
    this.scope.invoke(this.order.Continue)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully removed from hold.', 'close', { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public confirm(): void {
    this.scope.invoke(this.order.Confirm)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully confirmed.', 'close', { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public finish(): void {
    this.scope.invoke(this.order.Continue)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully finished.', 'close', { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public cancelOrderItem(orderItem: SalesOrderItem): void {
    this.scope.invoke(orderItem.Cancel)
      .subscribe((invoked: Invoked) => {
        this.snackBar.open('Order Item successfully cancelled.', 'close', { duration: 5000 });
        this.refresh();
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public rejectOrderItem(orderItem: SalesOrderItem): void {
    this.scope.invoke(orderItem.Reject)
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
          this.scope.invoke(orderItem.Delete)
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
          this.scope.invoke(salesTerm.Delete)
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
    this.scope.invoke(this.order.Ship)
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
    this.scope.invoke(this.order.Invoice)
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

      const fetches: Fetch[] = [new Fetch({
        id: this.order.id,
        name: 'invoices',
        path: new Path({ step: this.m.SalesOrder.SalesInvoicesWhereSalesOrder }),
      })];

      this.scope.load('Pull', new PullRequest({ fetches }))
        .subscribe((loaded) => {
          const invoices = loaded.collections.invoices as SalesInvoice[];
          if (invoices.length === 1) {
            this.router.navigate(['/accountsreceivable/invoice/' + invoices[0].id]);
          }
        },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        });
    }
  }
