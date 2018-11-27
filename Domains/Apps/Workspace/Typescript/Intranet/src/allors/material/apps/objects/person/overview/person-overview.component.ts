import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, Saved, SessionService, NavigationService, NavigationActivatedRoute } from '../../../../../angular';
import { CommunicationEvent, ContactMechanism, InternalOrganisation, Organisation, OrganisationContactKind, OrganisationContactRelationship, PartyContactMechanism, Person, PersonRole, WorkEffort, WorkEffortPartyAssignment, SerialisedItem } from '../../../../../domain';
import { PullRequest } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './person-overview.component.html',
  providers: [SessionService]
})
export class PersonOverviewComponent implements OnInit, OnDestroy {

  m: MetaDomain;

  title = 'Person Overview';
  person: Person;
  organisation: Organisation;
  internalOrganisation: InternalOrganisation;

  communicationEvents: CommunicationEvent[];
  workEffortPartyAssignments: WorkEffortPartyAssignment[];

  currentContactMechanisms: PartyContactMechanism[] = [];
  inactiveContactMechanisms: PartyContactMechanism[] = [];
  allContactMechanisms: PartyContactMechanism[] = [];
  contactKindsText: string;

  isActiveCustomer: boolean;
  isActiveEmployee: boolean;

  roles: PersonRole[];
  activeRoles: PersonRole[] = [];
  rolesText: string;

  private customerRole: PersonRole;
  private employeeRole: PersonRole;
  private contactRole: PersonRole;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  private fetcher: Fetcher;
  serialisedItems: SerialisedItem[];

  constructor(
    @Self() private allors: SessionService,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    titleService.setTitle(this.title);

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }


  public ngOnInit(): void {

    const { m, pull, tree, x } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();

          const partyContactMechanismTree = tree.PartyContactMechanism({
            ContactPurposes: x,
            ContactMechanism: {
              PostalAddress_PostalBoundary: {
                Country: x,
              }
            },
          });

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Person({
              object: id,
              include: {
                Locale: x,
                LastModifiedBy: x,
                Salutation: x,
                PartyContactMechanisms: partyContactMechanismTree,
                CurrentPartyContactMechanisms: partyContactMechanismTree,
                InactivePartyContactMechanisms: partyContactMechanismTree,
                GeneralCorrespondence: {
                  PostalBoundary: {
                    Country: x,
                  }
                }
              }
            }),
            pull.Party({
              object: id,
              fetch: {
                CommunicationEventsWhereInvolvedParty: {
                  include: {
                    CommunicationEventState: x,
                    FromParties: x,
                    ToParties: x,
                    InvolvedParties: x,
                  }
                }
              }
            }),
            pull.Person({
              object: id,
              fetch: {
                WorkEffortPartyAssignmentsWhereParty: {
                  include: {
                    Assignment: {
                      WorkEffortState: x,
                      Priority: x,
                    }
                  }
                }
              }
            }),
            pull.Person({
              object: id,
              fetch: {
                OrganisationContactRelationshipsWhereContact: {
                  include: {
                    Organisation: x,
                    ContactKinds: x,
                  }
                }
              }
            }),
            pull.Party({
              object: id,
              name: 'OwnedSerialisedItems',
              fetch: {
                SerialisedItemsWhereOwnedBy: {
                  include: {
                    SerialisedItemState: x
                  }
                }
              }
            }),
            pull.Party({
              object: id,
              name: 'RentedSerialisedItems',
              fetch: {
                SerialisedItemsWhereRentedBy: {
                  include: {
                    SerialisedItemState: x
                  }
                }
              }
            }),
            pull.PersonRole()
          ];

          return this.allors
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();

        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.person = loaded.objects.Person as Person;

        const organisationContactRelationships = loaded.collections.OrganisationContactRelationships as OrganisationContactRelationship[];

        if (organisationContactRelationships.length > 0) {
          const organisationContactRelationship = organisationContactRelationships[0];
          this.organisation = organisationContactRelationship.Organisation as Organisation;
          this.contactKindsText = organisationContactRelationship.ContactKinds
            .map((v: OrganisationContactKind) => v.Description)
            .reduce((acc: string, cur: string) => acc + ', ' + cur);
        }

        this.communicationEvents = loaded.collections.CommunicationEvents as CommunicationEvent[];
        this.workEffortPartyAssignments = loaded.collections.WorkEffortAssignmentsWhereProfessional as WorkEffortPartyAssignment[];

        this.currentContactMechanisms = this.person.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.inactiveContactMechanisms = this.person.InactivePartyContactMechanisms as PartyContactMechanism[];
        this.allContactMechanisms = this.currentContactMechanisms.concat(this.inactiveContactMechanisms);

        const ownedSerialisedItems = loaded.collections.OwnedSerialisedItems as SerialisedItem[];
        const rentedSerialisedItems = loaded.collections.RentedSerialisedItems as SerialisedItem[];
        this.serialisedItems = ownedSerialisedItems.concat(rentedSerialisedItems);

        this.roles = loaded.collections.PersonRoles as PersonRole[];
        this.customerRole = this.roles.find((v) => v.UniqueId.toUpperCase() === 'B29444EF-0950-4D6F-AB3E-9C6DC44C050F');
        this.employeeRole = this.roles.find((v) => v.UniqueId.toUpperCase() === 'DB06A3E1-6146-4C18-A60D-DD10E19F7243');
        this.contactRole = this.roles.find((v) => v.UniqueId.toUpperCase() === 'FA2DF11E-7795-4DF7-8B3F-4FD87D0C4D8E');

        this.activeRoles = [];
        this.rolesText = '';
        if (this.internalOrganisation.ActiveCustomers.includes(this.person)) {
          this.isActiveCustomer = true;
          this.activeRoles.push(this.customerRole);
        }

        if (this.internalOrganisation.ActiveEmployees.includes(this.person)) {
          this.isActiveEmployee = true;
          this.activeRoles.push(this.employeeRole);
        }

        if (this.organisation !== null) {
          this.activeRoles.push(this.contactRole);
        }

        if (this.activeRoles.length > 0) {
          this.rolesText = this.activeRoles
            .map((v: PersonRole) => v.Name)
            .reduce((acc: string, cur: string) => acc + ', ' + cur);
        }
      }, this.errorService.handler);
  }

  public cancelCommunication(communicationEvent: CommunicationEvent): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to cancel this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(communicationEvent.Cancel)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public closeCommunication(communicationEvent: CommunicationEvent): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to close this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(communicationEvent.Close)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public reopenCommunication(communicationEvent: CommunicationEvent): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to reopen this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(communicationEvent.Reopen)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public deleteCommunication(communicationEvent: CommunicationEvent): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(communicationEvent.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public removeContactMechanism(partyContactMechanism: PartyContactMechanism): void {

    partyContactMechanism.ThroughDate = new Date();
    this.allors
      .save()
      .subscribe((saved: Saved) => {
        this.allors.session.reset();
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public activateContactMechanism(partyContactMechanism: PartyContactMechanism): void {

    partyContactMechanism.ThroughDate = undefined;
    this.allors
      .save()
      .subscribe((saved: Saved) => {
        this.allors.session.reset();
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public deleteContactMechanism(contactMechanism: ContactMechanism): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(contactMechanism.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public deleteWorkEffort(workEffort: WorkEffort): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this work effort?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(workEffort.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public deleteSerialisedItem(item: SerialisedItem): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(item.Delete)
            .subscribe((invoked: Invoked) => {

              this.serialisedItems = this.serialisedItems.filter(v => v !== item);

              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }
}
