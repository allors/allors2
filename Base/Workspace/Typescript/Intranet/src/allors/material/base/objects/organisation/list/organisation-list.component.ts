import { Component, OnDestroy, OnInit, ViewChild, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { combineLatest, Subscription } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Like, ContainedIn, Filter } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, RefreshService, Action, MetaService, SearchFactory, TestScope } from '../../../../../angular';
import { TableRow, OverviewService, DeleteService, Table, Sorter, MethodService } from '../../../..';

import { Organisation, Country } from '../../../../../domain';

import { ObjectService } from '../../../../../material/core/services/object';

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
export class OrganisationListComponent extends TestScope implements OnInit, OnDestroy {

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
    titleService: Title
  ) {
    super();

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
        'street',
        'locality',
        'country',
        'phone',
        { name: 'lastModifiedDate', sort: true },
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
        propertyType: m.Party.PartyContactMechanisms,
        extent: new Filter({
          objectType: m.PartyContactMechanism,
          predicate: new ContainedIn({
            propertyType: m.PartyContactMechanism.ContactMechanism,
            extent: new Filter({
              objectType: m.PostalAddress,
              predicate: new ContainedIn({
                propertyType: m.PostalAddress.Country,
                parameter: 'country'
              })
            })
          })
        })
      }),
      new ContainedIn({
        propertyType: m.Party.PartyContactMechanisms,
        extent: new Filter({
          objectType: m.PartyContactMechanism,
          predicate: new ContainedIn({
            propertyType: m.PartyContactMechanism.ContactMechanism,
            extent: new Filter({
              objectType: m.PostalAddress,
              predicate: new Like({
                roleType: m.PostalAddress.Locality,
                parameter: 'city'
              })
            })
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
        display: (v: Country) => v && v.Name
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
        }, [, , , , ]),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.Organisation({
              predicate,
              sort: sorter.create(sort),
              include: {
                PartyContactMechanisms: {
                  ContactMechanism: {
                    PostalAddress_Country: x
                  }
                },
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
