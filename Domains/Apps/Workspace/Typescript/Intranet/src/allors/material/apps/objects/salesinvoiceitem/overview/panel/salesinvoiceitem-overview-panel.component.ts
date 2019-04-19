import { Component, Self, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService,  Action, MetaService, ContextService } from '../../../../../../angular';
import { SalesInvoiceItem, SalesInvoice } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, EditService } from '../../../../..';
import * as moment from 'moment';

import { MatSnackBar } from '@angular/material';

import { ObjectData, ObjectService } from '../../../../../base/services/object';

interface Row extends TableRow {
  object: SalesInvoiceItem;
  item: string;
  type: string;
  status: string;
  quantity: number;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'salesinvoiceitem-overview-panel',
  templateUrl: './salesinvoiceitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class SalesInvoiceItemOverviewPanelComponent {

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
          type: `${v.InvoiceItemType && v.InvoiceItemType.Name}`,
          status: `${v.SalesInvoiceItemState && v.SalesInvoiceItemState.Name}`,
          quantity: v.Quantity,
        } as Row;
      });
    };
  }
}
