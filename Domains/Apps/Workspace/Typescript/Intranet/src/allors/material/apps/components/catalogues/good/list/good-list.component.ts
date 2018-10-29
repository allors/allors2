import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';
import { Title } from '@angular/platform-browser';
import { MatSnackBar, MatTableDataSource, MatDialog, PageEvent } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Like, Sort, Equals, Contains, SessionObject } from '../../../../../../framework';
import { ErrorService, MediaService, x, Allors, NavigationService, Invoked } from '../../../../../../angular';
import { AllorsFilterService } from '../../../../../../angular/base/filter';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { Sorter } from '../../../../../base/sorting';
import { StateService } from '../../../../services/StateService';

import { Good, Part, ProductCategory, ProductType, Ownership, InventoryItemKind, Organisation, SerialisedInventoryItemState, InternalOrganisation } from '../../../../../../domain';
import { Fetcher } from '../../../Fetcher';
import { GoodEditComponent } from '../../good/edit/good-edit.component';

interface Row {
  good: Good;
  name: string;
  qoh: number;
}

@Component({
  templateUrl: './good-list.component.html',
  providers: [Allors, AllorsFilterService]
})
export class GoodListComponent implements OnInit, OnDestroy {

  public title = 'Products';

  public productCategories: ProductCategory[];
  public productTypes: ProductType[];
  public ownerships: Ownership[];
  public inventoryItemKinds: InventoryItemKind[];
  public owners: Organisation[];
  public manufacturers: Organisation[];
  public suppliers: Organisation[];
  public serialisedInventoryItemStates: SerialisedInventoryItemState[];
  public parts: Part[];

  public displayedColumns = ['select', 'name', 'category', 'qty on hand', 'menu'];
  public selection = new SelectionModel<Row>(true, []);

  public total: number;
  public dataSource = new MatTableDataSource<Row>();

  private sort$: BehaviorSubject<Sort>;
  private refresh$: BehaviorSubject<Date>;
  private pager$: BehaviorSubject<PageEvent>;

  private subscription: Subscription;
  private readonly fetcher: Fetcher;

  constructor(
    @Self() private allors: Allors,
    @Self() private filterService: AllorsFilterService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private location: Location,
    private titleService: Title,
    private stateService: StateService) {

    this.titleService.setTitle(this.title);

    this.sort$ = new BehaviorSubject<Sort>(undefined);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, allors.pull);
    this.pager$ = new BehaviorSubject<PageEvent>(Object.assign(new PageEvent(), { pageIndex: 0, pageSize: 50 }));
  }

  ngOnInit(): void {
    const { m, pull, scope } = this.allors;

    const predicate = new And([
      new Like({ roleType: m.Good.Name, parameter: 'Name' }),
      new Like({ roleType: m.Good.Sku, parameter: 'Sku' }),
      new Like({ roleType: m.Good.Keywords, parameter: 'Keyword' }),
      new Contains({ propertyType: m.Good.ProductCategories, parameter: 'Category' })
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        name: m.Good.Name
      }
    );

    this.subscription = combineLatest(this.refresh$, this.filterService.filterFields$, this.sort$, this.pager$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
            internalOrganisationId,
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

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
            this.fetcher.internalOrganisation,
            // pull.Brand({ sort: new Sort(m.Brand.Name) }),
            // pull.Model({ sort: new Sort(m.Model.Name) }),
            pull.Part({
              include: {
                InventoryItemKind: x
              },
            }),
            pull.InventoryItemKind({ sort: new Sort(m.InventoryItemKind.Name) }),
            pull.Ownership({ sort: new Sort(m.Ownership.Name) }),
            pull.SerialisedInventoryItemState({
                predicate: new Equals({ propertyType: m.SerialisedInventoryItemState.IsActive, value: true }),
                sort: new Sort(m.SerialisedInventoryItemState.Name)
              }),
            pull.ProductCategory({ sort: new Sort(m.ProductCategory.Name) }),
            pull.ProductType({ sort: new Sort(m.ProductType.Name) }),
            pull.Organisation({
              name: 'Owners',
              sort: new Sort(m.Organisation.PartyName),
            }),
            pull.Organisation({
                name: 'Manufacturers',
                predicate: new Equals({ propertyType: m.Organisation.IsManufacturer, value: true }),
                sort: [ new Sort(m.Organisation.Name) ],
              }),

            pull.Good({
              predicate,
              sort: sorter.create(sort),
              include: {
                LocalisedNames: x,
                PrimaryProductCategory: x,
                PrimaryPhoto: x
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        scope.session.reset();
        this.total = loaded.values.Goods_total;
        const goods = loaded.collections.Goods as Good[];

        this.dataSource.data = goods.map((v) => {
          return {
            good: v,
            name: v.Name,
            category: v.PrimaryProductCategory,
            qoh: v.Part.QuantityOnHand
          } as Row;
        });

        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;
        this.suppliers = internalOrganisation.ActiveSuppliers as Organisation[];

        this.serialisedInventoryItemStates = loaded.collections.SerialisedInventoryItemStates as SerialisedInventoryItemState[];
        this.inventoryItemKinds = loaded.collections.InventoryItemKinds as InventoryItemKind[];
        this.productCategories = loaded.collections.ProductCategories as ProductCategory[];
        this.productTypes = loaded.collections.ProductTypes as ProductType[];
        this.ownerships = loaded.collections.OwnerShips as Ownership[];
        this.manufacturers = loaded.collections.Manufacturers as Organisation[];
        this.owners = loaded.collections.Owners as Organisation[];
        this.parts = loaded.collections.Parts as Part[];

        // TODO:
        const partByGoodId = goods.reduce((map, obj) => {
          map[obj.id] = obj.Part;
          return map;
        }, {});

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

  public get hasSelection() {
    return !this.selection.isEmpty();
  }

  public get hasDeleteSelection() {
    return this.selectedGoods.filter((v) => v.CanExecuteDelete).length > 0;
  }

  public get selectedGoods() {
    return this.selection.selected.map(v => v.good);
  }

  public isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  public masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  public goBack(): void {
    this.location.back();
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public sort(event: Sort): void {
    this.sort$.next(event);
  }

  public page(event: PageEvent): void {
    this.pager$.next(event);
  }

  public delete(good: Good | Good[]): void {

    const { scope } = this.allors;

    const goods = good instanceof SessionObject ? [good as Good] : good instanceof Array ? good : [];
    const methods = goods.filter((v) => v.CanExecuteDelete).map((v) => v.Delete);

    if (methods.length > 0) {
      this.dialogService
        .confirm(
          methods.length === 1 ?
            { message: 'Are you sure you want to delete this good?' } :
            { message: 'Are you sure you want to delete these goods?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope.invoke(methods)
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
  }

  public addNew() {
    const dialogRef = this.dialog.open(GoodEditComponent, {
      autoFocus: false,
      disableClose: true
    });
    dialogRef.afterClosed().subscribe(result => {
      this.refresh();
    });
  }

  // public addNew(): void {
  //   const dialogRef = this.dialog.open(NewGoodDialogComponent, {
  //     data: { chosenGood: this.chosenGood },
  //     height: '300px',
  //     width: '700px',
  //   });

  //   dialogRef.afterClosed().subscribe((answer: string) => {
  //     if (answer === 'Serialised') {
  //       this.router.navigate(['/serialisedGood']);
  //     }
  //     if (answer === 'NonSerialised') {
  //       this.router.navigate(['/nonSerialisedGood']);
  //     }
  //   });
  // }
}
