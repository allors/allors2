import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatSnackBar } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, MediaService, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { Brand, Good, InternalOrganisation, InventoryItemKind, Model, Organisation, Ownership, ProductCategory, ProductType, SerialisedInventoryItemState, SerialisedInventoryItem, NonSerialisedInventoryItem, Media, Product } from '../../../../../domain';
import { And, ContainedIn, Contains, Equals, Like, Predicate, PullRequest, Sort, Filter, ObjectType } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

import { NewGoodDialogComponent } from '../../catalogues/good/newgood-dialog.module';
import { debounceTime, distinctUntilChanged, startWith, scan, switchMap } from 'rxjs/operators';

interface SearchData {
  name?: string;
  sku?: string;
  keyword?: string;
  isSerialised?: boolean;
  productCategory?: ProductCategory;
  inventoryItemKind?: InventoryItemKind;
  serialisedInventoryItem?: SerialisedInventoryItem;
  nonSerialisedInventoryItem?: NonSerialisedInventoryItem[];
  available?: number;
  avatar?: Media;
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

  public goods: Product[] = [];
  public productCategories: ProductCategory[];
  public selectedProductCategory: ProductCategory;
  public productCategory: ProductCategory;
  public productTypes: ProductType[];
  public selectedProductType: ProductType;
  public productType: ProductType;
  public serialisedInventoryItemStates: SerialisedInventoryItemState[];
  public ownerships: Ownership[];
  public inventoryItemKinds: InventoryItemKind[];
  public suppliers: Organisation[];
  public owners: Organisation[];
  public manufacturers: Organisation[];

