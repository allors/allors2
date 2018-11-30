import { Component, Input, Output, EventEmitter, Self } from '@angular/core';

import { ObjectType, Pull } from '../../../../../framework';
import { SessionService, AllorsPanelService, Loaded } from '../../../../../angular';

import { CommunicationEvent, PartyContactMechanism } from '../../../../../domain';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'partycontactmechanism-minpanel',
  templateUrl: './partycontactmechanism-minpanel.component.html',
  providers: [AllorsPanelService]
})
export class PartyContactMechanismMinPanelComponent {

  constructor(
    public allors: SessionService,
    @Self() public panelService: AllorsPanelService) {

      panelService.name = 'partycontactmechanism';
  }

}
