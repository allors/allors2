import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Like, Equals, Contains, ContainedIn, Filter } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, StateService } from '../../../..';

import { Part, GoodIdentificationType, IGoodIdentification, Facility, Organisation, Brand, Model, InventoryItemKind, ProductType } from '../../../../../domain';

import { ObjectService } from '../../../../../material/base/services/object';

interface Row extends TableRow {
  object: Part;
  name: string;
  partNo: string;
  type: string;
  qoh: number;
  brand: string;
  model: string;
  kind: string;
}

@Component({
  templateUrl: './part-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class PartListComponent implements OnInit, OnDestroy {

  public title = 'Parts';

  table: Table<Row>;

  edit: Action;
  delete: Action;

  private subscription: Subscription;
  goodIdentificationTypes: GoodIdentificationType[];

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
    private errorService: ErrorService,
    private stateService: StateService,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        { name: 'partNo' },
        { name: 'type' },
        { name: 'qoh' },
        { name: 'brand' },
        { name: 'model' },
        { name: 'kind' }
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

    const internalOrganisationPredicate = new Equals({ propertyType: m.Part.InternalOrganisation });

    const predicate = new And([
      internalOrganisationPredicate,
      new Like({ roleType: m.Part.Name, parameter: 'name' }),
      new Like({ roleType: m.Part.Keywords, parameter: 'keyword' }),
      new Like({ roleType: m.Part.HsCode, parameter: 'hsCode' }),
      new Contains({ propertyType: m.Part.GoodIdentifications, parameter: 'identification' }),
      new Contains({ propertyType: m.Part.SuppliedBy, parameter: 'supplier' }),
      new Equals({ propertyType: m.Part.ManufacturedBy, parameter: 'manufacturer' }),
      new Equals({ propertyType: m.Part.Brand, parameter: 'brand' }),
      new Equals({ propertyType: m.Part.Model, parameter: 'model' }),
      new Equals({ propertyType: m.Part.InventoryItemKind, parameter: 'kind' }),
      new Equals({ propertyType: m.Part.ProductType, parameter: 'type' }),
      new ContainedIn({
        propertyType: m.Part.InventoryItemsWherePart,
        extent: new Filter({
          objectType: m.InventoryItem,
          predicate: new Equals({
            propertyType: m.InventoryItem.Facility,
            parameter: 'facility'
          })
        })
      })
    ]);

    const typeSearch = new SearchFactory({
      objectType: m.ProductType,
      roleTypes: [m.ProductType.Name],
    });

    const kindSearch = new SearchFactory({
      objectType: m.InventoryItemKind,
      predicates: [new Equals ({ propertyType: m.Enumeration.IsActive, value: true })],
      roleTypes: [m.InventoryItemKind.Name],
    });

    const brandSearch = new SearchFactory({
      objectType: m.Brand,
      roleTypes: [m.Brand.Name],
    });

    const modelSearch = new SearchFactory({
      objectType: m.Model,
      roleTypes: [m.Model.Name],
    });

    const manufacturerSearch = new SearchFactory({
      objectType: m.Organisation,
      predicates: [new Equals ({ propertyType: m.Organisation.IsManufacturer, value: true })],
      roleTypes: [m.Organisation.PartyName],
    });

    const idSearch = new SearchFactory({
      objectType: m.IGoodIdentification,
      roleTypes: [m.IGoodIdentification.Identification],
    });

    const facilitySearch = new SearchFactory({
      objectType: m.Facility,
      roleTypes: [m.Facility.Name],
    });

    this.filterService.init(predicate,
      {
        supplier: { search: this.stateService.suppliersFilter, display: (v: Organisation) => v.PartyName },
        manufacturer: { search: manufacturerSearch, display: (v: Organisation) => v.PartyName },
        brand: { search: brandSearch, display: (v: Brand) => v.Name },
        model: { search: modelSearch, display: (v: Model) => v.Name },
        kind: { search: kindSearch, display: (v: InventoryItemKind) => v.Name },
        type: { search: typeSearch, display: (v: ProductType) => v.Name },
        identification: { search: idSearch, display: (v: IGoodIdentification) => v.Identification },
        facility: { search: facilitySearch, display: (v: Facility) => v.Name },
      });

    const sorter = new Sorter(
      {
        name: m.Part.Name,
        // partNo: m.PartNumber.Identification,
        // type: m.ProductType.Name,
        // brand: m.Brand.Name,
        // model: m.Model.Name,
        // kind: m.InventoryItemKind.Name
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent, internalOrganisationId]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
            internalOrganisationId
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          internalOrganisationPredicate.object = internalOrganisationId;

          const pulls = [
            pull.Part({
              predicate,
              sort: sorter.create(sort),
              include: {
                Brand: x,
                Model: x,
                ProductType: x,
                PrimaryPhoto: x,
                InventoryItemKind: x,
                GoodIdentifications: {
                  GoodIdentificationType: x
                },
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
            pull.GoodIdentificationType(),
            pull.BasePrice(),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const parts = loaded.collections.Parts as Part[];
        this.goodIdentificationTypes = loaded.collections.GoodIdentificationTypes as GoodIdentificationType[];
        const partNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === '5735191a-cdc4-4563-96ef-dddc7b969ca6');

        const partNumberByPart = parts.reduce((map, obj) => {
          map[obj.id] = obj.GoodIdentifications.filter(v => v.GoodIdentificationType === partNumberType).map(w => w.Identification);
          return map;
        }, {});

        this.table.total = loaded.values.Parts_total;

        this.table.data = parts.map((v) => {
          return {
            object: v,
            name: v.Name,
            partNo: partNumberByPart[v.id][0],
            qoh: v.QuantityOnHand,
            type: v.ProductType ? v.ProductType.Name : '',
            brand: v.Brand ? v.Brand.Name : '',
            model: v.Model ? v.Model.Name : '',
            kind: v.InventoryItemKind.Name,
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
