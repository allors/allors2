import { Component, EventEmitter, Input, OnInit , Output } from '@angular/core';

import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../../../angular';
import { ContactMechanismPurpose, PartyContactMechanism, WebAddress } from '../../../../../../domain';
import { PullRequest, Query } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';

@Component({
  selector: 'party-contactmechanism',
  templateUrl: './party-contactmechanism-inline.component.html',
})
export class PartyContactMechanismInlineComponent {
  @Output() public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: Scope;
}
