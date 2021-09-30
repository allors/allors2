import { Component, Self, HostBinding } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { formatDistance } from 'date-fns';

import { MetaService, NavigationService, PanelService, RefreshService, ContextService } from '@allors/angular/services/core';
import { PurchaseOrderItem, PurchaseOrder } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, EditService, MethodService } from '@allors/angular/material/core';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData, ObjectService } from '@allors/angular/material/services/core';

interface Row extends TableRow {
  object: PurchaseOrderItem;
  item: string;
  itemId; string;
  type: string;
  state: string;
  ordered: string;
  received: string;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseorderitem-overview-panel',
  templateUrl: './purchaseorderitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class PurchaseOrderItemOverviewPanelComponent extends TestScope {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  order: PurchaseOrder;
  objects: PurchaseOrderItem[];
  table: Table<Row>;

  delete: Action;
  edit: Action;
  cancel: Action;
  reject: Action;
  reopen: Action;
  quickReceive: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.metaService.m.PurchaseOrder.PurchaseOrderItems,
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

    panel.name = 'purchaseordertitem';
    panel.title = 'Purchase Order Items';
    panel.icon = 'business';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = editService.edit();
    this.cancel = methodService.create(allors.context, this.m.PurchaseOrderItem.Cancel, { name: 'Cancel' });
    this.reject = methodService.create(allors.context, this.m.PurchaseOrderItem.Reject, { name: 'Reject' });
    this.reopen = methodService.create(allors.context, this.m.PurchaseOrderItem.Reopen, { name: 'Reopen' });
    this.quickReceive = methodService.create(allors.context, this.m.PurchaseOrderItem.QuickReceive, { name: 'QuickReceive' });

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'item', sort },
        { name: 'itemId' },
        { name: 'type', sort },
        { name: 'state', sort },
        { name: 'ordered', sort },
        { name: 'received', sort },
        { name: 'lastModifiedDate', sort },
      ],
      actions: [
        this.edit,
        this.delete,
        this.cancel,
        this.reject,
        this.reopen,
        this.quickReceive
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${panel.name}_${this.m.PurchaseOrderItem.name}`;
    const orderPullName = `${panel.name}_${this.m.PurchaseOrder.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.PurchaseOrder({
          name: pullName,
          object: id,
          fetch: {
            PurchaseOrderItems: {
              include: {
                InvoiceItemType: x,
                PurchaseOrderItemState: x,
                Part: x,
                SerialisedItem: x,
              }
            }
          }
        }),
        pull.PurchaseOrder({
          name: orderPullName,
          object: id
        }),
      );
    };

    panel.onPulled = (loaded) => {

      this.objects = loaded.collections[pullName] as PurchaseOrderItem[];
      this.order = loaded.objects[orderPullName] as PurchaseOrder;
      this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
      this.table.data = this.objects.map((v) => {
        return {
          object: v,
          item: (v.Part && v.Part.Name) || (v.SerialisedItem && v.SerialisedItem.Name) || v.Description,
          itemId: v.SerialisedItem && v.SerialisedItem.ItemNumber,
          type: `${v.InvoiceItemType && v.InvoiceItemType.Name}`,
          state: `${v.PurchaseOrderItemState && v.PurchaseOrderItemState.Name}`,
          ordered: v.QuantityOrdered,
          received: v.QuantityReceived,
          lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date())
        } as Row;
      });
    };
  }
}
