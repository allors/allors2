import { Component, Self, OnInit } from '@angular/core';

import { NavigationService, Action, PanelService, RefreshService, ErrorService, MetaService, ActionTarget } from '../../../../../../angular';
import { Meta } from '../../../../../../meta';
import { InventoryItem, NonSerialisedInventoryItem } from '../../../../../../domain';
import { DeleteService, EditService, TableRow, Table, Sorter } from '../../../../..';
import { ObjectService, CreateData, OverviewService } from '../../../../../../material';

interface Row extends TableRow {
  object: InventoryItem;
  facility: string;
  part: string;
  uom: string;
  status: string;
  qoh: number;
  atp: number;
  committedOut: number;
  expectedIn: number;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nonserialisedinventoryitem-overview-panel',
  templateUrl: './nonserialisedinventoryitem-overview-panel.component.html',
  providers: [PanelService]
})
export class NonSerialisedInventoryItemComponent implements OnInit {

  m: Meta;

  table: Table<Row>;

  edit: Action;
  changeInventory: Action;

  objects: NonSerialisedInventoryItem[];

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

    this.panel.name = 'nonserialised Inventory item';
    this.panel.title = 'Non Serialised Inventory items';
    this.panel.icon = 'link';
    this.panel.expandable = true;

    this.edit = this.editService.edit();
    this.changeInventory = {
      name: () => 'Change Inventory',
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
        { name: 'part', sort: true },
        { name: 'uom', sort: true },
        { name: 'status', sort: true },
        'qoh',
        'atp',
        'committedOut',
        'expectedIn'
      ],
      defaultAction: this.changeInventory,
    });

    const sorter = new Sorter(
      {
        facility: [m.Facility.Name],
        part: [m.Part.Name],
        uom: [m.UnitOfMeasure.Name],
        status: [m.NonSerialisedInventoryItemState.Name],
      }
    );

    const pullName = `${this.panel.name}_${this.m.NonSerialisedInventoryItem.name}`;

    this.panel.onPull = (pulls) => {
      const id = this.panel.manager.id;

      pulls.push(
        pull.Part({
          name: pullName,
          object: id,
          fetch: {
            InventoryItemsWherePart: {
              include: {
                NonSerialisedInventoryItem_InventoryItemState: x,
                Facility: x,
                UnitOfMeasure: x
              }
            }
          },
        })
      );

      this.panel.onPulled = (loaded) => {

        this.objects = loaded.collections[pullName] as NonSerialisedInventoryItem[];
        this.objects = this.objects.filter(v => v.QuantityOnHand > 0 || v.QuantityCommittedOut > 0 || v.QuantityExpectedIn > 0 || v.AvailableToPromise > 0);

        if (this.objects) {
          this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
          this.table.data = this.objects.map((v) => {
            return {
              object: v,
              facility: v.Facility.Name,
              part: v.Part.Name,
              status: 'TODO',
              // status: v.NonSerialisedInventoryItemState ? v.NonSerialisedInventoryItemState.Name : '',
              uom: v.UnitOfMeasure.Abbreviation || v.UnitOfMeasure.Name,
              qoh: v.QuantityOnHand,
              atp: v.AvailableToPromise,
              committedOut: v.QuantityCommittedOut,
              expectedIn: v.QuantityExpectedIn,
            } as Row;
          });
        }
      };
    };
  }
}
