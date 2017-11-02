import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Filter, Invoked, Loaded, Saved, Scope } from "@allors";
import { Contains, Equals, Fetch, Like, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "@allors";
import { Good, ProductQuote, QuoteItem, RequestItem, SerialisedInventoryItem, UnitOfMeasure } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  templateUrl: "./quoteitem.component.html",
})
export class QuoteItemEditComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Edit Quote Item";
  public subTitle: string;
  public quote: ProductQuote;
  public quoteItem: QuoteItem;
  public requestItem: RequestItem;
  public inventoryItems: SerialisedInventoryItem[];
  public inventoryItem: SerialisedInventoryItem;
  public goods: Good[];
  public unitsOfMeasure: UnitOfMeasure[];

  public goodsFilter: Filter;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {
    this.m = this.allorsService.meta;

    this.goodsFilter = new Filter({scope: this.scope, objectType: this.m.Product, roleTypes: [this.m.Product.Name]});

    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const itemId: string = this.route.snapshot.paramMap.get("itemId");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            name: "productQuote",
          }),
          new Fetch({
            id: itemId,
            include: [
              new TreeNode({ roleType: m.QuoteItem.QuoteItemState }),
              new TreeNode({ roleType: m.QuoteItem.RequestItem }),
            ],
            name: "quoteItem",
          }),
          new Fetch({
            id: itemId,
            name: "requestItem",
            path: new Path({ step: m.QuoteItem.RequestItem }),
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
            }),
            new Query(
              {
                name: "unitsOfMeasure",
                objectType: m.UnitOfMeasure,
              }),
          ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }))
          .switchMap((loaded: Loaded) => {
            this.quote = loaded.objects.productQuote as ProductQuote;
            this.quoteItem = loaded.objects.quoteItem as QuoteItem;
            this.requestItem = loaded.objects.requestItem as RequestItem;
            this.goods = loaded.collections.goods as Good[];
            this.unitsOfMeasure = loaded.collections.unitsOfMeasure as UnitOfMeasure[];

            if (!this.quoteItem) {
              this.title = "Add Quote Item";
              this.quoteItem = this.scope.session.create("QuoteItem") as QuoteItem;
              this.quote.AddQuoteItem(this.quoteItem);
            }

            const inventoryItemFetch: Fetch[] = [
              new Fetch({
                id: this.quoteItem.Product.id,
                name: "inventoryItem",
                path: new Path({ step: m.Good.InventoryItemsWhereGood }),
              }),
            ];

            return this.scope
              .load("Pull", new PullRequest({ fetch: inventoryItemFetch }));
          });
      })
      .subscribe((loaded: Loaded) => {
        this.inventoryItems = loaded.collections.inventoryItem as SerialisedInventoryItem[];
        this.inventoryItem = this.inventoryItems[0];
      },
      (error: Error) => {
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

  public submit(): void {
    const submitFn: () => void = () => {
      this.scope.invoke(this.quoteItem.Submit)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully submitted.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                submitFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            submitFn();
          }
        });
    } else {
      submitFn();
    }
  }

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.quoteItem.Cancel)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(["/orders/productQuote/" + this.quote.id]);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
