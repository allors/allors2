import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { CommunicationEventPurpose, Party, Person, Organisation, FaceToFaceCommunication, OrganisationContactRelationship, CommunicationEventState } from '../../../../../domain';
import { PullRequest, Sort, Equals, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { CreateData } from '../../../../../material/base/services/object';

@Component({
  templateUrl: './facetofacecommunication-edit.component.html',
  providers: [ContextService]
})
export class FaceToFaceCommunicationEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  addFromParty = false;
  addToParty = false;

  party: Party;
  person: Person;
  organisation: Organisation;
  purposes: CommunicationEventPurpose[];
  contacts: Party[] = [];
  communicationEvent: FaceToFaceCommunication;
  eventStates: CommunicationEventState[];
  title: string;

  private subscription: Subscription;
  parties: Party[];

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<FaceToFaceCommunicationEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
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
            pull.FaceToFaceCommunication({
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
                  PartiesWhereCommunicationEvent: {
                    include: {
                      CurrentContacts: x
                    }
                  }
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
          this.title = 'Add Meeting';
          this.communicationEvent = this.allors.context.create('FaceToFaceCommunication') as FaceToFaceCommunication;

          this.party = this.organisation || this.person;
        } else {
          this.communicationEvent = loaded.objects.FaceToFaceCommunication as FaceToFaceCommunication;

          if (this.communicationEvent.CanWriteActualEnd) {
            this.title = 'Edit Meeting';
          } else {
            this.title = 'View Meeting';
          }
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
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

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.communicationEvent.id,
          objectType: this.communicationEvent.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  private addContactRelationship(party: Person): void {
    if (this.organisation) {
      const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      relationShip.Contact = party;
      relationShip.Organisation = this.organisation;
    }
  }
}
