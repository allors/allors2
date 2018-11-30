import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, SessionService, NavigationActivatedRoute, NavigationService } from '../../../../../angular';
import { PullRequest } from '../../../../../../allors/framework';
import { LetterCorrespondence, Party, WorkTask } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { Title } from '@angular/platform-browser';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './lettercorrespondence-overview.component.html',
  providers: [SessionService]
})
export class LetterCorrespondenceComponent implements OnInit, OnDestroy {

  public title = 'Communication Event overview';
  public itemTitle: string;
  public m: MetaDomain;

  public letterCorrespondence: LetterCorrespondence;
  public party: Party;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;

  constructor(
    @Self() private allors: SessionService,
    public navigationService: NavigationService,
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

    const { m, pull, x } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const navRoute = new NavigationActivatedRoute(this.route);
          const id = navRoute.id();
          const partyId = navRoute.queryParam(m.Party);

          const pulls = [
            pull.Party({ object: partyId }),
            pull.LetterCorrespondence({
              object: id,
              include: {
                Originators: x,
                Receivers: x,
                EventPurposes: x,
                CommunicationEventState: x,
                ContactMechanisms: x,
                WorkEfforts: {
                  WorkEffortState: x,
                  Priority: x,
                },
                PostalAddresses: {
                  PostalBoundary: {
                    Country: x,
                  }
                }
              }
            })
          ];

          return this.allors
          .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();

        this.party = loaded.objects.Party as Party;
        this.letterCorrespondence = loaded.objects.LetterCorrespondence as LetterCorrespondence;

      }, this.errorService.handler);
  }

  public deleteWorkEffort(worktask: WorkTask): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this work task?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(worktask.Delete)
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
