import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationService, NavigationActivatedRoute, MetaService } from '../../../../../angular';
import { CommunicationEventPurpose, ContactMechanism, EmailAddress, EmailCommunication, EmailTemplate, InternalOrganisation, Party, PartyContactMechanism, Person, Organisation } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './facetofacecommunication-create.component.html',
  providers: [ContextService]
})
export class FaceToFaceCommunicationCreateComponent implements OnInit, OnDestroy {

  public title = 'Meeting';

  public addOriginator = false;
  public addAddressee = false;

  public m: Meta;

  public party: Party;
  public person: Person;
  public organisation: Organisation;
  public purposes: CommunicationEventPurpose[];
  public contacts: Party[] = [];

  public ownEmailAddresses: EmailAddress[] = [];
  public allEmailAddresses: EmailAddress[];
  public emailTemplate: EmailTemplate;

  public communicationEvent: EmailCommunication;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<FaceToFaceCommunicationCreateComponent>,
    public metaService: MetaService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

          const organisationId = this.data && this.data.organisationId;
          const personId = this.data && this.data.organisationId;

          let pulls = [
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
            pull.EmailAddress({
              sort: new Sort(m.EmailAddress.ElectronicAddressString)
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
                    ContactMechanism: x,
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
                          ContactMechanism: x,
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
        this.allEmailAddresses = loaded.collections.EmailAddresses as EmailAddress[];
        this.ownEmailAddresses = internalOrganisation.ActiveEmployees
          .map((v) => v.CurrentPartyContactMechanisms
            .filter((w) => w && w.ContactMechanism.objectType === m.EmailAddress)
            .map((w) => w.ContactMechanism as EmailAddress))
          .reduce((acc, v) => acc.concat(v), []);

        this.person = loaded.objects.Person as Person;
        this.organisation = loaded.objects.Organisation as Organisation;
        if (!this.organisation && loaded.collections.Organisations && loaded.collections.Organisations.length > 0) {
          // TODO: check active
          this.organisation = loaded.collections.Organisations[0] as Organisation;
        }

        this.party = this.organisation || this.person;

        this.contacts = this.contacts.concat(internalOrganisation.ActiveEmployees);
        if (!!this.organisation) {
          this.contacts = this.contacts.concat(this.organisation.CurrentContacts);
        }
        if (!!this.person) {
          this.contacts.push(this.person);
        }

        this.communicationEvent = this.allors.context.create('EmailCommunication') as EmailCommunication;
        this.emailTemplate = this.allors.context.create('EmailTemplate') as EmailTemplate;
        this.communicationEvent.EmailTemplate = this.emailTemplate;
        this.communicationEvent.Originator = this.party && this.party.GeneralEmail;
        this.communicationEvent.IncomingMail = false;
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
