import { Component, Self, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, ErrorService, Action, MetaService, ActionTarget, Invoked } from '../../../../../../angular';
import { RequestItem as SalesOrderItem, PurchaseInvoiceItem } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table } from '../../../../..';

import { ISessionObject } from 'src/allors/framework';
import { MatSnackBar } from '@angular/material';

import { CreateData, ObjectService, EditData, ObjectData } from '../../../../../../material/base/services/object';
interface Row extends TableRow {
  object: PurchaseInvoiceItem;
  item: string;
  quantity: number;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseinvoiceitem-overview-panel',
  templateUrl: './purchaseinvoiceitem-overview-panel.component.html',
  providers: [PanelService]
})
export class PurchaseInvoiceItemOverviewPanelComponent {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  purchaseInvoiceItems: PurchaseInvoiceItem[];
  table: Table<Row>;

  delete: Action;

  edit: Action = {
    name: (target: ActionTarget) => 'Edit',
    description: (target: ActionTarget) => 'Edit',
    disabled: (target: ActionTarget) => !this.objectService.hasEditControl(target as ISessionObject),
    execute: (target: ActionTarget) => this.objectService.edit(target as ISessionObject).subscribe((v) => this.refreshService.refresh()),
    result: null
  };

  get createData(): CreateData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.metaService.m.Request.RequestItems,
    };
  }

  constructor(
    @Self() public panel: PanelService,
    public objectService: ObjectService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
    public snackBar: MatSnackBar
  ) {

    this.m = this.metaService.m;

    panel.name = 'purchaseinvoicetitem';
    panel.title = 'Purchase Invoice Items';
    panel.icon = 'contacts';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'item' },
        { name: 'quantity' },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit
    });

    const pullName = `${panel.name}_${this.m.PurchaseInvoiceItem.name}`;

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
                InvoiceItemType: x,
              }
            }
          }
        }));
    };

    panel.onPulled = (loaded) => {

      this.purchaseInvoiceItems = loaded.collections[pullName] as PurchaseInvoiceItem[];
      this.table.total = loaded.values[`${pullName}_total`] || this.purchaseInvoiceItems.length;
      this.table.data = this.purchaseInvoiceItems.map((v) => {
        return {
          object: v,
          item:  (v.InvoiceItemType && v.InvoiceItemType.Name) || '',
          quantity: v.Quantity,
        } as Row;
      });
    };

  }
}
