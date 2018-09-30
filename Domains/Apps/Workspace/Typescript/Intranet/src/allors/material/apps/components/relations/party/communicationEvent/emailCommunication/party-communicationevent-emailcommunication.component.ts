import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Scope, WorkspaceService, x, Allors } from '../../../../../../../angular';
import { CommunicationEventPurpose, ContactMechanism, EmailAddress, EmailCommunication, EmailTemplate, InternalOrganisation, Party, PartyContactMechanism, Person } from '../../../../../../../domain';
import { Fetch, PullRequest, TreeNode, Sort, Equals } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';
import { StateService } from '../../../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './party-communicationevent-emailcommunication.component.html',
  providers: [Allors]
})
export class PartyCommunicationEventEmailCommunicationComponent implements OnInit, OnDestroy {

  public title = 'Email Communication';
  public subTitle: string;

  public addOriginator = false;
  public addAddressee = false;

  public m: MetaDomain;

  public communicationEvent: EmailCommunication;
  public employees: Person[];
  public party: Party;
  public purposes: CommunicationEventPurpose[];
  public ownEmailAddresses: EmailAddress[] = [];
  public allEmailAddresses: EmailAddress[];
  public emailTemplate: EmailTemplate;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([, , internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const roleId: string = this.route.snapshot.paramMap.get('roleId');

          const pulls = [
            pull.Party({ object: id, include: { GeneralEmail: x } }),
            pull.CommunicationEvent({
              object: roleId,
              include: {
                EmailCommunication_Originator: x,
                EmailCommunication_Addressees: x,
                EmailCommunication_EmailTemplate: x,
                EventPurposes: x,
                CommunicationEventState: x
              }
            }),
            pull.InternalOrganisation({
              object: id,
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
              sort: new Sort(m.CommunicationEventPurpose.Name),
            }),
            pull.EmailAddress({
              sort: new Sort(m.EmailAddress.ElectronicAddressString)
            }),
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        scope.session.reset();

        this.party = loaded.objects.party as Party;
        const internalOrganisation: InternalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
        this.employees = internalOrganisation.ActiveEmployees;
        this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];
        this.allEmailAddresses = loaded.collections.emailAddresses as EmailAddress[];
        this.communicationEvent = loaded.objects.communicationEvent as EmailCommunication;

        if (!this.communicationEvent) {
          this.communicationEvent = scope.session.create('EmailCommunication') as EmailCommunication;
          this.emailTemplate = scope.session.create('EmailTemplate') as EmailTemplate;
          this.communicationEvent.EmailTemplate = this.emailTemplate;
          this.communicationEvent.Originator = this.party.GeneralEmail;
          this.communicationEvent.IncomingMail = false;
        }

        for (const employee of this.employees) {
          const employeeContactMechanisms: ContactMechanism[] = employee.CurrentPartyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
          for (const contactMechanism of employeeContactMechanisms) {
            if (contactMechanism.objectType.name === 'EmailAddress') {
              this.ownEmailAddresses.push(contactMechanism as EmailAddress);
            }
          }
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

    const cancelFn: () => void = () => {
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
