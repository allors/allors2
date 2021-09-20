import { Component, Self, OnInit, HostBinding } from '@angular/core';

import { MetaService, NavigationService, PanelService, RefreshService } from '@allors/angular/services/core';
import { SerialisedItem } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, OverviewService } from '@allors/angular/material/core';
import { TestScope, Action, ActionTarget } from '@allors/angular/core';
import { ObjectData, ObjectService } from '@allors/angular/material/services/core';



interface Row extends TableRow {
  object: SerialisedItem;
  number: string;
  name: string;
  availability: string;
  onWebsite: string;
  ownership: string;
  ownedBy: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialiseditem-overview-panel',
  templateUrl: './serialiseditem-overview-panel.component.html',
  providers: [PanelService]
})
export class SerialisedItemOverviewPanelComponent extends TestScope implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: SerialisedItem[] = [];
  table: Table<Row>;

  delete: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.panel.name = 'serialiseditem';
    this.panel.title = 'Serialised Assets';
    this.panel.icon = 'link';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number' },
        { name: 'name' },
        { name: 'availability' },
        { name: 'onWebsite' },
        { name: 'ownership' },
        { name: 'ownedBy' },
      ],
      actions: [
        {
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
        },
        this.overviewService.overview(),
        this.delete,
      ],
      defaultAction: this.overviewService.overview(),
      autoSort: true,
      autoFilter: true,
    });

    const partSerialisedItemsName = `${this.panel.name}_${this.m.SerialisedItem.name}`;
    const ownedSerialisedItemsName = `${this.panel.name}_${this.m.SerialisedItem.name}_OwnedSerialisedItemsName`;
    const rentedSerialisedItemsName = `${this.panel.name}_${this.m.SerialisedItem.name}_RentedSerialisedItems`;

    this.panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;
      const id = this.panel.manager.id;

      pulls.push(
        pull.Part({
          name: partSerialisedItemsName,
          object: id,
          fetch: {
            SerialisedItems: {
              include: {
                OwnedBy: x,
                Ownership: x,
                SerialisedItemAvailability: x,
                SerialisedItemState: x,
              }
            }
          }
        }),
        pull.Party({
          object: id,
          name: ownedSerialisedItemsName,
          fetch: {
            SerialisedItemsWhereOwnedBy: {
              include: {
                OwnedBy: x,
                Ownership: x,
                SerialisedItemAvailability: x,
                SerialisedItemState: x,
              }
            }
          }
        }),
        pull.Party({
          object: id,
          name: rentedSerialisedItemsName,
          fetch: {
            SerialisedItemsWhereRentedBy: {
              include: {
                OwnedBy: x,
                Ownership: x,
                SerialisedItemAvailability: x,
                SerialisedItemState: x,
              }
            }
          }
        }),
      );

      this.panel.onPulled = (loaded) => {
        const partSerialisedItems = loaded.collections[partSerialisedItemsName] as SerialisedItem[];
        const ownedSerialisedItems = loaded.collections[ownedSerialisedItemsName] as SerialisedItem[];
        const rentedSerialisedItems = loaded.collections[rentedSerialisedItemsName] as SerialisedItem[];

        this.objects = [];

        if (ownedSerialisedItems !== undefined) {
          this.objects = this.objects.concat(ownedSerialisedItems);
        }

        if (rentedSerialisedItems !== undefined) {
          this.objects = this.objects.concat(rentedSerialisedItems);
        }

        if (partSerialisedItems !== undefined) {
          this.objects = this.objects.concat(partSerialisedItems);
        }

        if (this.objects) {
          this.table.total = this.objects.length;
          this.table.data = this.objects.map((v) => {
            return {
              object: v,
              number: v.ItemNumber,
              name: v.displayName,
              availability: v.SerialisedItemAvailability ? v.SerialisedItemAvailability.Name : '',
              onWebsite: v.AvailableForSale ? 'Yes' : 'No',
              ownership: v.Ownership ? v.Ownership.Name : '',
              ownedBy: v.OwnedBy ? v.OwnedBy.displayName : '',
            } as Row;
          });
        }
      };
    };
  }
}
