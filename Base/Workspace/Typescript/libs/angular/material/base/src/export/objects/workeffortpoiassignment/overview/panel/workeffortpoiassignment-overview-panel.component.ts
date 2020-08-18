import { Component, OnInit, Self, HostBinding } from '@angular/core';

import { MetaService, NavigationService, PanelService, RefreshService } from '@allors/angular/services/core';
import { WorkEffort, WorkEffortPurchaseOrderItemAssignment } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, EditService } from '@allors/angular/material/core';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData } from '@allors/angular/material/services/core';

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
  templateUrl: './workeffortpoiassignment-overview-panel.component.html',
  providers: [PanelService]
})
export class WorkEffortPOIAssignmentOverviewPanelComponent extends TestScope implements OnInit {
  workEffort: WorkEffort;

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: WorkEffortPurchaseOrderItemAssignment[] = [];

  delete: Action;
  edit: Action;
  table: Table<TableRow>;

  get createData(): ObjectData {
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

    public deleteService: DeleteService,
    public editService: EditService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.panel.name = 'workeffortpurchaseorderitemassignment';
    // this.panel.title = 'PurchaseOrder Item Assignment';
    this.panel.title = 'Subcontracted Work';
    this.panel.icon = 'work';
    this.panel.expandable = true;

    this.edit = this.editService.edit();
    this.delete = this.deleteService.delete(this.panel.manager.context);

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
                  TakenViaSupplier: x,
                  TakenViaSubcontractor: x,
                },
                PurchaseOrderItem: x
              }
            }
          }
        }),
        pull.WorkEffort({
          object: id,
        }),
      );
    };

    this.panel.onPulled = (loaded) => {
      this.workEffort = loaded.objects.WorkEffort as WorkEffort;
      this.objects = loaded.collections[pullName] as WorkEffortPurchaseOrderItemAssignment[];

      if (this.objects) {
        this.table.total = this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            supplier: (v.PurchaseOrder.TakenViaSupplier && v.PurchaseOrder.TakenViaSupplier.displayName) || (v.PurchaseOrder.TakenViaSubcontractor && v.PurchaseOrder.TakenViaSubcontractor.displayName),
            description: v.PurchaseOrderItem.Description,
            orderNumber: v.PurchaseOrder.OrderNumber,
            quantity: v.Quantity,
          } as Row;
        });
      }
    };
  }
}
