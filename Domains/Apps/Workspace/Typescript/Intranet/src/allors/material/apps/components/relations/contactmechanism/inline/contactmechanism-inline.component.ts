import { Component, EventEmitter, Input, Output } from '@angular/core';

import { SessionService } from '../../../../../../angular';
import { PartyContactMechanism } from '../../../../../../domain';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-contactmechanism',
  templateUrl: './contactmechanism-inline.component.html',
})
export class ContactMechanismInlineComponent {
  @Output() public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  constructor(public allors: SessionService) { }
}
