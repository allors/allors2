import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { CatScope, InternalOrganisation, Locale, ProductCategory, Singleton } from "../../../../../domain";
import { Equals, Fetch, PullRequest, Query, Sort, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";
import { Fetcher } from "../../Fetcher";

@Component({
  templateUrl: "./category.component.html",
})
export class CategoryComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public category: ProductCategory;
  public title: string;
  public subTitle: string;

  public locales: Locale[];
  public categories: ProductCategory[];
  public catScopes: CatScope[];
  public internalOrganisation: InternalOrganisation;

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  private fetcher: Fetcher;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          this.fetcher.locales,
          this.fetcher.internalOrganisation,
          new Fetch({
            id,
            include: [
              new TreeNode({
                nodes: [new TreeNode({ roleType: m.LocalisedText.Locale })],
                roleType: m.ProductCategory.LocalisedNames,
              }),
              new TreeNode({
                nodes: [new TreeNode({ roleType: m.LocalisedText.Locale })],
                roleType: m.ProductCategory.LocalisedDescriptions,
              }),
            ],
            name: "category",
          }),
        ];

        const queries: Query[] = [
          new Query(this.m.CatScope),
          new Query(
            {
              name: "categories",
              objectType: this.m.ProductCategory,
              sort: [new Sort({ roleType: m.ProductCategory.Name, direction: "Asc" })],
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {

        this.internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
        this.category = loaded.objects.category as ProductCategory;
        this.categories = loaded.collections.categories as ProductCategory[];
        this.catScopes = loaded.collections.CatScopes as CatScope[];
        this.locales = loaded.collections.locales as Locale[];

        if (!this.category) {
          this.category = this.scope.session.create("ProductCategory") as ProductCategory;
          this.category.InternalOrganisation = this.internalOrganisation;
        }

        this.title = "Category";
        this.subTitle = this.category.Name;
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

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
