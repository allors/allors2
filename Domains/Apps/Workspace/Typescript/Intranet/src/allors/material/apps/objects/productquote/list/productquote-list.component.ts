import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Equals } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, SessionService, NavigationService, Action, RefreshService, MetaService } from '../../../../../angular';
import { Sorter, TableRow, Table, NavigateService, DeleteService, StateService } from '../../../..';

import { Quote } from '../../../../../domain';

interface Row extends TableRow {
  object: Quote;
  number: string;
  receiver: string;
  state: string;
  responseRequired: string;
  description: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './productquote-list.component.html',
  providers: [SessionService, AllorsFilterService]
})
export class ProductQuotesOverviewComponent implements OnInit, OnDestroy {

  public title = 'Quotes';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: SessionService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigateService: NavigateService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private stateService: StateService,
    titleService: Title,
  ) {
    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number' },
        { name: 'originator' },
        { name: 'state' },
        { name: 'description' },
        { name: 'responseRequired' },
        'lastModifiedDate'
      ],
      actions: [
        navigateService.overview(),
        this.delete
      ],
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.Quote.Issuer });
    const predicate = new And([
      // new Like({ roleType: m.Person.FirstName, parameter: 'firstName' }),
      internalOrganisationPredicate
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        lastModifiedDate: m.Quote.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          internalOrganisationPredicate.value = internalOrganisationId;

          const pulls = [
            pull.Quote({
              predicate,
              sort: sorter.create(sort),
              include: {
                Receiver: x,
                QuoteState: x,
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();
        const requests = loaded.collections.Quotes as Quote[];
        this.table.total = loaded.values.Requests_total;
        this.table.data = requests.map((v) => {
          return {
            object: v,
            number: `${v.QuoteNumber}`,
            receiver: v.Receiver && v.Receiver.displayName,
            state: `${v.QuoteState && v.QuoteState.Name}`,
            description: `${v.Description || ''}`,
            responseRequired: v.RequiredResponseDate && moment(v.RequiredResponseDate).format('MMM Do YY'),
            lastModifiedDate: moment(v.LastModifiedDate).fromNow()
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
