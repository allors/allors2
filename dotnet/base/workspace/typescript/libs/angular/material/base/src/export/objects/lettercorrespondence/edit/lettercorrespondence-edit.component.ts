import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService } from '@allors/angular/services/core';
import { Person, Party, Organisation, CommunicationEventPurpose, CommunicationEventState, OrganisationContactRelationship, ContactMechanism, LetterCorrespondence, PartyContactMechanism, PostalAddress } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { InternalOrganisationId } from '@allors/angular/base';
import { IObject, ISessionObject } from '@allors/domain/system';
import { Equals, Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './lettercorrespondence-edit.component.html',
  providers: [ContextService]
})
export class LetterCorrespondenceEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  addFromParty = false;
  addToParty = false;
  addFromAddress = false;
  addToAddress = false;

  party: Party;
  person: Person;
  organisation: Organisation;
  purposes: CommunicationEventPurpose[];
  fromPostalAddresses: ContactMechanism[] = [];
  toPostalAddresses: ContactMechanism[] = [];
  communicationEvent: LetterCorrespondence;
  eventStates: CommunicationEventState[];
  contacts: Party[] = [];
  parties: Party[];
  title: string;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<LetterCorrespondenceEditComponent>,
    public refreshService: RefreshService,
    private saveService: SaveService,
    public metaService: MetaService,
    public navigation: NavigationService,
    private internalOrganisationId: InternalOrganisationId,
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          let pulls = [
            pull.Organisation({
              object: this.internalOrganisationId.value,
              name: 'InternalOrganisation',
              include: {
                ActiveEmployees: {
                  CurrentPartyContactMechanisms: {
                    ContactMechanism: {
                      PostalAddress_Country: x
                    },
                  }
                }
              }
            }),
            pull.CommunicationEventPurpose({
              predicate: new Equals({ propertyType: m.CommunicationEventPurpose.IsActive, value: true }),
              sort: new Sort(m.CommunicationEventPurpose.Name)
            }),
            pull.CommunicationEventState({
              sort: new Sort(m.CommunicationEventState.Name)
            }),
          ];

          if (!isCreate) {
            pulls = [
              ...pulls,
              pull.LetterCorrespondence({
                object: this.data.id,
                include: {
                  FromParty: {
                    CurrentPartyContactMechanisms: {
                      ContactMechanism: x
                    }
                  },
                  ToParty: {
                    CurrentPartyContactMechanisms: {
                      ContactMechanism: x
                    }
                  },
                  PostalAddress: {
                    Country: x
                  },
                  EventPurposes: x,
                  CommunicationEventState: x
                }
              }),
              pull.CommunicationEvent({
                object: this.data.id,
                fetch: {
                  InvolvedParties: x,
                }
              }),
            ];
          }

          if (isCreate && this.data.associationId) {
            pulls = [
              ...pulls,
              pull.Organisation({
                object: this.data.associationId,
                include: {
                  CurrentContacts: x,
                  CurrentPartyContactMechanisms: {
                    ContactMechanism: {
                      PostalAddress_Country: x
                    },
                  }
                }
              }),
              pull.Person({
                object: this.data.associationId,
              }),
              pull.Person({
                object: this.data.associationId,
                fetch: {
                  OrganisationContactRelationshipsWhereContact: {
                    Organisation: {
                      include: {
                        CurrentContacts: x,
                        CurrentPartyContactMechanisms: {
                          ContactMechanism: {
                            PostalAddress_Country: x
                          },
                        }
                      }
                    }
                  }
                }
              })
            ];
          }

          return this.allors.context
            .load(new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.purposes = loaded.collections.CommunicationEventPurposes as CommunicationEventPurpose[];
        this.eventStates = loaded.collections.CommunicationEventStates as CommunicationEventState[];
        this.parties = loaded.collections.InvolvedParties as Party[];

        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;

        this.person = loaded.objects.Person as Person;
        this.organisation = loaded.objects.Organisation as Organisation;

        if (isCreate) {
          this.title = 'Add Letter';
          this.communicationEvent = this.allors.context.create('LetterCorrespondence') as LetterCorrespondence;

          this.party = this.organisation || this.person;
        } else {
          this.communicationEvent = loaded.objects.LetterCorrespondence as LetterCorrespondence;

          this.updateFromParty(this.communicationEvent.FromParty);
          this.updateToParty(this.communicationEvent.ToParty);

          if (this.communicationEvent.CanWriteActualEnd) {
            this.title = 'Edit Letter';
          } else {
            this.title = 'View Letter';
          }
        }

        const contacts = new Set<Party>();

        if (!!this.organisation) {
          contacts.add(this.organisation);
        }

        if (internalOrganisation.ActiveEmployees !== undefined) {
          internalOrganisation.ActiveEmployees.reduce((c, e) => c.add(e), contacts);
        }

        if (!!this.organisation && this.organisation.CurrentContacts !== undefined) {
          this.organisation.CurrentContacts.reduce((c, e) => c.add(e), contacts);
        }

        if (!!this.person) {
          contacts.add(this.person);
        }

        if (!!this.parties) {
          this.parties.reduce((c, e) => c.add(e), contacts);
        }

        this.contacts.push(...contacts);
        this.sortContacts();
    });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public fromAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    if (!!this.communicationEvent.FromParty) {
      this.communicationEvent.FromParty.AddPartyContactMechanism(partyContactMechanism);
    }

    const address = partyContactMechanism.ContactMechanism as PostalAddress;

    this.fromPostalAddresses.push(address);
    this.communicationEvent.PostalAddress = address;
  }

  public toAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    if (!!this.communicationEvent.ToParty) {
      this.communicationEvent.ToParty.AddPartyContactMechanism(partyContactMechanism);
    }

    const address = partyContactMechanism.ContactMechanism as PostalAddress;

    this.toPostalAddresses.push(address);
    this.communicationEvent.PostalAddress = address;
  }

  public fromPartyAdded(fromParty: Person): void {
    this.addContactRelationship(fromParty);
    this.communicationEvent.FromParty = fromParty;
    this.contacts.push(fromParty);
    this.sortContacts();
  }

  public toPartyAdded(toParty: Person): void {
    this.addContactRelationship(toParty);
    this.communicationEvent.ToParty = toParty;
    this.contacts.push(toParty);
    this.sortContacts();
  }

  private addContactRelationship(party: Person): void {
    if (this.organisation) {
      const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      relationShip.Contact = party;
      relationShip.Organisation = this.organisation;
    }
  }

  public fromPartySelected(party: ISessionObject) {
    if (party) {
      this.updateFromParty(party as Party);
    }
  }

  private sortContacts(): void {
    this.contacts.sort((a, b) => (a.displayName > b.displayName) ? 1 : ((b.displayName > a.displayName) ? -1 : 0));
  }

  private updateFromParty(party: Party): void {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          PartyContactMechanisms: {
            include: {
              ContactMechanism: {
                ContactMechanismType: x,
                PostalAddress_Country: x
              },
            }
          }
        },
      })
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.PartyContactMechanisms as PartyContactMechanism[];
        this.fromPostalAddresses = partyContactMechanisms.filter((v) => v.ContactMechanism.objectType === this.metaService.m.PostalAddress).map((v) => v.ContactMechanism);
      });
  }

  public toPartySelected(party: ISessionObject) {
    if (party) {
      this.updateToParty(party as Party);
    }
  }

  private updateToParty(party: Party): void {
    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          PartyContactMechanisms: {
            include: {
              ContactMechanism: {
                ContactMechanismType: x
              }
            }
          }
        },
      })
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.PartyContactMechanisms as PartyContactMechanism[];
        this.toPostalAddresses = partyContactMechanisms.filter((v) => v.ContactMechanism.objectType === this.metaService.m.PostalAddress).map((v) => v.ContactMechanism);
      });
  }
  public addressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.party.AddPartyContactMechanism(partyContactMechanism);

    const postalAddress = partyContactMechanism.ContactMechanism as PostalAddress;
    this.fromPostalAddresses.push(postalAddress);
    this.communicationEvent.PostalAddress = postalAddress;
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(
        () => {
          const data: IObject = {
            id: this.communicationEvent.id,
            objectType: this.communicationEvent.objectType,
          };

          this.dialogRef.close(data);
          this.refreshService.refresh();
        },
        this.saveService.errorHandler
      );
  }
}
