import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Subscription } from "rxjs/Rx";

import { TdDialogService, TdMediaService } from "@covalent/core";

import { AllorsService, ErrorService, Loaded, Scope } from "@allors";
import { And, ContainedIn, Contains, Equals, Like, Page, Predicate, PullRequest, Query, Sort, TreeNode } from "@allors";
import { Brand, Good, InventoryItemKind, Model, Organisation, OrganisationRole, Ownership, ProductCategory, ProductType, SerialisedInventoryItemState } from "@allors";
import { MetaDomain } from "@allors";

interface SearchData {
  name: string;
  articleNumber: string;
  productCategory: string;
  productType: string;
  brand: string;
  model: string;
  status: string;
  ownership: string;
  inventoryItemKind: string;
  supplier: string;
  manufacturer: string;
  owner: string;
  keyword: string;
}

@Component({
  templateUrl: "./goods-overview.component.html",
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

  public objectStates: SerialisedInventoryItemState[];
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
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.titleService.setTitle("Products");

    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      articleNumber: [""],
      brand: [""],
      inventoryItemKind: [""],
      keyword: [""],
      manufacturer: [""],
      model: [""],
      name: [""],
      owner: [""],
      ownership: [""],
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

    const m: MetaDomain = this.allors.meta;

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {

        const rolesQuery: Query[] = [
          new Query(
            {
              name: "organisationRoles",
              objectType: m.OrganisationRole,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: rolesQuery }))
          .switchMap((rolesLoaded: Loaded) => {
            const organisationRoles: OrganisationRole[] = rolesLoaded.collections.organisationRoles as OrganisationRole[];
            const manufacturerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === "Manufacturer");
            const supplierRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === "Supplier");

            const searchQuery: Query[] = [
              new Query(
                {
                  name: "brands",
                  objectType: m.Brand,
                }),
              new Query(
                {
                  name: "models",
                  objectType: m.Model,
                }),
              new Query(
                {
                  name: "inventoryItemKinds",
                  objectType: m.InventoryItemKind,
                }),
              new Query(
                {
                  name: "categories",
                  objectType: m.ProductCategory,
                }),
              new Query(
                {
                  name: "productTypes",
                  objectType: m.ProductType,
                }),
              new Query(
                {
                  name: "organisations",
                  objectType: m.ProductType,
                }),
              new Query(
                {
                  name: "ownerships",
                  objectType: m.Ownership,
                }),
              new Query(
                {
                  name: "manufacturers",
                  objectType: m.Organisation,
                  predicate: new Contains({ roleType: m.Organisation.OrganisationRoles, object: manufacturerRole }),
                  sort: [new Sort ({ roleType: m.Organisation.Name, direction: "Asc" })],
                }),
                new Query(
                  {
                    name: "suppliers",
                    objectType: m.Organisation,
                    predicate: new Contains({ roleType: m.Organisation.OrganisationRoles, object: supplierRole }),
                    sort: [new Sort ({ roleType: m.Organisation.Name, direction: "Asc" })],
                  }),
              ];

            return this.scope
              .load("Pull", new PullRequest({ query: searchQuery }))
              .switchMap((loaded: Loaded) => {

                this.brands = loaded.collections.brands as Brand[];
                this.brand = this.brands.find((v: Brand) => v.Name === data.brand);

                this.models = loaded.collections.models as Model[];
                this.model = this.models.find((v: Model) => v.Name === data.model);

                this.inventoryItemKinds = loaded.collections.inventoryItemKinds as InventoryItemKind[];
                this.inventoryItemKind = this.inventoryItemKinds.find((v: InventoryItemKind) => v.Name === data.inventoryItemKind);

                this.productCategories = loaded.collections.categories as ProductCategory[];
                this.productCategory = this.productCategories.find((v: ProductCategory) => v.Name === data.productCategory);

                this.productTypes = loaded.collections.productTypes as ProductType[];
                this.productType = this.productTypes.find((v: ProductType) => v.Name === data.productType);

                this.ownerships = loaded.collections.ownerships as Ownership[];
                this.ownership = this.ownerships.find((v: Ownership) => v.Name === data.ownership);

                this.manufacturers = loaded.collections.manufacturers as Organisation[];
                this.manufacturer = this.manufacturers.find((v: Organisation) => v.Name === data.manufacturer);

                this.suppliers = loaded.collections.suppliers as Organisation[];
                this.supplier = this.suppliers.find((v: Organisation) => v.Name === data.supplier);

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

                if (data.brand) {
                  goodsPredicates.push(new Contains({ roleType: m.Good.StandardFeatures, object: this.brand }));
                }

                if (data.model) {
                  goodsPredicates.push(new Contains({ roleType: m.Good.StandardFeatures, object: this.model }));
                }

                if (data.productCategory) {
                  goodsPredicates.push(new Contains({ roleType: m.Good.ProductCategories, object: this.productCategory }));
                }

                if (data.inventoryItemKind) {
                  goodsPredicates.push(new Equals({ roleType: m.Good.InventoryItemKind, value: this.inventoryItemKind }));
                }

                if (data.manufacturer) {
                  goodsPredicates.push(new Equals({ roleType: m.Good.ManufacturedBy, value: this.manufacturer }));
                }

                if (data.supplier) {
                  goodsPredicates.push(new Equals({ roleType: m.Good.SuppliedBy, value: this.supplier }));
                }

                if (data.owner || data.ownership) {
                  const inventoryPredicate: And = new And();
                  const inventoryPredicates: Predicate[] = inventoryPredicate.predicates;

                  if (data.owner) {
                    const like: string = data.owner.replace("*", "%") + "%";
                    inventoryPredicates.push(new Like({ roleType: m.SerialisedInventoryItem.Owner, value: like }));
                  }

                  if (data.ownership) {
                    inventoryPredicates.push(new Equals({ roleType: m.SerialisedInventoryItem.Ownership, value: this.ownership }));
                  }

                  const serialisedInventoryQuery: Query = new Query({
                    objectType: m.SerialisedInventoryItem,
                    predicate: inventoryPredicate,
                  });

                  const containedIn: ContainedIn = new ContainedIn({ associationType: m.Good.InventoryItemsWhereGood, query: serialisedInventoryQuery });
                  goodsPredicates.push(containedIn);
                }

                if (data.productType) {
                  const inventoryPredicate: And = new And();
                  const inventoryPredicates: Predicate[] = inventoryPredicate.predicates;

                  if (data.productType) {
                    inventoryPredicates.push(new Equals({ roleType: m.InventoryItem.ProductType, value: this.productType }));
                  }

                  const inventoryQuery: Query = new Query({
                    objectType: m.InventoryItem,
                    predicate: inventoryPredicate,
                  });

                  const containedIn: ContainedIn = new ContainedIn({ associationType: m.Good.InventoryItemsWhereGood, query: inventoryQuery });
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
              });
          });
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
