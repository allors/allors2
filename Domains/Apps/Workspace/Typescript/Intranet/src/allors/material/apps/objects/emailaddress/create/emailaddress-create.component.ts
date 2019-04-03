import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import {  ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Enumeration, ElectronicAddress, PartyContactMechanism, Party } from '../../../../../domain';
import { PullRequest, Sort, Equals, IObject } from '../../../../../framework';
import { CreateData, SaveService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './emailaddress-create.component.html',
  providers: [ContextService]
})
export class EmailAddressCreateComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  contactMechanism: ElectronicAddress;
  contactMechanismTypes: Enumeration[];

  public title = 'Add Email Address';

  private subscription: Subscription;
  partyContactMechanism: PartyContactMechanism;
  party: Party;
  contactMechanismPurposes: Enumeration[];


  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<EmailAddressCreateComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const pulls = [
            pull.Party({
              object: this.data.associationId,
            }),
            pull.ContactMechanismPurpose({
              predicate: new Equals({ propertyType: m.ContactMechanismPurpose.IsActive, value: true }),
              sort: new Sort(this.m.ContactMechanismPurpose.Name)
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.party = loaded.objects.Party as Party;
        this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as Enumeration[];

        this.contactMechanism = this.allors.context.create('EmailAddress') as ElectronicAddress;

        this.partyContactMechanism = this.allors.context.create('PartyContactMechanism') as PartyContactMechanism;
        this.partyContactMechanism.UseAsDefault = true;
        this.partyContactMechanism.ContactMechanism = this.contactMechanism;

        this.party.AddPartyContactMechanism(this.partyContactMechanism);
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.contactMechanism.id,
          objectType: this.contactMechanism.objectType,
        };

        this.dialogRef.close(data);
      },
      this.saveService.errorHandler
    );
  }
}
