import { Component, OnInit, Self, HostBinding, AfterViewInit, OnDestroy, Injector, Input } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { formatDistance, format, isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, RefreshService, Action, NavigationService, PanelService, PanelManagerService, ContextService, NavigationActivatedRoute, ActionTarget } from '@allors/angular/core';
import { CommunicationEvent, ContactMechanism, CustomerShipment, ShipmentItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState, InventoryItem, NonSerialisedInventoryItem, Part, NonUnifiedPart, Organisation, SupplierOffering, PartyContactMechanism, PartyRate, ProductIdentification, PurchaseInvoiceItem, PurchaseInvoice, PurchaseOrderItem, PurchaseOrder, QuoteItem, ProductQuote, RepeatingSalesInvoice, RequestForQuote, Quote, SalesInvoiceItem } from '@allors/domain/generated';
import { TableRow, Table, EditService, DeleteService, ObjectData, ObjectService, OverviewService, MethodService } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { RoleType } from '@allors/meta/system';
import { Pull, Fetch, Step, Sort, Equals } from '@allors/data/system';

interface Row extends TableRow {
  object: SalesInvoiceItem;
  item: string;
  itemId: string;
  type: string;
  state: string;
  quantity: string;
  totalExVat: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'salesinvoiceitem-overview-panel',
  templateUrl: './salesinvoiceitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class SalesInvoiceItemOverviewPanelComponent extends TestScope {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  salesInvoiceItems: SalesInvoiceItem[];
  invoice: SalesInvoice;
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.metaService.m.SalesInvoice.SalesInvoiceItems,
    };
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public objectService: ObjectService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public editService: EditService,
    public deleteService: DeleteService,
    public snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'salesinvoicetitem';
    panel.title = 'Sales Invoice Items';
    panel.icon = 'business';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'item', sort },
        { name: 'itemId' },
        { name: 'type', sort },
        { name: 'state', sort },
        { name: 'quantity', sort },
        { name: 'totalExVat', sort },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${panel.name}_${this.m.SalesInvoiceItem.name}`;
    const invoicePullName = `${panel.name}_${this.m.SalesInvoice.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.SalesInvoice({
          name: pullName,
          object: id,
          fetch: {
            SalesInvoiceItems: {
              include: {
                SalesInvoiceItemState: x,
                InvoiceItemType: x,
                SerialisedItem: x,
              }
            }
          }
        }),
        pull.SalesInvoice({
          name: invoicePullName,
          object: id
        }),
      );
    };

    panel.onPulled = (loaded) => {

      this.salesInvoiceItems = loaded.collections[pullName] as SalesInvoiceItem[];
      this.invoice = loaded.objects[invoicePullName] as SalesInvoice;
      this.table.total = loaded.values[`${pullName}_total`] || this.salesInvoiceItems.length;
      this.table.data = this.salesInvoiceItems.map((v) => {
        return {
          object: v,
          item: (v.Product && v.Product.Name) || (v.SerialisedItem && v.SerialisedItem.Name) || '',
          itemId: `${v.SerialisedItem && v.SerialisedItem.ItemNumber}`,
          type: `${v.InvoiceItemType && v.InvoiceItemType.Name}`,
          state: `${v.SalesInvoiceItemState && v.SalesInvoiceItemState.Name}`,
          quantity: v.Quantity,
          totalExVat: v.TotalExVat
        } as Row;
      });
    };
  }
}
