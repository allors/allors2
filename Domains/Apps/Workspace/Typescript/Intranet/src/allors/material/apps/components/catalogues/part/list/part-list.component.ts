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

import { Part, ProductType, Brand, Model, GoodIdentificationType } from '../../../../../../domain';
import { Fetcher } from '../../../Fetcher';
import { stringify } from '@angular/core/src/render3/util';
import { MetaDomain } from 'src/allors/meta';

interface Row {
  part: Part;
  name: string;
  partNumber: string;
  productType: string;
  qoh: number;
  brand: string;
  model: string;
}

@Component({
  templateUrl: './part-list.component.html',
  providers: [Allors, AllorsFilterService]
})
export class PartListComponent implements OnInit, OnDestroy {

  public title = 'Parts';

  public displayedColumns = ['select', 'name', 'part No.', 'product type', 'qoh', 'brand', 'model', 'menu'];
  public selection = new SelectionModel<Row>(true, []);

  public total: number;
  public dataSource = new MatTableDataSource<Row>();

  private sort$: BehaviorSubject<Sort>;
  private refresh$: BehaviorSubject<Date>;
  private pager$: BehaviorSubject<PageEvent>;

  private subscription: Subscription;
  private readonly fetcher: Fetcher;
  goodIdentificationTypes: GoodIdentificationType[];

  public m: MetaDomain;

  constructor(
    @Self() public allors: Allors,
    @Self() private filterService: AllorsFilterService,
    public navigationService: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    private location: Location,
    private titleService: Title,
    private stateService: StateService) {

    this.titleService.setTitle(this.title);

    this.m = this.allors.m;

    this.sort$ = new BehaviorSubject<Sort>(undefined);
    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, allors.pull);
    this.pager$ = new BehaviorSubject<PageEvent>(Object.assign(new PageEvent(), { pageIndex: 0, pageSize: 50 }));
  }

  ngOnInit(): void {
    const { m, pull, scope } = this.allors;

    const predicate = new And([
      new Like({ roleType: m.Part.Name, parameter: 'Name' }),
      // new ContainedIn({
      //   propertyType: m.Part.GoodIdentifications,
      //   extent: new Filter({
      //     objectType: m.GoodIdentification,
      //     predicate: new Like({ roleType: m.GoodIdentification.Identification, parameter: 'PartNo' })
      //   })
      // }),
      // new Contains({ propertyType: m.Part.SuppliedBy, parameter: 'Supplier' }),
      // new Like({ roleType: m.Part.ProductType, parameter: 'ProductType' }),
      // new Like({ roleType: m.Part.Brand, parameter: 'Brand' }),
      // new Like({ roleType: m.Part.Model, parameter: 'Model' }),
      // new Like({ roleType: m.Part.InventoryItemKind, parameter: 'InventoryItemKind' }),
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        name: m.Part.Name
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
            pull.Part({
              predicate,
              sort: sorter.create(sort),
              include: {
                Brand: x,
                Model: x,
                ProductType: x,
                PrimaryPhoto: x,
                GoodIdentifications: {
                  GoodIdentificationType: x
                },
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
            pull.GoodIdentificationType(),
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        scope.session.reset();
        this.total = loaded.values.Goods_total;

        const parts = loaded.collections.Parts as Part[];
        this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];
        const partNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === '5735191a-cdc4-4563-96ef-dddc7b969ca6');

        const partNumberByPart = parts.reduce((map, obj) => {
          map[obj.id] = obj.GoodIdentifications.filter(v => v.GoodIdentificationType === partNumberType).map(w => w.Identification);
          return map;
        }, {});

        this.dataSource.data = parts.map((v) => {
          return {
            part: v,
            partNumber: partNumberByPart[v.id][0],
            name: v.Name,
            qoh: v.QuantityOnHand,
            productType: v.ProductType ? v.ProductType.Name : '',
            brand: v.Brand ? v.Brand.Name : '',
            model: v.Model ? v.Model.Name : '',
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
    return this.selectedParts.filter((v) => v.CanExecuteDelete).length > 0;
  }

  public get selectedParts() {
    return this.selection.selected.map(v => v.part);
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

  public delete(part: Part | Part[]): void {

    const { scope } = this.allors;

    const parts = part instanceof SessionObject ? [part as Part] : part instanceof Array ? part : [];
    const methods = parts.filter((v) => v.CanExecuteDelete).map((v) => v.Delete);

    if (methods.length > 0) {
      this.dialogService
        .confirm(
          methods.length === 1 ?
            { message: 'Are you sure you want to delete this part?' } :
            { message: 'Are you sure you want to delete these parts?' })
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
}
