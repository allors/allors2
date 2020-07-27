import { Component, OnDestroy, OnInit, ViewChild, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { combineLatest, Subscription } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment/moment';

import { PullRequest, And, Like, ContainedIn, Filter, GreaterThan, Equals } from '../../../../../framework';
import { MediaService, ContextService, NavigationService, RefreshService, Action, MetaService, SearchFactory, TestScope, FetcherService, FilterBuilder } from '../../../../../angular';
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
  isCustomer: string;
  isSupplier: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './organisation-list.component.html',
  providers: [ContextService]
})
export class OrganisationListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Organisations';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;
  internalOrganisation: Organisation;
  filterBuilder: FilterBuilder;

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
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    const { m } = this.metaService;

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
      actions: [
        overviewService.overview(),
        this.delete,
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
        propertyType: m.Organisation.SupplierRelationshipsWhereSupplier,
        extent: new Filter({
          objectType: m.SupplierRelationship,
          predicate: new Equals({
            propertyType: m.SupplierRelationship.InternalOrganisation,
            parameter: 'supplierFor'
          })
        })
      }),
      new ContainedIn({
        propertyType: m.Party.CustomerRelationshipsWhereCustomer,
        extent: new Filter({
          objectType: m.CustomerRelationship,
          predicate: new Equals({
            propertyType: m.CustomerRelationship.InternalOrganisation,
            parameter: 'customerAt'
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
      }),
    ]);

    const countrySearch = new SearchFactory({
      objectType: m.Country,
      roleTypes: [m.Country.Name],
    });

    const internalOrganisationSearch = new SearchFactory({
      objectType: m.Organisation,
      roleTypes: [m.Organisation.Name],
      post: (predicate: And) => {
        predicate.operands.push(new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }));
      },
    });

    this.filterBuilder = new FilterBuilder(predicate, {
      customerAt: {
        search: internalOrganisationSearch,
        initialValue: this.internalOrganisation,
        display: (v: Organisation) => v && v.Name
      },
      supplierFor: {
        search: internalOrganisationSearch,
        initialValue: this.internalOrganisation,
        display: (v: Organisation) => v && v.Name
      },
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

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterBuilder.filterFields$, this.table.sort$, this.table.pager$)
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
            this.fetcher.internalOrganisation,
            pull.Organisation({
              predicate,
              sort: sorter.create(sort),
              include: {
                CustomerRelationshipsWhereCustomer: x,
                SupplierRelationshipsWhereSupplier: x,
                PartyContactMechanisms: {
                  ContactMechanism: {
                    PostalAddress_Country: x
                  }
                },
              },
              parameters: this.filterBuilder.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

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
