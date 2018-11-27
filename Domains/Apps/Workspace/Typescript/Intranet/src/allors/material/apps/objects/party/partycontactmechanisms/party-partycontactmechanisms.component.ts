import { Component, Input, Output, EventEmitter } from '@angular/core';

import { SessionService } from '../../../../../angular';
import { CommunicationEvent, PartyContactMechanism } from '../../../../../domain';
import { ObjectType } from '../../../../../framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-partycontactmechanisms',
  templateUrl: './party-partycontactmechanisms.component.html',
})
export class PartyPartyContactMechanismsComponent {

  @Input() currentContactMechanisms: PartyContactMechanism[];

  @Input() inactiveContactMechanisms: PartyContactMechanism[];

  @Input() allContactMechanisms: PartyContactMechanism[];

  @Output() add: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() edit: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() remove: EventEmitter<CommunicationEvent> = new EventEmitter<CommunicationEvent>();

  @Output() delete: EventEmitter<CommunicationEvent> = new EventEmitter<CommunicationEvent>();

  @Output() activate: EventEmitter<CommunicationEvent> = new EventEmitter<CommunicationEvent>();

  contactMechanismsCollection = 'Current';

  constructor(public allors: SessionService) { }

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


}
