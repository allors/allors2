import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, Invoked, RefreshService,  Action } from '../../../../../../angular';
import { ProductQuote, Good, SalesOrder, SalesOrderItem, SalesInvoice, BillingProcess, SerialisedInventoryItemState } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { MatSnackBar } from '@angular/material';
import { Sort, Equals } from 'src/allors/framework';
import { PrintService } from '../../../../../../material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'salesorder-overview-summary',
  templateUrl: './salesorder-overview-summary.component.html',
  providers: [PanelService]
})
export class SalesOrderOverviewSummaryComponent {

  m: Meta;

  order: SalesOrder;
  quote: ProductQuote;
  orderItems: SalesOrderItem[] = [];
  goods: Good[] = [];
  salesInvoices: SalesInvoice[] = [];
  billingProcesses: BillingProcess[];
  billingForOrderItems: BillingProcess;
  selectedSerialisedInventoryState: string;
  inventoryItemStates: SerialisedInventoryItemState[];

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

    const salesOrderPullName = `${panel.name}_${this.m.SalesOrder.name}`;
    const salesInvoicePullName = `${panel.name}_${this.m.SalesInvoice.name}`;
    const goodPullName = `${panel.name}_${this.m.Good.name}`;
    const billingProcessPullName = `${panel.name}_${this.m.BillingProcess.name}`;
    const serialisedInventoryItemStatePullName = `${panel.name}_${this.m.SerialisedInventoryItemState.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, x } = this.metaService;

      pulls.push(

        pull.SalesOrder({
          name: salesOrderPullName,
          object: this.panel.manager.id,
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
        pull.SalesOrder({
          name: salesInvoicePullName,
          object: this.panel.manager.id,
          fetch: { SalesInvoicesWhereSalesOrder: x }
        }),
        pull.Good({
          name: goodPullName,
          sort: new Sort(m.Good.Name),
        }),
        pull.BillingProcess({
          name: billingProcessPullName,
          sort: new Sort(m.BillingProcess.Name),
        }),
        pull.SerialisedInventoryItemState({
          name: serialisedInventoryItemStatePullName,
          predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
          sort: new Sort(m.SerialisedInventoryItemState.Name)
        }),
      );
    };

    panel.onPulled = (loaded) => {
      this.order = loaded.objects[salesOrderPullName] as SalesOrder;
      this.orderItems = loaded.collections[salesOrderPullName] as SalesOrderItem[];
      this.goods = loaded.collections[goodPullName] as Good[];
      this.billingProcesses = loaded.collections[billingProcessPullName] as BillingProcess[];
      this.billingForOrderItems = this.billingProcesses.find((v: BillingProcess) => v.UniqueId.toUpperCase() === 'AB01CCC2-6480-4FC0-B20E-265AFD41FAE2');
      this.inventoryItemStates = loaded.collections[serialisedInventoryItemStatePullName] as SerialisedInventoryItemState[];

      this.salesInvoices = loaded.collections[salesInvoicePullName] as SalesInvoice[];
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

  public finish(): void {

    this.panel.manager.context.invoke(this.order.Continue)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully finished.', 'close', { duration: 5000 });
      });
  }

  public ship(): void {

    this.panel.manager.context.invoke(this.order.Ship)
      .subscribe((invoked: Invoked) => {
        this.panel.toggle();
        this.snackBar.open('Customer shipment successfully created.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      });
  }

  public invoice(): void {

    this.panel.manager.context.invoke(this.order.Invoice)
      .subscribe((invoked: Invoked) => {
        this.panel.toggle();
        this.snackBar.open('Sales invoice successfully created.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      });
  }
}
