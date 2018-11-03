import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Location } from '@angular/common';
import { Title } from '@angular/platform-browser';
import { MatSnackBar, MatTableDataSource, MatDialog, PageEvent } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';

import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Like, Sort, Equals, Contains, SessionObject, ContainedIn, Filter } from '../../../../../../framework';
import { ErrorService, MediaService, x, Allors, NavigationService, Invoked } from '../../../../../../angular';
import { AllorsFilterService } from '../../../../../../angular/base/filter';
import { AllorsMaterialDialogService } from '../../../../../base/services/dialog';
import { Sorter } from '../../../../../base/sorting';
import { StateService } from '../../../../services/StateService';

import { Good, Part, ProductCategory, ProductType, Ownership, Organisation, SerialisedInventoryItemState, InternalOrganisation } from '../../../../../../domain';
import { Fetcher } from '../../../Fetcher';
import { GoodEditComponent } from '../../good/edit/good-edit.component';

interface Row {
  good: Good;
  name: string;
  qoh: number;
}

@Component({
  templateUrl: './part-list.component.html',
  providers: [Allors, AllorsFilterService]
})
export class PartListComponent implements OnInit, OnDestroy {

  public title = 'Products';

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
      new Like({ roleType: m.Good.Keywords, parameter: 'Keyword' }),
      new ContainedIn({
        propertyType: m.Good.GoodIdentifications,
        extent: new Filter({
          objectType: m.GoodIdentification,
          predicate: new Like({ roleType: m.GoodIdentification.Identification, parameter: 'Sku' })
        })
      }),
      new Contains({ propertyType: m.Good.ProductCategories, parameter: 'Category' }),
      new Contains({ propertyType: m.Good.SuppliedBy, parameter: 'Supplier' }),
      new ContainedIn({
        propertyType: m.Good.Part,
        extent: new Filter({
          objectType: m.Part,
          predicate: new Like({ roleType: m.Part.ProductType, parameter: 'ProductType' })
        })
      }),
      new ContainedIn({
        propertyType: m.Good.Part,
        extent: new Filter({
          objectType: m.Part,
          predicate: new Like({ roleType: m.Part.Brand, parameter: 'Brand' })
        })
      }),
      new ContainedIn({
        propertyType: m.Good.Part,
        extent: new Filter({
          objectType: m.Part,
          predicate: new Like({ roleType: m.Part.Model, parameter: 'Model' })
        })
      }),
      new ContainedIn({
        propertyType: m.Good.Part,
        extent: new Filter({
          objectType: m.Part,
          predicate: new Like({ roleType: m.Part.InventoryItemKind, parameter: 'InventoryItemKind' })
        })
      }),
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

          const pulls = [
            this.fetcher.internalOrganisation,
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
