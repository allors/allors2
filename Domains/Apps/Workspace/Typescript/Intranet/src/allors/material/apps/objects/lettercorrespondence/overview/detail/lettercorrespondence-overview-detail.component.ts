import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationService, NavigationActivatedRoute, MetaService, PanelService, RefreshService } from '../../../../../../angular';
import { CommunicationEventPurpose, EmailAddress, LetterCorrespondence, EmailTemplate, InternalOrganisation, Party, Person, Organisation, PostalAddress, OrganisationContactRelationship, PartyContactMechanism } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, map, filter } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'lettercorrespondence-overview-detail',
  templateUrl: './lettercorrespondence-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class LetterCorrespondenceOverviewDetailComponent implements OnInit, OnDestroy {
  public title = 'Letter Correspondence';

  public addSender = false;
  public addReceiver = false;
  public addAddress = false;

  public m: MetaDomain;

  public party: Party;
  public person: Person;
  public organisation: Organisation;
  public purposes: CommunicationEventPurpose[];
  public contacts: Party[] = [];

  public postalAddresses: PostalAddress[] = [];

  public letterCorrespondence: LetterCorrespondence;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.m = this.metaService.m;

    panel.name = 'detail';
    panel.title = 'Details';
    panel.icon = 'email';
    panel.maximizable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.LetterCorrespondence.objectType.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isNormal) {
        const { pull, x } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.LetterCorrespondence({
            name: pullName,
            object: id,
            include: {
              Originators: x,
              Receivers: x,
              EventPurposes: x,
              CommunicationEventState: x,
              ContactMechanisms: x,
              WorkEfforts: {
                WorkEffortState: x,
                Priority: x,
              },
              PostalAddresses: {
                PostalBoundary: {
                  Country: x,
                }
              }
            }
          })
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isNormal) {
        this.letterCorrespondence = loaded.objects[pullName] as LetterCorrespondence;
      }
    };
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        filter((v) => {
          return this.panel.isMaximized;
        }),
        switchMap(([, , , internalOrganisationId]) => {
          const navRoute = new NavigationActivatedRoute(this.route);
          const id = this.panel.manager.id;

          const personId = navRoute.queryParam(m.Person);
          const organisationId = navRoute.queryParam(m.Organisation);

          let pulls = [
            pull.LetterCorrespondence({
              object: id,
              include: {
                Originators: x,
                Receivers: x,
                PostalAddresses: {
                  PostalBoundary: {
                    Country: x,
                  }
                }
              }
            }),
            pull.Organisation({
              object: internalOrganisationId,
              name: 'InternalOrganisation',
              include: {
                ActiveEmployees: {
                  CurrentPartyContactMechanisms: {
                    ContactMechanism: {
                      PostalAddress_PostalBoundary: {
                        Country: x,
                      }
                    },
                  }
                }
              }
            }),
            pull.CommunicationEventPurpose({
              predicate: new Equals({ propertyType: m.CommunicationEventPurpose.IsActive, value: true }),
              sort: new Sort(m.CommunicationEventPurpose.Name)
            }),
          ];

          if (!!organisationId) {
            pulls = [
              ...pulls,
              pull.Organisation({
                object: organisationId,
                include: {
                  CurrentContacts: x,
                  CurrentPartyContactMechanisms: {
                    ContactMechanism: {
                      PostalAddress_PostalBoundary: {
                        Country: x,
                      }
                    },
                  }
                }
              }
              )
            ];
          }

          if (!!personId) {
            pulls = [
              ...pulls,
              pull.Person({
                object: personId,
              }),
              pull.Person({
                object: personId,
                fetch: {
                  OrganisationContactRelationshipsWhereContact: {
                    Organisation: {
                      include: {
                        CurrentContacts: x,
                        CurrentPartyContactMechanisms: {
                          ContactMechanism: {
                            PostalAddress_PostalBoundary: {
                              Country: x,
                            }
                          },
                        }
                      }
                    }
                  }
                }
              })
            ];
          }

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.purposes = loaded.collections.CommunicationEventPurposes as CommunicationEventPurpose[];
        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.postalAddresses = internalOrganisation.ActiveEmployees
          .map((v) => v.CurrentPartyContactMechanisms
            .filter((w) => w && w.ContactMechanism.objectType === m.EmailAddress.objectType)
            .map((w) => w.ContactMechanism as PostalAddress))
          .reduce((acc, v) => acc.concat(v), []);

        this.person = loaded.objects.Person as Person;
        this.organisation = loaded.objects.Organisation as Organisation;
        if (!this.organisation && loaded.collections.Organisations && loaded.collections.Organisations.length > 0) {
          // TODO: check active
          this.organisation = loaded.collections.Organisations[0] as Organisation;
        }

        this.party = this.organisation || this.person;

        this.contacts = this.contacts.concat(internalOrganisation.ActiveEmployees);
        this.contacts = this.contacts.concat(this.organisation && this.organisation.CurrentContacts);
        if (!!this.person) {
          this.contacts.push(this.person);
        }

        this.letterCorrespondence = loaded.objects.LetterCorrespondence as LetterCorrespondence;
      },
        (error: any) => {
          this.errorService.handle(error);
          this.panel.toggle();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public senderCancelled(): void {
    this.addSender = false;
  }

  public receiverCancelled(): void {
    this.addReceiver = false;
  }

  public addressCancelled(): void {
    this.addAddress = false;
  }

  public senderAdded(id: string): void {

    this.addSender = false;

    const sender: Person = this.allors.context.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = sender;
    relationShip.Organisation = this.organisation;

    this.letterCorrespondence.AddOriginator(sender);
  }

  public receiverAdded(id: string): void {

    this.addReceiver = false;

    const receiver: Person = this.allors.context.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = receiver;
    relationShip.Organisation = this.organisation;

    this.letterCorrespondence.AddReceiver(receiver);
  }

  public addressAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addAddress = false;

    this.party.AddPartyContactMechanism(partyContactMechanism);

    const postalAddress = partyContactMechanism.ContactMechanism as PostalAddress;
    this.postalAddresses.push(postalAddress);
    this.letterCorrespondence.AddPostalAddress(postalAddress);
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.panel.toggle();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
