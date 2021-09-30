import { Component, Output, EventEmitter, OnInit, OnDestroy, Input } from '@angular/core';

import { ContextService, MetaService } from '@allors/angular/services/core';
import { PartyContactMechanism, ContactMechanismPurpose, EmailAddress } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { Equals, Sort } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-contactmechanism-emailAddress',
  templateUrl: './emailaddress-inline.component.html',
})
export class PartyContactMechanismEmailAddressInlineComponent implements OnInit, OnDestroy {
  @Output() public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: ContextService;

  public contactMechanismPurposes: ContactMechanismPurpose[];

  public partyContactMechanism: PartyContactMechanism;
  public emailAddress: EmailAddress;

  public m: Meta;

  constructor(private allors: ContextService, public metaService: MetaService) {
    this.m = this.metaService.m;
  }

  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.ContactMechanismPurpose({
        predicate: new Equals({ propertyType: this.m.ContactMechanismPurpose.IsActive, value: true }),
        sort: new Sort(this.m.ContactMechanismPurpose.Name),
      }),
    ];

    this.allors.context.load(new PullRequest({ pulls })).subscribe(
      (loaded) => {
        this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as ContactMechanismPurpose[];

        this.partyContactMechanism = this.allors.context.create('PartyContactMechanism') as PartyContactMechanism;
        this.emailAddress = this.allors.context.create('EmailAddress') as EmailAddress;
        this.partyContactMechanism.ContactMechanism = this.emailAddress;
      },
      (error: any) => {
        this.cancelled.emit();
      }
    );
  }

  public ngOnDestroy(): void {
    if (!!this.partyContactMechanism) {
      this.allors.context.delete(this.partyContactMechanism);
      this.allors.context.delete(this.emailAddress);
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
