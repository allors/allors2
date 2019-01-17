import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, Self } from '@angular/core';

import { ErrorService, ContextService, MetaService } from '../../../../../angular';
import { ContactMechanismPurpose, PartyContactMechanism, WebAddress } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';

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
    private errorService: ErrorService,
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

    this.allors.context.load('Pull', new PullRequest({ pulls })).subscribe(
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
