import { Component, Self } from '@angular/core';

import { MetaService, NavigationService, PanelService } from '@allors/angular/services/core';
import { Organisation } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ActionTarget, TestScope } from '@allors/angular/core';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'organisation-overview-summary',
  templateUrl: './organisation-overview-summary.component.html',
  providers: [PanelService]
})
export class OrganisationOverviewSummaryComponent extends TestScope {

  m: Meta;

  organisation: Organisation;
  contactKindsText: string;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'summary';

    const organisationPullName = `${panel.name}_${this.m.Organisation.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Organisation({
          name: organisationPullName,
          object: id,
          include: {
            Locale: x,
            LastModifiedBy: x,
          }
        }));
    };

    panel.onPulled = (loaded) => {
      this.organisation = loaded.objects[organisationPullName] as Organisation;
    };
  }
}
