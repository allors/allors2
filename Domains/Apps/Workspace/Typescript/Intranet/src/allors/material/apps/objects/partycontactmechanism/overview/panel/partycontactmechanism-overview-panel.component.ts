import { Component, Self } from '@angular/core';
import { ContextService, PanelService, NavigationService, Saved, RefreshService, ErrorService, Action, MetaService } from '../../../../../../angular';
import { PartyContactMechanism, Person } from '../../../../../../domain';
import { MetaDomain } from '../../../../../../meta';
import { ObjectType, ISessionObject } from '../../../../../../framework';
import { DeleteService } from '../../../../..';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'partycontactmechanism-overview-panel',
  templateUrl: './partycontactmechanism-overview-panel.component.html',
  providers: [PanelService]
})
export class PartyContactMechanismOverviewPanelComponent {
  m: MetaDomain;

  person: Person;

  delete: Action;

  contactMechanismsCollection = 'Current';
  currentContactMechanisms: PartyContactMechanism[];
  inactiveContactMechanisms: PartyContactMechanism[];
  allContactMechanisms: PartyContactMechanism[];

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
  ) {

    this.m = this.metaService.m;
    this.delete = deleteService.delete(panel.manager.context);

    panel.name = 'partycontactmechanism';
    panel.title = 'Contact Mechanisms';
    panel.icon = 'contacts';
    panel.maximizable = true;

    const personPullName = `${panel.name}_${this.m.Person.objectType.name}`;

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
    };

    panel.onPulled = (loaded) => {
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
    this.panel.manager.context
      .save()
      .subscribe((saved: Saved) => {
        this.panel.manager.context.reset();
        this.refreshService.refresh();
      },
        this.errorService.handle);
  }

  activate(partyContactMechanism: PartyContactMechanism): void {

    partyContactMechanism.ThroughDate = undefined;
    this.panel.manager.context
      .save()
      .subscribe(() => {
        this.panel.manager.context.reset();
        this.refreshService.refresh();
      },
        this.errorService.handle);
  }
}
