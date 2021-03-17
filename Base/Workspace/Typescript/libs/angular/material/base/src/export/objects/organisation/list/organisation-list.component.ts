import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { formatDistance } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { ObjectService } from '@allors/angular/material/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, MethodService, OverviewService, DeleteService, Sorter } from '@allors/angular/material/core';
import { Organisation, Country } from '@allors/domain/generated';
import { FetcherService } from '@allors/angular/base';
import { And, Like, ContainedIn, Extent, Equals } from '@allors/data/system';

interface Row extends TableRow {
  object: Organisation;
  name: string;
  street: string;
  locality: string;
  country: string;
  phone: string;
  isCustomer: string;
  isSupplier: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './organisation-list.component.html',
  providers: [ContextService],
})
export class OrganisationListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Organisations';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;
  internalOrganisation: Organisation;
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,

    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public methodService: MethodService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private fetcher: FetcherService,
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });


    // this.delete2 = methodService.create(allors.context, m.Organisation.Delete, { name: 'Delete (Method)' });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        'street',
        'locality',
        'country',
        'phone',
        'isCustomer',
        'isSupplier',
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [overviewService.overview(), this.delete],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.Organisation.filter = m.Organisation.filter ?? new Filter(m.Organisation.filterDefinition);

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
            this.fetcher.internalOrganisation,
            pull.Organisation({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.Organisation.sorter.create(sort) : null,
              include: {
                CustomerRelationshipsWhereCustomer: x,
                SupplierRelationshipsWhereSupplier: x,
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

        this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;
        const organisations = loaded.collections.Organisations as Organisation[];

        this.table.total = loaded.values.Organisations_total;
        this.table.data = organisations.map((v) => {
          return {
            object: v,
            name: v.displayName,
            street: v.displayAddress,
            locality: v.displayAddress2,
            country: v.displayAddress3,
            phone: v.displayPhone,
            isCustomer: v.CustomerRelationshipsWhereCustomer.length > 0 ? 'Yes' : 'No',
            isSupplier: v.SupplierRelationshipsWhereSupplier.length > 0 ? 'Yes' : 'No',
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
