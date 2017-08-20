import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "../../../../../angular";
import { Equals, Fetch, Like, Page, Path, PullRequest, Query, Sort, TreeNode } from "../../../../../domain";
import { Good, ProductQuote, QuoteItem } from "../../../../../domain";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./productQuoteOverview.component.html",
})
export class ProductQuoteOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Quote Overview";
  public quote: ProductQuote;
  public quoteItems: QuoteItem[] = [];
  public goods: Good[]= [];

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public dialogService: TdDialogService,
    private snackBar: MdSnackBar,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open("items saved", "close", { duration: 1000 });
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
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
                  new TreeNode({ roleType: m.QuoteItem.Product }),
                ],
                roleType: m.Quote.QuoteItems,
              }),
              new TreeNode({ roleType: m.Quote.Receiver }),
              new TreeNode({ roleType: m.Quote.CurrentObjectState }),
              new TreeNode({ roleType: m.Quote.CreatedBy }),
              new TreeNode({ roleType: m.Quote.LastModifiedBy }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.QuoteStatus.QuoteObjectState }),
                ],
                roleType: m.Quote.QuoteStatuses,
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
                roleType: m.Quote.FullfillContactMechanism,
              }),
            ],
            name: "quote",
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {
        this.goods = loaded.collections.goods as Good[];
        this.quote = loaded.objects.quote as ProductQuote;
        if (this.quote) {
          this.quoteItems = this.quote.QuoteItems;
        }
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

  public checkType(obj: any): string {
    return obj.objectType.name;
  }

  public deleteQuoteItem(quoteItem: QuoteItem): void {
    this.quote.RemoveQuoteItem(quoteItem);
  }

  public addQuoteItem(): void {
    const quoteItem = this.scope.session.create("QuoteItem") as QuoteItem;
    quoteItem.Quantity = 1;
    this.quote.AddQuoteItem(quoteItem);
  }
}
