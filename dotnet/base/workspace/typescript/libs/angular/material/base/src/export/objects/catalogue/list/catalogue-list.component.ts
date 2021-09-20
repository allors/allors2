import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { Catalogue, Scope } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, EditService, DeleteService, Sorter } from '@allors/angular/material/core';
import { And, Like, Equals } from '@allors/data/system';
import { InternalOrganisationId } from '@allors/angular/base';
import { TestScope, Filter, SearchFactory, FilterDefinition, Action } from '@allors/angular/core';

interface Row extends TableRow {
  object: Catalogue;
  name: string;
  description: string;
  scope: string;
}

@Component({
  templateUrl: './catalogue-list.component.html',
  providers: [ContextService]
})
export class CataloguesListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Catalogues';

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
        { name: 'description', sort: true },
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
    this.filter = m.Catalogue.filter = m.Catalogue.filter ?? new Filter(m.Catalogue.filterDefinition);

    const internalOrganisationPredicate = new Equals({ propertyType: m.Catalogue.InternalOrganisation });
    const predicate = new And([
      internalOrganisationPredicate,
      this.filter.definition.predicate
    ]);

    this.subscription = combineLatest([this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$, this.internalOrganisationId.observable$])
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
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
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            pull.Catalogue({
              predicate: predicate,
              sort: sort ? m.Catalogue.sorter.create(sort) : null,
              include: {
                CatalogueImage: x,
                ProductCategories: x,
                CatScope: x
              },
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const objects = loaded.collections.Catalogues as Catalogue[];
        this.table.total = loaded.values.Catalogues_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            name: `${v.Name}`,
            description: `${v.Description || ''}`,
            scope: v.CatScope.Name
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
