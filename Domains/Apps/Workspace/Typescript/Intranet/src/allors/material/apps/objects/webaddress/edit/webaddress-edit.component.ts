import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, Saved, SessionService, NavigationActivatedRoute, NavigationService } from '../../../../../angular';
import { Enumeration, PartyContactMechanism, WebAddress, Party } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './webaddress-edit.html',
  providers: [SessionService]
})
export class EditWebAddressComponent implements OnInit, OnDestroy {

  public title = 'Web Address';

  public m: MetaDomain;

  public party: Party;
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanism: WebAddress;
  public contactMechanismPurposes: Enumeration[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: SessionService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.allors;

    this.subscription = this.route.url
      .pipe(
        switchMap((url: any) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();
          const partyId = navRoute.queryParam(m.Party);

          let pulls = [
            pull.ContactMechanismPurpose({
              predicate: new Equals({ propertyType: m.ContactMechanismPurpose.IsActive, value: true }),
              sort: new Sort(m.ContactMechanismPurpose.Name)
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
                    ContactMechanism: x,
                  }
                }
              }
              )
            ];

          } else {
            pulls = [
              ...pulls,
              pull.WebAddress({
                object: id,
                fetch: {
                  PartyContactMechanismsWhereContactMechanism: {
                    PartyWherePartyContactMechanism: {
                      include: {
                        PartyContactMechanisms: {
                          ContactPurposes: x,
                          ContactMechanism: x,
                        }
                      }
                    }
                  }
                }
              }),
              pull.WebAddress({
                object: id,
                fetch: {
                  PartyContactMechanismsWhereContactMechanism: {
                    include: {
                      ContactMechanism: x
                    }
                  }
                }
              }),
            ];

          }

          return this.allors
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        }))
      .subscribe(({ loaded, add }) => {

        this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as Enumeration[];

        if (add) {
          this.party = loaded.objects.Party as Party;
          this.contactMechanism = this.allors.session.create('WebAddress') as WebAddress;
          this.partyContactMechanism = this.allors.session.create('PartyContactMechanism') as PartyContactMechanism;
          this.partyContactMechanism.ContactMechanism = this.contactMechanism;
          this.partyContactMechanism.UseAsDefault = true;
          this.party.AddPartyContactMechanism(this.partyContactMechanism);

        } else {
          this.party = loaded.collections.Parties && (loaded.collections.Parties as Party[])[0];
          const partyContactMechanisms = loaded.collections.PartyContactMechanisms as PartyContactMechanism[];
          this.partyContactMechanism = partyContactMechanisms && partyContactMechanisms[0];
          this.contactMechanism = this.partyContactMechanism.ContactMechanism as WebAddress;
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

    this.allors
      .save()
      .subscribe((saved: Saved) => {
        this.navigation.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
