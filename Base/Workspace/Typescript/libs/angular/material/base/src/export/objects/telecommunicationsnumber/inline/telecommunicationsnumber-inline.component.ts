import { Component, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';

import { ContextService, MetaService } from '@allors/angular/services/core';
import { PartyContactMechanism, ContactMechanismPurpose, Enumeration, ContactMechanismType, TelecommunicationsNumber } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { Equals, Sort } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';


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

  public m: Meta;

  constructor(
    private allors: ContextService,
    public metaService: MetaService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull } = this.metaService;

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

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as ContactMechanismPurpose[];
        this.contactMechanismTypes = loaded.collections.ContactMechanismTypes as ContactMechanismType[];

        this.partyContactMechanism = this.allors.context.create('PartyContactMechanism') as PartyContactMechanism;
        this.telecommunicationsNumber = this.allors.context.create('TelecommunicationsNumber') as TelecommunicationsNumber;
        this.partyContactMechanism.ContactMechanism = this.telecommunicationsNumber;
      });
  }

  public ngOnDestroy(): void {

    if (!!this.partyContactMechanism) {
      this.allors.context.delete(this.partyContactMechanism);
      this.allors.context.delete(this.telecommunicationsNumber);
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
