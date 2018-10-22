import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, Self } from '@angular/core';

import { ErrorService, Loaded, Scope, WorkspaceService, Allors } from '../../../../../../angular';
import { ContactMechanismPurpose, Country, PartyContactMechanism, PostalAddress, PostalBoundary } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'party-contactmechanism-postaladdress',
  templateUrl: './postaladdress-inline.component.html',
  providers: [Allors]
})
export class PartyContactMechanismPostalAddressInlineComponent implements OnInit, OnDestroy {

  @Output()
  public saved: EventEmitter<PartyContactMechanism> = new EventEmitter<PartyContactMechanism>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  @Input() public scope: Scope;

  public countries: Country[];
  public contactMechanismPurposes: ContactMechanismPurpose[];

  public partyContactMechanism: PartyContactMechanism;
  public postalAddress: PostalAddress;
  public postalBoundary: PostalBoundary;

  public m: MetaDomain;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService) {

    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    const pulls = [
      pull.Country({
        sort: new Sort(this.m.Country.Name)
      }),
      pull.ContactMechanismPurpose({
        predicate: new Equals({ propertyType: this.m.ContactMechanismPurpose.IsActive, value: true }),
        sort: new Sort(this.m.ContactMechanismPurpose.Name)
      })
    ];

    scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.countries = loaded.collections.countries as Country[];
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as ContactMechanismPurpose[];

        this.partyContactMechanism = scope.session.create('PartyContactMechanism') as PartyContactMechanism;
        this.postalAddress = scope.session.create('PostalAddress') as PostalAddress;
        this.postalBoundary = scope.session.create('PostalBoundary') as PostalBoundary;
        this.partyContactMechanism.ContactMechanism = this.postalAddress;
        this.postalAddress.PostalBoundary = this.postalBoundary;
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
      scope.session.delete(this.postalAddress);
      scope.session.delete(this.postalBoundary);
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
