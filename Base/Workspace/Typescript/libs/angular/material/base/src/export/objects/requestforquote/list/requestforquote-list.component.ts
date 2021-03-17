import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService, UserId } from '@allors/angular/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter } from '@allors/angular/material/core';
import { Person, Organisation, Request, RequestState, Party } from '@allors/domain/generated';
import { And, Equals } from '@allors/data/system';
import { InternalOrganisationId, FetcherService } from '@allors/angular/base';
import { format, formatDistance } from 'date-fns';

interface Row extends TableRow {
  object: Request;
  number: string;
  from: string;
  state: string;
  description: string;
  responseRequired: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './requestforquote-list.component.html',
  providers: [ContextService],
})
export class RequestForQuoteListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Requests';

  delete: Action;

  table: Table<Row>;

  user: Person;
  internalOrganisation: Organisation;
  canCreate: boolean;

  private subscription: Subscription;
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,

    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private internalOrganisationId: InternalOrganisationId,
    private userId: UserId,
    private fetcher: FetcherService,
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
      columns: [
        { name: 'number', sort: true },
        { name: 'from' },
        { name: 'state' },
        { name: 'description', sort: true },
        { name: 'responseRequired', sort: true },
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
    this.filter = m.RequestForQuote.filter = m.RequestForQuote.filter ?? new Filter(m.RequestForQuote.filterDefinition);

    const internalOrganisationPredicate = new Equals({ propertyType: m.Request.Recipient });
    const predicate = new And([
      internalOrganisationPredicate,
      this.filter.definition.predicate
    ]);

    this.subscription = combineLatest([
      this.refreshService.refresh$,
      this.filter.fields$,
      this.table.sort$,
      this.table.pager$,
      this.internalOrganisationId.observable$]
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
          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Person({
              object: this.userId.value,
            }),
            pull.Request({
              predicate,
              sort: sort ? m.RequestForQuote.sorter.create(sort) : null,
              include: {
                Originator: x,
                RequestState: x,
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
        this.user = loaded.objects.Person as Person;

        this.canCreate = this.internalOrganisation.CanExecuteCreateRequest;

        const requests = loaded.collections.Requests as Request[];
        this.table.total = loaded.values.Requests_total;
        this.table.data = requests
          .filter((v) => v.CanReadRequestNumber)
          .map((v) => {
            return {
              object: v,
              number: `${v.RequestNumber}`,
              from: v.Originator && v.Originator.displayName,
              state: `${v.RequestState && v.RequestState.Name}`,
              description: `${v.Description || ''}`,
              responseRequired: v.RequiredResponseDate && format(new Date(v.RequiredResponseDate), 'dd-MM-yyyy'),
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
