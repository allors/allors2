import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService } from '../../../../../../angular';
import { SerialisedItem } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'serialiseditem-overview-summary',
  templateUrl: './serialiseditem-overview-summary.component.html',
  providers: [PanelService]
})
export class SerialisedItemOverviewSummaryComponent {

  m: Meta;

  serialisedItem: SerialisedItem;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const serialisedItemPullName = `${panel.name}_${this.m.SerialisedItem.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.SerialisedItem({
          name: serialisedItemPullName,
          object: id,
          include: {
            SerialisedItemState: x,
            OwnedBy: x,
            RentedBy: x
          }
        }));
    };

    panel.onPulled = (loaded) => {
      this.serialisedItem = loaded.objects[serialisedItemPullName] as SerialisedItem;
    };
  }
}
