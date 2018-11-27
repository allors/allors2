import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, Self } from '@angular/core';

import { ErrorService, SessionService } from '../../../../../angular';
import { ContactMechanismPurpose, Country, PartyContactMechanism, PostalAddress, PostalBoundary } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-contactmechanism-postaladdress',
  templateUrl: './postaladdress-inline.component.html',
})
export class PartyContactMechanismPostalAddressInlineComponent implements OnInit, OnDestroy {

  @Output()
  public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  public countries: Country[];
  public contactMechanismPurposes: ContactMechanismPurpose[];

  public partyContactMechanism: PartyContactMechanism;
  public postalAddress: PostalAddress;
  public postalBoundary: PostalBoundary;

  public m: MetaDomain;

  constructor(
    private allors: SessionService,
    private errorService: ErrorService) {

    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.allors;

    const pulls = [
      pull.Country({
        sort: new Sort(this.m.Country.Name)
      }),
      pull.ContactMechanismPurpose({
        predicate: new Equals({ propertyType: this.m.ContactMechanismPurpose.IsActive, value: true }),
        sort: new Sort(this.m.ContactMechanismPurpose.Name)
      })
    ];

    this.allors
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.countries = loaded.collections.countries as Country[];
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];

        this.partyContactMechanism = this.allors.session.create('PartyContactMechanism') as PartyContactMechanism;
        this.postalAddress = this.allors.session.create('PostalAddress') as PostalAddress;
        this.postalBoundary = this.allors.session.create('PostalBoundary') as PostalBoundary;
        this.partyContactMechanism.ContactMechanism = this.postalAddress;
        this.postalAddress.PostalBoundary = this.postalBoundary;
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {

    if (!!this.partyContactMechanism) {
      this.allors.session.delete(this.partyContactMechanism);
      this.allors.session.delete(this.postalAddress);
      this.allors.session.delete(this.postalBoundary);
    }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
    this.saved.emit(this.partyContactMechanism);

    this.partyContactMechanism = undefined;
    this.postalAddress = undefined;
    this.postalBoundary = undefined;
  }
}
