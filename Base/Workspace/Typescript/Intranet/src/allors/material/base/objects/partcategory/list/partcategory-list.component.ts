import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Equals, Like, ContainedIn, Filter, Contains, Exists, Not } from '../../../../../framework';
import { MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, InternalOrganisationId, TestScope, FilterBuilder } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, EditService } from '../../../..';

import { PartCategory } from '../../../../../domain';

interface Row extends TableRow {
  object: PartCategory;
  name: string;
  primaryParent: string;
  secondaryParents: string;
  scope: string;
}

@Component({
  templateUrl: './partcategory-list.component.html',
  providers: [ContextService]
})
export class PartCategoryListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Part Categories';

  table: Table<Row>;

  edit: Action;
  delete: Action;

  private subscription: Subscription;
  filterBuilder: FilterBuilder;

  constructor(
    @Self() public allors: ContextService,
    
    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public editService: EditService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private internalOrganisationId: InternalOrganisationId,
    titleService: Title,
  ) {
    super();

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
        { name: 'primaryParent', sort: true },
        { name: 'secondaryParents', sort: true },
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

    const predicate = new And([
      new Like({ roleType: m.PartCategory.Name, parameter: 'name' }),
    ]);

    this.filterBuilder = new FilterBuilder(predicate);

    const sorter = new Sorter(
      {
        name: m.PartCategory.Name,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterBuilder.filterFields$, this.table.sort$, this.table.pager$, this.internalOrganisationId.observable$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
            internalOrganisationId
          ];
        }, [, , , , , ]),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          const pulls = [
            pull.PartCategory({
              predicate,
              sort: sorter.create(sort),
              include: {
                CategoryImage: x,
                LocalisedNames: x,
                LocalisedDescriptions: x,
                PrimaryParent: {
                  PrimaryAncestors: x,
                },
                SecondaryParents: {
                  PrimaryAncestors: x,
                }
              },
              parameters: this.filterBuilder.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const objects = loaded.collections.PartCategories as PartCategory[];
        this.table.total = loaded.values.PartCategories_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            name: v.Name,
            primaryParent: v.PrimaryParent && v.PrimaryParent.displayName,
            secondaryParents: v.SecondaryParents.map(w => w.displayName).join(', ')
          } as Row;
        });
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
