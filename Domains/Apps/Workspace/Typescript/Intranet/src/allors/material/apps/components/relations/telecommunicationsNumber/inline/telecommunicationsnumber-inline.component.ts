import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, Self } from '@angular/core';

import { ErrorService, Loaded, Scope, WorkspaceService, Allors } from '../../../../../../angular';
import { ContactMechanismPurpose, ContactMechanismType, Enumeration, PartyContactMechanism, TelecommunicationsNumber } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-contactmechanism-telecommunicationsnumber',
  templateUrl: './telecommunicationsnumber-inline.component.html',
  providers: [Allors]
})
export class PartyContactMechanismTelecommunicationsNumberInlineComponent implements OnInit, OnDestroy {

  @Output()
  public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: Scope;

  public contactMechanismPurposes: Enumeration[];
  public contactMechanismTypes: ContactMechanismType[];

  public partyContactMechanism: PartyContactMechanism;
  public telecommunicationsNumber: TelecommunicationsNumber;

  public m: MetaDomain;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService) {

    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

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

    scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];
        this.contactMechanismTypes = loaded.collections.contactMechanismTypes as ContactMechanismType[];

        this.partyContactMechanism = scope.session.create('PartyContactMechanism') as PartyContactMechanism;
        this.telecommunicationsNumber = scope.session.create('TelecommunicationsNumber') as TelecommunicationsNumber;
        this.partyContactMechanism.ContactMechanism = this.telecommunicationsNumber;
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    const { scope } = this.allors;

    if (!!this.partyContactMechanism) {
      scope.session.delete(this.partyContactMechanism);
      scope.session.delete(this.telecommunicationsNumber);
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
