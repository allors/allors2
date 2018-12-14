import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, Saved, ContextService, NavigationService, NavigationActivatedRoute, MetaService, RefreshService } from '../../../../../angular';
import { CommunicationEventPurpose, ContactMechanism, InternalOrganisation, LetterCorrespondence, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, PostalAddress } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './lettercorrespondence-create.component.html',
  providers: [ContextService]
})
export class LetterCorrespondenceCreateComponent
  implements OnInit, OnDestroy {
  public title = 'Letter Correspondence';

  public addSender = false;
  public addReceiver = false;
  public addAddress = false;

  public m: Meta;

  public party: Party;
  public person: Person;
  public organisation: Organisation;
  public purposes: CommunicationEventPurpose[];
  public contacts: Party[] = [];

  public postalAddresses: ContactMechanism[] = [];

  public communicationEvent: LetterCorrespondence;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<LetterCorrespondenceCreateComponent>,
    public refreshService: RefreshService,
    public metaService: MetaService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService
  ) {
    this.m = this.metaService.m;
  }

  get PartyIsOrganisation(): boolean {
    return this.party.objectType.name === 'Organisation';
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const organisationId = this.data && this.data.organisationId;
          const personId = this.data && this.data.organisationId;

          let pulls = [
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

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.purposes = loaded.collections.CommunicationEventPurposes as CommunicationEventPurpose[];
        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.postalAddresses = internalOrganisation.ActiveEmployees
          .map((v) =>
            v.CurrentPartyContactMechanisms
              .filter((w) => w && w.ContactMechanism.objectType === m.EmailAddress)
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
        this.contacts = this.contacts.concat(this.organisation.CurrentContacts);
        if (!!this.person) {
          this.contacts.push(this.person);
        }

        this.communicationEvent = loaded.objects.LetterCorrespondence as LetterCorrespondence;

        if (!this.communicationEvent) {
          this.communicationEvent = this.allors.context.create('LetterCorrespondence') as LetterCorrespondence;
          this.communicationEvent.IncomingLetter = true;
        }
      },
        (error: any) => {
          this.errorService.handle(error);
          this.dialogRef.close();
        }
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

    this.communicationEvent.AddOriginator(sender);
  }

  public receiverAdded(id: string): void {

    this.addReceiver = false;

    const receiver: Person = this.allors.context.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = receiver;
    relationShip.Organisation = this.organisation;

    this.communicationEvent.AddReceiver(receiver);
  }

  public addressAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addAddress = false;

    this.party.AddPartyContactMechanism(partyContactMechanism);

    const postalAddress = partyContactMechanism.ContactMechanism as PostalAddress;
    this.postalAddresses.push(postalAddress);
    this.communicationEvent.AddPostalAddress(postalAddress);
  }

  public save(): void {

    this.allors.context.save().subscribe(
      (saved: Saved) => {
        this.dialogRef.close();
      },
      (error: Error) => {
        this.errorService.handle(error);
      }
    );
  }
}
