import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, Saved, x, Allors, NavigationService } from '../../../../../../angular';
import { CommunicationEvent, ContactMechanism, InternalOrganisation, Organisation, OrganisationContactRelationship, OrganisationRole, PartyContactMechanism, Person, TelecommunicationsNumber } from '../../../../../../domain';
import { PullRequest } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/StateService';
import { Fetcher } from '../../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { Title } from '@angular/platform-browser';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './organisation-overview.component.html',
  providers: [Allors]
})
export class OrganisationOverviewComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title = 'Organisation overview';
  public organisation: Organisation;
  public internalOrganisation: InternalOrganisation;
  public communicationEvents: CommunicationEvent[];

  public contactsCollection = 'Current';
  public currentContactRelationships: OrganisationContactRelationship[] = [];
  public inactiveContactRelationships: OrganisationContactRelationship[] = [];
  public allContactRelationships: OrganisationContactRelationship[] = [];

  public contactMechanismsCollection = 'Current';
  public currentContactMechanisms: PartyContactMechanism[] = [];
  public inactiveContactMechanisms: PartyContactMechanism[] = [];
  public allContactMechanisms: PartyContactMechanism[] = [];

  public roles: OrganisationRole[];
  public activeRoles: OrganisationRole[] = [];
  public rolesText: string;
  private customerRole: OrganisationRole;
  private supplierRole: OrganisationRole;
  private manufacturerRole: OrganisationRole;
  private isActiveCustomer: boolean;
  private isActiveSupplier: boolean;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private fetcher: Fetcher;

  constructor(
    @Self() public allors: Allors,
    public navigation: NavigationService,
    public router: Router,
    private errorService: ErrorService,
    private titleService: Title,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    titleService.setTitle(this.title);

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  get contactRelationships(): any {

    switch (this.contactsCollection) {
      case 'Current':
        return this.currentContactRelationships;
      case 'Inactive':
        return this.inactiveContactRelationships;
      case 'All':
      default:
        return this.allContactRelationships;
    }
  }

  get contactMechanisms(): any {

    switch (this.contactMechanismsCollection) {
      case 'Current':
        return this.currentContactMechanisms;
      case 'Inactive':
        return this.inactiveContactMechanisms;
      case 'All':
      default:
        return this.allContactMechanisms;
    }
  }

  public ngOnInit(): void {

    const { m, pull, tree, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const organisationContactRelationshipTree = tree.OrganisationContactRelationship(
            {
              ContactKinds: x,
              Contact: {
                PartyContactMechanisms: {
                  ContactMechanism: {
                    ContactMechanismType: x,
                  }
                },
                Salutation: x,
              },
            }
          );

          const partyContactMechanismTree = tree.PartyContactMechanism(
            {
              ContactMechanism: {
                ContactMechanismType: x,
                PostalAddress_PostalBoundary: {
                  Country: x,
                },
              },
              ContactPurposes: x
            }
          );

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Organisation({
              object: id,
              include: {
                Locale: x,
                LogoImage: x,
                IndustryClassifications: x,
                OrganisationClassifications: x,
                LastModifiedBy: x,
                CurrentContacts: {
                  PartyContactMechanisms: x,
                },
                CurrentOrganisationContactRelationships: organisationContactRelationshipTree,
                InactiveOrganisationContactRelationships: organisationContactRelationshipTree,
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
                  }
                }
              }
            }),
            pull.OrganisationRole()
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        scope.session.reset();
        this.internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;

        this.organisation = loaded.objects.Organisation as Organisation;
        this.communicationEvents = loaded.collections.CommunicationEvents as CommunicationEvent[];

        this.currentContactRelationships = this.organisation.CurrentOrganisationContactRelationships as OrganisationContactRelationship[];
        this.inactiveContactRelationships = this.organisation.InactiveOrganisationContactRelationships as OrganisationContactRelationship[];
        this.allContactRelationships = this.currentContactRelationships.concat(this.inactiveContactRelationships);

        this.currentContactMechanisms = this.organisation.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.inactiveContactMechanisms = this.organisation.InactivePartyContactMechanisms as PartyContactMechanism[];
        this.allContactMechanisms = this.currentContactMechanisms.concat(this.inactiveContactMechanisms);

        this.roles = loaded.collections.OrganisationRoles as OrganisationRole[];
        this.customerRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === '8B5E0CEE-4C98-42F1-8F18-3638FBA943A0');
        this.supplierRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === '8C6D629B-1E27-4520-AA8C-E8ADF93A5095');
        this.manufacturerRole = this.roles.find((v: OrganisationRole) => v.UniqueId.toUpperCase() === '32E74BEF-2D79-4427-8902-B093AFA81661');

        this.activeRoles = [];
        this.rolesText = '';
        if (this.internalOrganisation.ActiveCustomers.includes(this.organisation)) {
          this.isActiveCustomer = true;
          this.activeRoles.push(this.customerRole);
        }

        if (this.internalOrganisation.ActiveSuppliers.includes(this.organisation)) {
          this.isActiveSupplier = true;
          this.activeRoles.push(this.supplierRole);
        }

        if (this.organisation.IsManufacturer) {
          this.activeRoles.push(this.manufacturerRole);
        }

        if (this.activeRoles.length > 0) {
          this.rolesText = this.activeRoles
            .map((v: OrganisationRole) => v.Name)
            .reduce((acc: string, cur: string) => acc + ', ' + cur);
        }
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public removeContact(organisationContactRelationship: OrganisationContactRelationship): void {
    const { scope } = this.allors;

    organisationContactRelationship.ThroughDate = new Date();
    scope
      .save()
      .subscribe((saved: Saved) => {
        scope.session.reset();
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public activateContact(organisationContactRelationship: OrganisationContactRelationship): void {
    const { scope } = this.allors;

    organisationContactRelationship.ThroughDate = undefined;
    scope
      .save()
      .subscribe((saved: Saved) => {
        scope.session.reset();
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public deleteContact(person: Person): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(person.Delete)
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
    const { scope } = this.allors;

    partyContactMechanism.ThroughDate = new Date();
    scope
      .save()
      .subscribe((saved: Saved) => {
        scope.session.reset();
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public activateContactMechanism(partyContactMechanism: PartyContactMechanism): void {
    const { scope } = this.allors;

    partyContactMechanism.ThroughDate = undefined;
    scope
      .save()
      .subscribe((saved: Saved) => {
        scope.session.reset();
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public deleteContactMechanism(contactMechanism: ContactMechanism): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(contactMechanism.Delete)
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

  public cancelCommunication(communicationEvent: CommunicationEvent): void {
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to cancel this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Cancel)
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
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to close this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Close)
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
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to reopen this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Reopen)
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
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(communicationEvent.Delete)
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

  public goBack(): void {
    window.history.back();
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public isPhone(contactMechanism: ContactMechanism): boolean {
    if (contactMechanism.objectType.name === 'TelecommunicationsNumber') {
      const phoneCommunication: TelecommunicationsNumber = contactMechanism as TelecommunicationsNumber;
      return phoneCommunication.ContactMechanismType && phoneCommunication.ContactMechanismType.Name === 'Phone';
    }
  }
}
