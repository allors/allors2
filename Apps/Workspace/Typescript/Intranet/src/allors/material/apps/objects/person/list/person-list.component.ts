import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Like, ContainedIn, Filter } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, TestScope } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService } from '../../../..';

import { Person, Country } from '../../../../../domain';

import { ObjectService } from '../../../../../material/core/services/object';

interface Row extends TableRow {
  object: Person;
  name: string;
  email: string;
  phone: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './person-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class PersonListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'People';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
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
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        { name: 'email', sort: true },
        { name: 'phone', sort: true },
        'lastModifiedDate'
      ],
      actions: [
        overviewService.overview(),
        this.delete
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
      initialSort: 'name'
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.Person.FirstName, parameter: 'firstName' }),
      new Like({ roleType: m.Person.LastName, parameter: 'lastName' }),
      new ContainedIn({
        propertyType: m.Party.GeneralCorrespondence,
        extent: new Filter({
          objectType: m.PostalAddress,
          predicate: new ContainedIn({
            propertyType: m.PostalAddress.Country,
            parameter: 'country'
          })
        })
      })
    ]);

    const countrySearch = new SearchFactory({
      objectType: m.Country,
      roleTypes: [m.Country.Name],
    });

    this.filterService.init(predicate, { country: { search: countrySearch, display: (v: Country) => v && v.Name } });

    const sorter = new Sorter(
      {
        name: [m.Person.FirstName, m.Person.LastName],
        lastModifiedDate: m.Person.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
          ];
        }, [, , , ,]),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.Person({
              predicate,
              sort: sorter.create(sort),
              include: {
                Salutation: x,
                Picture: x,
                GeneralPhoneNumber: x,
                GeneralEmail: x,
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
        const people = loaded.collections.People as Person[];
        this.table.total = loaded.values.People_total;
        this.table.data = people.map((v) => {
          return {
            object: v,
            name: v.displayName,
            email: v.displayEmail,
            phone: v.displayPhone,
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
