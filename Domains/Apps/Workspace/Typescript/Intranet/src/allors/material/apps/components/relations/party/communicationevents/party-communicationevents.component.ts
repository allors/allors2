import { Component, Input, Output, EventEmitter } from '@angular/core';

import { NavigationService, Allors} from '../../../../../../angular';
import { CommunicationEvent} from '../../../../../../domain';
import { ObjectType } from '../../../../../../framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-communicationevents',
  templateUrl: './party-communicationevents.component.html',
})
export class PartyCommnunicationEventsComponent {

  @Input() communicationEvents: CommunicationEvent[];

  @Output() add: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() delete: EventEmitter<CommunicationEvent> = new EventEmitter<CommunicationEvent>();

  constructor(
    public allors: Allors,
    public navigation: NavigationService
  ) { }
}
