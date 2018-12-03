import { Component, Self } from '@angular/core';
import { SessionService, AllorsPanelService, NavigationService, Saved, AllorsRefreshService, ErrorService, Action } from '../../../../../angular';
import { PartyContactMechanism, Person } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta';
import { ObjectType, ISessionObject } from '../../../../../framework';
import { DeleteService } from '../../../../../material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'partycontactmechanism-panel',
  templateUrl: './partycontactmechanism-panel.component.html',
  providers: [AllorsPanelService]
})
export class PartyContactMechanismPanelComponent {
  m: MetaDomain;

  person: Person;

  delete: Action;

  contactMechanismsCollection = 'Current';
  currentContactMechanisms: PartyContactMechanism[];
  inactiveContactMechanisms: PartyContactMechanism[];
  allContactMechanisms: PartyContactMechanism[];

  constructor(
    public allors: SessionService,
    @Self() public panelService: AllorsPanelService,
    public refreshService: AllorsRefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
  ) {

    this.m = this.allors.m;
    this.delete = deleteService.delete(allors);

    panelService.name = 'partycontactmechanism';
    panelService.title = 'Contact Mechanisms';
    panelService.icon = 'contacts';
    panelService.maximizable = true;

    const personPullName = `${panelService.name}_${this.m.Person.objectType.name}`;

    panelService.prePull = (pulls) => {
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
    };

    panelService.postPull = (loaded) => {
      this.person = loaded.objects[personPullName] as Person;

      this.currentContactMechanisms = this.person.CurrentPartyContactMechanisms as PartyContactMechanism[];
      this.inactiveContactMechanisms = this.person.InactivePartyContactMechanisms as PartyContactMechanism[];
      this.allContactMechanisms = this.currentContactMechanisms.concat(this.inactiveContactMechanisms);
    };
  }

  get contactMechanisms(): any {

    switch (this.contactMechanismsCollection) {
      case 'Current':
        return this.currentContactMechanisms;
      case 'Inactive':
        return this.inactiveContactMechanisms;
      case 'All':
      default:
        return this.allContactMechanisms;
    }
  }

  add(objectType: ObjectType) {
    this.navigation.add(objectType, this.person);
  }

  edit(object: ISessionObject) {
    this.navigation.edit(object, this.person);
  }

  remove(partyContactMechanism: PartyContactMechanism): void {

    partyContactMechanism.ThroughDate = new Date();
    this.allors
      .save()
      .subscribe((saved: Saved) => {
        this.allors.session.reset();
        this.refreshService.refresh();
      },
        this.errorService.handle);
  }

  activate(partyContactMechanism: PartyContactMechanism): void {

    partyContactMechanism.ThroughDate = undefined;
    this.allors
      .save()
      .subscribe(() => {
        this.allors.session.reset();
        this.refreshService.refresh();
      },
        this.errorService.handle);
  }
}
