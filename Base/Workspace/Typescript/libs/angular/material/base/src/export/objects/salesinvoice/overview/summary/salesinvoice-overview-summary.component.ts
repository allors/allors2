import { Component, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { MetaService, NavigationService, PanelService, RefreshService } from '@allors/angular/services/core';
import { SalesInvoice, SalesOrder, RepeatingSalesInvoice, Good, WorkEffort } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { PrintService } from '@allors/angular/base';
import { Sort, Equals } from '@allors/data/system';
import { Action, ActionTarget } from '@allors/angular/core';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'salesinvoice-overview-summary',
  templateUrl: './salesinvoice-overview-summary.component.html',
  providers: [PanelService]
})
export class SalesInvoiceOverviewSummaryComponent {

  m: Meta;

  invoice: SalesInvoice;
  orders: SalesOrder[];
  repeatingInvoices: RepeatingSalesInvoice[];
  repeatingInvoice: RepeatingSalesInvoice;
  goods: Good[] = [];

  print: Action;
  workEfforts: WorkEffort[];

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    public printService: PrintService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    public snackBar: MatSnackBar) {

    this.m = this.metaService.m;

    this.print = printService.print();

    panel.name = 'summary';

    const salesInvoicePullName = `${panel.name}_${this.m.PurchaseInvoice.name}`;
    const salesOrderPullName = `${panel.name}_${this.m.PurchaseOrder.name}`;
    const workEffortPullName = `${panel.name}_${this.m.WorkEffort.name}`;
    const goodPullName = `${panel.name}_${this.m.Good.name}`;
    const repeatingSalesInvoicePullName = `${panel.name}_${this.m.Good.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, x } = this.metaService;

      const { id } = this.panel.manager;

      pulls.push(
        pull.SalesInvoice({
          name: salesInvoicePullName,
          object: id,
          include: {
            SalesInvoiceItems: {
              Product: x,
              InvoiceItemType: x,
            },
            SalesTerms: {
              TermType: x,
            },
            PrintDocument: {
              Media: x
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
            BillToContactMechanism: {
              PostalAddress_Country: x
            },
            ShipToAddress: {
              Country: x
            },
            BillToEndCustomerContactMechanism: {
              PostalAddress_Country: x
            },
            ShipToEndCustomerAddress: {
              Country: x
            }
          }
        }),
        pull.SalesInvoice({
          name: salesOrderPullName,
          object: id,
          fetch: {
            SalesOrders: x
          }
        }),
        pull.SalesInvoice({
          name: workEffortPullName,
          object: id,
          fetch: {
            WorkEfforts: x
          }
        }),
        pull.Good({
          name: goodPullName,
          sort: new Sort(m.Good.Name),
        }),
        pull.RepeatingSalesInvoice({
          name: repeatingSalesInvoicePullName,
          predicate: new Equals({ propertyType: m.RepeatingSalesInvoice.Source, object: id }),
          include: {
            Frequency: x,
            DayOfWeek: x
          }
        }),
      );
    };

    panel.onPulled = (loaded) => {
      this.goods = loaded.collections.Goods as Good[];
      this.orders = loaded.collections.SalesOrders as SalesOrder[];
      this.workEfforts = loaded.collections[workEffortPullName] as WorkEffort[];
      this.invoice = loaded.objects.SalesInvoice as SalesInvoice;
      this.repeatingInvoices = loaded.collections.RepeatingSalesInvoices as RepeatingSalesInvoice[];
      if (this.repeatingInvoices.length > 0) {
        this.repeatingInvoice = this.repeatingInvoices[0];
      } else {
        this.repeatingInvoice = undefined;
      }
    };
  }

  send() {

    this.panel.manager.context.invoke(this.invoice.Send)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully sent.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.invoice.CancelInvoice)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public writeOff(): void {

    this.panel.manager.context.invoke(this.invoice.WriteOff)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully written off.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public reopen(): void {

    this.panel.manager.context.invoke(this.invoice.Reopen)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully Reopened.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public credit(): void {

    this.panel.manager.context.invoke(this.invoice.Credit)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully Credited.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public copy(): void {

    this.panel.manager.context.invoke(this.invoice.Copy)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully copied.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }
}

