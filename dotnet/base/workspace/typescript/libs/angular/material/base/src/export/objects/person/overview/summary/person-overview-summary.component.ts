import { Component, Self } from '@angular/core';

import { MetaService, NavigationService, PanelService, MediaService } from '@allors/angular/services/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TestScope } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'person-overview-summary',
  templateUrl: './person-overview-summary.component.html',
  providers: [PanelService]
})
export class PersonOverviewSummaryComponent extends TestScope {

  m: Meta;

  person: Person;
  organisation: Organisation;
  contactKindsText: string;
  organisationContactRelationships: OrganisationContactRelationship[];

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    private mediaService: MediaService
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'summary';

    const personPullName = `${panel.name}_${this.m.Person.name}`;
    const organisationContactRelationshipsPullName = `${panel.name}_${this.m.OrganisationContactRelationship.name}`;

    panel.onPull = (pulls) => {
      const { pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      const partyContactMechanismTree = tree.PartyContactMechanism({
        ContactPurposes: x,
        ContactMechanism: {
          PostalAddress_Country: x
        },
      });

      pulls.push(
        pull.Person({
          name: personPullName,
          object: id,
          include: {
            Locale: x,
            LastModifiedBy: x,
            Salutation: x,
            Picture: x,
            PartyContactMechanisms: partyContactMechanismTree,
            CurrentPartyContactMechanisms: partyContactMechanismTree,
            InactivePartyContactMechanisms: partyContactMechanismTree,
            GeneralCorrespondence: x
          }
        }));

      pulls.push(
        pull.Person({
          name: organisationContactRelationshipsPullName,
          object: id,
          fetch: {
            OrganisationContactRelationshipsWhereContact: {
              include: {
                Organisation: x,
                ContactKinds: x,
              }
            }
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.person = loaded.objects[personPullName] as Person;

      this.organisationContactRelationships = loaded.collections[organisationContactRelationshipsPullName] as OrganisationContactRelationship[];

      if (this.organisationContactRelationships.length > 0) {
        const organisationContactRelationship = this.organisationContactRelationships[0];
        this.organisation = organisationContactRelationship.Organisation as Organisation;

        if (organisationContactRelationship.ContactKinds.length > 0) {
          this.contactKindsText = organisationContactRelationship.ContactKinds
            .map((v: OrganisationContactKind) => v.Description)
            .reduce((acc: string, cur: string) => acc + ', ' + cur);
        }
      }
    };
  }

  get src(): string {
    const media = this.person.Picture;
    if (media) {
      if (media.InDataUri) {
        return media.InDataUri;
      } else if (media.UniqueId) {
        return this.mediaService.url(media);
      }
    }

    return undefined;
  }
}
