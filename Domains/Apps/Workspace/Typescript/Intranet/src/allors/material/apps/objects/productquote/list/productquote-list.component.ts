import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Equals } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, InternalOrganisationId, TestScope } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, PrintService } from '../../../../../material';

import { Quote, QuoteState, Party } from '../../../../../domain';

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
  providers: [ContextService, AllorsFilterService]
})
export class ProductQuoteListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Quotes';

  table: Table<Row>;

  delete: Action;
  print: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public printService: PrintService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private internalOrganisationId: InternalOrganisationId,
    titleService: Title,
  ) {
    super();

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
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
      actions: [
        overviewService.overview(),
        this.print,
        this.delete
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
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


    this.filterService.init(predicate, {
      active: { initialValue: true },
      state: { search: stateSearch, display: (v: QuoteState) => v && v.Name },
      to: { search: receiverSearch, display: (v: Party) => v && v.PartyName },
    });

    const sorter = new Sorter(
      {
        number: m.Quote.QuoteNumber,
        description: m.Quote.Description,
        responseRequired: m.Quote.RequiredResponseDate,
        lastModifiedDate: m.Quote.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$, this.internalOrganisationId.observable$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
            internalOrganisationId
          ];
        }, [, , , , ,]),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            pull.Quote({
              predicate,
              sort: sorter.create(sort),
              include: {
                PrintDocument: {
                  Media: x
                },
                Receiver: x,
                QuoteState: x,
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
        const requests = loaded.collections.Quotes as Quote[];
        this.table.total = loaded.values.Requests_total;
        this.table.data = requests.map((v) => {
          return {
            object: v,
            number: `${v.QuoteNumber}`,
            to: v.Receiver && v.Receiver.displayName,
            state: `${v.QuoteState && v.QuoteState.Name}`,
            description: `${v.Description || ''}`,
            responseRequired: v.RequiredResponseDate && moment(v.RequiredResponseDate).format('MMM Do YY'),
            lastModifiedDate: moment(v.LastModifiedDate).fromNow()
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
