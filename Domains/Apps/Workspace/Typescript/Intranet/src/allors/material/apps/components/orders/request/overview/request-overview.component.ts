import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, SessionService } from '../../../../../../angular';
import { InventoryItem, NonSerialisedInventoryItem, ProductQuote, RequestForQuote, RequestItem, SerialisedInventoryItem } from '../../../../../../domain';
import { Fetch, PullRequest, TreeNode } from '../../../../../../framework';
import { MetaDomain } from '../../../../../../meta';
import { StateService } from '../../../../services/state';
import { Fetcher } from '../../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';

@Component({
  templateUrl: './request-overview.component.html',
  providers: [SessionService]
})
export class RequestOverviewComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public title = 'Requests Overview';
  public request: RequestForQuote;
  public quote: ProductQuote;
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;

  private subscription: Subscription;

  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    @Self() private allors: SessionService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.m = this.allors.m;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.Request(
              {
                object: id,
                include: {
                  FullfillContactMechanism: {
                    PostalAddress_PostalBoundary: {
                      Country: x,
                    }
                  },
                  RequestItems: {
                    Product: x,
                    RequestItem: x,
                  },
                  Originator: x,
                  ContactPerson: x,
                  RequestState: x,
                  Currency: x,
                  CreatedBy: x,
                  LastModifiedBy: x,
                }
              }
            )
          ];

          if (id != null) {
            pulls.push(
              pull.RequestForQuote(
                {
                  object: id,
                  fetch: {
                    QuoteWhereRequest: x
                  }
                }
              )
            );
          }

          return this.allors
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();
        this.request = loaded.objects.request as RequestForQuote;
        this.quote = loaded.objects.quote as ProductQuote;
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
  }

  public createQuote(): void {

    this.allors.invoke(this.request.CreateQuote)
      .subscribe((invoked: Invoked) => {
        this.snackBar.open('Quote successfully created.', 'close', { duration: 5000 });
        this.gotoQuote();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public gotoQuote(): void {

    const { m, pull, x } = this.allors;

    const pulls = [
      pull.RequestForQuote(
        {
          object: this.request,
          fetch: {
            QuoteWhereRequest: x,
          }
        }
      )
    ];

    this.allors.load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        const quote = loaded.objects.quote as ProductQuote;
        this.router.navigate(['/orders/productQuote/' + quote.id]);
      }, this.errorService.handler);
  }

  public submit(): void {

    this.allors.invoke(this.request.Submit)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully submitted.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public cancel(): void {

    this.allors.invoke(this.request.Cancel)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public hold(): void {

    this.allors.invoke(this.request.Hold)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully held.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public reject(): void {

    this.allors.invoke(this.request.Reject)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public cancelRequestItem(requestItem: RequestItem): void {

    this.allors.invoke(requestItem.Cancel)
      .subscribe((invoked: Invoked) => {
        this.snackBar.open('Request Item successfully cancelled.', 'close', { duration: 5000 });
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public deleteRequestItem(requestItem: RequestItem): void {

    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this item?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.allors.invoke(requestItem.Delete)
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
}
