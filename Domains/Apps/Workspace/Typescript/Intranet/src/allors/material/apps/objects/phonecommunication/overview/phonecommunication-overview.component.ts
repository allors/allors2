import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, ContextService, NavigationActivatedRoute, NavigationService, MetaService } from '../../../../../angular';
import { PullRequest, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

import { EmailCommunication, Party, PhoneCommunication, WorkTask } from '../../../../../domain';

@Component({
  templateUrl: './phonecommunication-overview.component.html',
  providers: [ContextService]
})
export class PhoneCommunicationOverviewComponent implements OnInit, OnDestroy {

  public title = 'Communication Event overview';
  public m: MetaDomain;

  public phoneCommunication: PhoneCommunication;
  public party: Party;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    public metaService: MetaService,
    public navigationService: NavigationService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.m = this.metaService.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();
          const partyId = navRoute.queryParam(m.Party);

          const pulls = [
            pull.Party({ object: partyId }),
            pull.PhoneCommunication({
              object: id,
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

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.party = loaded.objects.Party as Party;
        this.phoneCommunication = loaded.objects.PhoneCommunication as PhoneCommunication;

      }, this.errorService.handler);
  }

  public deleteWorkEffort(worktask: WorkTask): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this work task?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.context.invoke(worktask.Delete)
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
