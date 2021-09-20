import { Component, Output, EventEmitter, OnInit, OnDestroy, Input } from '@angular/core';

import { ContextService, MetaService } from '@allors/angular/services/core';
import { PartyContactMechanism, ContactMechanismPurpose, EmailAddress, Facility, FacilityType, Organisation, Enumeration, ContactMechanismType, TelecommunicationsNumber, WebAddress } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { Equals, Sort } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { FetcherService } from '@allors/angular/base';

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

  public m: Meta;

  constructor(
    private allors: ContextService,
    public metaService: MetaService,

  ) {
    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.ContactMechanismPurpose({
        predicate: new Equals({ propertyType: this.m.ContactMechanismPurpose.IsActive, value: true }),
        sort: new Sort(this.m.ContactMechanismPurpose.Name)
      })
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe(
      (loaded) => {
        this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as ContactMechanismPurpose[];
        this.partyContactMechanism = this.allors.context.create('PartyContactMechanism') as PartyContactMechanism;
        this.webAddress = this.allors.context.create('WebAddress') as WebAddress;
        this.partyContactMechanism.ContactMechanism = this.webAddress;
      },
      (error: any) => {
        this.cancelled.emit();
      },
    );
  }

  public ngOnDestroy(): void {

    if (!!this.partyContactMechanism) {
      this.allors.context.delete(this.partyContactMechanism);
      this.allors.context.delete(this.webAddress);
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
