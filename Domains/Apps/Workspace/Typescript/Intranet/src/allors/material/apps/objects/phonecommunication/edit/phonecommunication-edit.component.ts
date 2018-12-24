import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationService, MetaService, RefreshService } from '../../../../../angular';
import { CommunicationEventPurpose, ContactMechanism, PhoneCommunication, Party, PartyContactMechanism, Person, Organisation, TelecommunicationsNumber, OrganisationContactRelationship, CommunicationEventState } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap, map } from 'rxjs/operators';
import { ObjectData, EditData, CreateData } from 'src/allors/material/base/services/object';

@Component({
  templateUrl: './phonecommunication-edit.component.html',
  providers: [ContextService]
})
export class PhoneCommunicationEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  addCaller = false;
  addReceiver = false;
  addPhoneNumber = false;

  communicationEvent: PhoneCommunication;
  party: Party;
  person: Person;
  organisation: Organisation;
  purposes: CommunicationEventPurpose[];
  contacts: Party[] = [];
  phonenumbers: ContactMechanism[] = [];
  employees: Person[] = [];
  title: string;

  private subscription: Subscription;
  eventStates: CommunicationEventState[];

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & EditData,
    public dialogRef: MatDialogRef<PhoneCommunicationEditComponent>,
    public refreshService: RefreshService,
    public metaService: MetaService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as EditData).id === undefined;

          let pulls = [
            pull.PhoneCommunication({
              object: this.data.id,
              include: {
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
                  CurrentPartyContactMechanisms: {
                    ContactMechanism: x
                  }
                }
              }),
              pull.Person({
                object: this.data.associationId,
                include: {
                  CurrentPartyContactMechanisms: {
                    ContactMechanism: x
                  }
                }
              }),
              pull.Person({
                object: this.data.associationId,
                fetch: {
                  OrganisationContactRelationshipsWhereContact: {
                    Organisation: {
                      include: {
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

        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        this.employees = internalOrganisation.ActiveEmployees;

        this.person = loaded.objects.Person as Person;
        this.organisation = loaded.objects.Organisation as Organisation;

        this.party = this.organisation || this.person;

        this.contacts = this.contacts.concat(internalOrganisation.ActiveEmployees);

        if (!!this.organisation) {
          this.contacts = this.contacts.concat(this.organisation.CurrentContacts);
        }

        if (!!this.person) {
          this.contacts.push(this.person);
        }

        // TODO: phone number from organisation, person or contacts ...
        // this.phonenumbers = this.party.CurrentPartyContactMechanisms.filter((v) => v.ContactMechanism.objectType === m.TelecommunicationsNumber).map((v) => v.ContactMechanism);

        if (isCreate) {
          this.title = 'Add Phone call';
          this.communicationEvent = this.allors.context.create('PhoneCommunication') as PhoneCommunication;
          this.communicationEvent.IncomingCall = false;

          this.party.AddCommunicationEvent(this.communicationEvent);
        } else {

          this.communicationEvent = loaded.objects.PhoneCommunication as PhoneCommunication;

          if (this.communicationEvent.CanWriteActualEnd) {
            this.title = 'Edit Phone call';
          } else {
            this.title = 'View Phone call';
          }
        }
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public phoneNumberAdded(partyContactMechanism: PartyContactMechanism): void {

    if (!!this.organisation) {
      this.organisation.AddPartyContactMechanism(partyContactMechanism);
    }

    const phonenumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
    this.communicationEvent.AddContactMechanism(phonenumber);

    this.phonenumbers.push(phonenumber);
  }

  public callerAdded(caller: Person): void {

    const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = caller;
    relationShip.Organisation = this.organisation;

    this.communicationEvent.AddCaller(caller);
  }

  public receiverAdded(receiver: Person): void {

    const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = receiver;
    relationShip.Organisation = this.organisation;

    this.communicationEvent.AddReceiver(receiver);
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: ObjectData = {
          id: this.communicationEvent.id,
          objectType: this.communicationEvent.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
