import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "../../../../../angular";
import { Equals, Fetch, Like, Page, Path, PullRequest, Query, Sort, TreeNode } from "../../../../../domain";
import { Product, RequestForQuote, RequestItem } from "../../../../../domain";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./requestOverview.component.html",
})
export class RequestOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Requests Overview";
  public request: RequestForQuote;
  public products: Product[] = [];

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public dialogService: TdDialogService,
    private snackBar: MdSnackBar,
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
            include: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.RequestItem.Product }),
                ],
                roleType: m.Request.RequestItems,
              }),
              new TreeNode({ roleType: m.Request.RequestItems }),
              new TreeNode({ roleType: m.Request.Originator }),
              new TreeNode({ roleType: m.Request.CurrentObjectState }),
              new TreeNode({ roleType: m.Request.CreatedBy }),
              new TreeNode({ roleType: m.Request.LastModifiedBy }),
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: m.RequestStatus.RequestObjectState }),
                ],
                roleType: m.Request.RequestStatuses,
              }),
              new TreeNode({
                nodes: [
                  new TreeNode({
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                    roleType: m.PostalAddress.PostalBoundary,
                  }),
                ],
                roleType: m.Request.FullfillContactMechanism,
              }),
            ],
            name: "request",
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "products",
              objectType: m.Product,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {
        this.products = loaded.collections.products as Product[];
        this.request = loaded.objects.request as RequestForQuote;
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

  public checkType(obj: any): string {
    return obj.objectType.name;
  }

  public createQuote(): void {

    this.scope.invoke(this.request.CreateQuote)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open("Quote successfully created.", "close", { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }
}
