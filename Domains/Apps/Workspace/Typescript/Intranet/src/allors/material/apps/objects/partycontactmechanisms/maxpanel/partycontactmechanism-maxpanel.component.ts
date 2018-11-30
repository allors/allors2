import { Component, Self } from '@angular/core';
import { SessionService, AllorsPanelService, Loaded, NavigationService, Saved, AllorsRefreshService, ErrorService, Action } from '../../../../../angular';
import { PartyContactMechanism, Person, ContactMechanism } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta';
import { Pull, ObjectType, ISessionObject, ResponseError } from '../../../../../framework';
import { AllorsMaterialDialogService, DeleteService } from '../../../../../material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'partycontactmechanism-maxpanel',
  templateUrl: './partycontactmechanism-maxpanel.component.html',
  providers: [AllorsPanelService]
})
export class PartyContactMechanismMaxPanelComponent {
  m: MetaDomain;

  person: Person;

  delete: Action;

  contactMechanismsCollection = 'Current';
  currentContactMechanisms: PartyContactMechanism[];
  inactiveContactMechanisms: PartyContactMechanism[];
  allContactMechanisms: PartyContactMechanism[];

  private personPullName: string;

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
    panelService.prePull = (pulls) => this.prePull(pulls);
    panelService.postPull = (loaded) => this.postPull(loaded);

    this.personPullName = `${panelService.name}_${this.m.Person.objectType.name}`;
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
        name: this.personPullName,
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
  }

  postPull(loaded: Loaded) {
    this.person = loaded.objects.Person as Person;

    this.currentContactMechanisms = this.person.CurrentPartyContactMechanisms as PartyContactMechanism[];
    this.inactiveContactMechanisms = this.person.InactivePartyContactMechanisms as PartyContactMechanism[];
    this.allContactMechanisms = this.currentContactMechanisms.concat(this.inactiveContactMechanisms);
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
