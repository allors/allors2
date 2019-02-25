import { Component, Self, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, ErrorService, Action, MetaService, ContextService } from '../../../../../../angular';
import { PurchaseOrderItem, PurchaseOrder } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, EditService, MethodService } from '../../../../..';
import * as moment from 'moment';

import { MatSnackBar } from '@angular/material';

import { CreateData, ObjectService } from '../../../../../../material/base/services/object';

interface Row extends TableRow {
  object: PurchaseOrderItem;
  item: string;
  state: string;
  ordered: number;
  received: number;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseorderitem-overview-panel',
  templateUrl: './purchaseorderitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class PurchaseOrderItemOverviewPanelComponent {

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
  confirm: Action;
  approve: Action;
  continue: Action;

  get createData(): CreateData {
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
    public errorService: ErrorService,
    public methodService: MethodService,
    public deleteService: DeleteService,
    public editService: EditService,
    public snackBar: MatSnackBar
  ) {

    this.m = this.metaService.m;

    panel.name = 'purchaseordertitem';
    panel.title = 'Purchase Order Items';
    panel.icon = 'business';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = editService.edit();
    this.cancel = methodService.create(allors.context, this.m.PurchaseOrderItem.Cancel, { name: 'Cancel' });
    this.reject = methodService.create(allors.context, this.m.PurchaseOrderItem.Reject, { name: 'Reject' });
    this.confirm = methodService.create(allors.context, this.m.PurchaseOrderItem.Confirm, { name: 'Confirm' });
    this.approve = methodService.create(allors.context, this.m.PurchaseOrderItem.Approve, { name: 'Approve' });

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'item', sort },
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
        this.confirm,
        this.approve,
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
          item: (v.Part && v.Part.Name) || (v.SerialisedItem && v.SerialisedItem.Name) || '',
          state: `${v.PurchaseOrderItemState && v.PurchaseOrderItemState.Name}`,
          ordered: v.QuantityOrdered,
          received: v.QuantityReceived,
          lastModifiedDate: moment(v.LastModifiedDate).fromNow()
        } as Row;
      });
    };
  }
}
