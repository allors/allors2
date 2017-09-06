import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, ErrorService, Loaded, Saved, Scope } from "../../../../angular";
import { Equals, Fetch, Like, Page, Path, PullRequest, PushResponse, Query, Sort, TreeNode } from "../../../../domain";
import { CatScope, Locale, ProductCategory, Singleton } from "../../../../domain";
import { MetaDomain } from "../../../../meta/index";

@Component({
  templateUrl: "./edit.component.html",
})
export class CategoryEditComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;

  public category: ProductCategory;
  public title: string;
  public subTitle: string;

  public singleton: Singleton;
  public locales: Locale[];
  public categories: ProductCategory[];
  public catScopes: CatScope[];

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

        const query: Query[] = [
          new Query(
            {
              include: [
                new TreeNode({ roleType: m.Singleton.Locales }),
              ],
              name: "singletons",
              objectType: this.m.Singleton,
            }),
          new Query(
            {
              name: "categories",
              objectType: this.m.ProductCategory,
            }),
          new Query(
            {
              name: "catScopes",
              objectType: this.m.CatScope,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.category = loaded.objects.category as ProductCategory;
        if (!this.category) {
          this.category = this.scope.session.create("ProductCategory") as ProductCategory;
        }

        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.categories = loaded.collections.categories as ProductCategory[];
        this.catScopes = loaded.collections.catScopes as CatScope[];
        this.locales = this.singleton.Locales;
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

  public goBack(): void {
    window.history.back();
  }
}
