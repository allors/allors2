import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, Self } from '@angular/core';

import { ErrorService, ContextService, MetaService } from '../../../../../angular';
import { ContactMechanismPurpose, EmailAddress, PartyContactMechanism } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-contactmechanism-emailAddress',
  templateUrl: './emailaddress-inline.component.html',
})
export class PartyContactMechanismEmailAddressInlineComponent
  implements OnInit, OnDestroy {

  @Output() public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output() public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: ContextService;

  public contactMechanismPurposes: ContactMechanismPurpose[];

  public partyContactMechanism: PartyContactMechanism;
  public emailAddress: EmailAddress;

  public m: MetaDomain;

  constructor(
    private allors: ContextService,
    public metaService: MetaService,
    private errorService: ErrorService,
  ) {
    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.ContactMechanismPurpose(
        {
          predicate: new Equals({ propertyType: this.m.ContactMechanismPurpose.IsActive, value: true }),
          sort: new Sort(this.m.ContactMechanismPurpose.Name)
        }
      )
    ];

    this.allors.context.load('Pull', new PullRequest({ pulls })).subscribe(
      (loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];

        this.partyContactMechanism = this.allors.context.create('PartyContactMechanism') as PartyContactMechanism;
        this.emailAddress = this.allors.context.create('EmailAddress') as EmailAddress;
        this.partyContactMechanism.ContactMechanism = this.emailAddress;
      },
      (error: any) => {
        this.cancelled.emit();
      },
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
