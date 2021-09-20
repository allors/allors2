import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { formatDistance } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter, MethodService } from '@allors/angular/material/core';
import { Party, Shipment, ShipmentState } from '@allors/domain/generated';
import { And, Equals, ContainedIn, Extent, Or } from '@allors/data/system';
import { InternalOrganisationId, PrintService } from '@allors/angular/base';
import { Meta } from '@allors/meta/generated';

interface Row extends TableRow {
  object: Shipment;
  number: string;
  from: string;
  to: string;
  state: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './shipment-list.component.html',
  providers: [ContextService],
})
export class ShipmentListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Shipments';

  m: Meta;

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,

    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public printService: PrintService,
    public methodService: MethodService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private internalOrganisationId: InternalOrganisationId,
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.m = this.metaService.m;

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort },
        { name: 'from', sort },
        { name: 'to', sort },
        { name: 'state', sort },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [overviewService.overview(), this.delete],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'number',
      initialSortDirection: 'desc',
    });
  }

  ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.Shipment.filter = m.Shipment.filter ?? new Filter(m.Shipment.filterDefinition);

    const fromInternalOrganisationPredicate = new Equals({ propertyType: m.Shipment.ShipFromParty });
    const toInternalOrganisationPredicate = new Equals({ propertyType: m.Shipment.ShipToParty });

    const predicate = new And([
      new Or([fromInternalOrganisationPredicate, toInternalOrganisationPredicate]),
      this.filter.definition.predicate
    ]);


    const sorter = new Sorter({
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
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {
          fromInternalOrganisationPredicate.object = internalOrganisationId;
          toInternalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            pull.Shipment({
              predicate,
              sort: sort ? m.Shipment.sorter.create(sort) : null,
              include: {
                ShipToParty: x,
                ShipFromParty: x,
                ShipmentState: x,
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
        const objects = loaded.collections.Shipments as Shipment[];
        this.table.total = loaded.values.Shipments_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            number: `${v.ShipmentNumber}`,
            from: v.ShipFromParty.displayName,
            to: v.ShipToParty.displayName,
            state: `${v.ShipmentState && v.ShipmentState.Name}`,
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
