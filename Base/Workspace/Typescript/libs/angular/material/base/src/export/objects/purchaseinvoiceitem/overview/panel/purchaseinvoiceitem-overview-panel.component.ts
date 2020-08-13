import { Component, Self, HostBinding } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { MetaService, NavigationService, PanelService, RefreshService, ContextService } from '@allors/angular/services/core';
import { PurchaseInvoice, PurchaseInvoiceItem } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, EditService } from '@allors/angular/material/core';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData, ObjectService } from '@allors/angular/material/services/core';

interface Row extends TableRow {
  object: PurchaseInvoiceItem;
  item: string;
  itemId: string;
  type: string;
  state: string;
  quantity: string;
  totalExVat: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseinvoiceitem-overview-panel',
  templateUrl: './purchaseinvoiceitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class PurchaseInvoiceItemOverviewPanelComponent extends TestScope {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  purchaseInvoiceItems: PurchaseInvoiceItem[];
  invoice: PurchaseInvoice;
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.metaService.m.PurchaseInvoice.PurchaseInvoiceItems,
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

    panel.name = 'purchaseinvoicetitem';
    panel.title = 'Purchase Invoice Items';
    panel.icon = 'business';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'item', sort },
        { name: 'itemId'},
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

    const pullName = `${panel.name}_${this.m.PurchaseInvoiceItem.name}`;
    const invoicePullName = `${panel.name}_${this.m.PurchaseInvoice.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.PurchaseInvoice({
          name: pullName,
          object: id,
          fetch: {
            PurchaseInvoiceItems: {
              include: {
                PurchaseInvoiceItemState: x,
                Part: x,
                InvoiceItemType: x
              }
            }
          }
        }),
        pull.PurchaseInvoice({
          name: invoicePullName,
          object: id
        }),
      );
    };

    panel.onPulled = (loaded) => {

      this.purchaseInvoiceItems = loaded.collections[pullName] as PurchaseInvoiceItem[];
      this.invoice = loaded.objects[invoicePullName] as PurchaseInvoice;
      this.table.total = loaded.values[`${pullName}_total`] || this.purchaseInvoiceItems.length;
      this.table.data = this.purchaseInvoiceItems.map((v) => {
        return {
          object: v,
          item: (v.Part && v.Part.Name) || '',
          itemId: v.SerialisedItem && v.SerialisedItem.ItemNumber,
          type: `${v.InvoiceItemType && v.InvoiceItemType.Name}`,
          state: `${v.PurchaseInvoiceItemState && v.PurchaseInvoiceItemState.Name}`,
          quantity: v.Quantity,
          totalExVat: v.TotalExVat
        } as Row;
      });
    };
  }
}
