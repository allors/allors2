import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, UrlSegment } from '@angular/router';


import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Invoked, Loaded, Saved, Scope, WorkspaceService } from '../../../../../../../angular';
import { CommunicationEventPurpose, FaceToFaceCommunication, InternalOrganisation, Organisation, OrganisationContactRelationship, Party, Person, Singleton } from '../../../../../../../domain';
import { Fetch, Path, PullRequest, Query, TreeNode } from '../../../../../../../framework';
import { MetaDomain } from '../../../../../../../meta';
import { StateService } from '../../../../../services/StateService';
import { DialogService } from '../../../../../../base/services/dialog';

@Component({
  templateUrl: './party-communicationevent-facetofacecommunication.component.html',
})
export class PartyCommunicationEventFaceToFaceCommunicationComponent implements OnInit, OnDestroy {

  public title = 'Face to Face Communication (Meeting)';
  public subTitle: string;

  public addParticipant = false;

  public m: MetaDomain;

  public communicationEvent: FaceToFaceCommunication;
  public parties: Party[];
  public party: Party;
  public purposes: CommunicationEventPurpose[];
  public singleton: Singleton;
  public employees: Person[];
  public contacts: Party[] = [];

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private dialogService: DialogService,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute,
    
    private stateService: StateService,
  ) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  get PartyIsOrganisation(): boolean {
    return this.party.objectType.name === 'Organisation';
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const roleId: string = this.route.snapshot.paramMap.get('roleId');

        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Party.CurrentContacts }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.PartyContactMechanism.ContactMechanism }),
                ],
                roleType: m.Party.CurrentPartyContactMechanisms,
              }),
            ],
            name: 'party',
          }),
          new Fetch({
            id: roleId,
            include: [
              new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
              new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
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
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {

        this.scope.session.reset();

        const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
        this.employees = internalOrganisation.ActiveEmployees;
        this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];
        this.party = loaded.objects.party as Party;
        this.communicationEvent = loaded.objects.communicationEvent as FaceToFaceCommunication;

        if (!this.communicationEvent) {
          this.communicationEvent = this.scope.session.create('FaceToFaceCommunication') as FaceToFaceCommunication;
          if (this.party.objectType.name === 'Person') {
            this.communicationEvent.AddParticipant(this.party);
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
      },
    );
  }

  public participantCancelled(): void {
    this.addParticipant = false;
  }

  public participantAdded(id: string): void {
    this.addParticipant = false;

    const participant: Person = this.scope.session.get(id) as Person;
    const relationShip: OrganisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    relationShip.Contact = participant;
    relationShip.Organisation = this.party as Organisation;

    this.communicationEvent.AddParticipant(participant);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.communicationEvent.Cancel)
        .subscribe((invoked: Invoked) => {
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
              .subscribe((saved: Saved) => {
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
        .subscribe((invoked: Invoked) => {
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
              .subscribe((saved: Saved) => {
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
    const cancelFn = () => {
      this.scope.invoke(this.communicationEvent.Reopen)
        .subscribe((invoked: Invoked) => {
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
              .subscribe((saved: Saved) => {
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

  public save(form: any): void {

    this.scope
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
