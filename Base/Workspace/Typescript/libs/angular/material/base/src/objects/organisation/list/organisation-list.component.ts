import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { formatDistance } from 'date-fns';

import {
  ContextService,
  TestScope,
  MetaService,
  RefreshService,
  Action,
  NavigationService,
  MediaService,
  UserId,
  Filter,
  SearchFactory,
  FilterDefinition,
} from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, ObjectService, MethodService, OverviewService, DeleteService, Sorter } from '@allors/angular/material/core';
import { Notification, Organisation, Country } from '@allors/domain/generated';
import { FetcherService } from '@allors/angular/base';
import { And, Like, ContainedIn, Extent, Equals } from '@allors/data/system';

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
  filter: Filter;

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
        extent: new Extent({
          objectType: m.SupplierRelationship,
          predicate: new Equals({
            propertyType: m.SupplierRelationship.InternalOrganisation,
            parameter: 'supplierFor'
          })
        })
      }),
      new ContainedIn({
        propertyType: m.Party.CustomerRelationshipsWhereCustomer,
        extent: new Extent({
          objectType: m.CustomerRelationship,
          predicate: new Equals({
            propertyType: m.CustomerRelationship.InternalOrganisation,
            parameter: 'customerAt'
          })
        })
      }),
      new ContainedIn({
        propertyType: m.Party.PartyContactMechanisms,
        extent: new Extent({
          objectType: m.PartyContactMechanism,
          predicate: new ContainedIn({
            propertyType: m.PartyContactMechanism.ContactMechanism,
            extent: new Extent({
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
        extent: new Extent({
          objectType: m.PartyContactMechanism,
          predicate: new ContainedIn({
            propertyType: m.PartyContactMechanism.ContactMechanism,
            extent: new Extent({
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

    const filterDefinition = new FilterDefinition(predicate, {
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
    this.filter = new Filter(filterDefinition);
    
    const sorter = new Sorter(
      {
        name: m.Organisation.Name,
        lastModifiedDate: m.Organisation.LastModifiedDate,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$)
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
              parameters: this.filter.parameters(filterFields),
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
