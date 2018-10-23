import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, Saved, x, Allors, NavigationActivatedRoute, NavigationService } from '../../../../../../angular';
import { CommunicationEventPurpose, ContactMechanism, InternalOrganisation, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, PhoneCommunication, TelecommunicationsNumber } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/StateService';
import { switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './phonecommunication-edit.component.html',
  providers: [Allors]
})
export class EditPhoneCommunicationComponent implements OnInit, OnDestroy {

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

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  get PartyIsOrganisation(): boolean {
    return this.party.objectType.name === 'Organisation';
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.param();
          const personId = navRoute.queryParam(m.Person);
          const organisationId = navRoute.queryParam(m.Organisation);

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
            pull.CommunicationEventPurpose({
              predicate: new Equals({ propertyType: m.CommunicationEventPurpose.IsActive, value: true }),
              sort: new Sort(m.CommunicationEventPurpose.Name)
            }),
          ];

          const add = !id;

          if (add) {
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

          } else {
            pulls = [
              ...pulls,
              pull.FaceToFaceCommunication({
                object: id,
                include: {
                  FromParties: x,
                  ToParties: x,
                  EventPurposes: x,
                  CommunicationEventState: x,
                }
              }),
            ];
          }

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, add }))
            );
        })
      )
      .subscribe(({ loaded, add }) => {

        scope.session.reset();

        this.purposes = loaded.collections.CommunicationEventPurposes as CommunicationEventPurpose[];
        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.employees = internalOrganisation.ActiveEmployees;

        if (add) {
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

          this.communicationEvent = scope.session.create('PhoneCommunication') as PhoneCommunication;
        } else {
          this.communicationEvent = loaded.objects.PhoneCommunication as PhoneCommunication;
        }

        // TODO: phone number from organisation, person or contacts ...
        this.phonenumbers = this.party.CurrentPartyContactMechanisms.filter((v) => v.ContactMechanism.objectType === m.TelecommunicationsNumber._objectType).map((v) => v.ContactMechanism);

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
    const { scope } = this.allors;

    this.addCaller = false;

    const person: Person = scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = person;
    relationShip.Organisation = this.organisation;

    this.communicationEvent.AddCaller(person);
  }

  public receiverCancelled(): void {
    this.addReceiver = false;
  }

  public receiverAdded(id: string): void {
    const { scope } = this.allors;

    this.addReceiver = false;

    const person: Person = scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = person;
    relationShip.Organisation = this.organisation;

    this.communicationEvent.AddReceiver(person);
  }

  public cancel(): void {
    const { scope } = this.allors;

    const cancelFn: () => void = () => {
      scope.invoke(this.communicationEvent.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      // TODO:
      /*  this.dialogService
        .openConfirm({ message: 'Save changes?' })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe((saved: Saved) => {
                scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.handle(error);
              });
          } else {
            cancelFn();
          }
        }); */
    } else {
      cancelFn();
    }
  }

  public close(): void {
    const { scope } = this.allors;

    const cancelFn: () => void = () => {
      scope.invoke(this.communicationEvent.Close)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      // TODO:
      /*  this.dialogService
        .openConfirm({ message: 'Save changes?' })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe((saved: Saved) => {
                scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.handle(error);
              });
          } else {
            cancelFn();
          }
        }); */
    } else {
      cancelFn();
    }
  }

  public reopen(): void {
    const { scope } = this.allors;

    const cancelFn: () => void = () => {
      scope.invoke(this.communicationEvent.Reopen)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      // TODO:
      /*  this.dialogService
         .openConfirm({ message: 'Save changes?' })
         .afterClosed().subscribe((confirm: boolean) => {
           if (confirm) {
             scope
               .save()
               .subscribe((saved: Saved) => {
                 scope.session.reset();
                 cancelFn();
               },
               (error: Error) => {
                 this.errorService.handle(error);
               });
           } else {
             cancelFn();
           }
         }); */
    } else {
      cancelFn();
    }
  }

  public save(): void {
    const { scope } = this.allors;

    scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
