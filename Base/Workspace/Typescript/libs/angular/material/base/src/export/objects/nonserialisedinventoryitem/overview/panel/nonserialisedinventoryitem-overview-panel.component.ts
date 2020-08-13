import { Component, OnInit, Self, HostBinding } from '@angular/core';

import { MetaService, NavigationService, PanelService, RefreshService } from '@allors/angular/services/core';
import { InventoryItem, NonSerialisedInventoryItem } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, OverviewService, EditService } from '@allors/angular/material/core';
import { TestScope, Action, ActionTarget } from '@allors/angular/core';
import { ObjectData, ObjectService } from '@allors/angular/material/services/core';


interface Row extends TableRow {
  object: InventoryItem;
  facility: string;
  part: string;
  uom: string;
  location: string;
  qoh: string;
  atp: string;
  committedOut: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nonserialisedinventoryitem-overview-panel',
  templateUrl: './nonserialisedinventoryitem-overview-panel.component.html',
  providers: [PanelService]
})
export class NonSerialisedInventoryItemComponent extends TestScope implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  table: Table<Row>;

  edit: Action;
  changeInventory: Action;

  objects: NonSerialisedInventoryItem[];

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
    public editService: EditService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    const { pull, x } = this.metaService;

    this.panel.name = 'nonserialised Inventory item';
    this.panel.title = 'Nonserialised Inventory items';
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
      result: null,
    };

    this.table = new Table({
      selection: false,
      columns: [
        { name: 'facility' },
        { name: 'part' },
        { name: 'uom' },
        { name: 'location' },
        'qoh',
        'atp',
        'committedOut',
      ],
      actions: [
        this.edit,
        this.changeInventory,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

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
                InventoryItemTransactionsWhereInventoryItem: {
                  Reason: x
                },
                NonSerialisedInventoryItem_NonSerialisedInventoryItemState: x,
                Facility: x,
                UnitOfMeasure: x,
              }
            }
          },
        })
      );

      this.panel.onPulled = (loaded) => {

        this.objects = loaded.collections[pullName] as NonSerialisedInventoryItem[];
        // this.objects = this.objects.filter(v => v.QuantityOnHand > 0 || v.QuantityCommittedOut > 0 || v.QuantityExpectedIn > 0 || v.AvailableToPromise > 0);

        if (this.objects) {
          this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
          this.table.data = this.objects.map((v) => {
            return {
              object: v,
              facility: v.Facility.Name,
              part: v.Part.Name,
              uom: v.UnitOfMeasure.Abbreviation || v.UnitOfMeasure.Name,
              location: v.PartLocation,
              qoh: v.QuantityOnHand,
              atp: v.AvailableToPromise,
              committedOut: v.QuantityCommittedOut,
            } as Row;
          });
        }
      };
    };
  }
}
