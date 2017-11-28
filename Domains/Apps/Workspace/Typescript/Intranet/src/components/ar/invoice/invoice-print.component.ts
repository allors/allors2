import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { BehaviorSubject, Subscription } from "rxjs/Rx";

import { MetaDomain, SalesInvoice } from "@allors/workspace";
import { Scope, WorkspaceService, ErrorService, Loaded } from "@allors/base-angular";
import { Fetch, PullRequest } from "@allors/framework";

@Component({
  encapsulation: ViewEncapsulation.Native,
  styleUrls: ["./salesinvoice.css"],
  template: `<div [innerHTML]="body"></div>`,
})

export class InvoicePrintComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public invoice: SalesInvoice;
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
            name: "invoice",
          }),
        ];

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
