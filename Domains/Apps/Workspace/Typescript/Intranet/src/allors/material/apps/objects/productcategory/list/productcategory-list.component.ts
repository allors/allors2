import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Equals, Like, ContainedIn, Filter, Contains, Exists, Not } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, StateService, EditService } from '../../../..';

import { ProductCategory, CatScope, Good } from '../../../../../domain';

interface Row extends TableRow {
  object: ProductCategory;
  name: string;
  scope: string;
}

@Component({
  templateUrl: './productcategory-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class ProductCategoriesOverviewComponent implements OnInit, OnDestroy {

  public title = 'Categories';

  table: Table<Row>;

  edit: Action;
  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public editService: EditService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private stateService: StateService,
    titleService: Title,
  ) {
    titleService.setTitle(this.title);

    this.edit = editService.edit();
    this.edit.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        { name: 'scope', sort: true }
      ],
      actions: [
        this.edit,
        this.delete
      ],
      defaultAction: this.edit,
      pageSize: 50,
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.ProductCategory.InternalOrganisation });
    const predicate = new And([
      internalOrganisationPredicate,
      new Like({ roleType: m.ProductCategory.Name, parameter: 'name' }),
      new Equals({ propertyType: m.ProductCategory.CatScope, parameter: 'scope' }),
      new Contains({ propertyType: m.ProductCategory.Products, parameter: 'product' })
    ]);

    const scopeSearch = new SearchFactory({
      objectType: m.CatScope,
      roleTypes: [m.CatScope.Name],
    });

    const productSearch = new SearchFactory({
      objectType: m.Good,
      roleTypes: [m.Good.Name],
    });

    this.filterService.init(predicate,
      {
        scope: { search: scopeSearch, display: (v: CatScope) => v.Name },
        product: { search: productSearch, display: (v: Good) => v.Name }
      });

    const sorter = new Sorter(
      {
        name: m.Catalogue.Name,
        description: m.Catalogue.Description,
        scope: m.CatScope.Name,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
            internalOrganisationId
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            pull.ProductCategory({
              predicate,
              sort: sorter.create(sort),
              include: {
                CategoryImage: x,
                LocalisedNames: x,
                LocalisedDescriptions: x,
                CatScope: x,
                Parents: x
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const objects = loaded.collections.ProductCategories as ProductCategory[];
        this.table.total = loaded.values.ProductCategories_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            name: v.Name,
            scope: v.CatScope.Name
          } as Row;
        });
      }, this.errorService.handler);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
