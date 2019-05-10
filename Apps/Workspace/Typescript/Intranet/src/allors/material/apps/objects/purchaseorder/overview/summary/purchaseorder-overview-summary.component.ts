import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, Invoked, RefreshService, Action } from '../../../../../../angular';
import { PurchaseOrder, PurchaseInvoice} from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { MatSnackBar } from '@angular/material';
import { PrintService } from '../../../../../../material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseorder-overview-summary',
  templateUrl: './purchaseorder-overview-summary.component.html',
  providers: [PanelService]
})
export class PurchaseOrderOverviewSummaryComponent {

  m: Meta;

  order: PurchaseOrder;
  purchaseInvoices: PurchaseInvoice[] = [];

  print: Action;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    public printService: PrintService,
    public refreshService: RefreshService,
    public snackBar: MatSnackBar) {

    this.m = this.metaService.m;

    this.print = printService.print();

    panel.name = 'summary';

    const puchaseOrderPullName = `${panel.name}_${this.m.PurchaseOrder.name}`;
    const purchaseInvoicePullName = `${panel.name}_${this.m.PurchaseInvoice.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, x } = this.metaService;

      pulls.push(

        pull.PurchaseOrder({
          name: puchaseOrderPullName,
          object: this.panel.manager.id,
          include: {
            TakenViaSupplier: x,
            PurchaseOrderState: x,
            PurchaseOrderShipmentState: x,
            PurchaseOrderPaymentState: x,
            CreatedBy: x,
            LastModifiedBy: x,
            PrintDocument: {
              Media: x
            },
          }
        }),
        pull.PurchaseOrder({
          name: purchaseInvoicePullName,
          object: this.panel.manager.id,
          fetch: { PurchaseInvoicesWherePurchaseOrder: x }
        }),
      );
    };

    panel.onPulled = (loaded) => {
      this.order = loaded.objects[puchaseOrderPullName] as PurchaseOrder;
      this.purchaseInvoices = loaded.collections[purchaseInvoicePullName] as PurchaseInvoice[];
    };
  }

  public approve(): void {

    this.panel.manager.context.invoke(this.order.Approve)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
      });
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.order.Cancel)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      });
  }

  public reject(): void {

    this.panel.manager.context.invoke(this.order.Reject)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      });
  }

  public hold(): void {

    this.panel.manager.context.invoke(this.order.Hold)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully put on hold.', 'close', { duration: 5000 });
      });
  }

  public continue(): void {

    this.panel.manager.context.invoke(this.order.Continue)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully removed from hold.', 'close', { duration: 5000 });
      });
  }

  public confirm(): void {

    this.panel.manager.context.invoke(this.order.Confirm)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully confirmed.', 'close', { duration: 5000 });
      });
  }

  public reopen(): void {

    this.panel.manager.context.invoke(this.order.Reopen)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
      });
  }

  public send(): void {

    this.panel.manager.context.invoke(this.order.Send)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully send.', 'close', { duration: 5000 });
      });
  }

  public invoice(): void {

    this.panel.manager.context.invoke(this.order.Invoice)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully created purchase invoice', 'close', { duration: 5000 });
      });
  }

  public quickReceive(): void {

    this.panel.manager.context.invoke(this.order.QuickReceive)
      .subscribe((invoked: Invoked) => {
        this.panel.toggle();
        this.snackBar.open('inventory created for all items', 'close', { duration: 5000 });
        this.refreshService.refresh();
      });
  }
}
