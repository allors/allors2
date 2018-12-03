import { Component, Output, EventEmitter, Self } from '@angular/core';

import { NavigationService, SessionService, Action, AllorsPanelService, AllorsRefreshService, ErrorService } from '../../../../../angular';
import { MetaDomain } from '../../../../../meta';
import { SerialisedItem, Part, Party } from '../../../../../domain';
import { DeleteService } from '../../../../../material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialiseditem-panel',
  templateUrl: './serialiseditem-panel.component.html',
  providers: [AllorsPanelService]
})
export class SerialisedItemPanelComponent {

  m: MetaDomain;

  part: Part;
  owner: Party;
  serialisedItems: SerialisedItem[];

  delete: Action;

  constructor(
    public allors: SessionService,
    @Self() public panelService: AllorsPanelService,
    public refreshService: AllorsRefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
  ) {

    this.m = this.allors.m;
    this.delete = deleteService.delete(allors);

    panelService.name = 'serialiseditem';
    panelService.title = 'Serialized Items';
    panelService.icon = 'link';
    panelService.maximizable = true;

    const ownerName = `${panelService.name}_Owner`;
    const ownedSerialisedItemsName = `${panelService.name}_OwnedSerialisedItemsName`;
    const rentedSerialisedItemsName = `${panelService.name}_RentedSerialisedItems`;

    panelService.prePull = (pulls) => {
      const { m, pull, tree, x } = this.allors;

      const id = this.panelService.panelsService.id;

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

      panelService.postPull = (loaded) => {
        this.owner = loaded.objects[ownerName] as Party;

        const ownedSerialisedItems = loaded.collections[ownedSerialisedItemsName] as SerialisedItem[];
        const rentedSerialisedItems = loaded.collections[rentedSerialisedItemsName] as SerialisedItem[];
        this.serialisedItems = ownedSerialisedItems.concat(rentedSerialisedItems);
      };
    };

  }
}
