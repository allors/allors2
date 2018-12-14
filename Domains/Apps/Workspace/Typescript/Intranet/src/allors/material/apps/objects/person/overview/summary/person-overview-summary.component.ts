import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService } from '../../../../../../angular';
import { Person, Organisation, OrganisationContactKind, OrganisationContactRelationship } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'person-overview-summary',
  templateUrl: './person-overview-summary.component.html',
  providers: [PanelService]
})
export class PersonOverviewSummaryComponent {

  m: Meta;

  person: Person;
  organisation: Organisation;
  contactKindsText: string;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const personPullName = `${panel.name}_${this.m.Person.name}`;
    const organisationContactRelationshipsPullName = `${panel.name}_${this.m.OrganisationContactRelationship.name}`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      const partyContactMechanismTree = tree.PartyContactMechanism({
        ContactPurposes: x,
        ContactMechanism: {
          PostalAddress_PostalBoundary: {
            Country: x,
          }
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
            PartyContactMechanisms: partyContactMechanismTree,
            CurrentPartyContactMechanisms: partyContactMechanismTree,
            InactivePartyContactMechanisms: partyContactMechanismTree,
            GeneralCorrespondence: {
              PostalBoundary: {
                Country: x,
              }
            }
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

      const organisationContactRelationships = loaded.collections[organisationContactRelationshipsPullName] as OrganisationContactRelationship[];

      if (organisationContactRelationships.length > 0) {
        const organisationContactRelationship = organisationContactRelationships[0];
        this.organisation = organisationContactRelationship.Organisation as Organisation;
        this.contactKindsText = organisationContactRelationship.ContactKinds
          .map((v: OrganisationContactKind) => v.Description)
          .reduce((acc: string, cur: string) => acc + ', ' + cur);
      }
    };
  }
}
