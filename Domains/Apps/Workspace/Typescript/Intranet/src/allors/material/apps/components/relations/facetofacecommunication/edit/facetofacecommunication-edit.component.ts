import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, x, Allors, NavigationActivatedRoute, NavigationService } from '../../../../../../angular';
import { CommunicationEventPurpose, FaceToFaceCommunication, InternalOrganisation, Organisation, OrganisationContactRelationship, Party, Person, Singleton } from '../../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { switchMap, map } from 'rxjs/operators';
import { load, loadQueryList } from '@angular/core/src/render3/instructions';

@Component({
  templateUrl: './facetofacecommunication-edit.component.html',
  providers: [Allors]
})
export class EditFaceToFaceCommunicationComponent implements OnInit, OnDestroy {

  public title = 'Face to Face Communication (Meeting)';

  public addParticipant = false;

  public m: MetaDomain;

  public party: Party;
  public person: Person;
  public organisation: Organisation;
  public purposes: CommunicationEventPurpose[];
  public contacts: Party[] = [];

  public communicationEvent: FaceToFaceCommunication;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    public navigation: NavigationService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute,
    private stateService: StateService,
  ) {
    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

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
                  Participants: x,
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

          this.communicationEvent = scope.session.create('FaceToFaceCommunication') as FaceToFaceCommunication;
          this.communicationEvent.AddParticipant(this.person);

        } else {
          this.communicationEvent = loaded.objects.CommunicationEvent as FaceToFaceCommunication;
        }
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public participantCancelled(): void {
    this.addParticipant = false;
  }

  public participantAdded(id: string): void {
    const { scope } = this.allors;

    this.addParticipant = false;

    const participant: Person = scope.session.get(id) as Person;
    this.communicationEvent.AddParticipant(participant);

    if (!!this.organisation) {
      const relationShip: OrganisationContactRelationship = scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
      relationShip.Contact = participant;
      relationShip.Organisation = this.organisation;
    }

  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public cancel(): void {
    const { scope } = this.allors;

    const cancelFn: () => void = () => {
      scope.invoke(this.communicationEvent.Cancel)
        .subscribe(() => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe(() => {
                scope.session.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public close(): void {
    const { scope } = this.allors;

    const cancelFn: () => void = () => {
      scope.invoke(this.communicationEvent.Close)
        .subscribe(() => {
          this.refresh();
          this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe(() => {
                scope.session.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public reopen(): void {
    const { scope } = this.allors;

    const cancelFn = () => {
      scope.invoke(this.communicationEvent.Reopen)
        .subscribe(() => {
          this.refresh();
          this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe(() => {
                scope.session.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public save(): void {
    const { scope } = this.allors;

    scope
      .save()
      .subscribe(() => {
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
