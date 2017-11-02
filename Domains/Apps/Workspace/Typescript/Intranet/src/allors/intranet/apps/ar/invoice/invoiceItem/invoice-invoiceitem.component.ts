import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Filter, Invoked, Loaded, Saved, Scope } from "@allors";
import { Contains, Equals, Fetch, Like, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "@allors";
import { Good, SalesInvoice, SalesInvoiceItem, SalesInvoiceItemType, SalesOrderItem } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  templateUrl: "./invoice-invoiceitem.component.html",
})
export class InvoiceInvoiceItemComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Edit Sales Invoice Item";
  public subTitle: string;
  public invoice: SalesInvoice;
  public invoiceItem: SalesInvoiceItem;
  public orderItem: SalesOrderItem;
  public goods: Good[];
  public salesInvoiceItemTypes: SalesInvoiceItemType[];
  public productItemType: SalesInvoiceItemType;

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

    this.scope = new Scope(allorsService.database, allorsService.workspace);
    this.m = this.allorsService.meta;
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
            name: "salesInvoice",
          }),
          new Fetch({
            id: itemId,
            include: [
              new TreeNode({ roleType: m.SalesInvoiceItem.SalesInvoiceItemState }),
              new TreeNode({ roleType: m.SalesInvoiceItem.SalesOrderItem }),
            ],
            name: "invoiceItem",
          }),
        ];

        const rolesQuery: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
            }),
          new Query(
            {
              name: "salesInvoiceItemTypes",
              objectType: m.SalesInvoiceItemType,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch, query: rolesQuery }));
      })
      .subscribe((loaded: Loaded) => {

        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.invoiceItem = loaded.objects.invoiceItem as SalesInvoiceItem;
        this.orderItem = loaded.objects.orderItem as SalesOrderItem;
        this.goods = loaded.collections.goods as Good[];
        this.salesInvoiceItemTypes = loaded.collections.salesInvoiceItemTypes as SalesInvoiceItemType[];

        if (!this.invoiceItem) {
          this.title = "Add invoice Item";
          this.invoiceItem = this.scope.session.create("SalesInvoiceItem") as SalesInvoiceItem;
          this.productItemType = this.salesInvoiceItemTypes.find((v: SalesInvoiceItemType) => v.UniqueId.toUpperCase() === "0D07F778-2735-44CB-8354-FB887ADA42AD");
          this.invoice.AddSalesInvoiceItem(this.invoiceItem);
        }
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

  public save(): void {

    if (this.invoiceItem.Product) {
      this.invoiceItem.SalesInvoiceItemType = this.productItemType;
    }

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(["/ar/invoice/" + this.invoice.id]);
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
