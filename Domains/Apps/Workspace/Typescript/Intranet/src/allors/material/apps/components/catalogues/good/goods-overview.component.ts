import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable, Subscription } from 'rxjs';

import { ErrorService, Invoked, MediaService, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { Brand, Good, InternalOrganisation, InventoryItemKind, Model, Organisation, Ownership, ProductCategory, ProductType, SerialisedInventoryItemState, SerialisedInventoryItem, NonSerialisedInventoryItem, Media } from '../../../../../domain';
import { And, ContainedIn, Contains, Equals, Like, Predicate, PullRequest, Sort, Filter } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

import { NewGoodDialogComponent } from '../../catalogues/good/newgood-dialog.module';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

interface Row {
  good: Good;
  isSerialised: boolean;
  serialisedInventoryItem: SerialisedInventoryItem;
  nonSerialisedInventoryItem: NonSerialisedInventoryItem[];
  available: number;
  avatar: Media;
  name: string;
  description: string;
  articleNumber: string;
}

interface SearchData {
  name: string;
  articleNumber: string;
  productCategory: string;
  productType: string;
  brand: string;
  model: string;
  state: string;
  ownership: string;
  inventoryItemKind: string;
  supplier: string;
  manufacturer: string;
  owner: string;
  keyword: string;
}

@Component({
  templateUrl: './goods-overview.component.html',
})
export class GoodsOverviewComponent implements OnInit, OnDestroy {
  public m: MetaDomain;
  public advancedSearch: boolean;
  public title = 'Products';
  public searchForm: FormGroup;
  public filtered: Good[];
  public chosenGood: string;

  public rows: Row[] = [];
  public selected: Row[] = [];

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

  public serialisedInventoryItemStates: SerialisedInventoryItemState[];
  public selectedSerialisedInventoryItemState: SerialisedInventoryItemState;
  public serialisedInventoryItemState: SerialisedInventoryItemState;

  public ownerships: Ownership[];
  public selectedOwnership: Ownership;
  public ownership: Ownership;

  public inventoryItemKinds: InventoryItemKind[];
  public selectedInventoryItemKind: InventoryItemKind;
  public inventoryItemKind: InventoryItemKind;

  public suppliers: Organisation[];
  public selectedSupplier: Organisation;
  public supplier: Organisation;

  public owners: Organisation[];
  public selectedOwner: Organisation;
  public owner: Organisation;

  public manufacturers: Organisation[];
  public selectedManufacturer: Organisation;
  public manufacturer: Organisation;

  private refresh$: BehaviorSubject<Date>;

  private subscription: Subscription;
  private scope: Scope;
  private fetcher: Fetcher;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private router: Router,
    public mediaService: MediaService,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle(this.title);

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.chosenGood = 'Serialised';

    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.fetcher = new Fetcher(this.stateService, this.dataService);

    this.searchForm = this.formBuilder.group({
      articleNumber: [''],
      brand: [''],
      inventoryItemKind: [''],
      keyword: [''],
      manufacturer: [''],
      model: [''],
      name: [''],
      owner: [''],
      ownership: [''],
      productCategory: [''],
      productType: [''],
      supplier: [''],
      state: [''],
    });
  }

  ngOnInit(): void {

    const { m, pull } = this.dataService;

    const search$ = this.searchForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        startWith({}),
      );

    const combined$ = Observable.combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousData, previousDate, previousInternalOrganisationId], [data, date, internalOrganisationId]) => {
          return [data, date, internalOrganisationId];
        }, [])
      );

    this.subscription = combined$
      .pipe(
        switchMap(([data, , internalOrganisationId]) => {
          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Brand({ sort: new Sort(m.Brand.Name) }),
            pull.Model({ sort: new Sort(m.Model.Name) }),
            pull.InventoryItemKind({ sort: new Sort(m.InventoryItemKind.Name) }),
            pull.Ownership({ sort: new Sort(m.Ownership.Name) }),
            pull.SerialisedInventoryItemState(
              {
                predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
                sort: new Sort(m.SerialisedInventoryItemState.Name)
              }
            ),
            pull.Organisation({ sort: new Sort(m.Organisation.PartyName) }),
            pull.ProductCategory({ sort: new Sort(m.ProductCategory.Name) }),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }),
            pull.Organisation(
              {
                predicate: new Equals({ propertyType: m.Organisation.IsManufacturer, value: true }),
                sort: [
                  new Sort(m.Organisation.Name),
                ],
              }
            )
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                this.serialisedInventoryItemStates = loaded.collections.serialisedInventoryItemStates as SerialisedInventoryItemState[];
                this.serialisedInventoryItemState = this.serialisedInventoryItemStates.find(
                  (v: SerialisedInventoryItemState) => v.Name === data.state,
                );

                const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
                this.suppliers = internalOrganisation.ActiveSuppliers as Organisation[];

                this.brands = loaded.collections.brands as Brand[];
                this.brand = this.brands.find(
                  (v: Brand) => v.Name === data.brand,
                );

                this.models = loaded.collections.models as Model[];
                this.model = this.models.find(
                  (v: Model) => v.Name === data.model,
                );

                this.inventoryItemKinds = loaded.collections.inventoryItemKinds as InventoryItemKind[];
                this.inventoryItemKind = this.inventoryItemKinds.find(
                  (v: InventoryItemKind) => v.Name === data.inventoryItemKind,
                );

                this.productCategories = loaded.collections.productCategories as ProductCategory[];
                this.productCategory = this.productCategories.find(
                  (v: ProductCategory) => v.Name === data.productCategory,
                );

                this.productTypes = loaded.collections.productTypes as ProductType[];
                this.productType = this.productTypes.find(
                  (v: ProductType) => v.Name === data.productType,
                );

                this.ownerships = loaded.collections.ownerShips as Ownership[];
                this.ownership = this.ownerships.find(
                  (v: Ownership) => v.Name === data.ownership,
                );

                this.manufacturers = loaded.collections.manufacturers as Organisation[];
                this.manufacturer = this.manufacturers.find(
                  (v: Organisation) => v.Name === data.manufacturer,
                );

                this.suppliers = internalOrganisation.ActiveSuppliers as Organisation[];
                this.supplier = this.suppliers.find(
                  (v: Organisation) => v.Name === data.supplier,
                );

                this.owners = loaded.collections.organisations as Organisation[];
                this.owner = this.owners.find(
                  (v: Organisation) => v.Name === data.owner,
                );

                const goodsPredicate: And = new And();
                const goodsPredicates: Predicate[] = goodsPredicate.operands;

                if (data.name) {
                  const like: string = data.name.replace('*', '%') + '%';
                  goodsPredicates.push(
                    new Like({ roleType: m.Good.Name, value: like }),
                  );
                }

                // TODO:
                // if (data.articleNumber) {
                //   const like: string =
                //     data.articleNumber.replace('*', '%') + '%';
                //   goodsPredicates.push(
                //     new Like({ roleType: m.Good.ArticleNumber, value: like }),
                //   );
                // }

                if (data.keyword) {
                  const like: string = data.keyword.replace('*', '%') + '%';
                  goodsPredicates.push(
                    new Like({ roleType: m.Good.Keywords, value: like }),
                  );
                }

                // TODO:
                // if (data.brand) {
                //   goodsPredicates.push(
                //     new Contains({
                //       object: this.brand,
                //       propertyType: m.Good.StandardFeatures,
                //     }),
                //   );
                // }

                // TODO:
                // if (data.model) {
                //   goodsPredicates.push(
                //     new Contains({
                //       object: this.model,
                //       propertyType: m.Good.StandardFeatures,
                //     }),
                //   );
                // }

                if (data.productCategory) {
                  goodsPredicates.push(
                    new Contains({
                      object: this.productCategory,
                      propertyType: m.Good.ProductCategories,
                    }),
                  );
                }

                // TODO:
                // if (data.inventoryItemKind) {
                //   goodsPredicates.push(
                //     new Equals({
                //       propertyType: m.Good.InventoryItemKind,
                //       value: this.inventoryItemKind,
                //     }),
                //   );
                // }

                // TODO:
                // if (data.manufacturer) {
                //   goodsPredicates.push(
                //     new Equals({
                //       propertyType: m.Good.ManufacturedBy,
                //       value: this.manufacturer,
                //     }),
                //   );
                // }

                if (data.supplier) {
                  goodsPredicates.push(
                    new Contains({
                      object: this.supplier,
                      propertyType: m.Good.SuppliedBy,
                    }),
                  );
                }

                // TODO:
                // if (data.productType) {
                //   goodsPredicates.push(
                //     new Equals({
                //       propertyType: m.Product.ProductType,
                //       value: this.productType,
                //     }),
                //   );
                // }

                if (data.owner || data.ownership || data.state) {
                  const inventoryPredicate: And = new And();
                  const inventoryPredicates: Predicate[] =
                    inventoryPredicate.operands;

                  if (data.ownership) {
                    inventoryPredicates.push(
                      new Equals({
                        propertyType: m.SerialisedInventoryItem.Ownership,
                        value: this.ownership,
                      }),
                    );
                  }

                  // TODO:
                  // if (data.owner) {
                  //   inventoryPredicates.push(
                  //     new Equals({
                  //       propertyType: m.SerialisedInventoryItem.Owner,
                  //       value: this.owner,
                  //     }),
                  //   );
                  // }

                  if (data.state) {
                    inventoryPredicates.push(
                      new Equals({
                        propertyType: m.SerialisedInventoryItem.SerialisedInventoryItemState,
                        value: this.serialisedInventoryItemState,
                      }),
                    );
                  }

                  const serialisedInventoryQuery = new Filter({
                    objectType: m.SerialisedInventoryItem,
                    predicate: inventoryPredicate,
                  });

                  // TODO:
                  // const containedIn: ContainedIn = new ContainedIn({
                  //   propertyType: m.Good.InventoryItemsWhereGood,
                  //   extent: serialisedInventoryQuery,
                  // });
                  // goodsPredicates.push(containedIn);
                }

                const internalorganisationPredicate: And = new And();
                const internalorganisationPredicates: Predicate[] = internalorganisationPredicate.operands;

                internalorganisationPredicates.push(
                  new Equals({
                    propertyType: m.VendorProduct.InternalOrganisation,
                    value: internalOrganisationId,
                  }),
                );

                const vendorProductQuery = new Filter({
                  objectType: m.VendorProduct,
                  predicate: internalorganisationPredicate,
                });

                const VendorProductscontainedIn: ContainedIn = new ContainedIn({
                  propertyType: m.Product.VendorProductsWhereProduct,
                  extent: vendorProductQuery,
                });

                goodsPredicates.push(VendorProductscontainedIn);

                const queries = [

                  // TODO: Add to pull.SerialisedInventoryItem
                  // new TreeNode({
                  //   roleType: m.SerialisedInventoryItem.Good,
                  //   nodes: [
                  //     new TreeNode({ roleType: m.Good.PrimaryPhoto }),
                  //     new TreeNode({ roleType: m.Good.LocalisedNames }),
                  //     new TreeNode({ roleType: m.Good.LocalisedDescriptions }),
                  //     new TreeNode({ roleType: m.Good.LocalisedComments }),
                  //     new TreeNode({ roleType: m.Good.PrimaryProductCategory }),
                  //   ]
                  // }),

                  // TODO: Add to pull.NonSerialisedInventoryItem
                  // include: [
                  //   new TreeNode({
                  //     roleType: m.NonSerialisedInventoryItem.Good,
                  //     nodes: [
                  //       new TreeNode({ roleType: m.Good.PrimaryPhoto }),
                  //       new TreeNode({ roleType: m.Good.LocalisedNames }),
                  //       new TreeNode({ roleType: m.Good.LocalisedDescriptions }),
                  //       new TreeNode({ roleType: m.Good.LocalisedComments }),
                  //       new TreeNode({ roleType: m.Good.PrimaryProductCategory }),
                  //     ]
                  //   }),
                  // ],

                  pull.SerialisedInventoryItem({
                    include: {
                      SerialisedInventoryItemState: x,
                      SerialisedInventoryItemCharacteristics: {
                        SerialisedInventoryItemCharacteristicType: {
                          UnitOfMeasure: x
                        }
                      }
                    }
                  }),
                  pull.NonSerialisedInventoryItem(),
                  pull.Good({
                    include: {
                      PrimaryPhoto: x,
                      LocalisedNames: x,
                      LocalisedDescriptions: x,
                      PrimaryProductCategory: x,
                    },
                    predicate: goodsPredicate,
                    sort: new Sort(m.Good.Name),
                  })
                ];

                return this.scope.load(
                  'Pull',
                  new PullRequest({ pulls: queries }),
                );
              })
            );
        })
      )
      .subscribe(
        (loaded) => {
          const serialisedInventoryItems = loaded.collections.serialisedInventoryItems as SerialisedInventoryItem[];
          const nonSerialisedInventoryItems = loaded.collections.nonSerialisedInventoryItems as NonSerialisedInventoryItem[];
          const goods = loaded.collections['Goods'] as Good[];

          // TODO
          // const serialisedInventoryItemByGoodId = serialisedInventoryItems.reduce((map, obj) => {
          //   map[obj.Good.id] = obj;
          //   return map;
          // }, {});

          // const nonSerialisedInventoryItemByGoodId: { [id: string]: NonSerialisedInventoryItem[] } = nonSerialisedInventoryItems.reduce((map, obj) => {
          //   if (map[obj.Good.id]) {
          //     map[obj.Good.id].push(obj);
          //   } else {
          //     map[obj.Good.id] = [obj];
          //   }
          //   return map;
          // }, {});

          // this.rows = goods.map((v: Good) => {
          //   return {
          //     good: v,
          //     serialisedInventoryItem: serialisedInventoryItemByGoodId[v.id],
          //     nonSerialisedInventoryItem: nonSerialisedInventoryItemByGoodId[v.id],
          //   };
          // }).map<Row>(v => {
          //   return {
          //     good: v.good,
          //     isSerialised: this.serialisedGood(v.good),
          //     serialisedInventoryItem: v.serialisedInventoryItem,
          //     nonSerialisedInventoryItem: v.nonSerialisedInventoryItem,
          //     available: v.serialisedInventoryItem ? 1 : v.nonSerialisedInventoryItem ? v.nonSerialisedInventoryItem.reduce((acc, v) => acc + v.AvailableToPromise, 0) : 0,
          //     avatar: v.good.PrimaryPhoto,
          //     name: v.good.Name,
          //     description: v.good.Description,
          //     articleNumber: v.good.ArticleNumber
          //   };
          // });
        },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }


  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public delete(good: Good): void {
    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this product?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(good.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public addGood(): void {
    const dialogRef = this.dialog.open(NewGoodDialogComponent, {
      data: { chosenGood: this.chosenGood },
      height: '300px',
      width: '700px',
    });

    dialogRef.afterClosed().subscribe((answer: string) => {
      if (answer === 'Serialised') {
        this.router.navigate(['/serialisedGood']);
      }
      if (answer === 'NonSerialised') {
        this.router.navigate(['/nonSerialisedGood']);
      }
    });
  }

  public serialisedGood(good: Good): boolean {
    // TODO:
    // return (
    //   good.InventoryItemKind ===
    //   this.inventoryItemKinds.find(
    //     (v: InventoryItemKind) =>
    //       v.UniqueId.toUpperCase() === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE',
    //   )
    // );
  }

  public goBack(): void {
    window.history.back();
  }

  public onView(good: Good): void {
    this.router.navigate(['/good/' + good.id]);
  }
}
