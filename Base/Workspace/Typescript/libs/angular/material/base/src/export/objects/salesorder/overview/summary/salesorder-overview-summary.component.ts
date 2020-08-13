import { Component, Self, OnInit, OnDestroy, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, SalesOrder, ProductQuote, SalesOrderItem, Good, Shipment, BillingProcess, SerialisedInventoryItemState } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId, PrintService } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';


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
  shipments: Shipment[] = [];
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
    private saveService: SaveService,
    public snackBar: MatSnackBar) {

    this.m = this.metaService.m;

    this.print = printService.print();

    panel.name = 'summary';

    const salesOrderPullName = `${panel.name}_${this.m.SalesOrder.name}`;
    const salesInvoicePullName = `${panel.name}_${this.m.SalesInvoice.name}`;
    const shipmentPullName = `${panel.name}_${this.m.Shipment.name}`;
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
            PrintDocument: {
              Media: x
            },
            ShipToAddress: {
              Country: x,
            },
            BillToEndCustomerContactMechanism: {
              PostalAddress_Country: x
            },
            ShipToEndCustomerAddress: {
              Country: x
            },
            BillToContactMechanism: {
              PostalAddress_Country: x
            }
          }
        }),
        pull.SalesOrder({
          name: salesInvoicePullName,
          object: this.panel.manager.id,
          fetch: { SalesInvoicesWhereSalesOrder: x }
        }),
        pull.SalesOrder({
          name: shipmentPullName,
          object: this.panel.manager.id,
          fetch: {
            SalesOrderItems: {
              OrderShipmentsWhereOrderItem: {
                ShipmentItem: {
                  ShipmentWhereShipmentItem: x
                }
              }
            }
          }
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
      this.billingForOrderItems = this.billingProcesses.find((v: BillingProcess) => v.UniqueId === 'ab01ccc2-6480-4fc0-b20e-265afd41fae2');
      this.inventoryItemStates = loaded.collections[serialisedInventoryItemStatePullName] as SerialisedInventoryItemState[];

      this.salesInvoices = loaded.collections[salesInvoicePullName] as SalesInvoice[];
      this.shipments = loaded.collections[shipmentPullName] as Shipment[];
    };
  }

  public approve(): void {

    this.panel.manager.context.invoke(this.order.Approve)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public setReadyForPosting() {

    this.panel.manager.context.invoke(this.order.SetReadyForPosting)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully set ready for posting.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public reopen() {

    this.panel.manager.context.invoke(this.order.Reopen)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public post() {

    this.panel.manager.context.invoke(this.order.Post)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully posted.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public accept() {

    this.panel.manager.context.invoke(this.order.Accept)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully accepted.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public revise(): void {

    this.panel.manager.context.invoke(this.order.Revise)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully revised.', 'close', { duration: 5000 });
      },
        this.saveService.errorHandler);
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.order.Cancel)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public reject(): void {

    this.panel.manager.context.invoke(this.order.Reject)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public hold(): void {

    this.panel.manager.context.invoke(this.order.Hold)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully put on hold.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public continue(): void {

    this.panel.manager.context.invoke(this.order.Continue)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully removed from hold.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public finish(): void {

    this.panel.manager.context.invoke(this.order.Continue)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully finished.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public ship(): void {

    this.panel.manager.context.invoke(this.order.Ship)
      .subscribe((invoked: Invoked) => {
        this.panel.toggle();
        this.snackBar.open('Customer shipment successfully created.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
      this.saveService.errorHandler);
  }

  public invoice(): void {

    this.panel.manager.context.invoke(this.order.Invoice)
      .subscribe((invoked: Invoked) => {
        this.panel.toggle();
        this.snackBar.open('Sales invoice successfully created.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
      this.saveService.errorHandler);
  }
}
