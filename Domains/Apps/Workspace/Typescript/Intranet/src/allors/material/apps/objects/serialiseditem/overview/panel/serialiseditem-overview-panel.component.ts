import { Component, Self, OnInit, HostBinding } from '@angular/core';

import { NavigationService, Action, PanelService, RefreshService, ErrorService, MetaService, ActionTarget } from '../../../../../../angular';
import { Meta } from '../../../../../../meta';
import { SerialisedItem } from '../../../../../../domain';
import { DeleteService, TableRow, Table } from '../../../../..';
import { ObjectService, CreateData, OverviewService } from '../../../../../../material';

interface Row extends TableRow {
  object: SerialisedItem;
  number: string;
  name: string;
  status: string;
  ownership: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialiseditem-overview-panel',
  templateUrl: './serialiseditem-overview-panel.component.html',
  providers: [PanelService]
})
export class SerialisedItemOverviewPanelComponent implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: SerialisedItem[] = [];
  table: Table<Row>;

  delete: Action;

  get createData(): CreateData {
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
    public errorService: ErrorService,
    public deleteService: DeleteService,
  ) {

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.panel.name = 'serialiseditem';
    this.panel.title = 'Serialised Assets';
    this.panel.icon = 'link';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort },
        { name: 'name', sort },
        { name: 'status', sort },
        { name: 'ownership', sort },
      ],
      actions: [
        {
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
                SerialisedItemState: x,
                Ownership: x
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
                SerialisedItemState: x,
                Ownership: x
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
                SerialisedItemState: x,
                Ownership: x
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
              status: v.SerialisedItemState ? v.SerialisedItemState.Name : '',
              ownership: v.Ownership ? v.Ownership.Name : '',
            } as Row;
          });
        }
      };
    };
  }
}
