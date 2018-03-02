import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy , OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";
import { Subscription } from "rxjs/Subscription";

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { ProductType, SerialisedInventoryItemCharacteristicType } from "../../../../../domain";
import { Fetch, PullRequest, Query, TreeNode, Sort } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./producttype.component.html",
})
export class ProductTypeComponent implements OnInit, OnDestroy {

  public title: string = "Edit Product Type";
  public subTitle: string;

  public m: MetaDomain;

  public productType: ProductType;

  public characteristics: SerialisedInventoryItemCharacteristicType[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetch: Fetch[] = [
          new Fetch({
            name: "productType",
            id,
            include: [
              new TreeNode({ roleType: m.ProductType.SerialisedInventoryItemCharacteristicTypes }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
            name: "characteristics",
            objectType: m.SerialisedInventoryItemCharacteristicType,
            sort: [new Sort({ roleType: m.SerialisedInventoryItemCharacteristicType.Name, direction: "Asc" })],
            }),
         ];

        return this.scope
          .load("Pull", new PullRequest({ fetch, query }));
      })
      .subscribe((loaded) => {

        this.productType = loaded.objects.productType as ProductType;
        if (!this.productType) {
          this.productType = this.scope.session.create("ProductType") as ProductType;
        }

        this.characteristics = loaded.collections.characteristics as SerialisedInventoryItemCharacteristicType[];
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
