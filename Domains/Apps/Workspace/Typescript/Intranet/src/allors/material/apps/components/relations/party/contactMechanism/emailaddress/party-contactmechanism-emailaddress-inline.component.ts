import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';

import { ErrorService, Loaded, Scope, WorkspaceService } from '../../../../../../../angular';
import { ContactMechanismPurpose, EmailAddress, PartyContactMechanism } from '../../../../../../../domain';
import { PullRequest, Query } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';

@Component({
  selector: 'party-contactmechanism-emailAddress',
  templateUrl: './party-contactmechanism-emailaddress-inline.component.html',
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
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
  ) {
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    const queries: Query[] = [
      new Query({
        name: 'contactMechanismPurposes',
        objectType: this.m.ContactMechanismPurpose,
      }),
    ];

    this.scope.load('Pull', new PullRequest({ queries })).subscribe(
      (loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];

        this.partyContactMechanism = this.scope.session.create('PartyContactMechanism') as PartyContactMechanism;
        this.emailAddress = this.scope.session.create('EmailAddress') as EmailAddress;
        this.partyContactMechanism.ContactMechanism = this.emailAddress;
      },
      (error: any) => {
        this.cancelled.emit();
      },
    );
  }

  public ngOnDestroy(): void {
    if (!!this.partyContactMechanism) {
      this.scope.session.delete(this.partyContactMechanism);
      this.scope.session.delete(this.emailAddress);
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
