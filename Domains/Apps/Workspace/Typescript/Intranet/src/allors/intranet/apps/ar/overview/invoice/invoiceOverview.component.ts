import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "@allors";
import { Equals, Fetch, Like, Page, Path, PullRequest, Query, Sort, TreeNode } from "@allors";
import { Good, SalesInvoice, SalesInvoiceItem, SalesOrder } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  templateUrl: "./invoiceOverview.component.html",
})
export class InvoiceOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Sales Invoice Overview";
  public order: SalesOrder;
  public invoice: SalesInvoice;
  public invoiceItems: SalesInvoiceItem[] = [];
  public goods: Good[] = [];

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public dialogService: TdDialogService,
    private snackBar: MatSnackBar,
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
                  new TreeNode({ roleType: m.SalesInvoiceItem.Product }),
                ],
                roleType: m.SalesInvoice.SalesInvoiceItems,
              }),
              new TreeNode({ roleType: m.SalesInvoice.BillToCustomer }),
              new TreeNode({ roleType: m.SalesInvoice.SalesInvoiceState }),
              new TreeNode({ roleType: m.SalesInvoice.CreatedBy }),
              new TreeNode({ roleType: m.SalesInvoice.LastModifiedBy }),
              new TreeNode({ roleType: m.SalesInvoice.SalesOrder }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.SalesInvoice.BillToContactMechanism,
              }),
            ],
            name: "invoice",
          }),
          new Fetch({
            id,
            name: "order",
            path: new Path({ step: m.SalesInvoice.SalesOrder }),
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
        this.order = loaded.objects.order as SalesOrder;
        this.invoice = loaded.objects.invoice as SalesInvoice;
        if (this.invoice) {
          this.invoiceItems = this.invoice.SalesInvoiceItems;
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

  public deleteInvoiceItem(invoiceItem: SalesInvoiceItem): void {
    this.invoice.RemoveSalesInvoiceItem(invoiceItem);
  }
}
