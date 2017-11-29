import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import 'rxjs/add/observable/combineLatest';

import { PostalAddress, MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask, WorkEffortPurpose, CommunicationEvent, Organisation, OrganisationContactRelationship, ContactMechanism, PersonRole, CustomerRelationship, Country, ProductCharacteristic, ProductQuote, RequestForQuote, Currency, Party, OrganisationRole } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked, Filter } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals, Contains } from "@allors/framework";

@Component({
  encapsulation: ViewEncapsulation.Native,
  styles: [`
.order-box {
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

  .order-box table {
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

.order-box table td {
  padding-top: 2px;
  vertical-align: top;
}

  .order-box table td.amount {
      text-align: right;
  }

tr.top table tr td.title {
  font-size: 24px;
  font-weight: bold;
  vertical-align: middle;
  text-align: left;
  color: #000;
}

.order-box table tr.top table tr td.number {
  font-size: 20px;
  font-weight: bold;
  vertical-align: middle;
  text-align: right;
}

.order-box table tr.header td {
  padding-top: 5px;
  padding-bottom: 5px;
  padding-right: 5px;
  font-weight: bold;
  vertical-align: middle;
  text-align: left;
}

.order-box table tr.item td {
  padding-top: 5px;
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

export class SalesOrderPrintComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public order: SalesOrder;
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
            name: "order",
          }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch }));
      })
      .subscribe((loaded: Loaded) => {
        this.order = loaded.objects.order as SalesOrder;
        const printContent = this.order.PrintContent;

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
