import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "../../../../../angular";
import { Equals, Fetch, Like, Page, Path, PullRequest, Query, Sort, TreeNode } from "../../../../../domain";
import { Product, Request, RequestItem } from "../../../../../domain";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./requestOverview.component.html",
})
export class RequestOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;
  public title: string = "Requests Overview";
  public request: Request;
  public requestItems: RequestItem[] = [];
  public products: Product[]= [];

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
            name: "request",
            id,
            include: [
              new TreeNode({
                roleType: m.Request.RequestItems,
                nodes: [
                  new TreeNode({ roleType: m.RequestItem.Product }),
                ],
              }),
              new TreeNode({ roleType: m.Request.Originator }),
              new TreeNode({ roleType: m.Request.CurrentObjectState }),
              new TreeNode({ roleType: m.Request.CreatedBy }),
              new TreeNode({ roleType: m.Request.LastModifiedBy }),
              new TreeNode({
                roleType: m.Request.RequestStatuses,
                nodes: [
                  new TreeNode({ roleType: m.RequestStatus.RequestObjectState }),
                ],
              }),
              new TreeNode({
                roleType: m.Request.FullfillContactMechanism,
                nodes: [
                  new TreeNode({
                    roleType: m.PostalAddress.PostalBoundary,
                    nodes: [
                      new TreeNode({ roleType: m.PostalBoundary.Country }),
                    ],
                  }),
                ],
              }),
            ],
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
        this.request = loaded.objects.request as Request;
        if (this.request) {
          this.requestItems = this.request.RequestItems;
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

  public checkType(obj: any): string {
    return obj.objectType.name;
  }

  public deleteRequestItem(requestItem: RequestItem): void {
    this.request.RemoveRequestItem(requestItem);
  }

  public addRequestItem(): void {
    const requestItem = this.scope.session.create("RequestItem") as RequestItem;
    requestItem.Quantity = 1;
    this.request.AddRequestItem(requestItem);
  }
}
