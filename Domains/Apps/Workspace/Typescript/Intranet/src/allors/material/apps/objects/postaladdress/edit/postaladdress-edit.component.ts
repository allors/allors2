import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ErrorService, Saved, ContextService, NavigationActivatedRoute, NavigationService, MetaService } from '../../../../../angular';
import { Country, Enumeration, PartyContactMechanism, PostalAddress, Party, PostalBoundary } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './postaladdress-edit.html',
  providers: [ContextService]
})
export class PostalAddressEditComponent implements OnInit, OnDestroy {

  public title = 'Postal Address';

  public m: MetaDomain;

  public party: Party;
  public partyContactMechanism: PartyContactMechanism;
  public contactMechanism: PostalAddress;
  public postalBoundary: PostalBoundary;
  public contactMechanismPurposes: Enumeration[];
  public countries: Country[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: AllorsMaterialDialogService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

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
            }),
            pull.Country({
              sort: new Sort(m.Country.Name)
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
                      PostalAddress_PostalBoundary: {
                        Country: x
                      }
                    },
                  }
                }
              }
              )
            ];

          } else {
            pulls = [
              ...pulls,
              pull.PostalAddress({
                object: id,
                fetch: {
                  PartyContactMechanismsWhereContactMechanism: {
                    PartyWherePartyContactMechanism: {
                      include: {
                        PartyContactMechanisms: {
                          ContactPurposes: x,
                          ContactMechanism: {
                            PostalAddress_PostalBoundary: {
                              Country: x
                            }
                          },
                        }
                      }
                    }
                  }
                }
              }),
              pull.PostalAddress({
                object: id,
                fetch: {
                  PartyContactMechanismsWhereContactMechanism: {
                    include: {
                      ContactMechanism: {
                        PostalAddress_PostalBoundary: {
                          Country: x
                        }
                      }
                    }
                  }
                }
              }),
            ];

          }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            )
            ;
        })
      )
      .subscribe(({ loaded, add }) => {

        this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as Enumeration[];
        this.countries = loaded.collections.Countries as Country[];

        if (add) {
          this.party = loaded.objects.Party as Party;

          this.contactMechanism = this.allors.context.create('PostalAddress') as PostalAddress;
          this.postalBoundary = this.allors.context.create('PostalBoundary') as PostalBoundary;
          this.contactMechanism.PostalBoundary = this.postalBoundary;
          this.partyContactMechanism = this.allors.context.create('PartyContactMechanism') as PartyContactMechanism;
          this.partyContactMechanism.ContactMechanism = this.contactMechanism;
          this.partyContactMechanism.UseAsDefault = true;
          this.party.AddPartyContactMechanism(this.partyContactMechanism);

        } else {
          this.party = loaded.collections.Parties && (loaded.collections.Parties as Party[])[0];
          const partyContactMechanisms = loaded.collections.PartyContactMechanisms as PartyContactMechanism[];
          this.partyContactMechanism = partyContactMechanisms && partyContactMechanisms[0];
          this.contactMechanism = this.partyContactMechanism.ContactMechanism as PostalAddress;
          this.contactMechanismPurposes = loaded.collections.ContactMechanismPurposes as Enumeration[];
        }

      },
        (error: any) => {
          this.errorService.handle(error);
          this.subscription.unsubscribe();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        this.navigation.back();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
