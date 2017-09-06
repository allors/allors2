import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MdSnackBar, MdSnackBarConfig } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { TdDialogService, TdLoadingService, TdMediaService } from "@covalent/core";

import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "../../../../../angular";
import { And, ContainedIn, Contains, Equals, Like, Not, Or, Page, Predicate, PullRequest, Query, Sort, TreeNode } from "../../../../../domain";
import { Brand, Good, InventoryItem, InventoryItemKind, Model, Organisation, Ownership, ProductCategory, ProductType, SerialisedInventoryItemObjectState } from "../../../../../domain";
import { MetaDomain } from "../../../../../meta/index";

interface SearchData {
  name: string;
  articleNumber: string;
  productCategory: string;
  productType: string;
  brand: string;
  model: string;
  status: string;
  ownership: string;
  inventoryKind: string;
  supplier: string;
  manufacturer: string;
  owner: string;
  keyword: string;
}

@Component({
  templateUrl: "./goodsOverview.component.html",
})
export class GoodsOverviewComponent implements AfterViewInit, OnDestroy {

  public title: string = "Products";
  public total: number;
  public searchForm: FormGroup;
  public data: Good[];
  public filtered: Good[];

  public productCategories: ProductCategory[];
  public selectedProductCategory: ProductCategory;
  public productCategory: ProductCategory;

  public productTypes: ProductType[];
  public selectedProductType: ProductType;
  public productType: ProductType;

  public brands: Brand[];
  public selectedBrand: Brand;
  public brand: Brand;

  public models: Model[];
  public selectedModel: Model;
  public model: Model;

  public objectStates: SerialisedInventoryItemObjectState[];
  public selectedObjectState: Model;
  public objectState: Model;

  public ownerships: Ownership[];
  public selectedOwnership: Ownership;
  public ownership: Ownership;

  public inventoryItemKinds: InventoryItemKind[];
  public selectedInventoryItemKind: InventoryItemKind;
  public inventoryItemKind: InventoryItemKind;

  public suppliers: Organisation[];
  public selectedSupplier: Organisation;
  public supplier: Organisation;

  public manufacturers: Organisation[];
  public selectedManufacturer: Organisation;
  public manufacturer: Organisation;

  private refresh$: BehaviorSubject<Date>;
  private page$: BehaviorSubject<number>;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MdSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.titleService.setTitle("Products");

    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      articleNumber: [""],
      brand: [""],
      inventoryKind: [""],
      keyword: [""],
      manufacturer: [""],
      model: [""],
      name: [""],
      owner: [""],
      ownerStatus: [""],
      productCategory: [""],
      productType: [""],
      supplier: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable.combineLatest(search$, this.page$)
      .scan(([previousData, previousTake]: [SearchData, number], [data, take]: [SearchData, number]): [SearchData, number] => {
        return [
          data,
          data !== previousData ? 50 : take,
        ];
      }, [] as [SearchData, number]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.allors.meta;

        const goodsPredicate: And = new And();
        const goodsPredicates: Predicate[] = goodsPredicate.predicates;

        if (data.name) {
          const like: string = data.name.replace("*", "%") + "%";
          goodsPredicates.push(new Like({ roleType: m.Good.Name, value: like }));
        }

        if (data.articleNumber) {
          const like: string = data.articleNumber.replace("*", "%") + "%";
          goodsPredicates.push(new Like({ roleType: m.Good.ArticleNumber, value: like }));
        }

        if (data.keyword) {
          const like: string = data.keyword.replace("*", "%") + "%";
          goodsPredicates.push(new Like({ roleType: m.Good.Keywords, value: like }));
        }

        if (data.owner) {
          const inventoryPredicate: And = new And();
          const inventoryPredicates: Predicate[] = inventoryPredicate.predicates;

          if (data.owner) {
            const like: string = data.owner.replace("*", "%") + "%";
            inventoryPredicates.push(new Like({ roleType: m.SerialisedInventoryItem.Owner, value: like }));
          }

          const inventoryQuery: Query = new Query({
            name: "inventoryItems",
            objectType: m.SerialisedInventoryItem,
            predicate: inventoryPredicate,
          });

          const containedIn: ContainedIn = new ContainedIn({ associationType: m.Good.InventoryItemVersionedsWhereGood, query: inventoryQuery });
          goodsPredicates.push(containedIn);
        }

        const goodsQuery: Query[] = [new Query(
          {
            include: [
              new TreeNode({ roleType: m.Good.PrimaryPhoto }),
              new TreeNode({ roleType: m.Good.LocalisedNames }),
              new TreeNode({ roleType: m.Good.LocalisedDescriptions }),
              new TreeNode({ roleType: m.Good.PrimaryProductCategory }),
            ],
            name: "goods",
            objectType: m.Good,
            page: new Page({ skip: 0, take }),
            predicate: goodsPredicate,
          })];

        return this.scope.load("Pull", new PullRequest({ query: goodsQuery }));

      })
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.goods as Good[];
        this.total = loaded.values.goods_total;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  public more(): void {
    this.page$.next(this.data.length + 50);
  }

  public delete(good: Good): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this product?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  public goBack(): void {
    window.history.back();
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

  public onView(good: Good): void {
    this.router.navigate(["/good/" + good.id]);
  }
}
