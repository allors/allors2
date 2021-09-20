import { Component, Self, OnInit, HostBinding } from '@angular/core';

import { MetaService, NavigationService, PanelService, RefreshService } from '@allors/angular/services/core';
import { InventoryItem, SerialisedInventoryItem, SerialisedItem } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, OverviewService, EditService } from '@allors/angular/material/core';
import { TestScope, Action, ActionTarget } from '@allors/angular/core';
import { ObjectData, ObjectService } from '@allors/angular/material/services/core';
interface Row extends TableRow {
  object: InventoryItem;
  facility: string;
  item: string;
  quantity: number;
  state: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialisedinventoryitem-overview-panel',
  templateUrl: './serialisedinventoryitem-overview-panel.component.html',
  providers: [PanelService]
})
export class SerialisedInventoryItemComponent extends TestScope implements OnInit {
  serialisedItem: SerialisedItem;

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

    const { pull, x } = this.metaService;

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
        { name: 'quantity', sort: true },
        { name: 'state', sort: true },
      ],
      defaultAction: this.changeInventory,
    });

    const inventoryPullName = `${this.panel.name}_${this.m.SerialisedInventoryItem.name}`;
    const serialiseditemPullName = `${this.panel.name}_${this.m.SerialisedItem.name}`;
 
    this.panel.onPull = (pulls) => {
      const id = this.panel.manager.id;

      pulls.push(
        pull.Part({
          name: inventoryPullName,
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
        }),
        pull.SerialisedItem({
          name: serialiseditemPullName,
          object: id,
          fetch: {
            SerialisedInventoryItemsWhereSerialisedItem: {
              include: {
                Part: x,
                SerialisedInventoryItemState: x,
                Facility: x,
                UnitOfMeasure: x
              }
            }
          },
        }),
        pull.SerialisedItem({
          object: id,
          include: {
            PartWhereSerialisedItem: x
          }
        })
      );

      this.panel.onPulled = (loaded) => {

        this.serialisedItem = loaded.objects.SerialisedItem as SerialisedItem;
        const inventoryObjects = loaded.collections[inventoryPullName] as SerialisedInventoryItem[] ?? [];

        const serialisedItemobjects = loaded.collections[serialiseditemPullName] as SerialisedInventoryItem[] ?? [];
        const serialisedItemobjectsforPart = serialisedItemobjects.filter(v => v.Part === this.serialisedItem?.PartWhereSerialisedItem)

        this.objects = inventoryObjects.concat(serialisedItemobjectsforPart);

        if (this.objects) {
          this.table.total = loaded.values[`${this.objects.length}_total`] || this.objects.length;
          this.table.data = this.objects.map((v) => {
            return {
              object: v,
              facility: v.Facility.Name,
              item: v.SerialisedItem?.displayName,
              quantity: v.Quantity,
              state: v.SerialisedInventoryItemState ? v.SerialisedInventoryItemState.Name : ''
            } as Row;
          });
        }
      };
    };
  }
}
