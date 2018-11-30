import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, Self } from '@angular/core';

import { ErrorService, SessionService } from '../../../../../angular';
import { ContactMechanismPurpose, ContactMechanismType, Enumeration, PartyContactMechanism, TelecommunicationsNumber } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-contactmechanism-telecommunicationsnumber',
  templateUrl: './telecommunicationsnumber-inline.component.html',
})
export class PartyContactMechanismTelecommunicationsNumberInlineComponent implements OnInit, OnDestroy {

  @Output()
  public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  public contactMechanismPurposes: Enumeration[];
  public contactMechanismTypes: ContactMechanismType[];

  public partyContactMechanism: PartyContactMechanism;
  public telecommunicationsNumber: TelecommunicationsNumber;

  public m: MetaDomain;

  constructor(
    private allors: SessionService,
    private errorService: ErrorService) {

    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { pull } = this.allors;

    const pulls = [
      pull.ContactMechanismPurpose({
        predicate: new Equals({ propertyType: this.m.ContactMechanismPurpose.IsActive, value: true }),
        sort: new Sort(this.m.ContactMechanismPurpose.Name),
      }),
      pull.ContactMechanismType({
        predicate: new Equals({ propertyType: this.m.ContactMechanismType.IsActive, value: true }),
        sort: new Sort(this.m.ContactMechanismType.Name),
      })
    ];

    this.allors
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];
        this.contactMechanismTypes = loaded.collections.contactMechanismTypes as ContactMechanismType[];

        this.partyContactMechanism = this.allors.session.create('PartyContactMechanism') as PartyContactMechanism;
        this.telecommunicationsNumber = this.allors.session.create('TelecommunicationsNumber') as TelecommunicationsNumber;
        this.partyContactMechanism.ContactMechanism = this.telecommunicationsNumber;
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {

    if (!!this.partyContactMechanism) {
      this.allors.session.delete(this.partyContactMechanism);
      this.allors.session.delete(this.telecommunicationsNumber);
    }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
    this.saved.emit(this.partyContactMechanism);

    this.partyContactMechanism = undefined;
    this.telecommunicationsNumber = undefined;
  }
}