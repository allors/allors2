import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { Router } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Invoked, Loaded, Scope } from "@allors";
import { Fetch, Path, PullRequest, Query, TreeNode } from "@allors";
import { Product, ProductQuote, RequestForQuote } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  templateUrl: "./request-overview.component.html",
})
export class RequestOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Requests Overview";
  public request: RequestForQuote;
  public quote: ProductQuote;
  public products: Product[] = [];

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private router: Router,
    public dialogService: TdDialogService,
    private snackBar: MatSnackBar,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public ngOnInit(): void {

    this.subscription = this.route.url
      .switchMap((url: any) => {

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

        const query: Query[] = [
          new Query(
            {
              name: "products",
              objectType: m.Product,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {
        this.products = loaded.collections.products as Product[];
        this.request = loaded.objects.request as RequestForQuote;
        this.quote = loaded.objects.quote as ProductQuote;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
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

  public gotoQuote(): void {

    const fetch: Fetch[] = [new Fetch({
      id: this.request.id,
      name: "quote",
      path: new Path({ step: this.m.RequestForQuote.QuoteWhereRequest }),
    })];

    this.scope.load("Pull", new PullRequest({ fetch }))
      .subscribe((loaded: Loaded) => {
        const quote = loaded.objects.quote as ProductQuote;
        this.router.navigate(["/orders/productQuote/" + quote.id]);
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }
}
