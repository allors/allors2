import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Equals, ContainedIn, Filter } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, InternalOrganisationId, TestScope, FetcherService, UserId } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService } from '../../../..';

import { Request, RequestState, Party, Organisation, Person, UserGroup } from '../../../../../domain';

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
  providers: [ContextService, AllorsFilterService]
})
export class RequestForQuoteListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Requests';

  delete: Action;

  table: Table<Row>;

  user: Person;
  internalOrganisation: Organisation;
  canCreate: boolean;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private internalOrganisationId: InternalOrganisationId,
    private userId: UserId,
    private fetcher: FetcherService,
    titleService: Title,
  ) {
    super();

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
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
      actions: [
        overviewService.overview(),
        this.delete
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.Request.Recipient });
    const predicate = new And([
      internalOrganisationPredicate,
      new Equals({ propertyType: m.Request.RequestState, parameter: 'state' }),
      new Equals({ propertyType: m.Request.Originator, parameter: 'from' }),
    ]);

    const stateSearch = new SearchFactory({
      objectType: m.RequestState,
      roleTypes: [m.RequestState.Name],
    });

    const originatorSearch = new SearchFactory({
      objectType: m.Party,
      roleTypes: [m.Party.PartyName],
    });


    this.filterService.init(predicate, {
      active: { initialValue: true },
      state: { search: stateSearch, display: (v: RequestState) => v && v.Name },
      from: { search: originatorSearch, display: (v: Party) => v && v.PartyName },
    });

    const sorter = new Sorter(
      {
        number: m.Request.RequestNumber,
        description: m.Request.Description,
        responseRequired: m.Request.RequiredResponseDate,
        lastModifiedDate: m.Request.LastModifiedDate,
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
            this.fetcher.internalOrganisation,
            pull.Person({
              object: this.userId.value
            }),
            pull.Request({
              predicate,
              sort: sorter.create(sort),
              include: {
                Originator: x,
                RequestState: x,
              },
              parameters: this.filterService.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

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
        this.table.data = requests.filter(v => v.CanReadRequestNumber).map((v) => {
          return {
            object: v,
            number: `${v.RequestNumber}`,
            from: v.Originator && v.Originator.displayName,
            state: `${v.RequestState && v.RequestState.Name}`,
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
