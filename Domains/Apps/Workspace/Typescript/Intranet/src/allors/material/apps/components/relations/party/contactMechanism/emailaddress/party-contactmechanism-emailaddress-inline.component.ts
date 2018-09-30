import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, Self } from '@angular/core';

import { ErrorService, Scope, WorkspaceService, Allors } from '../../../../../../../angular';
import { ContactMechanismPurpose, EmailAddress, PartyContactMechanism } from '../../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-contactmechanism-emailAddress',
  templateUrl: './party-contactmechanism-emailaddress-inline.component.html',
  providers: [Allors]
})
export class PartyContactMechanismEmailAddressInlineComponent
  implements OnInit, OnDestroy {

  @Output() public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: Scope;

  public contactMechanismPurposes: ContactMechanismPurpose[];

  public partyContactMechanism: PartyContactMechanism;
  public emailAddress: EmailAddress;

  public m: MetaDomain;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
  ) {
    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    const pulls = [
      pull.ContactMechanismPurpose(
        {
          predicate: new Equals({ propertyType: this.m.ContactMechanismPurpose.IsActive, value: true }),
          sort: new Sort(this.m.ContactMechanismPurpose.Name)
        }
      )
    ];

    scope.load('Pull', new PullRequest({ pulls })).subscribe(
      (loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];

        this.partyContactMechanism = scope.session.create('PartyContactMechanism') as PartyContactMechanism;
        this.emailAddress = scope.session.create('EmailAddress') as EmailAddress;
        this.partyContactMechanism.ContactMechanism = this.emailAddress;
      },
      (error: any) => {
        this.cancelled.emit();
      },
    );
  }

  public ngOnDestroy(): void {
    const { scope } = this.allors;

    if (!!this.partyContactMechanism) {
      scope.session.delete(this.partyContactMechanism);
      scope.session.delete(this.emailAddress);
    }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
    this.saved.emit(this.partyContactMechanism);

    this.partyContactMechanism = undefined;
    this.emailAddress = undefined;
  }
}
