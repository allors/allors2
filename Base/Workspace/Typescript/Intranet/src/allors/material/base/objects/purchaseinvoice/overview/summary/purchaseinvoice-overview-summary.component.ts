import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, Invoked, RefreshService, Action } from '../../../../../../angular';
import { Good, PurchaseOrder, PurchaseInvoice } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Sort, Equals } from '../../../../../../../allors/framework';
import { PrintService, SaveService } from '../../../../../../material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseinvoice-overview-summary',
  templateUrl: './purchaseinvoice-overview-summary.component.html',
  providers: [PanelService]
})
export class PurchasInvoiceOverviewSummaryComponent {

  m: Meta;

  orders: PurchaseOrder[];
  invoice: PurchaseInvoice;
  goods: Good[] = [];

  print: Action;
  orderTotalExVat: number;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    public printService: PrintService,
    private saveService: SaveService,

    public refreshService: RefreshService,
    public snackBar: MatSnackBar) {

    this.m = this.metaService.m;

    this.print = printService.print();

    panel.name = 'summary';

    const purchaseInvoicePullName = `${panel.name}_${this.m.PurchaseInvoice.name}`;
    const purchaseOrderPullName = `${panel.name}_${this.m.PurchaseOrder.name}`;
    const goodPullName = `${panel.name}_${this.m.Good.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, x } = this.metaService;

      const { id } = this.panel.manager;

      pulls.push(
        pull.PurchaseInvoice({
          name: purchaseInvoicePullName,
          object: id,
          include: {
            PurchaseInvoiceItems: {
              InvoiceItemType: x
            },
            BilledFrom: x,
            BilledFromContactPerson: x,
            ShipToCustomer: x,
            BillToEndCustomer: x,
            BillToEndCustomerContactPerson: x,
            ShipToEndCustomer: x,
            ShipToEndCustomerContactPerson: x,
            PurchaseInvoiceState: x,
            CreatedBy: x,
            LastModifiedBy: x,
            BillToEndCustomerContactMechanism: {
              PostalAddress_Country: {
              }
            },
            ShipToEndCustomerAddress: {
              Country: x
            },
            PrintDocument: {
              Media: x
            },
          },
        }),
        pull.PurchaseInvoice({
          name: purchaseOrderPullName,
          object: id,
          fetch: {
            PurchaseOrders: x
          }
        }),
        pull.Good({
          name: goodPullName,
          sort: new Sort(m.Good.Name)
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.invoice = loaded.objects[purchaseInvoicePullName] as PurchaseInvoice;
      this.goods = loaded.collections[goodPullName] as Good[];
      this.orders = loaded.collections[purchaseOrderPullName] as PurchaseOrder[];

      this.orderTotalExVat = this.orders.reduce((partialOrderTotal, order) => partialOrderTotal + order.ValidOrderItems.reduce((partialItemTotal, item) => partialItemTotal + parseFloat(item.TotalExVat), 0), 0);
    };
  }

  public confirm(): void {

    this.panel.manager.context.invoke(this.invoice.Confirm)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully confirmed.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.invoice.Cancel)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public reopen(): void {

    this.panel.manager.context.invoke(this.invoice.Reopen)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public approve(): void {

    this.panel.manager.context.invoke(this.invoice.Approve)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public reject(): void {

    this.panel.manager.context.invoke(this.invoice.Reject)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }
}

