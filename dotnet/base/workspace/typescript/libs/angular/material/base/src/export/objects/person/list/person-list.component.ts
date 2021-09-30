import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { formatDistance } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { ObjectService } from '@allors/angular/material/services/core';
import { Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService } from '@allors/angular/material/core';
import { Person } from '@allors/domain/generated';

interface Row extends TableRow {
  object: Person;
  name: string;
  email: string;
  phone: string;
  isCustomer: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './person-list.component.html',
  providers: [ContextService],
})
export class PersonListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'People';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,

    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [{ name: 'name', sort: true }, { name: 'email' }, { name: 'phone' }, 'isCustomer', { name: 'lastModifiedDate', sort: true }],
      actions: [overviewService.overview(), this.delete],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'name',
    });
  }

  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.Person.filter = m.Person.filter ?? new Filter(m.Person.filterDefinition);

    this.subscription = combineLatest([this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$])
      .pipe(
        scan(
          ([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
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

          return [refresh, filterFields, sort, pageEvent];
        }),
        switchMap(([, filterFields, sort, pageEvent]) => {
          const pulls = [
            pull.Person({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.Person.sorter.create(sort) : null,
              include: {
                Salutation: x,
                Picture: x,
                PartyContactMechanisms: {
                  ContactMechanism: {
                    PostalAddress_Country: x,
                  },
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
        const people = loaded.collections.People as Person[];
        this.table.total = loaded.values.People_total;
        this.table.data = people.map((v) => {
          return {
            object: v,
            name: v.displayName,
            email: v.displayEmail,
            phone: v.displayPhone,
            isCustomer: v.CustomerRelationshipsWhereCustomer.length > 0 ? 'Yes' : 'No',
            lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date()),
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
