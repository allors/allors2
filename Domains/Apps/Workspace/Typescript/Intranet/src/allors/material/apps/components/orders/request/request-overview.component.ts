import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { Router } from '@angular/router';


import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';

import { ErrorService, Invoked, Loaded, Saved, Scope, WorkspaceService } from '../../../../../angular';
import { InventoryItem, NonSerialisedInventoryItem, ProductQuote, RequestForQuote, RequestItem, SerialisedInventoryItem } from '../../../../../domain';
import { Equals, Fetch, Path, PullRequest, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';

@Component({
  templateUrl: './request-overview.component.html',
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
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;
  private fetcher: Fetcher;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private router: Router,
    
    private snackBar: MatSnackBar,
    
    private changeDetectorRef: ChangeDetectorRef,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get('id');
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.RequestItem.Product }),
                  new TreeNode({ roleType: m.RequestItem.RequestItemState }),
                ],
                roleType: m.Request.RequestItems,
              }),
              new TreeNode({ roleType: m.Request.RequestItems }),
              new TreeNode({ roleType: m.Request.Originator }),
              new TreeNode({ roleType: m.Request.ContactPerson }),
              new TreeNode({ roleType: m.Request.RequestState }),
              new TreeNode({ roleType: m.Request.Currency }),
              new TreeNode({ roleType: m.Request.CreatedBy }),
              new TreeNode({ roleType: m.Request.LastModifiedBy }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.Request.FullfillContactMechanism,
              }),
            ],
            name: 'request',
          }),
        ];

        const quoteFetch: Fetch = new Fetch({
          id,
          name: 'quote',
          path: new Path({ step: m.RequestForQuote.QuoteWhereRequest }),
        });

        if (id != null) {
          fetches.push(quoteFetch);
        }

        return this.scope
          .load('Pull', new PullRequest({ fetches }));
      })
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.request = loaded.objects.request as RequestForQuote;
        this.quote = loaded.objects.quote as ProductQuote;
      },
      (error: any) => {
        this.errorService.handle(error);
        this.goBack();
      },
    );
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

    this.scope.invoke(this.request.CreateQuote)
      .subscribe((invoked: Invoked) => {
        this.snackBar.open('Quote successfully created.', 'close', { duration: 5000 });
        this.gotoQuote();
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public gotoQuote(): void {

    const fetches: Fetch[] = [new Fetch({
      id: this.request.id,
      name: 'quote',
      path: new Path({ step: this.m.RequestForQuote.QuoteWhereRequest }),
    })];

    this.scope.load('Pull', new PullRequest({ fetches }))
      .subscribe((loaded) => {
        const quote = loaded.objects.quote as ProductQuote;
        this.router.navigate(['/orders/productQuote/' + quote.id]);
      },
      (error: any) => {
        this.errorService.handle(error);
        this.goBack();
      });
  }

  public submit(): void {
    this.scope.invoke(this.request.Submit)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully submitted.', 'close', { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public cancel(): void {
    this.scope.invoke(this.request.Cancel)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public hold(): void {
    this.scope.invoke(this.request.Hold)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully held.', 'close', { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public reject(): void {
    this.scope.invoke(this.request.Reject)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open('Successfully rejected.', 'close', { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public cancelRequestItem(requestItem: RequestItem): void {
    this.scope.invoke(requestItem.Cancel)
      .subscribe((invoked: Invoked) => {
        this.snackBar.open('Request Item successfully cancelled.', 'close', { duration: 5000 });
        this.refresh();
      },
      (error: Error) => {
        this.errorService.handle(error);
      });
  }

  public deleteRequestItem(requestItem: RequestItem): void {
    // TODO:
    /*  this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this item?' })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(requestItem.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.handle(error);
            });
        }
    }); */
  }
}
