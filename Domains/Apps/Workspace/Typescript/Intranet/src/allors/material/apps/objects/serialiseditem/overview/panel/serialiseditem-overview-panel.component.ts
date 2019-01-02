import { Component, Self, OnInit } from '@angular/core';

import { NavigationService, Action, PanelService, RefreshService, ErrorService, MetaService } from '../../../../../../angular';
import { Meta } from '../../../../../../meta';
import { SerialisedItem, Part, Party } from '../../../../../../domain';
import { DeleteService, TableRow, Table } from '../../../../..';
import { ObjectService, CreateData } from '../../../../../base/services/object';
import { NavigateService } from 'src/allors/material/base/services/actions';

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
    public objectService: ObjectService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public navigateService: NavigateService,
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

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number' },
        { name: 'name' },
        { name: 'status' },
        { name: 'ownership' },
      ],
      actions: [
        this.navigateService.overview(),
        this.delete,
      ],
      defaultAction: this.navigateService.overview(),
    });

    const pullName = `${this.panel.name}_${this.m.SerialisedItem.name}`;
    const ownedSerialisedItemsName = `${this.panel.name}_${this.m.SerialisedItem.name}_OwnedSerialisedItemsName`;
    const rentedSerialisedItemsName = `${this.panel.name}_${this.m.SerialisedItem.name}_RentedSerialisedItems`;

    this.panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;
      const id = this.panel.manager.id;

      pulls.push(
        pull.Party({
          name: pullName,
          object: id,
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
        const ownedSerialisedItems = loaded.collections[ownedSerialisedItemsName] as SerialisedItem[];
        const rentedSerialisedItems = loaded.collections[rentedSerialisedItemsName] as SerialisedItem[];
        this.objects = ownedSerialisedItems.concat(rentedSerialisedItems);

        if (this.objects) {
          this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
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
