import { Component, Input, Output, EventEmitter } from '@angular/core';

import { NavigationService, Allors } from '../../../../../../angular';
import { CommunicationEvent, PartyContactMechanism } from '../../../../../../domain';
import { ObjectType } from '../../../../../../framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-partycontactmechanisms',
  templateUrl: './party-partycontactmechanisms.component.html',
})
export class PartyPartyContactMechanismsComponent {

  constructor(
    public allors: Allors,
    public navigation: NavigationService
  ) { }

}
