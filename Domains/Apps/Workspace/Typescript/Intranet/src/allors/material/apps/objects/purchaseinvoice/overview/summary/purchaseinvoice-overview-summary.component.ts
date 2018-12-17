import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, Invoked, RefreshService, ErrorService } from '../../../../../../angular';
import { Good, PurchaseOrder, PurchaseInvoice } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { MatSnackBar } from '@angular/material';
import { Sort, Equals } from 'src/allors/framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseinvoice-overview-summary',
  templateUrl: './purchaseinvoice-overview-summary.component.html',
  providers: [PanelService]
})
export class PurchasInvoiceOverviewSummaryComponent {

  m: Meta;

  public order: PurchaseOrder;
  public invoice: PurchaseInvoice;
  public goods: Good[] = [];

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    public refreshService: RefreshService,
    public snackBar: MatSnackBar,
    public errorService: ErrorService) {

    this.m = this.metaService.m;

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
            BillToCustomer: x,
            BillToCustomerContactPerson: x,
            ShipToEndCustomer: x,
            ShipToEndCustomerContactPerson: x,
            PurchaseInvoiceState: x,
            CreatedBy: x,
            LastModifiedBy: x,
            PurchaseOrder: x,
            BillToCustomerContactMechanism: {
              PostalAddress_Country: {
              }
            },
            ShipToEndCustomerAddress: {
              PostalBoundary: {
                Country: x
              }
            }
          },
        }),
        pull.PurchaseInvoice({
          name: purchaseOrderPullName,
          object: id,
          fetch: {
            PurchaseOrder: x
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
      this.order = loaded.objects[purchaseOrderPullName] as PurchaseOrder;
    };
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.invoice.CancelInvoice)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public approve(): void {

    this.panel.manager.context.invoke(this.invoice.Approve)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public finish(invoice: PurchaseInvoice): void {

    this.panel.manager.context.invoke(invoice.Finish)
      .subscribe((invoked: Invoked) => {
        this.snackBar.open('Successfully finished.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      });
    }
}

