import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { BehaviorSubject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Loaded, Saved, Scope } from "@allors";
import { Fetch, Path, PullRequest, Query, TreeNode } from "@allors";
import { ProductQuote } from "@allors";
import { MetaDomain } from "@allors";

@Component({
  encapsulation: ViewEncapsulation.Native,
  styleUrls: ["./productquote.css"],
  template: `<div [innerHTML]="body"></div>`,
})

export class ProductQuotePrintComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public quote: ProductQuote;
  public body: string;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;
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
