import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import * as moment from 'moment';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';

import { PullRequest, And, Equals, Like, Contains } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory } from '../../../../../angular';
import { Sorter, TableRow, Table, OverviewService, DeleteService, StateService, EditService } from '../../../..';

import { PositionTypeRate, PositionType, RateType } from '../../../../../domain';

interface Row extends TableRow {
  object: PositionTypeRate;
  positionType: string;
  rateType: string;
  from: string;
  through: string;
  rate: number;
  frequency: string;
}

@Component({
  templateUrl: './positiontyperate-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class PositionTypeRatesOverviewComponent implements OnInit, OnDestroy {

  public title = 'Position Type Rates';

  table: Table<Row>;

  edit: Action;
  delete: Action;

  private subscription: Subscription;
  positionTypes: PositionType[];

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public overviewService: OverviewService,
    public editService: EditService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private stateService: StateService,
    titleService: Title,
  ) {
    titleService.setTitle(this.title);

    this.edit = editService.edit();
    this.edit.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'positionType' },
        { name: 'rateType' },
        { name: 'from', sort },
        { name: 'through', sort },
        { name: 'rate', sort },
        { name: 'frequency' },
      ],
      actions: [
        this.edit,
        this.delete
      ],
      defaultAction: this.edit,
      pageSize: 50,
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Contains({ propertyType: m.PositionTypeRate.PositionTypesWherePositionTypeRate, parameter: 'positionType' }),
      new Equals({ propertyType: m.PositionTypeRate.RateType, parameter: 'rateType' }),
    ]);

    const positionTypeSearch = new SearchFactory({
      objectType: m.PositionType,
      roleTypes: [m.PositionType.Title],
    });

    const rateTypeSearch = new SearchFactory({
      objectType: m.RateType,
      roleTypes: [m.RateType.Name],
      predicates: [new Equals({ propertyType: m.RateType.IsActive, value: true })]
    });

    this.filterService.init(predicate,
      {
        active: { initialValue: true },
        positionType: { search: positionTypeSearch, display: (v: PositionType) => v.Title },
        rateType: { search: rateTypeSearch, display: (v: RateType) => v.Name },
      });

    const sorter = new Sorter(
      {
        rate: m.PositionTypeRate.Rate,
        from: m.PositionTypeRate.FromDate,
        through: m.PositionTypeRate.ThroughDate,
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
            pull.PositionTypeRate({
              predicate,
              sort: sorter.create(sort),
              include: {
                Frequency: x,
                RateType: x
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
            pull.PositionType({
              include: {
                PositionTypeRate: x
              }
            })
          ];

          return this.allors.context.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.positionTypes = loaded.collections.PositionTypes as PositionType[];
        const objects = loaded.collections.PositionTypeRates as PositionTypeRate[];

        this.table.total = loaded.values.PositionTypeRates_total;
        this.table.data = objects.map((v) => {
          return {
            object: v,
            positionType: this.positionTypes.filter(p => p.PositionTypeRate === v).map(p => p.Title).join(', '),
            rateType: v.RateType.Name,
            from: moment(v.FromDate).format('L'),
            through: v.ThroughDate !== null ? moment(v.ThroughDate).format('L') : '',
            rate: v.Rate,
            frequency: v.Frequency.Name,
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
