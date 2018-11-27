import { Component, Input, Output, EventEmitter } from '@angular/core';

import { NavigationService, SessionService } from '../../../../../angular';
import { CommunicationEvent } from '../../../../../domain';
import { ObjectType } from '../../../../../framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'communicationevent-embed',
  templateUrl: './communicationevent-embed.component.html',
})
export class CommunicationeventEmbedComponent {

  @Input() communicationEvents: CommunicationEvent[];

  @Output() add: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() edit: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() delete: EventEmitter<CommunicationEvent> = new EventEmitter<CommunicationEvent>();

  constructor(public allors: SessionService) { }
}
