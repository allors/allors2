import { Component, OnInit, Self, HostBinding } from '@angular/core';

import { MetaService, NavigationService, PanelService, RefreshService } from '@allors/angular/services/core';
import { WorkEffort, WorkEffortInventoryAssignment } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, EditService } from '@allors/angular/material/core';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData } from '@allors/angular/material/services/core';

interface Row extends TableRow {
  object: WorkEffortInventoryAssignment;
  part: string;
  facility: string;
  quantity: string;
  uom: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'workeffortinventoryassignment-overview-panel',
  templateUrl: './workeffortinventoryassignment-overview-panel.component.html',
  providers: [PanelService]
})
export class WorkEffortInventoryAssignmentOverviewPanelComponent extends TestScope implements OnInit {
  workEffort: WorkEffort;

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: WorkEffortInventoryAssignment[] = [];

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

    this.edit = this.editService.edit();

    this.panel.name = 'workeffortinventoryassignment';
    // this.panel.title = 'Inventory Assignment';
    this.panel.title = 'Parts Used';
    this.panel.icon = 'work';
    this.panel.expandable = true;

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'part' },
        { name: 'facility' },
        { name: 'quantity' },
        { name: 'uom' },
      ],
      actions: [
        this.edit,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${this.panel.name}_${this.m.WorkEffortInventoryAssignment.name}`;

    this.panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.WorkEffort({
          name: pullName,
          object: id,
          fetch: {
            WorkEffortInventoryAssignmentsWhereAssignment: {
              include: {
                InventoryItem: {
                  Part: x,
                  Facility: x,
                  UnitOfMeasure: x,
                  NonSerialisedInventoryItem_NonSerialisedInventoryItemState: x,
                  SerialisedInventoryItem_SerialisedInventoryItemState: x
                }
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
      this.objects = loaded.collections[pullName] as WorkEffortInventoryAssignment[];

      if (this.objects) {
        this.table.total = this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            part: v.InventoryItem.Part.Name,
            facility: v.InventoryItem.Facility.Name,
            quantity: v.Quantity,
            uom: v.InventoryItem.UnitOfMeasure.Name,
          } as Row;
        });
      }
    };
  }
}
