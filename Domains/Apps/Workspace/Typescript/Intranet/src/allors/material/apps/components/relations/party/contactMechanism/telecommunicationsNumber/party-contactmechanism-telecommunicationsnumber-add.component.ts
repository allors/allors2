import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Saved, Scope, WorkspaceService, x, Allors } from '../../../../../../../angular';
import { ContactMechanismType, Enumeration, Party, PartyContactMechanism, TelecommunicationsNumber } from '../../../../../../../domain';
import { PullRequest, TreeNode, Sort, Equals } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../../../base/services/dialog';

@Component({
  templateUrl: './party-contactmechanism-telecommunicationsnumber.html',
  providers: [Allors]
})
export class PartyContactMechanismTelecommunicationsNumberAddComponent implements OnInit, OnDestroy {

  public title = 'Telecommunications Number';
  public subTitle = 'add a telecommunications number';

  public m: MetaDomain;

  public party: Party;
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanism: TelecommunicationsNumber;
  public contactMechanismPurposes: Enumeration[];
  public contactMechanismTypes: ContactMechanismType[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = this.route.url
      .pipe(
        switchMap((url: any) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.Party(
              {
                object: id,
                include: {
                  PartyContactMechanisms: {
                    ContactPurposes: x,
                    ContactMechanism: {
                      ContactMechanismType: x,
                    }
                  }
                }
              }
            ),
            pull.ContactMechanismPurpose({
              predicate: new Equals({ propertyType: m.ContactMechanismPurpose.IsActive, value: true }),
              sort: new Sort(this.m.ContactMechanismPurpose.Name),
            }),
            pull.ContactMechanismType({
              predicate: new Equals({ propertyType: m.ContactMechanismType.IsActive, value: true }),
              sort: new Sort(this.m.ContactMechanismType.Name)
            })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.contactMechanismPurposes = loaded.collections.contactMechanismPurposes as Enumeration[];
        this.contactMechanismTypes = loaded.collections.contactMechanismTypes as ContactMechanismType[];
        const phone: ContactMechanismType = this.contactMechanismTypes.find((v: ContactMechanismType) => v.Name === 'Phone');
        this.party = loaded.objects.party as Party;

        if (!this.contactMechanism) {
          this.contactMechanism = scope.session.create('TelecommunicationsNumber') as TelecommunicationsNumber;
          this.contactMechanism.ContactMechanismType = phone;
        }

        this.partyContactMechanism = scope.session.create('PartyContactMechanism') as PartyContactMechanism;
        this.partyContactMechanism.ContactMechanism = this.contactMechanism;
        this.partyContactMechanism.UseAsDefault = true;

        this.party.AddPartyContactMechanism(this.partyContactMechanism);
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    const { scope } = this.allors;

    scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public goBack(): void {
    window.history.back();
  }
}
