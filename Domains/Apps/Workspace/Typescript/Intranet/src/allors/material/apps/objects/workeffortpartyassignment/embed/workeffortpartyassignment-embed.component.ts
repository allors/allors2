import { Component, Input, Output, EventEmitter } from '@angular/core';

import { NavigationService, SessionService} from '../../../../../angular';
import { CommunicationEvent, WorkEffortPartyAssignment, WorkEffort} from '../../../../../domain';
import { ObjectType } from '../../../../../framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'workeffortpartyassignment-embed',
  templateUrl: './workeffortpartyassignment-embed.component.html',
})
export class WorkEffortPartyAssignmentEmbedComponent {

  @Input() workEffortPartyAssignments: WorkEffortPartyAssignment[];

  @Output() add: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() edit: EventEmitter<WorkEffort> = new EventEmitter<WorkEffort>();

  @Output() delete: EventEmitter<WorkEffort> = new EventEmitter<WorkEffort>();

  constructor(
    public allors: SessionService,
    public navigation: NavigationService
  ) { }
}
