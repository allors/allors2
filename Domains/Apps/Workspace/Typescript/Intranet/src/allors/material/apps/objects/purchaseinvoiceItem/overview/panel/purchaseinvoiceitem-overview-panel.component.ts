import { Component, Self, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, ErrorService, Action, MetaService, ContextService } from '../../../../../../angular';
import { PurchaseInvoiceItem, PurchaseInvoice } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, EditService } from '../../../../..';

import { MatSnackBar } from '@angular/material';

import { CreateData, ObjectService } from '../../../../../../material/base/services/object';

interface Row extends TableRow {
  object: PurchaseInvoiceItem;
  item: string;
  type: string;
  status: string;
  quantity: number;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseinvoiceitem-overview-panel',
  templateUrl: './purchaseinvoiceitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class PurchaseInvoiceItemOverviewPanelComponent {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  purchaseInvoiceItems: PurchaseInvoiceItem[];
  invoice: PurchaseInvoice;
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): CreateData {
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
    public errorService: ErrorService,
    public editService: EditService,
    public deleteService: DeleteService,
    public snackBar: MatSnackBar
  ) {

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
        { name: 'type', sort },
        { name: 'state', sort },
        { name: 'quantity', sort },
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
                Product: x,
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
          item: (v.Product && v.Product.Name) || (v.Part && v.Part.Name) || '',
          type: `${v.InvoiceItemType && v.InvoiceItemType.Name}`,
          status: `${v.PurchaseInvoiceItemState && v.PurchaseInvoiceItemState.Name}`,
          quantity: v.Quantity,
        } as Row;
      });
    };
  }
}
