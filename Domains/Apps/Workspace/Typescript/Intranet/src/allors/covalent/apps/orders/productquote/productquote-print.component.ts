import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../angular";
import { Brand, Catalogue, CatScope, ContactMechanism, Currency, Facility, Good, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, ProductCategory, ProductCharacteristic, ProductCharacteristicValue, ProductFeature, ProductType, SalesInvoice, SalesInvoiceItem, SalesOrder, Singleton, VarianceReason, VatRate, VatRegime, RequestForQuote, ProductQuote, QuoteItem } from "../../../../domain";
import { And, ContainedIn, Contains, Fetch, Like, Page, Path, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

@Component({
  encapsulation: ViewEncapsulation.Native,
  styles: [`
  .quote-box {
    max-width: 800px;
    min-width: 800px;
    margin: auto;
    padding: 50px 30px 30px 30px;
    font-size: 13px;
    font-weight: normal;
    line-height: 13px;
    font-family:"Arial Rounded MT Bold", "Helvetica Rounded", Arial, sans-serif;
    color: #555;
  }

    .quote-box table {
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

  .quote-box table td {
    padding-top: 2px;
    vertical-align: top;
  }

    .quote-box table td.amount {
        text-align: right;
    }

  tr.top table tr td.title {
    font-size: 24px;
    font-weight: bold;
    vertical-align: middle;
    text-align: left;
    color: #000;
  }

  .quote-box table tr.top table tr td.number {
    font-size: 20px;
    font-weight: bold;
    vertical-align: middle;
    text-align: right;
  }

  .quote-box table tr.header td {
    padding-top: 5px;
    padding-bottom: 5px;
    padding-right: 5px;
    font-weight: bold;
    vertical-align: middle;
    text-align: left;
  }

  .quote-box table tr.item td {
    padding-top: 5px;
    padding-right: 5px;
  }

  .quote-box table tr.comment td {
    padding-top: 30px;
    padding-right: 5px;
  }

  .headerSpacer {
    height: 20px;
  }

  .logo {
    max-width: 200px;
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
  }
`],
  template: `<div [innerHTML]="body"></div>`,
})

export class ProductQuotePrintComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public quote: ProductQuote;
  public body: string;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope()
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {

    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            name: "quote",
          }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch }));
      })
      .subscribe((loaded: Loaded) => {
        this.quote = loaded.objects.quote as ProductQuote;
        const printContent = this.quote.PrintContent;

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