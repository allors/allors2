import { Component, Self, OnInit, OnDestroy, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood, SalesOrder, RepeatingSalesInvoice, Good, WorkEffort, PurchaseOrder, PurchaseInvoice, Shipment, NonUnifiedGood, BasePrice, PriceComponent, ProductIdentificationType, SerialisedItem, RequestForQuote, ProductQuote, CustomerShipment, Quote } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';
import { PrintService } from '../../../../services/actions';

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
      const { m, pull, x } = this.metaService;

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
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully invoiced.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.shipment.Cancel)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public hold(): void {

    this.panel.manager.context.invoke(this.shipment.Hold)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully put on hold.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public continue(): void {

    this.panel.manager.context.invoke(this.shipment.Continue)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully removed from hold.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public pick(): void {

    this.panel.manager.context.invoke(this.shipment.Pick)
      .subscribe((invoked: Invoked) => {
        this.panel.toggle();
        this.snackBar.open('Successfully picked.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
      this.saveService.errorHandler);
  }

  public ship(): void {

    this.panel.manager.context.invoke(this.shipment.Ship)
      .subscribe((invoked: Invoked) => {
        this.panel.toggle();
        this.snackBar.open('Successfully shipped.', 'close', { duration: 5000 });
        this.refreshService.refresh();
      },
      this.saveService.errorHandler);
  }
}
