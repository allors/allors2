import { Component, OnInit, Self, HostBinding, AfterViewInit, OnDestroy, Injector, Input } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { formatDistance, format, isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, RefreshService, Action, NavigationService, PanelService, PanelManagerService, ContextService, NavigationActivatedRoute, ActionTarget } from '@allors/angular/services/core';
import { CommunicationEvent, ContactMechanism, CustomerShipment, ShipmentItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState, InventoryItem, NonSerialisedInventoryItem, Part, NonUnifiedPart, Organisation, SupplierOffering, PartyContactMechanism, PartyRate, ProductIdentification, PurchaseInvoiceItem, PurchaseInvoice, PurchaseOrderItem, PurchaseOrder, QuoteItem, ProductQuote, RepeatingSalesInvoice, RequestForQuote, Quote, SalesInvoiceItem, SalesOrder, SalesOrderItem, SalesTerm, SerialisedItem, Party, Shipment } from '@allors/domain/generated';
import { TableRow, Table, EditService, DeleteService, ObjectData, ObjectService, OverviewService, MethodService } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { RoleType } from '@allors/meta/system';
import { Pull, Fetch, Step, Sort, Equals } from '@allors/data/system';

interface Row extends TableRow {
  object: ShipmentItem;
  item: string;
  state: string;
  quantity: string;
  picked: string;
  shipped: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'shipmentitem-overview-panel',
  templateUrl: './shipmentitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class ShipmentItemOverviewPanelComponent extends TestScope {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  shipment: Shipment;
  shipmentItems: ShipmentItem[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.metaService.m.Shipment.ShipmentItems,
    };
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public objectService: ObjectService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public methodService: MethodService,
    public deleteService: DeleteService,
    public editService: EditService,
    public snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'shipmentitem';
    panel.title = 'Shipment Items';
    panel.icon = 'shopping_cart';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'item', sort },
        { name: 'state', sort },
        { name: 'quantity', sort },
        { name: 'picked', sort },
        { name: 'shipped', sort },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${panel.name}_${this.m.ShipmentItem.name}`;
    const shipmentPullName = `${panel.name}_${this.m.Shipment.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Shipment({
          name: pullName,
          object: id,
          fetch: {
            ShipmentItems: {
              include: {
                ShipmentItemState: x,
                Good: x,
                Part: x
              }
            }
          }
        }),
        pull.Shipment({
          name: shipmentPullName,
          object: id
        }),
      );
    };

    panel.onPulled = (loaded) => {

      this.shipmentItems = loaded.collections[pullName] as ShipmentItem[];
      this.shipment = loaded.objects[shipmentPullName] as Shipment;
      this.table.total = loaded.values[`${pullName}_total`] || this.shipmentItems.length;
      this.table.data = this.shipmentItems.map((v) => {
        return {
          object: v,
          item: (v.Good && v.Good.Name) || (v.Part && v.Part.Name) || '',
          state: `${v.ShipmentItemState && v.ShipmentItemState.Name}`,
          quantity: v.Quantity,
          picked: v.QuantityPicked,
          shipped: v.QuantityShipped,
        } as Row;
      });
    };
  }
}
