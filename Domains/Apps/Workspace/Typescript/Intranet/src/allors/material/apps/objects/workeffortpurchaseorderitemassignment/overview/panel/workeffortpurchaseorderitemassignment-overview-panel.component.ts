import { Component, Self, OnInit, HostBinding } from '@angular/core';
import { NavigationService, Action, PanelService, RefreshService, ErrorService, MetaService } from '../../../../../../angular';
import { WorkEffortPurchaseOrderItemAssignment } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, EditService, Table, OverviewService, CreateData } from '../../../../..';
import * as moment from 'moment';

interface Row extends TableRow {
  object: WorkEffortPurchaseOrderItemAssignment;
  supplier: string;
  orderNumber: string;
  description: string;
  quantity: number;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'workeffortpurchaseorderitemassignment-overview-panel',
  templateUrl: './workeffortpurchaseorderitemassignment-overview-panel.component.html',
  providers: [PanelService]
})
export class WorkEffortPurchaseOrderItemAssignmentOverviewPanelComponent implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: WorkEffortPurchaseOrderItemAssignment[] = [];

  delete: Action;
  edit: Action;
  table: Table<TableRow>;

  get createData(): CreateData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
    public editService: EditService,
  ) {

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.panel.name = 'workeffortpurchaseorderitemassignment';
    this.panel.title = 'Work Effort PurchaseOrder Item Assignment';
    this.panel.icon = 'work';
    this.panel.expandable = true;

    this.edit = this.editService.edit();
    this.delete = this.deleteService.delete(this.panel.manager.context);

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'supplier' },
        { name: 'orderNumber' },
        { name: 'description' },
        { name: 'quantity' },
      ],
      actions: [
        this.edit,
        this.delete
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${this.panel.name}_${this.m.WorkEffortPurchaseOrderItemAssignment.name}`;

    this.panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.WorkEffort({
          name: pullName,
          object: id,
          fetch: {
            WorkEffortPurchaseOrderItemAssignmentsWhereAssignment: {
              include: {
                PurchaseOrder: {
                  TakenViaSupplier: x
                },
                PurchaseOrderItem: x
              }
            }
          }
        }),
      );
    };

    this.panel.onPulled = (loaded) => {
      this.objects = loaded.collections[pullName] as WorkEffortPurchaseOrderItemAssignment[];

      if (this.objects) {
        this.table.total = this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            supplier: v.PurchaseOrder.TakenViaSupplier.displayName,
            description: v.PurchaseOrderItem.displayName,
            orderNumber: v.PurchaseOrder.OrderNumber,
            quantity: v.Quantity,
          } as Row;
        });
      }
    };
  }
}
