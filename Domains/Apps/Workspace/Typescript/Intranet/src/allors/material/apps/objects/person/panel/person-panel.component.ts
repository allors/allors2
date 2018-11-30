import { Component, Self } from '@angular/core';
import { SessionService, AllorsPanelService, Loaded } from '../../../../../angular';
import { Pull } from '../../../../../framework';
import { Person, Organisation, OrganisationContactKind, OrganisationContactRelationship } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'person-panel',
  templateUrl: './person-panel.component.html',
  providers: [AllorsPanelService]
})
export class PersonPanelComponent {

  m: MetaDomain;

  person: Person;
  organisation: Organisation;
  contactKindsText: string;

  constructor(
    public allors: SessionService,
    @Self() public panelService: AllorsPanelService) {

    this.m = this.allors.m;

    panelService.name = 'person';
    panelService.prePull = (pulls) => this.prePull(pulls);
    panelService.postPull = (loaded) => this.postPull(loaded);
  }

  prePull(pulls: Pull[]) {
    const { m, pull, tree, x } = this.allors;

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
  }

  postPull(loaded: Loaded) {
    this.person = loaded.objects.Person as Person;

    const organisationContactRelationships = loaded.collections.OrganisationContactRelationships as OrganisationContactRelationship[];

    if (organisationContactRelationships.length > 0) {
      const organisationContactRelationship = organisationContactRelationships[0];
      this.organisation = organisationContactRelationship.Organisation as Organisation;
      this.contactKindsText = organisationContactRelationship.ContactKinds
        .map((v: OrganisationContactKind) => v.Description)
        .reduce((acc: string, cur: string) => acc + ', ' + cur);
    }

  }

}
