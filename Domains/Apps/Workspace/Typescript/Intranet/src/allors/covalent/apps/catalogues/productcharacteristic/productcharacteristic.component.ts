import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy , OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { Subscription } from "rxjs/Subscription";

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from "../../../../angular";
import { Locale, ProductCharacteristic, Singleton, UnitOfMeasure } from "../../../../domain";
import { Fetch, PullRequest, Query, TreeNode } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

@Component({
  templateUrl: "./productcharacteristic.component.html",
})
export class ProductCharacteristicComponent implements OnInit, OnDestroy {

  public title: string = "Product Characteristic";
  public subTitle: string;

  public m: MetaDomain;

  public productCharacteristic: ProductCharacteristic;

  public singleton: Singleton;
  public locales: Locale[];
  public uoms: UnitOfMeasure[];

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
            name: "productCharacteristic",
            id,
            include: [
              new TreeNode({ roleType: m.ProductCharacteristic.LocalisedNames }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: "singletons",
              objectType: this.m.Singleton,
              include: [
                new TreeNode({ roleType: m.Singleton.Locales }),
              ],
            }),
            new Query(
              {
                name: "uoms",
                objectType: this.m.UnitOfMeasure,
              }),
          ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded: Loaded) => {

        this.productCharacteristic = loaded.objects.productCharacteristic as ProductCharacteristic;
        if (!this.productCharacteristic) {
          this.productCharacteristic = this.scope.session.create("ProductCharacteristic") as ProductCharacteristic;
        }

        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.uoms = loaded.collections.uoms as UnitOfMeasure[];
        this.locales = this.singleton.Locales;
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

  public goBack(): void {
    window.history.back();
  }
}
