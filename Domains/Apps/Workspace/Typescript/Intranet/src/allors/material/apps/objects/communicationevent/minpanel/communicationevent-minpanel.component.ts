import { Component, Input, Output, EventEmitter, Self } from '@angular/core';

import { ObjectType } from '../../../../../framework';
import { SessionService, AllorsPanelService } from '../../../../../angular';
import { CommunicationEvent } from '../../../../../domain';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'communicationevent-minpanel',
  templateUrl: './communicationevent-minpanel.component.html',
  providers: [AllorsPanelService]
})
export class CommunicationeventMinPanelComponent {

  @Input() communicationEvents: CommunicationEvent[];

  @Output() add: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() edit: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() delete: EventEmitter<CommunicationEvent> = new EventEmitter<CommunicationEvent>();

  constructor(
    public allors: SessionService,
    @Self() public panelService: AllorsPanelService,
  ) {
    panelService.name = 'communicationevent';
  }
}
