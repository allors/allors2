import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Scope, WorkspaceService, LayoutService } from '../../../../../../../angular';
import { CommunicationEventPurpose, ContactMechanism, EmailAddress, EmailCommunication, EmailTemplate, InternalOrganisation, Party, PartyContactMechanism, Person } from '../../../../../../../domain';
import { Fetch, PullRequest, Query, TreeNode } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';
import { StateService } from '../../../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../../../base/services/dialog';

@Component({
  templateUrl: './party-communicationevent-emailcommunication.component.html',
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
  private scope: Scope;

  constructor(
    public layout: LayoutService,
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([,, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const roleId: string = this.route.snapshot.paramMap.get('roleId');

        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Party.GeneralEmail }),
            ],
            name: 'party',
          }),
          new Fetch({
            id: roleId,
            include: [
              new TreeNode({ roleType: m.EmailCommunication.Originator }),
              new TreeNode({ roleType: m.EmailCommunication.Addressees }),
              new TreeNode({ roleType: m.EmailCommunication.EmailTemplate }),
              new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
              new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
            ],
            name: 'communicationEvent',
          }),
          new Fetch({
            id: internalOrganisationId,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                    ],
                    roleType: m.Party.CurrentPartyContactMechanisms,
                  }),
                ],
                roleType: m.InternalOrganisation.ActiveEmployees,
              }),
            ],
            name: 'internalOrganisation',
          }),
        ];

        const queries: Query[] = [
          new Query(
            {
              name: 'purposes',
              objectType: this.m.CommunicationEventPurpose,
            }),
          new Query(
            {
              name: 'emailAddresses',
              objectType: this.m.EmailAddress,
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {

        this.scope.session.reset();

        this.party = loaded.objects.party as Party;
        const internalOrganisation: InternalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
        this.employees = internalOrganisation.ActiveEmployees;
        this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];
        this.allEmailAddresses = loaded.collections.emailAddresses as EmailAddress[];
        this.communicationEvent = loaded.objects.communicationEvent as EmailCommunication;

        if (!this.communicationEvent) {
          this.communicationEvent = this.scope.session.create('EmailCommunication') as EmailCommunication;
          this.emailTemplate = this.scope.session.create('EmailTemplate') as EmailTemplate;
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
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Cancel)
        .subscribe(() => {
            this.refresh();
            this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
          },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
        this.dialogService
         .confirm({ message: 'Save changes?' })
         .subscribe((confirm: boolean) => {
           if (confirm) {
             this.scope
               .save()
               .subscribe(() => {
                   this.scope.session.reset();
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
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Close)
        .subscribe(() => {
            this.refresh();
            this.snackBar.open('Successfully closed.', 'close', { duration: 5000 });
          },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
        this.dialogService
         .confirm({ message: 'Save changes?' })
         .subscribe((confirm: boolean) => {
           if (confirm) {
             this.scope
               .save()
               .subscribe(() => {
                   this.scope.session.reset();
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
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Reopen)
        .subscribe(() => {
            this.refresh();
            this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
          },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe(() => {
                  this.scope.session.reset();
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

    this.scope
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
