import { Component, Output, EventEmitter } from '@angular/core';

import { ContextService } from '@allors/angular/services/core';
import { PartyContactMechanism } from '@allors/domain/generated';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-contactmechanism',
  templateUrl: './contactmechanism-inline.component.html',
})
export class ContactMechanismInlineComponent {
  @Output() public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  constructor(public allors: ContextService) { }
}
