import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { Catalogue, CatScope, Locale, ProductCategory, Singleton } from "../../../../../domain";
import { Fetch, PullRequest, Query, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./catalogue.component.html",
})
export class CatalogueComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public catalogue: Catalogue;
  public title: string;
  public subTitle: string;

  public singleton: Singleton;
  public locales: Locale[];
  public categories: ProductCategory[];
  public catScopes: CatScope[];

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.Catalogue.CatalogueImage}),
              new TreeNode({
                nodes: [new TreeNode({ roleType: m.LocalisedText.Locale })],
                roleType: m.Catalogue.LocalisedNames,
               }),
              new TreeNode({
                nodes: [new TreeNode({ roleType: m.LocalisedText.Locale })],
                roleType: m.Catalogue.LocalisedDescriptions,
               }),
            ],
            name: "catalogue",
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              include: [
                new TreeNode({ roleType: m.Singleton.AdditionalLocales,
                   nodes: [
                      new TreeNode({ roleType: m.Locale.Language}),
                   ],
              }),
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
      .subscribe((loaded) => {

        this.catalogue = loaded.objects.catalogue as Catalogue;
        if (!this.catalogue) {
          this.catalogue = this.scope.session.create("Catalogue") as Catalogue;
        }

        this.title = this.catalogue.Name;

        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.categories = loaded.collections.categories as ProductCategory[];
        this.catScopes = loaded.collections.catScopes as CatScope[];
        this.locales = this.singleton.AdditionalLocales;
      },
      (error: Error) => {
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

  public imageSelected(id: string): void {
    this.update();
    this.snackBar.open("Catalogue succesfully saved.", "close", { duration: 5000 });
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

  public update(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.refresh();
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