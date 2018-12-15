import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService } from '../../../../../../angular';
import { Person, Organisation, OrganisationContactKind, OrganisationContactRelationship, Good } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'good-overview-summary',
  templateUrl: './good-overview-summary.component.html',
  providers: [PanelService]
})
export class GoodOverviewSummaryComponent {

  m: Meta;

  good: Good;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const pullName = `${panel.name}_${this.m.Good.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Good({
          name: pullName,
          object: id,
          include: {
            GoodIdentifications: {
              GoodIdentificationType: x
            },
            ProductCategories: x,
            Part: {
              Brand: x,
              Model: x
            }
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.good = loaded.objects[pullName] as Good;
    };
  }
}
