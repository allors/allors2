import { Component, Self, HostBinding } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { formatDistance } from 'date-fns';

import { MetaService, NavigationService, PanelService, RefreshService, ContextService } from '@allors/angular/services/core';
import { SalesOrder, SalesOrderItem } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, EditService, MethodService } from '@allors/angular/material/core';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData, ObjectService } from '@allors/angular/material/services/core';

interface Row extends TableRow {
  object: SalesOrderItem;
  item: string;
  itemId: string;
  type: string;
  state: string;
  ordered: string;
  shipped: string;
  reserved: string;
  short: string;
  returned: string;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'salesorderitem-overview-panel',
  templateUrl: './salesorderitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class SalesOrderItemOverviewPanelComponent extends TestScope {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  order: SalesOrder;
  salesOrderItems: SalesOrderItem[];
  table: Table<Row>;

  delete: Action;
  edit: Action;
  cancel: Action;
  reject: Action;
  reopen: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.metaService.m.SalesOrder.SalesOrderItems,
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

    panel.name = 'salesordertitem';
    panel.title = 'Sales Order Items';
    panel.icon = 'contacts';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = editService.edit();
    this.cancel = methodService.create(allors.context, this.m.SalesOrderItem.Cancel, { name: 'Cancel' });
    this.reject = methodService.create(allors.context, this.m.SalesOrderItem.Reject, { name: 'Reject' });
    this.reopen = methodService.create(allors.context, this.m.SalesOrderItem.Reopen, { name: 'Reopen' });

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'item', sort },
        { name: 'itemId' },
        { name: 'type', sort },
        { name: 'state', sort },
        { name: 'ordered', sort },
        { name: 'shipped', sort },
        { name: 'reserved', sort },
        { name: 'short', sort },
        { name: 'returned', sort },
        { name: 'lastModifiedDate', sort },
      ],
      actions: [
        this.edit,
        this.delete,
        this.cancel,
        this.reject,
        this.reopen,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${panel.name}_${this.m.SalesOrderItem.name}`;
    const orderPullName = `${panel.name}_${this.m.SalesOrder.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.SalesOrder({
          name: pullName,
          object: id,
          fetch: {
            SalesOrderItems: {
              include: {
                InvoiceItemType: x,
                SalesOrderItemState: x,
                Product: x,
                SerialisedItem: x,
              }
            }
          }
        }),
        pull.SalesOrder({
          name: orderPullName,
          object: id,
        }),
      );
    };

    panel.onPulled = (loaded) => {

      this.salesOrderItems = loaded.collections[pullName] as SalesOrderItem[];
      this.order = loaded.objects[orderPullName] as SalesOrder;
      this.table.total = loaded.values[`${pullName}_total`] || this.salesOrderItems.length;
      this.table.data = this.salesOrderItems.map((v) => {
        return {
          object: v,
          item: (v.Product && v.Product.Name) || (v.SerialisedItem && v.SerialisedItem.Name) || '',
          itemId: `${v.SerialisedItem && v.SerialisedItem.ItemNumber}`,
          type: `${v.InvoiceItemType && v.InvoiceItemType.Name}`,
          state: `${v.SalesOrderItemState && v.SalesOrderItemState.Name}`,
          ordered: v.QuantityOrdered,
          shipped: v.QuantityShipped,
          reserved: v.QuantityReserved,
          short: v.QuantityShortFalled,
          returned: v.QuantityReturned,
          lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date())
        } as Row;
      });
    };
  }
}
