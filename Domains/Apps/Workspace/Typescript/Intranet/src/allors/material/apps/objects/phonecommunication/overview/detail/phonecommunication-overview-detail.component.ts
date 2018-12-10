import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, ContextService, NavigationService, NavigationActivatedRoute, MetaService, PanelService, RefreshService } from '../../../../../../angular';
import { CommunicationEventPurpose, ContactMechanism, PhoneCommunication, InternalOrganisation, Party, PartyContactMechanism, Person, Organisation, TelecommunicationsNumber, OrganisationContactRelationship } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, map, filter } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'phonecommunication-overview-detail',
  templateUrl: './phonecommunication-overview-detail.component.html',
  providers: [ContextService, PanelService]
})
export class PhoneCommunicationOverviewDetailComponent implements OnInit, OnDestroy {

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

  public phoneCommunication: PhoneCommunication;

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
    panel.icon = 'phone';
    panel.expandable = true;

    // Minimized
    const pullName = `${this.panel.name}_${this.m.PhoneCommunication.objectType.name}`;

    panel.onPull = (pulls) => {
      if (this.panel.isCollapsed) {
        const { pull, x } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.PhoneCommunication({
            name: pullName,
            object: id,
            include: {
              FromParties: x,
              ToParties: x,
              EventPurposes: x,
              CommunicationEventState: x,
              ContactMechanisms: x,
              WorkEfforts: {
                WorkEffortState: x,
                Priority: x
              }
            }
          })
        );
      }
    };

    panel.onPulled = (loaded) => {
      if (this.panel.isCollapsed) {
        this.phoneCommunication = loaded.objects[pullName] as PhoneCommunication;
      }
    };
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.route.queryParams, this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        filter((v) => {
          return this.panel.isExpanded;
        }),
        switchMap(([, , , internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);

          const id = this.panel.manager.id;
          const personId = navRoute.queryParam(m.Person);
          const organisationId = navRoute.queryParam(m.Organisation);

          let pulls = [
            pull.PhoneCommunication({
              object: id,
              include: {
                FromParties: x,
                ToParties: x,
                EventPurposes: x,
                CommunicationEventState: x,
              }
            }),
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

        this.phoneCommunication = loaded.objects.PhoneCommunication as PhoneCommunication;

        // TODO: phone number from organisation, person or contacts ...
        this.phonenumbers = this.party.CurrentPartyContactMechanisms.filter((v) => v.ContactMechanism.objectType === m.TelecommunicationsNumber.objectType).map((v) => v.ContactMechanism);

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


  public phoneNumberCancelled(): void {
    this.addPhoneNumber = false;
  }

  public phoneNumberAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addPhoneNumber = false;

    if (!!this.organisation) {
      this.organisation.AddPartyContactMechanism(partyContactMechanism);
    }

    const phonenumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
    this.phoneCommunication.AddContactMechanism(phonenumber);

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

    this.phoneCommunication.AddCaller(person);
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

    this.phoneCommunication.AddReceiver(person);
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
