import { Observable, BehaviorSubject, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService, TdDialogService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta';
import { PullRequest, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { CommunicationEvent, EmailCommunication, FaceToFaceCommunication, LetterCorrespondence, Party, PhoneCommunication, WorkTask } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Invoked } from '../../../../../angular';

@Component({
  templateUrl: './communicationEventOverview.component.html',
})
export class PartyCommunicationEventOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;
  m: MetaDomain;

  communicationEventPrefetch: CommunicationEvent;
  communicationEvent: CommunicationEvent;
  party: Party;

  title: string = 'Communication Event overview';

  get isEmail(): boolean {
    return this.communicationEventPrefetch instanceof (EmailCommunication);
  }

  get isMeeting(): boolean {
    return this.communicationEventPrefetch instanceof (FaceToFaceCommunication);
  }

  get isPhone(): boolean {
    return this.communicationEventPrefetch instanceof (PhoneCommunication);
  }

  get isLetter(): boolean {
    return this.communicationEvent instanceof (LetterCorrespondence);
  }

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: TdDialogService,
    private snackBar: MdSnackBar,

    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  ngOnInit(): void {

    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const roleId: string = this.route.snapshot.paramMap.get('roleId');

        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'communicationEventPrefetch',
            id: roleId,
          }),
          new Fetch({
            name: 'party',
            id: id,
          }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch }))
          .switchMap((loaded: Loaded) => {
            this.scope.session.reset();

            this.communicationEventPrefetch = loaded.objects.communicationEventPrefetch as CommunicationEvent;
            this.party = loaded.objects.party as Party;

            const fetchEmail: Fetch[] = [
              new Fetch({
                name: 'communicationEvent',
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.EmailCommunication.Originator }),
                  new TreeNode({ roleType: m.EmailCommunication.Addressees }),
                  new TreeNode({ roleType: m.EmailCommunication.EmailTemplate }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CurrentObjectState }),
                  new TreeNode({
                    roleType: m.CommunicationEvent.WorkEfforts,
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.CurrentObjectState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                  }),
                ],
              }),
            ];

            const fetchLetter: Fetch[] = [
              new Fetch({
                name: 'communicationEvent',
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.LetterCorrespondence.Originators }),
                  new TreeNode({ roleType: m.LetterCorrespondence.Receivers }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CurrentObjectState }),
                  new TreeNode({
                    roleType: m.CommunicationEvent.WorkEfforts,
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.CurrentObjectState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                  }),
                  new TreeNode({
                    roleType: m.LetterCorrespondence.PostalAddresses,
                    nodes: [
                      new TreeNode({
                        roleType: m.PostalAddress.PostalBoundary,
                        nodes: [
                          new TreeNode({ roleType: m.PostalBoundary.Country }),
                        ],
                      }),
                    ],
                  }),
                ],
              }),
            ];

            const fetchMeeting: Fetch[] = [
              new Fetch({
                name: 'communicationEvent',
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CurrentObjectState }),
                  new TreeNode({
                    roleType: m.CommunicationEvent.WorkEfforts,
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.CurrentObjectState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                  }),
                ],
              }),
            ];

            const fetchPhone: Fetch[] = [
              new Fetch({
                name: 'communicationEvent',
                id: roleId,
                include: [
                  new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                  new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                  new TreeNode({ roleType: m.CommunicationEvent.CurrentObjectState }),
                  new TreeNode({
                    roleType: m.CommunicationEvent.WorkEfforts,
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.CurrentObjectState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                  }),
                ],
              }),
            ];

            if (this.isEmail) {
              return this.scope.load('Pull', new PullRequest({ fetch: fetchEmail }));
            }

            if (this.isMeeting) {
              return this.scope.load('Pull', new PullRequest({ fetch: fetchMeeting }));
            }

            if (this.isLetter) {
              return this.scope.load('Pull', new PullRequest({ fetch: fetchLetter }));
            }

            if (this.isPhone) {
              return this.scope.load('Pull', new PullRequest({ fetch: fetchPhone }));
            }
          });
      })
      .subscribe((loaded: Loaded) => {
        this.communicationEvent = loaded.objects.communicationEvent as CommunicationEvent;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  deleteWorkEffort(worktask: WorkTask): void {
    this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this work task?' })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(worktask.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  refresh(): void {
    this.refresh$.next(new Date());
  }

  goBack(): void {
    window.history.back();
  }
}
