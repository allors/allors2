import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy , OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { TdMediaService } from "@covalent/core";

import { Subscription } from "rxjs/Subscription";

import { ErrorService, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { IUnitOfMeasure, Locale, SerialisedInventoryItemCharacteristicType, Singleton, TimeFrequency, UnitOfMeasure } from "../../../../../domain";
import { Fetch, PullRequest, Query, Sort, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./productcharacteristic.component.html",
})
export class ProductCharacteristicComponent implements OnInit, OnDestroy {

  public title: string = "Product Characteristic";
  public subTitle: string;

  public m: MetaDomain;

  public productCharacteristic: SerialisedInventoryItemCharacteristicType;

  public singleton: Singleton;
  public locales: Locale[];
  public uoms: UnitOfMeasure[];
  public timeFrequencies: TimeFrequency[];
  public allUoms: IUnitOfMeasure[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnInit(): void {
    this.subscription = this.route.url
      .switchMap((url: any) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            name: "productCharacteristic",
            id,
            include: [
              new TreeNode({  roleType: m.SerialisedInventoryItemCharacteristicType.LocalisedNames,
                              nodes: [
                                        new TreeNode({ roleType: m.LocalisedText.Locale}),
                                      ] }),
            ],
          }),
        ];

        const queries: Query[] = [
          new Query(
            {
              name: "singletons",
              objectType: this.m.Singleton,
              include: [
                new TreeNode({ roleType: m.Singleton.AdditionalLocales,
                   nodes: [
                      new TreeNode({ roleType: m.Locale.Language}),
                   ],
              }),
              ],
            }),
            new Query(
              {
                name: "uoms",
                objectType: this.m.UnitOfMeasure,
              }),
            new Query(
              {
                name: "timeFrequencies",
                objectType: this.m.TimeFrequency,
              }),
            ];

        return this.scope
          .load("Pull", new PullRequest({ fetches, queries }));
      })
      .subscribe((loaded) => {

        this.productCharacteristic = loaded.objects.productCharacteristic as SerialisedInventoryItemCharacteristicType;
        if (!this.productCharacteristic) {
          this.productCharacteristic = this.scope.session.create("SerialisedInventoryItemCharacteristicType") as SerialisedInventoryItemCharacteristicType;
        }

        this.singleton = loaded.collections.singletons[0] as Singleton;
        this.locales = this.singleton.AdditionalLocales;
        this.uoms = loaded.collections.uoms as UnitOfMeasure[];
        this.timeFrequencies = loaded.collections.timeFrequencies as TimeFrequency[];
        this.allUoms = this.uoms.concat(this.timeFrequencies).sort( (a, b) => (a.Name > b.Name) ? 1 : ((b.Name > a.Name) ? -1 : 0));
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
