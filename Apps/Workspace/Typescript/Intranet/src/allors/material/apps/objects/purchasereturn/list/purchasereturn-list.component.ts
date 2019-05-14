import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Meta } from '../../../../../meta';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Equals, Filter, ContainedIn } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, InternalOrganisationId, TestScope } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, PrintService } from '../../../..';

import { PurchaseReturn, Party, PurchaseReturnState, Product, SerialisedItem } from '../../../../../domain';
import { MethodService } from '../../../../../material/base/services/actions';

interface Row extends TableRow {
  object: PurchaseReturn;
  number: string;
  supplier: string;
  state: string;
  customerReference: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './purchasereturn-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class PurchaseReturnListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Purchase Returns';

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

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort: true },
        { name: 'supplier' },
        { name: 'state' },
        { name: 'customerReference', sort: true },
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

    const internalOrganisationPredicate = new Equals({ propertyType: m.PurchaseReturn.ShipFromParty });
    const supplierPredicate = new Equals({ propertyType: m.SupplierRelationship.InternalOrganisation });

    const predicate = new And([
      internalOrganisationPredicate,
      new Equals({ propertyType: m.PurchaseReturn.ShipmentNumber, parameter: 'number' }),
      new Equals({ propertyType: m.PurchaseReturn.PurchaseReturnState, parameter: 'state' }),
      new Equals({ propertyType: m.PurchaseReturn.ShipToParty, parameter: 'supplier' }),
      new ContainedIn({
        propertyType: m.PurchaseReturn.ShipmentItems,
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
      objectType: m.PurchaseReturnState,
      roleTypes: [m.PurchaseReturnState.Name],
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
      state: { search: stateSearch, display: (v: PurchaseReturnState) => v && v.Name },
      supplier: { search: supplierSearch, display: (v: Party) => v && v.PartyName },
    });

    const sorter = new Sorter(
      {
        number: m.PurchaseReturn.ShipmentNumber,
        lastModifiedDate: m.PurchaseReturn.LastModifiedDate,
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
            pull.PurchaseReturn({
              predicate,
              sort: sorter.create(sort),
              include: {
                ShipToParty: x,
                PurchaseReturnState: x,
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
        const objects = loaded.collections.PurchaseReturns as PurchaseReturn[];
        this.table.total = loaded.values.PurchaseOrders_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            number: `${v.ShipmentNumber}`,
            supplier: v.ShipToParty.displayName,
            state: `${v.PurchaseReturnState && v.PurchaseReturnState.Name}`,
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
