import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, Loaded, Scope, WorkspaceService, x, Allors } from '../../../../../angular';
import { CommunicationEvent, EmailCommunication, FaceToFaceCommunication, LetterCorrespondence, Party, PhoneCommunication, WorkTask } from '../../../../../domain';
import { Fetch, PullRequest, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { Title } from '../../../../../../../node_modules/@angular/platform-browser';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './communicationevent-overview.component.html',
  providers: [Allors]
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
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const roleId: string = this.route.snapshot.paramMap.get('roleId');

          const pulls = [
            pull.CommunicationEvent({ object: id }),
            pull.Party({ object: id })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                this.communicationEventPrefetch = loaded.objects.communicationEventPrefetch as CommunicationEvent;
                this.party = loaded.objects.party as Party;

                const fetchEmail = [
                  pull.CommunicationEvent({
                    object: roleId,
                    include: {
                      EmailCommunication_Originator: x,
                      EmailCommunication_Addressees: x,
                      EmailCommunication_EmailTemplate: x,
                      EventPurposes: x,
                      CommunicationEventState: x,
                      ContactMechanisms: x,
                      WorkEfforts: {
                        WorkEffortState: x,
                        Priority: x,
                      }
                    }
                  })
                ];

                const fetchLetter = [
                  pull.CommunicationEvent({
                    object: roleId,
                    include: {
                      LetterCorrespondence_Originators: x,
                      LetterCorrespondence_Receivers: x,
                      EventPurposes: x,
                      CommunicationEventState: x,
                      ContactMechanisms: x,
                      WorkEfforts: {
                        WorkEffortState: x,
                        Priority: x,
                      },
                      LetterCorrespondence_PostalAddresses: {
                        PostalBoundary: {
                          Country: x,
                        }
                      }
                    }
                  })
                ];

                const fetchMeeting = [
                  pull.CommunicationEvent({
                    object: id,
                    include: {
                      FromParties: x,
                      ToParties: x,
                      EventPurposes: x,
                      CommunicationEventState: x,
                      ContactMechanisms: x,
                      WorkEfforts: {
                        WorkEffortState: x,
                        Priority: x,
                      }
                    }
                  })
                ];

                const fetchPhone = [
                  pull.CommunicationEvent({
                    object: roleId,
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
                ];

                if (this.isEmail) {
                  return scope.load('Pull', new PullRequest({ pulls: fetchEmail }));
                }

                if (this.isMeeting) {
                  return scope.load('Pull', new PullRequest({ pulls: fetchMeeting }));
                }

                if (this.isLetter) {
                  return scope.load('Pull', new PullRequest({ pulls: fetchLetter }));
                }

                if (this.isPhone) {
                  return scope.load('Pull', new PullRequest({ pulls: fetchPhone }));
                }
              })
            );
        })
      )
      .subscribe((loaded) => {
        scope.session.reset();
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
    const { scope } = this.allors;

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this work task?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          scope.invoke(worktask.Delete)
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
