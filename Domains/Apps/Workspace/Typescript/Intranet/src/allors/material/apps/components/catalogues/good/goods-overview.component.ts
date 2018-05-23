import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';

import 'rxjs/add/observable/combineLatest';



import { ErrorService, FilterFactory, Invoked, Loaded, MediaService, Scope, WorkspaceService } from '../../../../../angular';
import { Brand, Good, InternalOrganisation, InventoryItemKind, Model, Organisation, OrganisationRole, Ownership, ProductCategory, ProductType, SerialisedInventoryItemState } from '../../../../../domain';
import { And, ContainedIn, Contains, Equals, Fetch, Like, Page, Predicate, PullRequest, Query, Sort, TreeNode } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';

import { NewGoodDialogComponent } from '../../catalogues/good/newgood-dialog.module';

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
export class GoodsOverviewComponent implements OnDestroy {
  public m: MetaDomain;
  public title = 'Products';
  public total: number;
  public searchForm: FormGroup;
  public data: Good[];
  public filtered: Good[];
  public chosenGood: string;

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
  private page$: BehaviorSubject<number>;

  private subscription: Subscription;
  private scope: Scope;
  private fetcher: Fetcher;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private router: Router,
    
    
    public mediaService: MediaService,
    private changeDetectorRef: ChangeDetectorRef,
    private stateService: StateService) {

    this.titleService.setTitle('Products');

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.chosenGood = 'Serialised';

    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.fetcher = new Fetcher(this.stateService, this.m);

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

    this.page$ = new BehaviorSubject<number>(50);

    const search$ = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$ = Observable.combineLatest(search$, this.page$, this.refresh$, this.stateService.internalOrganisationId$)
      .scan(([previousData, previousTake, previousDate, previousInternalOrganisationId], [data, take, date, internalOrganisationId]) => {
        return [
          data,
          data !== previousData ? 50 : take,
          date,
          internalOrganisationId,
        ];
      }, [] as [SearchData, number, Date, InternalOrganisation],
    );

    const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

    this.subscription = combined$
      .switchMap(([data, take, , internalOrganisationId]) => {
          const fetches: Fetch[] = [
            this.fetcher.internalOrganisation,
          ];

          const searchQuery: Query[] = [
            new Query(m.Brand),
            new Query(m.Model),
            new Query(m.InventoryItemKind),
            new Query(m.Ownership),
            new Query(m.SerialisedInventoryItemState),
            new Query({
              name: 'organisations',
              objectType: m.Organisation,
              sort: [
                new Sort({ roleType: m.ProductType.Name, direction: 'Asc' }),
              ],
            }),
            new Query({
              name: 'productCategories',
              objectType: m.ProductCategory,
              sort: [
                new Sort({ roleType: m.ProductCategory.Name, direction: 'Asc' }),
              ],
            }),
            new Query({
              name: 'productTypes',
              objectType: m.ProductType,
              sort: [
                new Sort({ roleType: m.ProductType.Name, direction: 'Asc' }),
              ],
            }),
            new Query({
              name: 'manufacturers',
              objectType: m.Organisation,
              predicate: new Equals({ roleType: m.Organisation.IsManufacturer, value: true}),
              sort: [
                new Sort({ roleType: m.Organisation.Name, direction: 'Asc' }),
              ],
            }),
          ];

          return this.scope
            .load('Pull', new PullRequest({ fetches, queries: searchQuery }))
            .switchMap((loaded) => {
              this.serialisedInventoryItemStates = loaded.collections.SerialisedInventoryItemStates as SerialisedInventoryItemState[];
              this.serialisedInventoryItemState = this.serialisedInventoryItemStates.find(
                (v: SerialisedInventoryItemState) => v.Name === data.state,
              );

              const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;
              this.suppliers = internalOrganisation.ActiveSuppliers as Organisation[];

              this.brands = loaded.collections.Brands as Brand[];
              this.brand = this.brands.find(
                (v: Brand) => v.Name === data.brand,
              );

              this.models = loaded.collections.Models as Model[];
              this.model = this.models.find(
                (v: Model) => v.Name === data.model,
              );

              this.inventoryItemKinds = loaded.collections.InventoryItemKinds as InventoryItemKind[];
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

              this.ownerships = loaded.collections.Ownerships as Ownership[];
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
              const goodsPredicates: Predicate[] = goodsPredicate.predicates;

              if (data.name) {
                const like: string = data.name.replace('*', '%') + '%';
                goodsPredicates.push(
                  new Like({ roleType: m.Good.Name, value: like }),
                );
              }

              if (data.articleNumber) {
                const like: string =
                  data.articleNumber.replace('*', '%') + '%';
                goodsPredicates.push(
                  new Like({ roleType: m.Good.ArticleNumber, value: like }),
                );
              }

              if (data.keyword) {
                const like: string = data.keyword.replace('*', '%') + '%';
                goodsPredicates.push(
                  new Like({ roleType: m.Good.Keywords, value: like }),
                );
              }

              if (data.brand) {
                goodsPredicates.push(
                  new Contains({
                    object: this.brand,
                    roleType: m.Good.StandardFeatures,
                  }),
                );
              }

              if (data.model) {
                goodsPredicates.push(
                  new Contains({
                    object: this.model,
                    roleType: m.Good.StandardFeatures,
                  }),
                );
              }

              if (data.productCategory) {
                goodsPredicates.push(
                  new Contains({
                    object: this.productCategory,
                    roleType: m.Good.ProductCategories,
                  }),
                );
              }

              if (data.inventoryItemKind) {
                goodsPredicates.push(
                  new Equals({
                    roleType: m.Good.InventoryItemKind,
                    value: this.inventoryItemKind,
                  }),
                );
              }

              if (data.manufacturer) {
                goodsPredicates.push(
                  new Equals({
                    roleType: m.Good.ManufacturedBy,
                    value: this.manufacturer,
                  }),
                );
              }

              if (data.supplier) {
                goodsPredicates.push(
                  new Contains({
                    object: this.supplier,
                    roleType: m.Good.SuppliedBy,
                  }),
                );
              }

              if (data.productType) {
                goodsPredicates.push(
                  new Equals({
                    roleType: m.Product.ProductType,
                    value: this.productType,
                  }),
                );
              }

              if (data.owner || data.ownership || data.state) {
                const inventoryPredicate: And = new And();
                const inventoryPredicates: Predicate[] =
                  inventoryPredicate.predicates;

                if (data.ownership) {
                    inventoryPredicates.push(
                    new Equals({
                      roleType: m.SerialisedInventoryItem.Ownership,
                      value: this.ownership,
                    }),
                  );
                }

                if (data.owner) {
                  inventoryPredicates.push(
                    new Equals({
                      roleType: m.SerialisedInventoryItem.Owner,
                      value: this.owner,
                    }),
                  );
                }

                if (data.state) {
                  inventoryPredicates.push(
                    new Equals({
                      roleType: m.SerialisedInventoryItem.SerialisedInventoryItemState,
                      value: this.serialisedInventoryItemState,
                    }),
                  );
                }

                const serialisedInventoryQuery: Query = new Query({
                  objectType: m.SerialisedInventoryItem,
                  predicate: inventoryPredicate,
                });

                const containedIn: ContainedIn = new ContainedIn({
                  associationType: m.Good.InventoryItemsWhereGood,
                  query: serialisedInventoryQuery,
                });
                goodsPredicates.push(containedIn);
              }

              const internalorganisationPredicate: And = new And();
              const internalorganisationPredicates: Predicate[] = internalorganisationPredicate.predicates;

              internalorganisationPredicates.push(
                new Equals({
                  roleType: m.VendorProduct.InternalOrganisation,
                  value: internalOrganisationId,
                }),
              );

              const vendorProductQuery: Query = new Query({
                objectType: m.VendorProduct,
                predicate: internalorganisationPredicate,
              });

              const VendorProductscontainedIn: ContainedIn = new ContainedIn({
                associationType: m.Product.VendorProductsWhereProduct,
                query: vendorProductQuery,
              });

              goodsPredicates.push(VendorProductscontainedIn);

              const goodsQuery: Query[] = [
                new Query({
                  include: [
                    new TreeNode({ roleType: m.Good.PrimaryPhoto }),
                    new TreeNode({ roleType: m.Good.LocalisedNames }),
                    new TreeNode({ roleType: m.Good.LocalisedDescriptions }),
                    new TreeNode({ roleType: m.Good.PrimaryProductCategory }),
                  ],
                  name: 'goods',
                  objectType: m.Good,
                  page: new Page({ skip: 0, take }),
                  predicate: goodsPredicate,
                }),
              ];

              return this.scope.load(
                'Pull',
                new PullRequest({ queries: goodsQuery }),
              );
            });
      })
      .subscribe(
        (loaded) => {
          this.data = loaded.collections.goods as Good[];
          this.total = loaded.values.goods_total;
        },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public more(): void {
    this.page$.next(this.data.length + 50);
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public delete(good: Good): void {
    // TODO:
    /*  this.dialogService
      .openConfirm({ message: 'Are you sure you want to delete this product?' })
      .afterClosed()
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
      }); */
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
    return (
      good.InventoryItemKind ===
      this.inventoryItemKinds.find(
        (v: InventoryItemKind) =>
          v.UniqueId.toUpperCase() === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE',
      )
    );
  }

  public goBack(): void {
    window.history.back();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public onView(good: Good): void {
    this.router.navigate(['/good/' + good.id]);
  }
}
