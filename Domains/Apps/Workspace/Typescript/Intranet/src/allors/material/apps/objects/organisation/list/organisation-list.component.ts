import { Component, OnDestroy, OnInit, ViewChild, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Like } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, RefreshService, Action, MetaService } from '../../../../../angular';
import { TableRow, NavigateService, DeleteService, Table, Sorter } from '../../../../../material';

import { Organisation } from '../../../../../domain';
import { combineLatest, Subscription } from 'rxjs';

interface Row extends TableRow {
  object: Organisation;
  name: string;
  classification: string;
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

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigateService: NavigateService,
    public deleteService: DeleteService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private errorService: ErrorService,
    titleService: Title) {

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name', sort: true },
        { name: 'classification', sort: true },
        { name: 'street', sort: true },
        { name: 'locality', sort: true },
        { name: 'country', sort: true },
        { name: 'phone', sort: true },
        'lastModifiedDate'
      ],
      actions: [
        navigateService.overview(),
        this.delete
      ],
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.Organisation.Name, parameter: 'name' }),
    ]);

    this.filterService.init(predicate);

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
        switchMap(([refresh, filterFields, sort, pageEvent]) => {

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
        const people = loaded.collections.Organisations as Organisation[];
        this.table.total = loaded.values.Organisations_total;
        this.table.data = people.map((v) => {
          return {
            object: v,
            name: v.displayName,
            classification: v.displayClassification,
            street: v.displayAddress,
            locality: v.displayAddress2,
            country: v.displayAddress3,
            phone: v.displayPhone,
            lastModifiedDate: moment(v.LastModifiedDate).fromNow()
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
