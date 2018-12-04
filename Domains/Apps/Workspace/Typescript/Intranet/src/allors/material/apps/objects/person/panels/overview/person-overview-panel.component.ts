import { Component, Self } from '@angular/core';
import { SessionService, AllorsPanelService, NavigationService, MetaService } from '../../../../../../angular';
import { Person, Organisation, OrganisationContactKind, OrganisationContactRelationship } from '../../../../../../domain';
import { MetaDomain } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'person-panel',
  templateUrl: './person-overview-panel.component.html',
  providers: [AllorsPanelService]
})
export class PersonOverviewPanelComponent {

  m: MetaDomain;

  person: Person;
  organisation: Organisation;
  contactKindsText: string;

  constructor(
    public allors: SessionService,
    @Self() public panelService: AllorsPanelService,
    public metaService: MetaService,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panelService.name = 'person';

    const personPullName = `${panelService.name}_${this.m.Person.objectType.name}`;
    const organisationContactRelationshipsPullName = `${panelService.name}_${this.m.OrganisationContactRelationship.objectType.name}`;

    panelService.prePull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panelService.panelsService.id;

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

    panelService.postPull = (loaded) => {
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
