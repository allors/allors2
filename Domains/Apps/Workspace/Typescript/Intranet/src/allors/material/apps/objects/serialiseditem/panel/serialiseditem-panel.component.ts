import { Component, Output, EventEmitter, Self } from '@angular/core';

import { NavigationService, ContextService, Action, PanelService, RefreshService, ErrorService, MetaService } from '../../../../../angular';
import { MetaDomain } from '../../../../../meta';
import { SerialisedItem, Part, Party } from '../../../../../domain';
import { DeleteService } from '../../../../../material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialiseditem-panel',
  templateUrl: './serialiseditem-panel.component.html',
  providers: [PanelService]
})
export class SerialisedItemPanelComponent {

  m: MetaDomain;

  part: Part;
  owner: Party;
  serialisedItems: SerialisedItem[];

  delete: Action;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
  ) {

    this.m = this.metaService.m;
    this.delete = deleteService.delete(panel.manager.context);

    panel.name = 'serialiseditem';
    panel.title = 'Serialized Items';
    panel.icon = 'link';
    panel.maximizable = true;

    const ownerName = `${panel.name}_Owner`;
    const ownedSerialisedItemsName = `${panel.name}_OwnedSerialisedItemsName`;
    const rentedSerialisedItemsName = `${panel.name}_RentedSerialisedItems`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Party({
          object: id,
          name: ownerName,
        }),
        pull.Party({
          object: id,
          name: ownedSerialisedItemsName,
          fetch: {
            SerialisedItemsWhereOwnedBy: {
              include: {
                SerialisedItemState: x
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
                SerialisedItemState: x
              }
            }
          }
        }),
      );

      panel.onPulled = (loaded) => {
        this.owner = loaded.objects[ownerName] as Party;

        const ownedSerialisedItems = loaded.collections[ownedSerialisedItemsName] as SerialisedItem[];
        const rentedSerialisedItems = loaded.collections[rentedSerialisedItemsName] as SerialisedItem[];
        this.serialisedItems = ownedSerialisedItems.concat(rentedSerialisedItems);
      };
    };

  }
}
