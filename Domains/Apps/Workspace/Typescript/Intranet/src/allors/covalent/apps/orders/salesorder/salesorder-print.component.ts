import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Loaded, Scope, WorkspaceService } from "../../../../angular";
import { SalesOrder } from "../../../../domain";
import { Fetch, PullRequest } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

@Component({
  encapsulation: ViewEncapsulation.Native,
  styleUrls: ["./salesorder-print.component.scss"],
  templateUrl: "./salesorder-print.component.html",
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
