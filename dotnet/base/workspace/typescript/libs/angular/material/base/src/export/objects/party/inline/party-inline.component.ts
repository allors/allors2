import { Component, Output, EventEmitter } from '@angular/core';

import { ContextService } from '@allors/angular/services/core';
import { Party } from '@allors/domain/generated';


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
