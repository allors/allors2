import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Equals, Like } from '../../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, SessionService, NavigationService, Action, AllorsRefreshService } from '../../../../../../angular';
import { Sorter, TableRow, Table, NavigateService, DeleteService, StateService } from '../../../../..';

import { SerialisedItemCharacteristicType } from '../../../../../../domain';

interface Row extends TableRow {
  object: SerialisedItemCharacteristicType;
  name: string;
  active: boolean;
  uom: string;
}

@Component({
  templateUrl: './productcharacteristic-list.component.html',
  providers: [SessionService, AllorsFilterService]
})
export class ProductCharacteristicsOverviewComponent implements OnInit, OnDestroy {

  public title = 'Product Characteristic';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: SessionService,
    @Self() private filterService: AllorsFilterService,
    public refreshService: AllorsRefreshService,
    public navigateService: NavigateService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    private stateService: StateService,
    titleService: Title,
  ) {
    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name' },
        { name: 'description' }
      ],
      actions: [
        navigateService.edit(),
        this.delete
      ],
    });
  }

  ngOnInit(): void {

    const { m, pull, x } = this.allors;

    const predicate = new And([
      new Like({ roleType: m.Person.FirstName, parameter: 'name' }),
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        name: m.SerialisedItemCharacteristicType.Name,
        description: m.Catalogue.Description,
      }
    );

    this.subscription = combineLatest(this.refreshService.refresh$, this.filterService.filterFields$, this.table.sort$, this.table.pager$, this.stateService.internalOrganisationId$)
      .pipe(
        scan(([previousRefresh, previousFilterFields], [refresh, filterFields, sort, pageEvent]) => {
          return [
            refresh,
            filterFields,
            sort,
            (previousRefresh !== refresh || filterFields !== previousFilterFields) ? Object.assign({ pageIndex: 0 }, pageEvent) : pageEvent,
          ];
        }, []),
        switchMap(([, filterFields, sort, pageEvent, internalOrganisationId]) => {

          const pulls = [
            pull.SerialisedItemCharacteristicType({
              predicate,
              sort: sorter.create(sort),
              include: {
                UnitOfMeasure: x,
              },
              arguments: this.filterService.arguments(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            })];

          return this.allors.load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.session.reset();
        const requests = loaded.collections.SerialisedItemCharacteristicTypes as SerialisedItemCharacteristicType[];
        this.table.total = loaded.values.SerialisedItemCharacteristicTypes_total;
        this.table.data = requests.map((v) => {
          return {
            object: v,
            name: `${v.Name}`,
            active: v.IsActive,
            uom: `${v.UnitOfMeasure && v.UnitOfMeasure.Name}`,
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
