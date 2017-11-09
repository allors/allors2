import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Loaded, Saved, Scope } from "@allors";
import { Fetch, Path, PullRequest, Query, TreeNode } from "@allors";
import { SalesInvoice } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  encapsulation: ViewEncapsulation.Native,
  styles: [`
  .invoice-box {
    max-width: 800px;
    min-width: 800px;
    margin: auto;
    padding: 30px;
    font-size: 10px;
    font-weight: normal;
    line-height: 12px;
    font-family: "Helvetica Rounded", Arial, sans-serif;
    color: #555;
}

    .invoice-box table {
        width: 100%;
        line-height: inherit;
        text-align: left;
    }

tr.ruler-top-normal td {
    border-top: 1px solid #000;
}

tr.ruler-bottom-normal td {
    border-bottom: 1px solid #000;
}

tr.ruler-top td {
    border-top: 2px solid #000;
}

tr.ruler-bottom td {
    border-bottom: 2px solid #000;
}

.invoice-box table td {
    padding-top: 2px;
    vertical-align: top;
}

    .invoice-box table td.amount {
        text-align: right;
    }

tr.top table tr td.title {
    font-size: 24px;
    font-weight: bold;
    vertical-align: middle;
    text-align: left;
    color: #000;
}

.invoice-box table tr.top table tr td.number {
    font-size: 20px;
    font-weight: bold;
    vertical-align: middle;
    text-align: right;
}

.invoice-box table tr.header td {
    padding-top: 5px;
    padding-bottom: 5px;
    padding-right: 5px;
    font-weight: bold;
    vertical-align: middle;
    text-align: left;
}

.invoice-box table tr.item td {
    padding-top: 5px;
    padding-right: 5px;
}

.headerSpacer {
    height: 20px;
}

.logo {
    max-width: 150px;
    float: left;
}

.description {
    padding-top: 30px;
    padding-bottom: 15px;
    font-weight: bold;
}

.totals {
    float: right;
    width: 35%;
    position: relative;
    top: 100px;
}

.bold {
    font-weight: bold;
}

.footer {
    position: absolute;
    bottom: 10px;
}

    .footer p {
        margin: 3px;
        font-size: 8px;
        line-height: 10px;
        text-align: left;
        font-weight: normal;
    }

@media print {
  @page {
      size: auto; /* auto is the initial value */
      margin: 0; /* this affects the margin in the printer settings */
  }
}`,
  ],
  template: `<div [innerHTML]="body"></div>`,
})

export class InvoicePrintComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Sales Invoice Overview";
  public invoice: SalesInvoice;
  public body: string;

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

  public ngOnInit(): void {

    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            name: "invoice",
          }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch }));
      })
      .subscribe((loaded: Loaded) => {
        this.invoice = loaded.objects.invoice as SalesInvoice;
        const printContent = this.invoice.PrintContent;

        const wrapper = document.createElement("div");
        wrapper.innerHTML = printContent;
        this.body = wrapper.querySelector("#dataContainer").innerHTML;
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
}
