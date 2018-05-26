import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';


import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Invoked, Loaded, MediaService, PdfService, Saved, Scope, WorkspaceService } from '../../../../../angular';
import { Good, ProductQuote, QuoteItem, RequestForQuote, SalesOrder } from '../../../../../domain';
import { Fetch, Path, PullRequest, Query, TreeNode } from '../../../../../framework';
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

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.QuoteItem.Product }),
                  new TreeNode({ roleType: m.QuoteItem.QuoteItemState }),
                ],
                roleType: m.Quote.QuoteItems,
              }),
              new TreeNode({ roleType: m.Quote.Receiver }),
              new TreeNode({ roleType: m.Quote.ContactPerson }),
              new TreeNode({ roleType: m.Quote.QuoteState }),
              new TreeNode({ roleType: m.Quote.CreatedBy }),
              new TreeNode({ roleType: m.Quote.LastModifiedBy }),
              new TreeNode({ roleType: m.Quote.Request }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.Quote.FullfillContactMechanism,
              }),
            ],
            name: 'quote',
          }),
        ];

        const salesOrderFetch: Fetch = new Fetch({
          id,
          name: 'salesOrder',
          path: new Path({ step: m.ProductQuote.SalesOrderWhereQuote }),
        });

        if (id != null) {
          fetches.push(salesOrderFetch);
        }

        const queries: Query[] = [
          new Query(
            {
              name: 'goods',
              objectType: m.Good,
            }),
        ];

        return this.scope
          .load('Pull', new PullRequest({ fetches, queries }));
      })
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

    const fetches: Fetch[] = [new Fetch({
      id: this.quote.id,
      name: 'order',
      path: new Path({ step: this.m.ProductQuote.SalesOrderWhereQuote }),
    })];

    this.scope.load('Pull', new PullRequest({ fetches }))
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
