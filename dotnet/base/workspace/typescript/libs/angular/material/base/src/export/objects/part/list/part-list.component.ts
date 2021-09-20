import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { ObjectService } from '@allors/angular/material/services/core';
import { Filter, TestScope, Action } from '@allors/angular/core';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, OverviewService, DeleteService } from '@allors/angular/material/core';
import {
  Part,
  ProductIdentificationType,
} from '@allors/domain/generated';

interface Row extends TableRow {
  object: Part;
  name: string;
  partNo: string;
  type: string;
  qoh: string;
  brand: string;
  model: string;
  kind: string;
}

@Component({
  templateUrl: './part-list.component.html',
  providers: [ContextService],
})
export class PartListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Parts';

  table: Table<Row>;

  edit: Action;
  delete: Action;

  private subscription: Subscription;
  goodIdentificationTypes: ProductIdentificationType[];
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,

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
        { name: 'kind' },
      ],
      actions: [overviewService.overview(), this.delete],
      defaultAction: overviewService.overview(),
      pageSize: 50,
    });
  }

  ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.Part.filter = m.Part.filter ?? new Filter(m.Part.filterDefinition);

    this.subscription = combineLatest([this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$])
      .pipe(
        scan(
          ([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
            pageEvent =
            previousRefresh !== refresh || filterFields !== previousFilterFields
              ? {
                  ...pageEvent,
                  pageIndex: 0,
                }
              : pageEvent;

          if (pageEvent.pageIndex === 0) {
            this.table.pageIndex = 0;
          }

          return [refresh, filterFields, sort, pageEvent];
        }),
        switchMap(([, filterFields, sort, pageEvent]) => {
          const pulls = [
            pull.Part({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.Part.sorter.create(sort) : null,
              include: {
                Brand: x,
                Model: x,
                ProductType: x,
                PrimaryPhoto: x,
                InventoryItemKind: x,
                ProductIdentifications: {
                  ProductIdentificationType: x,
                },
              },
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
            pull.ProductIdentificationType(),
            pull.BasePrice(),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        const parts = loaded.collections.Parts as Part[];
        this.goodIdentificationTypes = loaded.collections.ProductIdentificationTypes as ProductIdentificationType[];
        const partNumberType = this.goodIdentificationTypes.find((v) => v.UniqueId === '5735191a-cdc4-4563-96ef-dddc7b969ca6');

        const partNumberByPart = parts.reduce((map, obj) => {
          map[obj.id] = obj.ProductIdentifications.filter((v) => v.ProductIdentificationType === partNumberType).map(
            (w) => w.Identification
          );
          return map;
        }, {});

        this.table.total = loaded.values.NonUnifiedParts_total;

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
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
