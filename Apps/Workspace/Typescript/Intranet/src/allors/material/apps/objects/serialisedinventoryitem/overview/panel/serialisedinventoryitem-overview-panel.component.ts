import { Component, Self, OnInit, HostBinding } from '@angular/core';

import { NavigationService, Action, PanelService, RefreshService, MetaService, ActionTarget, TestScope } from '../../../../../../angular';
import { Meta } from '../../../../../../meta';
import { InventoryItem, SerialisedInventoryItem } from '../../../../../../domain';
import { DeleteService, EditService, TableRow, Table, Sorter } from '../../../../..';
import { ObjectService, ObjectData, OverviewService } from '../../../../../../material';

interface Row extends TableRow {
  object: InventoryItem;
  facility: string;
  item: string;
  state: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialisedinventoryitem-overview-panel',
  templateUrl: './serialisedinventoryitem-overview-panel.component.html',
  providers: [PanelService]
})
export class SerialisedInventoryItemComponent extends TestScope implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  table: Table<Row>;

  edit: Action;
  changeInventory: Action;

  objects: SerialisedInventoryItem[];

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public objectService: ObjectService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public editService: EditService
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    const { pull, x, m } = this.metaService;

    this.panel.name = 'serialised Inventory item';
    this.panel.title = 'Serialised Inventory items';
    this.panel.icon = 'link';
    this.panel.expandable = true;

    this.edit = this.editService.edit();
    this.changeInventory = {
      name: 'changeinventory',
      displayName: () => 'Change Inventory',
      description: () => '',
      disabled: () => false,
      execute: (target: ActionTarget) => {
        if (!Array.isArray(target)) {
          this.factoryService.create(this.m.InventoryItemTransaction, {
            associationId: target.id,
            associationObjectType: target.objectType,
          });
        }
      },
      result: null
    };

    this.table = new Table({
      selection: false,
      columns: [
        { name: 'facility', sort: true },
        { name: 'item', sort: true },
        { name: 'state', sort: true },
      ],
      defaultAction: this.changeInventory,
    });

    const pullName = `${this.panel.name}_${this.m.SerialisedInventoryItem.name}`;

    this.panel.onPull = (pulls) => {
      const id = this.panel.manager.id;

      pulls.push(
        pull.Part({
          name: pullName,
          object: id,
          fetch: {
            InventoryItemsWherePart: {
              include: {
                SerialisedInventoryItem_SerialisedInventoryItemState: x,
                Facility: x,
                UnitOfMeasure: x
              }
            }
          },
        })
      );

      this.panel.onPulled = (loaded) => {

        this.objects = loaded.collections[pullName] as SerialisedInventoryItem[];
        this.objects = this.objects.filter(v => v.Quantity > 0);

        if (this.objects) {
          this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
          this.table.data = this.objects.map((v) => {
            return {
              object: v,
              facility: v.Facility.Name,
              item: v.SerialisedItem.displayName,
              state: v.SerialisedInventoryItemState ? v.SerialisedInventoryItemState.Name : ''
            } as Row;
          });
        }
      };
    };
  }
}
