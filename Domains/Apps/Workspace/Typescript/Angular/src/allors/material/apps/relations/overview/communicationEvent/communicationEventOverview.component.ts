import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "../../../../../angular";
import { Equals, Fetch, Like, Page, Path, PullRequest, Query, Sort, TreeNode } from "../../../../../domain";
import { CommunicationEvent, EmailCommunication, FaceToFaceCommunication, LetterCorrespondence, Party, PhoneCommunication, WorkTask } from "../../../../../domain";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./communicationEventOverview.component.html",
})
export class PartyCommunicationEventOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public title: string = "Communication Event overview";
  public m: MetaDomain;

  public communicationEventPrefetch: CommunicationEvent;
  public communicationEvent: CommunicationEvent;
  public  party: Party;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

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
    private snackBar: MatSnackBar,

    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const roleId: string = this.route.snapshot.paramMap.get("roleId");

        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id: roleId,
            name: "communicationEventPrefetch",
          }),
          new Fetch({
            name: "party",
            id,
          }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch }))
          .switchMap((loaded: Loaded) => {
            this.scope.session.reset();

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
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
                ],
                name: "communicationEvent",
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
                name: "communicationEvent",
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
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
                ],
                name: "communicationEvent",
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
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                      new TreeNode({ roleType: m.WorkEffort.Priority }),
                    ],
                    roleType: m.CommunicationEvent.WorkEfforts,
                  }),
                ],
                name: "communicationEvent",
              }),
            ];

            if (this.isEmail) {
              return this.scope.load("Pull", new PullRequest({ fetch: fetchEmail }));
            }

            if (this.isMeeting) {
              return this.scope.load("Pull", new PullRequest({ fetch: fetchMeeting }));
            }

            if (this.isLetter) {
              return this.scope.load("Pull", new PullRequest({ fetch: fetchLetter }));
            }

            if (this.isPhone) {
              return this.scope.load("Pull", new PullRequest({ fetch: fetchPhone }));
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

  public deleteWorkEffort(worktask: WorkTask): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this work task?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(worktask.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
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
