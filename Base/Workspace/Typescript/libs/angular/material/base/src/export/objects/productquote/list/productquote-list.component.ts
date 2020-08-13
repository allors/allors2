import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { format, formatDistance } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService, UserId } from '@allors/angular/services/core';
import { SearchFactory, FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService, Sorter } from '@allors/angular/material/core';
import { Person, Organisation, Quote, QuoteState, Party } from '@allors/domain/generated';
import { And, Equals } from '@allors/data/system';
import { InternalOrganisationId, FetcherService, PrintService } from '@allors/angular/base';

interface Row extends TableRow {
  object: Quote;
  number: string;
  to: string;
  state: string;
  responseRequired: string;
  description: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './productquote-list.component.html',
  providers: [ContextService],
})
export class ProductQuoteListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Quotes';

  table: Table<Row>;

  delete: Action;
  print: Action;

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
    public printService: PrintService,
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

    this.print = printService.print();

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort: true },
        { name: 'to' },
        { name: 'state' },
        { name: 'description', sort: true },
        { name: 'responseRequired', sort: true },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [overviewService.overview(), this.print, this.delete],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'number',
      initialSortDirection: 'desc',
    });
  }

  ngOnInit(): void {
    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.Quote.Issuer });
    const predicate = new And([
      internalOrganisationPredicate,
      new Equals({ propertyType: m.ProductQuote.QuoteState, parameter: 'state' }),
      new Equals({ propertyType: m.ProductQuote.Receiver, parameter: 'to' }),
    ]);

    const stateSearch = new SearchFactory({
      objectType: m.QuoteState,
      roleTypes: [m.QuoteState.Name],
    });

    const receiverSearch = new SearchFactory({
      objectType: m.Party,
      roleTypes: [m.Party.PartyName],
    });

    const filterDefinition = new FilterDefinition(predicate, {
      active: { initialValue: true },
      state: { search: stateSearch, display: (v: QuoteState) => v && v.Name },
      to: { search: receiverSearch, display: (v: Party) => v && v.PartyName },
    });
    this.filter = new Filter(filterDefinition);

    const sorter = new Sorter({
      number: m.Quote.SortableQuoteNumber,
      description: m.Quote.Description,
      responseRequired: m.Quote.RequiredResponseDate,
      lastModifiedDate: m.Quote.LastModifiedDate,
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
            return [
              refresh,
              filterFields,
              sort,
              previousRefresh !== refresh || filterFields !== previousFilterFields ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
              internalOrganisationId,
            ];
          },
          [, , , , ,]
        ),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {
          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.Person({
              object: this.userId.value,
            }),
            pull.Quote({
              predicate,
              sort: sorter.create(sort),
              include: {
                PrintDocument: {
                  Media: x,
                },
                Receiver: x,
                QuoteState: x,
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

        this.canCreate = this.internalOrganisation.CanExecuteCreateQuote;

        const quotes = loaded.collections.Quotes as Quote[];
        this.table.total = loaded.values.Quotes_total;
        this.table.data = quotes
          .filter((v) => v.CanReadQuoteNumber)
          .map((v) => {
            return {
              object: v,
              number: `${v.QuoteNumber}`,
              to: v.Receiver && v.Receiver.displayName,
              state: `${v.QuoteState && v.QuoteState.Name}`,
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
