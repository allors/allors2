import { Component, OnDestroy, OnInit, ViewChild, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { combineLatest, Subscription } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Like, ContainedIn, Filter } from '../../../../../framework';
import { AllorsFilterService,  MediaService, ContextService, NavigationService, RefreshService, Action, MetaService, SearchFactory } from '../../../../../angular';
import { TableRow, OverviewService, DeleteService, Table, Sorter, MethodService } from '../../../..';

import { Organisation, Country } from '../../../../../domain';

import { ObjectService } from '../../../../../material/base/services/object';

interface Row extends TableRow {
  object: Organisation;
  name: string;
  street: string;
  locality: string;
  country: string;
  phone: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './organisation-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class OrganisationListComponent implements OnInit, OnDestroy {

  public title = 'Organisations';

  table: Table<Row>;

  delete: Action;
  delete2: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public methodService: MethodService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    
    titleService: Title) {

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    const { m } = this.metaService;

    this.delete2 = methodService.create(allors.context, m.Organisation.Delete, { name: 'Delete (Method)' });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        { name: 'street', sort: true },
        { name: 'locality', sort: true },
        { name: 'country', sort: true },
        { name: 'phone', sort: true },
        'lastModifiedDate'
      ],
      actions: [
        overviewService.overview(),
        this.delete,
        this.delete2,
      ],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.Organisation.Name, parameter: 'name' }),
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

    this.filterService.init(predicate, {
      country: {
        search: countrySearch,
        display: (v: Country) => v.Name
      }
    });

    const sorter = new Sorter(
      {
        name: m.Organisation.Name,
        lastModifiedDate: m.Organisation.LastModifiedDate,
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
        }, []),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.Organisation({
              predicate,
              sort: sorter.create(sort),
              include: {
                GeneralCorrespondence: {
                  PostalBoundary: {
                    Country: x
                  }
                },
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
