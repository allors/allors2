import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, MediaService, PdfService, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { Good, ProductQuote, QuoteItem, RequestForQuote, SalesOrder } from '../../../../../domain';
import { PullRequest, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './productquote-overview.component.html',
})
export class ProductQuoteOverviewComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public title = 'Quote Overview';
  public request: RequestForQuote;
  public quote: ProductQuote;
  public goods: Good[] = [];
  public salesOrder: SalesOrder;

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    public mediaService: MediaService,
    public pdfService: PdfService,
    private dialogService: AllorsMaterialDialogService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('items saved', 'close', { duration: 1000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public ngOnInit(): void {

    const { m, pull } = this.dataService;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.Quote(
              {
                object: id,
                include: {
                  QuoteItems: {
                    Product: x,
                    QuoteItemState: x,
                  },
                  Receiver: x,
                  ContactPerson: x,
                  QuoteState: x,
                  CreatedBy: x,
                  LastModifiedBy: x,
                  Request: x,
                  FullfillContactMechanism: {
                    PostalAddress_PostalBoundary: {
                      Country: x,
                    }
                  }
                }
              }
            ),
            pull.Good({ sort: new Sort(m.Good.Name) })
          ];

          if (id != null) {
            pulls.push(
              pull.ProductQuote(
                {
                  object: id,
                  fetch: {
                    SalesOrderWhereQuote: x,
                  }
                }
              )
            );
          }

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.goods = loaded.collections.goods as Good[];
        this.quote = loaded.objects.quote as ProductQuote;
        this.salesOrder = loaded.objects.salesOrder as SalesOrder;
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public print() {
    this.pdfService.display(this.quote);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
  }

  public approve(): void {
    this.scope.invoke(this.quote.Approve)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public reject(): void {
    this.scope.invoke(this.quote.Reject)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public Order(): void {
    this.scope.invoke(this.quote.Order)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open('Order successfully created.', 'close', { duration: 5000 });
        this.gotoOrder();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public cancelQuoteItem(quoteItem: QuoteItem): void {
    this.scope.invoke(quoteItem.Cancel)
      .subscribe((invoked: Invoked) => {
        this.snackBar.open('Quote Item successfully cancelled.', 'close', { duration: 5000 });
        this.refresh();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public deleteQuoteItem(quoteItem: QuoteItem): void {
    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this item?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(quoteItem.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Quote Item successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public gotoOrder(): void {

    const { m, pull } = this.dataService;

    const pulls = [
      pull.ProductQuote({
        object: this.quote,
        fetch: {
          SalesOrderWhereQuote: x
        }
      })
    ];

    this.scope.load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        const order = loaded.objects.order as SalesOrder;
        this.router.navigate(['/orders/salesOrder/' + order.id]);
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        });
  }
}
