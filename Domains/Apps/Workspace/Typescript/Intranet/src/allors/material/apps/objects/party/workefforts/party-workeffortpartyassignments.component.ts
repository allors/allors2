import { Component, Input, Output, EventEmitter } from '@angular/core';

import { NavigationService, SessionService} from '../../../../../angular';
import { CommunicationEvent, WorkEffortPartyAssignment, WorkEffort} from '../../../../../domain';
import { ObjectType } from '../../../../../framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-workeffortpartyassignements',
  templateUrl: './party-workeffortpartyassignments.component.html',
})
export class PartyWorkEffortPartyAssignmentsComponent {

  @Input() workEffortPartyAssignments: WorkEffortPartyAssignment[];

  @Output() add: EventEmitter<ObjectType> = new EventEmitter<ObjectType>();

  @Output() edit: EventEmitter<WorkEffort> = new EventEmitter<WorkEffort>();

  @Output() delete: EventEmitter<WorkEffort> = new EventEmitter<WorkEffort>();

  constructor(
    public allors: SessionService,
    public navigation: NavigationService
  ) { }
}
