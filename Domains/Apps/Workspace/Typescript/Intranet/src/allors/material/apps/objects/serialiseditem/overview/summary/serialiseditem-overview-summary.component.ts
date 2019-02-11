import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService } from '../../../../../../angular';
import { SerialisedItem, Part } from '../../../../../../domain';
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
  part: Part;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const serialisedItemPullName = `${panel.name}_${this.m.SerialisedItem.name}`;
    const partPullName = `${panel.name}_${this.m.Part.name}`;

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
            RentedBy: x,
          }
        }),
        pull.SerialisedItem({
          name: partPullName,
          object: id,
          fetch: {
            PartWhereSerialisedItem: x
          }
        }),
      );
    };

    panel.onPulled = (loaded) => {
      this.serialisedItem = loaded.objects[serialisedItemPullName] as SerialisedItem;
      this.part = loaded.objects[partPullName] as Part;
    };
  }
}
