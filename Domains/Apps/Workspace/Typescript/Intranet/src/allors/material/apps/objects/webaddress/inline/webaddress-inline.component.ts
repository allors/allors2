import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, Self } from '@angular/core';

import { ErrorService, SessionService } from '../../../../../angular';
import { ContactMechanismPurpose, PartyContactMechanism, WebAddress } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-contactmechanism-webAddress',
  templateUrl: './webaddress-inline.component.html',
})
export class InlineWebAddressComponent implements OnInit, OnDestroy {
  @Output() public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  public contactMechanismPurposes: ContactMechanismPurpose[];

  public partyContactMechanism: PartyContactMechanism;
  public webAddress: WebAddress;

  public m: MetaDomain;

  constructor(
    private allors: SessionService,
    private errorService: ErrorService,
  ) {
    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.allors;

    const pulls = [
      pull.ContactMechanismPurpose({
        predicate: new Equals({ propertyType: this.m.ContactMechanismPurpose.IsActive, value: true }),
        sort: new Sort(this.m.ContactMechanismPurpose.Name)
      })
    ];

    this.allors.load('Pull', new PullRequest({ pulls })).subscribe(
      (loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];
        this.partyContactMechanism = this.allors.session.create('PartyContactMechanism') as PartyContactMechanism;
        this.webAddress = this.allors.session.create('WebAddress') as WebAddress;
        this.partyContactMechanism.ContactMechanism = this.webAddress;
      },
      (error: any) => {
        this.cancelled.emit();
      },
    );
  }

  public ngOnDestroy(): void {

    if (!!this.partyContactMechanism) {
      this.allors.session.delete(this.partyContactMechanism);
      this.allors.session.delete(this.webAddress);
    }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
    this.saved.emit(this.partyContactMechanism);

    this.partyContactMechanism = undefined;
    this.webAddress = undefined;
  }
}
