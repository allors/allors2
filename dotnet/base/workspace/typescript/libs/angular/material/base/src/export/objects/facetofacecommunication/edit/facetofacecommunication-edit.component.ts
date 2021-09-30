import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { Person, Party, Organisation, CommunicationEventPurpose, FaceToFaceCommunication, CommunicationEventState, OrganisationContactRelationship } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { InternalOrganisationId } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { Equals, Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './facetofacecommunication-edit.component.html',
  providers: [ContextService]
})
export class FaceToFaceCommunicationEditComponent extends TestScope implements OnInit, OnDestroy {

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
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<FaceToFaceCommunicationEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
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

          if (!isCreate) {
            pulls = [
              ...pulls,
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
              pull.CommunicationEvent({
                object: this.data.id,
                fetch: {
                  InvolvedParties: {
                    include: {
                      CurrentContacts: x
                    }
                  }
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

  private sortContacts(): void {
    this.contacts.sort((a, b) => (a.displayName > b.displayName) ? 1 : ((b.displayName > a.displayName) ? -1 : 0));
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
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

  private addContactRelationship(party: Person): void {
    if (this.organisation) {
      const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      relationShip.Contact = party;
      relationShip.Organisation = this.organisation;
    }
  }
}
