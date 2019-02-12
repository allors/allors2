import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService } from '../../../../../../angular';
import { UnifiedGood } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'unifiedgood-overview-summary',
  templateUrl: './unifiedgood-overview-summary.component.html',
  providers: [PanelService]
})
export class UnifiedGoodOverviewSummaryComponent {

  m: Meta;

  good: UnifiedGood;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const pullName = `${panel.name}_${this.m.UnifiedGood.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.UnifiedGood({
          name: pullName,
          object: id,
          include: {
            Brand: x,
            Model: x,
            ProductIdentifications: {
              ProductIdentificationType: x
            },
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.good = loaded.objects[pullName] as UnifiedGood;
    };
  }
}
