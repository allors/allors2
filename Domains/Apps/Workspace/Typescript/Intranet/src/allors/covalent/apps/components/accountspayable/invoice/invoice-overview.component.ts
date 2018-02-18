import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import { ErrorService, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { Good, PurchaseInvoice, PurchaseInvoiceItem, PurchaseOrder, SalesInvoice } from "../../../../../domain";
import { Fetch, Path, PullRequest, Query, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./invoice-overview.component.html",
})
export class InvoiceOverviewComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Purchase Invoice Overview";
  public order: PurchaseOrder;
  public invoice: PurchaseInvoice;
  public goods: Good[] = [];

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public dialogService: TdDialogService,
    private snackBar: MatSnackBar,
    public media: TdMediaService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef) {

    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
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
                  new TreeNode({ roleType: m.PurchaseInvoiceItem.Product }),
                  new TreeNode({ roleType: m.PurchaseInvoiceItem.InvoiceItemType }),
                ],
                roleType: m.PurchaseInvoice.PurchaseInvoiceItems,
              }),
              new TreeNode({ roleType: m.PurchaseInvoice.ContactPerson }),
              new TreeNode({ roleType: m.PurchaseInvoice.BilledFrom }),
              new TreeNode({ roleType: m.PurchaseInvoice.BillToCustomer }),
              new TreeNode({ roleType: m.PurchaseInvoice.PurchaseInvoiceState }),
              new TreeNode({ roleType: m.PurchaseInvoice.CreatedBy }),
              new TreeNode({ roleType: m.PurchaseInvoice.LastModifiedBy }),
              new TreeNode({ roleType: m.PurchaseInvoice.PurchaseOrder }),
            ],
            name: "invoice",
          }),
          new Fetch({
            id,
            name: "order",
            path: new Path({ step: m.PurchaseInvoice.PurchaseOrder }),
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "goods",
              objectType: m.Good,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.goods = loaded.collections.goods as Good[];
        this.order = loaded.objects.order as PurchaseOrder;
        this.invoice = loaded.objects.invoice as PurchaseInvoice;
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

  public deleteInvoiceItem(invoiceItem: PurchaseInvoiceItem): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this item?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(invoiceItem.Delete)
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

  public createInvoice(): void {
    this.scope.invoke(this.invoice.CreateSalesInvoice)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open("Sales Invoice successfully created.", "close", { duration: 5000 });
        this.gotoInvoice();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public gotoInvoice(): void {

      const fetch: Fetch[] = [new Fetch({
        id: this.invoice.id,
        name: "invoice",
        path: new Path({ step: this.m.PurchaseInvoice.SalesInvoiceWherePurchaseInvoice }),
      })];

      this.scope.load("Pull", new PullRequest({ fetch }))
        .subscribe((loaded) => {
          const invoice = loaded.objects.invoice as SalesInvoice;
          this.router.navigate(["/accountsreceivable/invoice/" + invoice.id]);
        },
        (error: any) => {
          this.errorService.message(error);
          this.goBack();
        });
    }
}
