import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription } from 'rxjs';

import { ErrorService, Invoked, Loaded, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../../../angular';
import { CommunicationEventPurpose, ContactMechanism, InternalOrganisation, LetterCorrespondence, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, PostalAddress, Singleton } from '../../../../../../../domain';
import { Fetch, PullRequest, TreeNode, Sort, Equals } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';
import { StateService } from '../../../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './party-communicationevent-lettercorrespondence.component.html'
})
export class PartyCommunicationEventLetterCorrespondenceComponent
  implements OnInit, OnDestroy {
  public scope: Scope;
  public title = 'Letter Correspondence';
  public subTitle: string;

  public addSender = false;
  public addReceiver = false;
  public addAddress = false;

  public m: MetaDomain;

  public communicationEvent: LetterCorrespondence;
  public employees: Person[];
  public contacts: Party[] = [];
  public party: Party;
  public purposes: CommunicationEventPurpose[];
  public postalAddresses: ContactMechanism[] = [];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService
  ) {
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  get PartyIsOrganisation(): boolean {
    return this.party.objectType.name === 'Organisation';
  }

  public ngOnInit(): void {

    const { m, pull } = this.dataService;

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {
          const id: string = this.route.snapshot.paramMap.get('id');
          const roleId: string = this.route.snapshot.paramMap.get('roleId');

          const pulls = [
            pull.Party({
              object: id,
              include: {
                CurrentContacts: x,
                CurrentPartyContactMechanisms: {
                  ContactMechanism: {
                    PostalAddress_PostalBoundary: {
                      Country: x,
                    }
                  }
                }
              }
            }),
            pull.CommunicationEvent({
              object: id,
              include: {
                LetterCorrespondence_Originators: x,
                LetterCorrespondence_Receivers: x,
                EventPurposes: x,
                CommunicationEventState: x,
                LetterCorrespondence_PostalAddresses: {
                  PostalBoundary: {
                    Country: x,
                  }
                }
              }
            }),
            pull.InternalOrganisation(
              {
                object: internalOrganisationId,
                include: {
                  ActiveEmployees: {
                    CurrentPartyContactMechanisms: {
                      ContactMechanism: {
                        PostalAddress_PostalBoundary: {
                          Country: x,
                        }
                      }
                    }
                  }
                }
              }
            ),
            pull.CommunicationEventPurpose({
              predicate: new Equals({ propertyType: m.CommunicationEventPurpose.IsActive, value: true }),
              sort: new Sort(m.CommunicationEventPurpose.Name),
            })
          ];

          return this.scope.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe(
        loaded => {
          this.scope.session.reset();

          this.party = loaded.objects.party as Party;
          const internalOrganisation: InternalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
          this.employees = internalOrganisation.ActiveEmployees;
          this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];
          this.communicationEvent = loaded.objects.communicationEvent as LetterCorrespondence;

          if (!this.communicationEvent) {
            this.communicationEvent = this.scope.session.create('LetterCorrespondence') as LetterCorrespondence;
            this.communicationEvent.IncomingLetter = true;
          }

          for (const employee of this.employees) {
            const employeeContactMechanisms: ContactMechanism[] = employee.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
            for (const contactMechanism of employeeContactMechanisms) {
              if (contactMechanism.objectType.name === 'PostalAddress') {
                this.postalAddresses.push(contactMechanism);
              }
            }
          }

          const contactMechanisms: ContactMechanism[] = this.party.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
          for (const contactMechanism of contactMechanisms) {
            if (contactMechanism.objectType.name === 'PostalAddress') {
              this.postalAddresses.push(contactMechanism);
            }
          }

          this.contacts.push(this.party);
          if (this.employees.length > 0) {
            this.contacts = this.contacts.concat(this.employees);
          }

          if (this.party.CurrentContacts.length > 0) {
            this.contacts = this.contacts.concat(this.party.CurrentContacts);
          }
        },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        }
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public senderCancelled(): void {
    this.addSender = false;
  }

  public receiverCancelled(): void {
    this.addReceiver = false;
  }

  public addressCancelled(): void {
    this.addAddress = false;
  }

  public senderAdded(id: string): void {
    this.addSender = false;

    const sender: Person = this.scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = sender;
    relationShip.Organisation = this.party as Organisation;

    this.communicationEvent.AddOriginator(sender);
  }

  public receiverAdded(id: string): void {
    this.addReceiver = false;

    const receiver: Person = this.scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = receiver;
    relationShip.Organisation = this.party as Organisation;

    this.communicationEvent.AddReceiver(receiver);
  }

  public addressAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addAddress = false;

    this.party.AddPartyContactMechanism(partyContactMechanism);

    const postalAddress = partyContactMechanism.ContactMechanism as PostalAddress;
    this.postalAddresses.push(postalAddress);
    this.communicationEvent.AddPostalAddress(postalAddress);
  }

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Cancel).subscribe(
        (invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', {
            duration: 5000
          });
        },
        (error: Error) => {
          this.errorService.handle(error);
        }
      );
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope.save().subscribe(
              (saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.handle(error);
              }
            );
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public close(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Close).subscribe(
        (invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully closed.', 'close', {
            duration: 5000
          });
        },
        (error: Error) => {
          this.errorService.handle(error);
        }
      );
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope.save().subscribe(
              (saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.handle(error);
              }
            );
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public reopen(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Reopen).subscribe(
        (invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully reopened.', 'close', {
            duration: 5000
          });
        },
        (error: Error) => {
          this.errorService.handle(error);
        }
      );
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope.save().subscribe(
              (saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.handle(error);
              }
            );
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public save(): void {
    this.scope.save().subscribe(
      (saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.handle(error);
      }
    );
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
