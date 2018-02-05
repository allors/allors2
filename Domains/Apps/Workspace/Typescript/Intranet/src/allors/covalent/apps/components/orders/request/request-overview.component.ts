import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { Router } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Invoked, Loaded, Scope, WorkspaceService } from "../../../../../angular";
import { InventoryItem, NonSerialisedInventoryItem, ProductQuote, RequestForQuote, RequestItem, SerialisedInventoryItem } from "../../../../../domain";
import { Fetch, Path, PullRequest, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./request-overview.component.html",
})
export class RequestOverviewComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Requests Overview";
  public request: RequestForQuote;
  public quote: ProductQuote;
  public inventoryItems: InventoryItem[];
  public serialisedInventoryItem: SerialisedInventoryItem;
  public nonSerialisedInventoryItem: NonSerialisedInventoryItem;

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private router: Router,
    public dialogService: TdDialogService,
    private snackBar: MatSnackBar,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.RequestItem.Product }),
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
            name: "request",
          }),
        ];

        const quoteFetch: Fetch = new Fetch({
          id,
          name: "quote",
          path: new Path({ step: m.RequestForQuote.QuoteWhereRequest }),
        });

        if (id != null) {
          fetch.push(quoteFetch);
        }

        return this.scope
          .load("Pull", new PullRequest({ fetch }));
      })
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.request = loaded.objects.request as RequestForQuote;
        this.quote = loaded.objects.quote as ProductQuote;
      },
      (error: any) => {
        this.errorService.message(error);
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
        this.snackBar.open("Quote successfully created.", "close", { duration: 5000 });
        this.gotoQuote();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public deleteRequestItem(requestItem: RequestItem): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this item?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(requestItem.Delete)
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

  public gotoQuote(): void {

    const fetch: Fetch[] = [new Fetch({
      id: this.request.id,
      name: "quote",
      path: new Path({ step: this.m.RequestForQuote.QuoteWhereRequest }),
    })];

    this.scope.load("Pull", new PullRequest({ fetch }))
      .subscribe((loaded) => {
        const quote = loaded.objects.quote as ProductQuote;
        this.router.navigate(["/orders/productQuote/" + quote.id]);
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }
}
