import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';

import { ErrorService, Scope, WorkspaceService, DataService } from '../../../../../../../angular';
import { ContactMechanismPurpose, EmailAddress, PartyContactMechanism } from '../../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
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
    private dataService: DataService,
    private errorService: ErrorService,
  ) {
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {

    const { m, pull } = this.dataService;

    const pulls = [
      pull.ContactMechanismPurpose(
        {
          predicate: new Equals({ propertyType: this.m.ContactMechanismPurpose.IsActive, value: true }),
          sort: new Sort(this.m.ContactMechanismPurpose.Name)
        }
      )
    ];

    this.scope.load('Pull', new PullRequest({ pulls })).subscribe(
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
