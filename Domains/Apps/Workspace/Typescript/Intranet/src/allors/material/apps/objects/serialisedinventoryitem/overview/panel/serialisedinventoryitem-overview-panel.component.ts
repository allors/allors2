import { Component, Self, OnInit } from '@angular/core';

import { NavigationService, Action, PanelService, RefreshService, ErrorService, MetaService, ActionTarget } from '../../../../../../angular';
import { Meta } from '../../../../../../meta';
import { InventoryItem, SerialisedInventoryItem } from '../../../../../../domain';
import { DeleteService, EditService, TableRow, Table, Sorter } from '../../../../..';
import { ObjectService, CreateData, OverviewService } from '../../../../../../material';

interface Row extends TableRow {
  object: InventoryItem;
  facility: string;
  item: string;
  status: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialisedinventoryitem-overview-panel',
  templateUrl: './serialisedinventoryitem-overview-panel.component.html',
  providers: [PanelService]
})
export class SerialisedInventoryItemComponent implements OnInit {

  m: Meta;

  table: Table<Row>;

  edit: Action;
  receiveInventory: Action;

  objects: SerialisedInventoryItem[];

  get createData(): CreateData {
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
    public errorService: ErrorService,
    public deleteService: DeleteService,
    public editService: EditService) {

    this.m = this.metaService.m;
  }

  ngOnInit() {

    const { pull, x, m } = this.metaService;

    this.panel.name = 'serialised Inventory item';
    this.panel.title = 'Serialised Inventory items';
    this.panel.icon = 'link';
    this.panel.expandable = true;

    this.edit = this.editService.edit();

    this.table = new Table({
      selection: false,
      columns: [
        { name: 'facility', sort: true },
        { name: 'item', sort: true },
        { name: 'status', sort: true },
      ],
      actions: [
        {
          name: () => 'ReceiveInventory',
          description: () => '',
          disabled: () => false,
          execute: (target: ActionTarget) => {
            if (!Array.isArray(target)) {
              this.factoryService.create(this.m.InventoryItemTransaction, this.createData);
            }
          },
          result: null
        }
      ],
      defaultAction: this.edit,
    });

    const sorter = new Sorter(
      {
        facility: [m.Facility.Name],
        item: [m.SerialisedItem.Name],
        status: [m.SerialisedInventoryItemState.Name],
      }
    );

    const pullName = `${this.panel.name}_${this.m.SerialisedInventoryItem.name}`;

    this.panel.onPull = (pulls) => {
      const id = this.panel.manager.id;

      pulls.push(
        pull.Part({
          object: id,
          fetch: {
            InventoryItemsWherePart: {
              include: {
                SerialisedInventoryItem_InventoryItemState: x,
                Facility: x,
                UnitOfMeasure: x
              }
            }
          },
        })
      );

      this.panel.onPulled = (loaded) => {

        this.objects = loaded.collections.InventoryItems as SerialisedInventoryItem[];

        if (this.objects) {
          this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
          this.table.data = this.objects.map((v) => {
            return {
              object: v,
              facility: v.Facility.Name,
              item: v.SerialisedItem.Name,
              status: v.InventoryItemState ? v.InventoryItemState.Name : ''
            } as Row;
          });
        }
      };
    };
  }
}
