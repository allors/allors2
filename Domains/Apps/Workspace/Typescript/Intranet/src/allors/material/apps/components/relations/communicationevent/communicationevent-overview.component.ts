import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Invoked, Loaded, Scope, WorkspaceService, LayoutService } from '../../../../../angular';
import { CommunicationEvent, EmailCommunication, FaceToFaceCommunication, LetterCorrespondence, Party, PhoneCommunication, WorkTask } from '../../../../../domain';
import { Fetch, PullRequest, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './communicationevent-overview.component.html',
})
export class CommunicationEventOverviewComponent implements OnInit, OnDestroy {

  public title = 'Communication Event overview';
  public itemTitle: string;
  public m: MetaDomain;

  public communicationEventPrefetch: CommunicationEvent;
  public communicationEvent: CommunicationEvent;
  public email: EmailCommunication;
  public letter: LetterCorrespondence;
  public phoneCall: PhoneCommunication;
  public party: Party;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  get isEmail(): boolean {
    return this.communicationEventPrefetch.objectType.name === 'EmailCommunication';
  }

  get isMeeting(): boolean {
    return this.communicationEventPrefetch.objectType.name === 'FaceToFaceCommunication';
  }

  get isPhone(): boolean {
    return this.communicationEventPrefetch.objectType.name === 'PhoneCommunication';
  }

  get isLetter(): boolean {
    return this.communicationEventPrefetch.objectType.name === 'LetterCorrespondence';
  }

  constructor(
    public layout: LayoutService,
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const roleId: string = this.route.snapshot.paramMap.get('roleId');

        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            id: roleId,
            name: 'communicationEventPrefetch',
          }),
          new Fetch({
            id,
            name: 'party',
          }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches }))
          .switchMap((loaded) => {
            this.communicationEventPrefetch = loaded.objects.communicationEventPrefetch as CommunicationEvent;
            this.party = loaded.objects.party as Party;

            const fetchEmail: Fetch[] = [
              new Fetch({
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.EmailCommunication.Originator }),
                  new TreeNode({ roleType: m.EmailCommunication.Addressees }),
                  new TreeNode({ roleType: m.EmailCommunication.EmailTemplate }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                  new TreeNode({ roleType: m.CommunicationEvent.ContactMechanisms }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
                ],
                name: 'communicationEvent',
              }),
            ];

            const fetchLetter: Fetch[] = [
              new Fetch({
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.LetterCorrespondence.Originators }),
                  new TreeNode({ roleType: m.LetterCorrespondence.Receivers }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                  new TreeNode({ roleType: m.CommunicationEvent.ContactMechanisms }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({
                        nodes: [
                          new TreeNode({ roleType: m.PostalBoundary.Country }),
                        ],
                        roleType: m.PostalAddress.PostalBoundary,
                      }),
                    ],
                    roleType: m.LetterCorrespondence.PostalAddresses,
                  }),
                ],
                name: 'communicationEvent',
              }),
            ];

            const fetchMeeting: Fetch[] = [
              new Fetch({
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                  new TreeNode({ roleType: m.CommunicationEvent.ContactMechanisms }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
                ],
                name: 'communicationEvent',
              }),
            ];

            const fetchPhone: Fetch[] = [
              new Fetch({
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                  new TreeNode({ roleType: m.CommunicationEvent.ContactMechanisms }),
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
                ],
                name: 'communicationEvent',
              }),
            ];

            if (this.isEmail) {
              return this.scope.load('Pull', new PullRequest({ fetches: fetchEmail }));
            }

            if (this.isMeeting) {
              return this.scope.load('Pull', new PullRequest({ fetches: fetchMeeting }));
            }

            if (this.isLetter) {
              return this.scope.load('Pull', new PullRequest({ fetches: fetchLetter }));
            }

            if (this.isPhone) {
              return this.scope.load('Pull', new PullRequest({ fetches: fetchPhone }));
            }
          });
      })
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.communicationEvent = loaded.objects.communicationEvent as CommunicationEvent;

        if (this.isEmail) {
          this.email = this.communicationEvent as EmailCommunication;
          if (this.email.IncomingMail) {
            this.itemTitle = 'Incoming Email';
          } else {
            this.itemTitle = 'Outgoing Email';
          }
        }

        if (this.isLetter) {
          this.letter = this.communicationEvent as LetterCorrespondence;
          if (this.letter.IncomingLetter) {
            this.itemTitle = 'Incoming Letter';
          } else {
            this.itemTitle = 'Outgoing Letter';
          }
        }

        if (this.isPhone) {
          this.phoneCall = this.communicationEvent as PhoneCommunication;
          if (this.phoneCall.IncomingCall) {
            this.itemTitle = 'Incoming Phone call';
          } else {
            this.itemTitle = 'Outgoing Phone call';
          }
        }
      },
      (error: any) => {
        this.errorService.handle(error);
        this.goBack();
      },
    );
  }

  public deleteWorkEffort(worktask: WorkTask): void {
     this.dialogService
      .confirm({ message: 'Are you sure you want to delete this work task?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(worktask.Delete)
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

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
