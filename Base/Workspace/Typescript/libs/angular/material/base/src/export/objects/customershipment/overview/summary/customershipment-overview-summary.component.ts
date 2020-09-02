import { Component, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { MetaService, NavigationService, PanelService, RefreshService, Invoked } from '@allors/angular/services/core';
import { SalesOrder, CustomerShipment } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { PrintService } from '@allors/angular/base';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'customershipment-overview-summary',
  templateUrl: './customershipment-overview-summary.component.html',
  providers: [PanelService]
})
export class CustomerShipmentOverviewSummaryComponent {

  m: Meta;

  shipment: CustomerShipment;
  salesOrders: SalesOrder[] = [];

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    public printService: PrintService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    public snackBar: MatSnackBar) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const shipmentPullName = `${panel.name}_${this.m.Shipment.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      pulls.push(

        pull.Shipment({
          name: shipmentPullName,
          object: this.panel.manager.id,
          include: {
            ShipmentItems: {
              Good: x,
              Part: x
            },
            ShipFromParty: x,
            ShipFromContactPerson: x,
            ShipToParty: x,
            ShipToContactPerson: x,
            ShipmentState: x,
            CreatedBy: x,
            LastModifiedBy: x,
            ShipToAddress: {
              Country: x,
            },
          }
        }),
        pull.Shipment({
          object: this.panel.manager.id,
          fetch: {
            ShipmentItems: {
              OrderShipmentsWhereShipmentItem: {
                OrderItem: {
                  OrderWhereValidOrderItem: x
                }
              }
            }
          }
        }),
      );
    };

    panel.onPulled = (loaded) => {
      this.shipment = loaded.objects[shipmentPullName] as CustomerShipment;
      this.salesOrders = loaded.collections.Orders as SalesOrder[];
    };
  }

  public invoice(): void {

    this.panel.manager.context.invoke(this.shipment.Invoice)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully invoiced.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.shipment.Cancel)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public hold(): void {

    this.panel.manager.context.invoke(this.shipment.Hold)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully put on hold.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public continue(): void {

    this.panel.manager.context.invoke(this.shipment.Continue)
      .subscribe(() => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully removed from hold.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public pick(): void {

    this.panel.manager.context.invoke(this.shipment.Pick)
      .subscribe(() => {
        this.panel.toggle();
        this.snackBar.open('Successfully picked.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
      this.saveService.errorHandler);
  }

  public ship(): void {

    this.panel.manager.context.invoke(this.shipment.Ship)
      .subscribe(() => {
        this.panel.toggle();
        this.snackBar.open('Successfully shipped.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
      this.saveService.errorHandler);
  }
}
