import { Component, OnInit, Self, HostBinding, AfterViewInit, OnDestroy, Injector, Input } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { formatDistance, format, isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, RefreshService, Action, NavigationService, PanelService, PanelManagerService, ContextService, NavigationActivatedRoute, ActionTarget } from '@allors/angular/services/core';
import { CommunicationEvent, ContactMechanism, CustomerShipment, ShipmentItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState, InventoryItem, NonSerialisedInventoryItem, Part, NonUnifiedPart, Organisation, SupplierOffering, PartyContactMechanism, PartyRate, ProductIdentification, PurchaseInvoiceItem, PurchaseInvoice, PurchaseOrderItem, PurchaseOrder, QuoteItem, ProductQuote } from '@allors/domain/generated';
import { TableRow, Table, EditService, DeleteService, ObjectData, ObjectService, OverviewService, MethodService } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { RoleType } from '@allors/meta/system';
import { Pull, Fetch, Step } from '@allors/data/system';


interface Row extends TableRow {
  object: QuoteItem;
  itemType: string;
  item: string;
  itemId: string;
  state: string;
  quantity: string;
  price: string;
  totalAmount: string;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'quoteitem-overview-panel',
  templateUrl: './quoteitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class QuoteItemOverviewPanelComponent extends TestScope {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  quote: ProductQuote;
  quoteItems: QuoteItem[];
  table: Table<Row>;
  quoteItem: QuoteItem;

  delete: Action;
  edit: Action;
  cancel: Action;
  reject: Action;
  submit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.metaService.m.Quote.QuoteItems,
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
    public editService: EditService,
    public deleteService: DeleteService,
    public snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'quoteitem';
    panel.title = 'Quote Items';
    panel.icon = 'contacts';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = this.editService.edit();
    this.cancel = methodService.create(allors.context, this.m.QuoteItem.Cancel, { name: 'Cancel' });
    this.reject = methodService.create(allors.context, this.m.QuoteItem.Reject, { name: 'Reject' });
    this.submit = methodService.create(allors.context, this.m.QuoteItem.Submit, { name: 'Submit' });

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'itemType' },
        { name: 'item' },
        { name: 'itemId' },
        { name: 'state' },
        { name: 'quantity'},
        { name: 'price'},
        { name: 'totalAmount'},
        { name: 'lastModifiedDate', sort },
      ],
      actions: [
        this.edit,
        this.delete,
        this.cancel,
        this.reject,
        this.submit
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${panel.name}_${this.m.QuoteItem.name}`;
    const quotePullName = `${panel.name}_${this.m.ProductQuote.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Quote({
          name: pullName,
          object: id,
          fetch: {
            QuoteItems: {
              include: {
                Product: x,
                SerialisedItem: x,
                InvoiceItemType: x,
              }
            }
          }
        }),
        pull.Quote({
          name: quotePullName,
          object: id
        }),
      );
    };

    panel.onPulled = (loaded) => {

      this.quoteItems = loaded.collections[pullName] as QuoteItem[];
      this.quote = loaded.objects[quotePullName] as ProductQuote;
      this.table.total = loaded.values[`${pullName}_total`] || this.quoteItems.length;
      this.table.data = this.quoteItems.map((v) => {
        return {
          object: v,
          itemType: v.InvoiceItemType.Name,
          item: (v.Product && v.Product.Name) || (v.SerialisedItem && v.SerialisedItem.Name) || '',
          itemId: `${v.SerialisedItem && v.SerialisedItem.ItemNumber}`,
          state: `${v.QuoteItemState && v.QuoteItemState.Name}`,
          quantity: v.Quantity,
          price: v.UnitPrice,
          totalAmount: v.TotalExVat,
          lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date())
        } as Row;
      });
    };
  }
}
