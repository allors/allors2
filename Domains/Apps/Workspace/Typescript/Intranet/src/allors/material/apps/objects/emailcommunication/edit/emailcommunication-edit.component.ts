import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { Subscription, combineLatest } from 'rxjs';

import {  ContextService, NavigationService, MetaService, RefreshService } from '../../../../../angular';
import { CommunicationEventPurpose, EmailAddress, EmailCommunication, EmailTemplate, Party, Person, Organisation, CommunicationEventState, ContactMechanism, PartyContactMechanism, OrganisationContactRelationship } from '../../../../../domain';
import { PullRequest, Sort, Equals, IObject } from '../../../../../framework';
import { CreateData, SaveService } from '../../../../../material';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './emailcommunication-edit.component.html',
  providers: [ContextService]
})
export class EmailCommunicationEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  addFromParty = false;
  addToParty = false;
  addFromEmail = false;
  addToEmail = false;

  communicationEvent: EmailCommunication;
  party: Party;
  person: Person;
  organisation: Organisation;
  purposes: CommunicationEventPurpose[];
  contacts: Party[] = [];
  fromEmails: ContactMechanism[] = [];
  toEmails: ContactMechanism[] = [];
  title: string;

  emailTemplate: EmailTemplate;

  private subscription: Subscription;
  eventStates: CommunicationEventState[];
  parties: Party[];

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<EmailCommunicationEditComponent>,
    public refreshService: RefreshService,
    public metaService: MetaService,
    public navigation: NavigationService,
    private saveService: SaveService,
    private stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;


    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as IObject).id === undefined;

          let pulls = [
            pull.EmailCommunication({
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
                FromEmail: x,
                ToEmail: x,
                EmailTemplate: x,
                EventPurposes: x,
                CommunicationEventState: x
              }
            }),
            pull.Organisation({
              object: this.stateService.internalOrganisationId,
              name: 'InternalOrganisation',
              include: {
                ActiveEmployees: {
                  CurrentPartyContactMechanisms: {
                    ContactMechanism: x,
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

          if (isCreate) {
            pulls = [
              ...pulls,
              pull.Organisation({
                object: this.data.associationId,
                include: {
                  CurrentContacts: x,
                  CurrentPartyContactMechanisms: {
                    ContactMechanism: x,
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
                          ContactMechanism: x,
                        }
                      }
                    }
                  }
                }
              })
            ];
          }

          if (!isCreate) {
            pulls = [
              ...pulls,
              pull.CommunicationEvent({
                object: this.data.id,
                fetch: {
                  PartiesWhereCommunicationEvent: x
                }
              }),
            ];
          }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {

        this.allors.context.reset();

        this.purposes = loaded.collections.CommunicationEventPurposes as CommunicationEventPurpose[];
        this.eventStates = loaded.collections.CommunicationEventStates as CommunicationEventState[];
        this.parties = loaded.collections.Parties as Party[];

        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;

        this.person = loaded.objects.Person as Person;
        this.organisation = loaded.objects.Organisation as Organisation;

        this.contacts = [];

        if (!!internalOrganisation && internalOrganisation.ActiveEmployees !== undefined) {
          this.contacts = this.contacts.concat(internalOrganisation.ActiveEmployees);
        }

        if (!!this.organisation && this.organisation.CurrentContacts !== undefined) {
          this.contacts = this.contacts.concat(this.organisation.CurrentContacts);
        }

        if (!!this.person) {
          this.contacts.push(this.person);
        }

        if (!!this.parties) {
          this.contacts.push(...this.parties);
          this.parties.forEach((party) => {
            this.contacts.push(...party.CurrentContacts);
          });
        }

        if (isCreate) {
          this.title = 'Add Email';
          this.communicationEvent = this.allors.context.create('EmailCommunication') as EmailCommunication;
          this.emailTemplate = this.allors.context.create('EmailTemplate') as EmailTemplate;
          this.communicationEvent.EmailTemplate = this.emailTemplate;

          this.party = this.organisation || this.person;

        } else {
          this.communicationEvent = loaded.objects.EmailCommunication as EmailCommunication;

          this.updateFromParty(this.communicationEvent.FromParty);
          this.updateToParty(this.communicationEvent.ToParty);

          if (this.communicationEvent.CanWriteActualEnd) {
            this.title = 'Edit Email';
          } else {
            this.title = 'View Email';
          }
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public fromEmailAdded(partyContactMechanism: PartyContactMechanism): void {

    if (!!this.communicationEvent.FromParty) {
      this.communicationEvent.FromParty.AddPartyContactMechanism(partyContactMechanism);
    }

    const emailAddress = partyContactMechanism.ContactMechanism as EmailAddress;

    this.fromEmails.push(emailAddress);
    this.communicationEvent.FromEmail = emailAddress;
  }

  public toEmailAdded(partyContactMechanism: PartyContactMechanism): void {

    if (!!this.communicationEvent.ToParty) {
      this.communicationEvent.ToParty.AddPartyContactMechanism(partyContactMechanism);
    }

    const emailAddress = partyContactMechanism.ContactMechanism as EmailAddress;

    this.toEmails.push(emailAddress);
    this.communicationEvent.FromEmail = emailAddress;
  }

  public fromPartyAdded(fromParty: Person): void {
    this.addContactRelationship(fromParty);
    this.communicationEvent.FromParty = fromParty;
    this.contacts.push(fromParty);
  }

  public toPartyAdded(toParty: Person): void {
    this.addContactRelationship(toParty);
    this.communicationEvent.ToParty = toParty;
    this.contacts.push(toParty);
  }

  public fromPartySelected(party: Party) {
    if (party) {
      this.updateFromParty(party);
    }
  }

  private addContactRelationship(party: Person): void {
    if (this.organisation) {
      const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      relationShip.Contact = party;
      relationShip.Organisation = this.organisation;
    }
  }

  private updateFromParty(party: Party): void {
    const { pull, tree, x } = this.metaService;

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
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.PartyContactMechanisms as PartyContactMechanism[];
        this.fromEmails = partyContactMechanisms.filter((v) => v.ContactMechanism.objectType === this.metaService.m.EmailAddress).map((v) => v.ContactMechanism);
      });
  }

  public toPartySelected(party: Party) {
    if (party) {
      this.updateToParty(party);
    }
  }

  private updateToParty(party: Party): void {
    const { pull, tree, x } = this.metaService;

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
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.PartyContactMechanisms as PartyContactMechanism[];
        this.toEmails = partyContactMechanisms.filter((v) => v.ContactMechanism.objectType === this.metaService.m.EmailAddress).map((v) => v.ContactMechanism);
      });
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.communicationEvent.id,
          objectType: this.communicationEvent.objectType,
        };

        this.dialogRef.close(data);
      },
      this.saveService.errorHandler
    );
  }
}
