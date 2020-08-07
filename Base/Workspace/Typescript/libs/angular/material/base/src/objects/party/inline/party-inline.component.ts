import { Component, Output, EventEmitter, OnInit, OnDestroy, Input } from '@angular/core';

import { ContextService, MetaService } from '@allors/angular/core';
import { PartyContactMechanism, ContactMechanismPurpose, EmailAddress, Facility, FacilityType, Organisation, Party } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { Equals, Sort } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { FetcherService } from '@allors/angular/base';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-party',
  templateUrl: './party-inline.component.html',
})
export class PartyInlineComponent {
  @Output() public saved: EventEmitter<Party> = new EventEmitter<Party>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  constructor(public allors: ContextService) { }
}
