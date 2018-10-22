import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';

import { ErrorService, Saved, x, Allors, NavigationService, NavigationActivatedRoute } from '../../../../../../angular';
import { Enumeration, PartyContactMechanism, TelecommunicationsNumber, Party, ContactMechanismType } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './telecommunicationsnumber-edit.html',
  providers: [Allors]
})
export class TelecommunicationsNumberEditComponent implements OnInit, OnDestroy {

  public title = 'Telecommunications Number';

  public m: MetaDomain;

  public party: Party;
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanism: TelecommunicationsNumber;
  public contactMechanismPurposes: Enumeration[];
  public contactMechanismTypes: Enumeration[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private navigation: NavigationService,
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

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();
          const partyId = navRoute.queryParam(m.Party);

          let pulls = [
            pull.ContactMechanismPurpose({
              predicate: new Equals({ propertyType: m.ContactMechanismPurpose.IsActive, value: true }),
              sort: new Sort(m.ContactMechanismPurpose.Name)
            }),
            pull.ContactMechanismType({
              predicate: new Equals({ propertyType: m.ContactMechanismType.IsActive, value: true }),
              sort: new Sort(this.m.ContactMechanismType.Name)
            })
          ];

          const add = !id;

          if (add) {
            pulls = [
              ...pulls,
              pull.Party({
                object: partyId,
                include: {
                  PartyContactMechanisms: {
                    ContactPurposes: x,
                    ContactMechanism: {
                      ContactMechanismType: x,
                    },
                  }
                }
              }
              )
            ];

          } else {
            pulls = [
              ...pulls,
              pull.EmailAddress({
                object: id,
                fetch: {
                  PartyContactMechanismsWhereContactMechanism: {
                    PartyWherePartyContactMechanism: {
                      include: {
                        PartyContactMechanisms: {
                          ContactPurposes: x,
                          ContactMechanism: {
                            ContactMechanismType: x,
                          },
                        }
                      }
                    }
                  }
                }
              }),
              pull.EmailAddress({
                object: id,
                fetch: {
                  PartyContactMechanismsWhereContactMechanism: {
                    include: {
                      ContactMechanism: {
                        ContactMechanismType: x,
                      }
                    }
                  }
                }
              }),
            ];

          }

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {

        this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as Enumeration[];
        this.contactMechanismTypes = loaded.collections.ContactMechanismTypes as Enumeration[];

        if (add) {
          this.party = loaded.objects.Party as Party;
          // TODO: Should be lookup on UniqueId
          const phone: ContactMechanismType = this.contactMechanismTypes.find((v: ContactMechanismType) => v.Name === 'Phone');
          this.contactMechanism = scope.session.create('TelecommunicationsNumber') as TelecommunicationsNumber;
          this.contactMechanism.ContactMechanismType = phone;
          this.partyContactMechanism = scope.session.create('PartyContactMechanism') as PartyContactMechanism;
          this.partyContactMechanism.ContactMechanism = this.contactMechanism;
          this.partyContactMechanism.UseAsDefault = true;
          this.party.AddPartyContactMechanism(this.partyContactMechanism);

        } else {
          this.party = loaded.collections.Parties && (loaded.collections.Parties as Party[])[0];
          const partyContactMechanisms = loaded.collections.PartyContactMechanisms as PartyContactMechanism[];
          this.partyContactMechanism = partyContactMechanisms && partyContactMechanisms[0];
          this.contactMechanism = this.partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
          this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as Enumeration[];
        }
      },
        (error: any) => {
          this.errorService.handle(error);
          this.navigation.back();
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
        this.navigation.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
