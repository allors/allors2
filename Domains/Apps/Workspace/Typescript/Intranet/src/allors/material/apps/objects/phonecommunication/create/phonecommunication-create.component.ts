import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationService, NavigationActivatedRoute, MetaService, RefreshService } from '../../../../../angular';
import { CommunicationEventPurpose, ContactMechanism, PhoneCommunication, InternalOrganisation, Party, PartyContactMechanism, Person, Organisation, TelecommunicationsNumber, OrganisationContactRelationship } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './phonecommunication-create.component.html',
  providers: [ContextService]
})
export class PhoneCommunicationCreateComponent implements OnInit, OnDestroy {

  public title = 'Phone Communication';

  public addCaller = false;
  public addReceiver = false;
  public addPhoneNumber = false;

  public m: MetaDomain;

  public party: Party;
  public person: Person;
  public organisation: Organisation;
  public purposes: CommunicationEventPurpose[];
  public contacts: Party[] = [];
  public phonenumbers: ContactMechanism[] = [];
  public employees: Person[];

  public communicationEvent: PhoneCommunication;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<PhoneCommunicationCreateComponent>,
    public refreshService: RefreshService,
    public metaService: MetaService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

          const organisationId = this.data && this.data.organisationId;
          const personId = this.data && this.data.organisationId;

          const pulls = [
            pull.Organisation({
              object: internalOrganisationId,
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
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();

        this.purposes = loaded.collections.CommunicationEventPurposes as CommunicationEventPurpose[];
        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.employees = internalOrganisation.ActiveEmployees;

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

        this.communicationEvent = this.allors.context.create('PhoneCommunication') as PhoneCommunication;

        // TODO: phone number from organisation, person or contacts ...
        this.phonenumbers = this.party.CurrentPartyContactMechanisms.filter((v) => v.ContactMechanism.objectType === m.TelecommunicationsNumber.objectType).map((v) => v.ContactMechanism);

      },
        (error: any) => {
          this.errorService.handle(error);
          this.dialogRef.close();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }


  public phoneNumberCancelled(): void {
    this.addPhoneNumber = false;
  }

  public phoneNumberAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addPhoneNumber = false;

    if (!!this.organisation) {
      this.organisation.AddPartyContactMechanism(partyContactMechanism);
    }

    const phonenumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
    this.communicationEvent.AddContactMechanism(phonenumber);

    this.phonenumbers.push(phonenumber);
  }

  public callerCancelled(): void {
    this.addCaller = false;
  }

  public callerAdded(id: string): void {

    this.addCaller = false;

    const person: Person = this.allors.context.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = person;
    relationShip.Organisation = this.organisation;

    this.communicationEvent.AddCaller(person);
  }

  public receiverCancelled(): void {
    this.addReceiver = false;
  }

  public receiverAdded(id: string): void {

    this.addReceiver = false;

    const person: Person = this.allors.context.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = person;
    relationShip.Organisation = this.organisation;

    this.communicationEvent.AddReceiver(person);
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        this.dialogRef.close();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
