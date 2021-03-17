import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, EditService } from '@allors/angular/material/core';
import { PartCategory } from '@allors/domain/generated';
import { And, Like } from '@allors/data/system';
import { InternalOrganisationId } from '@allors/angular/base';

interface Row extends TableRow {
  object: PartCategory;
  name: string;
  primaryParent: string;
  secondaryParents: string;
  scope: string;
}

@Component({
  templateUrl: './partcategory-list.component.html',
  providers: [ContextService],
})
export class PartCategoryListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Part Categories';

  table: Table<Row>;

  edit: Action;
  delete: Action;

  private subscription: Subscription;
  filter: Filter;

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
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.edit = editService.edit();
    this.edit.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        { name: 'primaryParent', sort: true },
        { name: 'secondaryParents', sort: true },
      ],
      actions: [this.edit, this.delete],
      defaultAction: this.edit,
      pageSize: 50,
    });
  }

  ngOnInit(): void {
    const { m, pull, x } = this.metaService;

    const predicate = new And([new Like({ roleType: m.PartCategory.Name, parameter: 'name' })]);

    const filterDefinition = new FilterDefinition(predicate);
    this.filter = new Filter(filterDefinition);

    const sorter = new Sorter({
      name: m.PartCategory.Name,
    });

    this.subscription = combineLatest(
      this.refreshService.refresh$,
      this.filter.fields$,
      this.table.sort$,
      this.table.pager$,
      this.internalOrganisationId.observable$
    )
      .pipe(
        scan(
          ([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
            pageEvent =
            previousRefresh !== refresh || filterFields !== previousFilterFields
              ? {
                  ...pageEvent,
                  pageIndex: 0,
                }
              : pageEvent;

          if (pageEvent.pageIndex === 0) {
            this.table.pageIndex = 0;
          }

          return [refresh, filterFields, sort, pageEvent, internalOrganisationId];
        }),
        switchMap(([, filterFields, sort, pageEvent]) => {
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
                },
              },
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
          ];

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
            secondaryParents: v.SecondaryParents.map((w) => w.displayName).join(', '),
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
