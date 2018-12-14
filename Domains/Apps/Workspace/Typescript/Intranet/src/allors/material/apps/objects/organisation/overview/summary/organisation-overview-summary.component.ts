import { Component, Self } from '@angular/core';
import { ContextService, PanelService, NavigationService, MetaService } from '../../../../../../angular';
import { Organisation, OrganisationContactKind, OrganisationContactRelationship } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'organisation-overview-summary',
  templateUrl: './organisation-overview-summary.component.html',
  providers: [PanelService]
})
export class OrganisationOverviewSummaryComponent {

  m: Meta;

  organisation: Organisation;
  contactKindsText: string;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const organisationPullName = `${panel.name}_${this.m.Organisation.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

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
