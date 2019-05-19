import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Meta } from '../../../../../meta';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Equals, Filter, ContainedIn } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, InternalOrganisationId, TestScope } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, PrintService } from '../../../..';

import { Shipment, Party, ShipmentState } from '../../../../../domain';
import { MethodService } from '../../../../../material/base/services/actions';

interface Row extends TableRow {
  object: Shipment;
  number: string;
  from: string;
  to: string;
  state: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './shipment-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class ShipmentListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Shipments';

  m: Meta;

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public printService: PrintService,
    public methodService: MethodService,
    public deleteService: DeleteService,
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

    this.m = this.metaService.m;

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort },
        { name: 'from', sort },
        { name: 'to', sort },
        { name: 'state', sort },
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

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const internalOrganisationPredicate = new Equals({ propertyType: m.Shipment.ShipFromParty });
    const supplierPredicate = new Equals({ propertyType: m.SupplierRelationship.InternalOrganisation });

    const predicate = new And([
      internalOrganisationPredicate,
      new Equals({ propertyType: m.Shipment.ShipmentNumber, parameter: 'number' }),
      new Equals({ propertyType: m.Shipment.ShipmentState, parameter: 'state' }),
      new Equals({ propertyType: m.Shipment.ShipToParty, parameter: 'supplier' }),
      new ContainedIn({
        propertyType: m.Shipment.ShipmentItems,
        extent: new Filter({
          objectType: m.ShipmentItem,
          predicate: new ContainedIn({
            propertyType: m.ShipmentItem.Part,
            parameter: 'part'
          })
        })
      }),
    ]);

    const stateSearch = new SearchFactory({
      objectType: m.ShipmentState,
      roleTypes: [m.ShipmentState.Name],
    });

    const supplierSearch = new SearchFactory({
      objectType: m.Organisation,
      predicates: [
        new ContainedIn({
          propertyType: m.Organisation.SupplierRelationshipsWhereSupplier,
          extent: new Filter({
            objectType: m.SupplierRelationship,
            predicate: supplierPredicate,
          })
        })],
      roleTypes: [m.Organisation.PartyName],
    });

    this.filterService.init(predicate, {
      active: { initialValue: true },
      state: { search: stateSearch, display: (v: ShipmentState) => v && v.Name },
      supplier: { search: supplierSearch, display: (v: Party) => v && v.PartyName },
    });

    const sorter = new Sorter(
      {
        number: m.Shipment.ShipmentNumber,
        from: m.Shipment.ShipFromParty,
        to: m.Shipment.ShipToParty,
        lastModifiedDate: m.Shipment.LastModifiedDate,
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
        }, [ , , , , , ]),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          internalOrganisationPredicate.object = internalOrganisationId;
          supplierPredicate.object = internalOrganisationId;

          const pulls = [
            pull.Shipment({
              predicate,
              sort: sorter.create(sort),
              include: {
                ShipToParty: x,
                ShipFromParty: x,
                ShipmentState: x,
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
        const objects = loaded.collections.Shipments as Shipment[];
        this.table.total = loaded.values.Shipments_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            number: `${v.ShipmentNumber}`,
            from: v.ShipFromParty.displayName,
            to: v.ShipToParty.displayName,
            state: `${v.ShipmentState && v.ShipmentState.Name}`,
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