  private refresh$: BehaviorSubject<Date>;
  public search$: BehaviorSubject<SearchData>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private titleService: Title,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private router: Router,
    public mediaService: MediaService,
    private dialogService: AllorsMaterialDialogService,
    private stateService: StateService) {

    this.titleService.setTitle(this.title);

    this.scope = this.workspaceService.createScope();
    this.search$ = new BehaviorSubject<SearchData>({});
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.chosenGood = 'Serialised';
  }

  ngOnInit(): void {

    const { m, pull } = this.dataService;

    const search$ = this.search$
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
      );

    this.subscription = combineLatest(search$, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([data, refresh, internalOrganisationId]) => {

          // const goodsPredicate: And = new And();
          // const goodsPredicates: Predicate[] = goodsPredicate.operands;

          // if (data.name) {
          //   const like: string = data.name.replace('*', '%') + '%';
          //   goodsPredicates.push(new Like({ roleType: m.Good.Name, value: like }));
          // }

          // if (data.sku) {
          //   const like: string = data.sku.replace('*', '%') + '%';
          //   goodsPredicates.push(new Like({ roleType: m.Good.Sku, value: like }));
          // }

          // if (data.keyword) {
          //   const like: string = data.keyword.replace('*', '%') + '%';
          //   goodsPredicates.push(new Like({ roleType: m.Good.Keywords, value: like }));
          // }

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

          // if (data.productCategory) {
          //   goodsPredicates.push(new Contains({ object: this.productCategory, propertyType: m.Good.ProductCategories }));
          // }

          // if (data.inventoryItemKind) {
          //   const FinishedGoodQuery = new Filter(
          //     {
          //       objectType: m.FinishedGood,
          //       predicate: new Equals({ propertyType: m.FinishedGood.InventoryItemKind, object: data.inventoryItemKind })
          //     }
          //   );
          //   goodsPredicates.push(new ContainedIn({ propertyType: m.Good.FinishedGood, extent: FinishedGoodQuery }));
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

          // if (data.supplier) {
          //   goodsPredicates.push(
          //     new Contains({
          //       object: this.supplier,
          //       propertyType: m.Good.SuppliedBy,
          //     }),
          //   );
          // }

          // TODO:
          // if (data.productType) {
          //   goodsPredicates.push(
          //     new Equals({
          //       propertyType: m.Product.ProductType,
          //       value: this.productType,
          //     }),
          //   );
          // }

          // if (data.owner || data.ownership || data.state) {
          //   const inventoryPredicate: And = new And();
          //   const inventoryPredicates: Predicate[] =
          //     inventoryPredicate.operands;

          //   if (data.ownership) {
          //     inventoryPredicates.push(
          //       new Equals({
          //         propertyType: m.SerialisedInventoryItem.Ownership,
          //         object: this.ownership,
          //       }),
          //     );
          //   }

          // TODO:
          // if (data.owner) {
          //   inventoryPredicates.push(
          //     new Equals({
          //       propertyType: m.SerialisedInventoryItem.Owner,
          //       value: this.owner,
          //     }),
          //   );
          // }

          // if (data.state) {
          //   inventoryPredicates.push(
          //     new Equals({
          //       propertyType: m.SerialisedInventoryItem.SerialisedInventoryItemState,
          //       object: this.serialisedInventoryItemState,
          //     }),
          //   );
          // }


          // TODO:
          // const containedIn: ContainedIn = new ContainedIn({
          //   propertyType: m.Good.InventoryItemsWhereGood,
          //   extent: serialisedInventoryQuery,
          // });
          // goodsPredicates.push(containedIn);
          // }

          // const internalorganisationPredicate: And = new And();
          // const internalorganisationPredicates: Predicate[] = internalorganisationPredicate.operands;

          // internalorganisationPredicates.push(
          //   new Equals({
          //     propertyType: m.VendorProduct.InternalOrganisation,
          //     value: internalOrganisationId,
          //   }),
          // );

          // const vendorProductQuery = new Filter({
          //   objectType: m.VendorProduct,
          //   predicate: internalorganisationPredicate,
          // });

          // const VendorProductscontainedIn: ContainedIn = new ContainedIn({
          //   propertyType: m.Product.VendorProductsWhereProduct,
          //   extent: vendorProductQuery,
          // });

          // goodsPredicates.push(VendorProductscontainedIn);

          const pulls = [
            // pull.Brand({ sort: new Sort(m.Brand.Name) }),
            // pull.Model({ sort: new Sort(m.Model.Name) }),
            // pull.InventoryItemKind({ sort: new Sort(m.InventoryItemKind.Name) }),
            // pull.Ownership({ sort: new Sort(m.Ownership.Name) }),
            // pull.SerialisedInventoryItemState(
            //   {
            //     predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
            //     sort: new Sort(m.SerialisedInventoryItemState.Name)
            //   }
            // ),
            // pull.Organisation({ sort: new Sort(m.Organisation.PartyName) }),
            // pull.ProductCategory({ sort: new Sort(m.ProductCategory.Name) }),
            // pull.ProductType({ sort: new Sort(m.ProductType.Name) }),
            // pull.Organisation(
            //   {
            //     predicate: new Equals({ propertyType: m.Organisation.IsManufacturer, value: true }),
            //     sort: [
            //       new Sort(m.Organisation.Name),
            //     ],
            //   }
            // ),
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

            // pull.SerialisedInventoryItem({
            //   include: {
            //     SerialisedInventoryItemState: x,
            //     SerialisedInventoryItemCharacteristics: {
            //       SerialisedInventoryItemCharacteristicType: {
            //         UnitOfMeasure: x
            //       }
            //     }
            //   }
            // }),
            // pull.NonSerialisedInventoryItem(),
            pull.Good({
              include: {
                PrimaryPhoto: x,
                LocalisedNames: x,
                LocalisedDescriptions: x,
                PrimaryProductCategory: x,
                FinishedGood: x
              },
              // predicate: goodsPredicate,
              sort: new Sort(m.Good.Name),
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.scope.session.reset();

        // const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        // this.suppliers = internalOrganisation.ActiveSuppliers as Organisation[];

        this.goods = loaded.collections.Goods as Good[];
        // this.serialisedInventoryItemStates = loaded.collections.SerialisedInventoryItemStates as SerialisedInventoryItemState[];
        // this.inventoryItemKinds = loaded.collections.InventoryItemKinds as InventoryItemKind[];
        // this.productCategories = loaded.collections.ProductCategories as ProductCategory[];
        // this.productTypes = loaded.collections.ProductTypes as ProductType[];
        // this.ownerships = loaded.collections.OwnerShips as Ownership[];
        // this.manufacturers = loaded.collections.Manufacturers as Organisation[];
        // this.owners = loaded.collections.Organisations as Organisation[];

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
            .subscribe(() => {
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
    return (
      good.FinishedGood.InventoryItemKind === this.inventoryItemKinds
        .find((v: InventoryItemKind) => v.UniqueId.toUpperCase() === '2596E2DD-3F5D-4588-A4A2-167D6FBE3FAE')
    );
    return false;
  }

  public goBack(): void {
    window.history.back();
  }

  public onView(good: Good): void {
    if (this.serialisedGood(good)) {
      this.router.navigate(['/serialisedGood/' + good.id]);
    } else {
      this.router.navigate(['/nonSerialisedGood/' + good.id]);
    }
  }
}
