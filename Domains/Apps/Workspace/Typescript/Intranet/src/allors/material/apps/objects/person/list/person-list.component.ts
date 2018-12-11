import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Like } from '../../../../../framework';
import { AllorsFilterService, ErrorService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService } from '../../../../../angular';
import { Sorter, TableRow, Table, NavigateService, DeleteService } from '../../../..';

import { Person } from '../../../../../domain';

import { ObjectService } from '../../../../../angular/base/object/object.service';

interface Row extends TableRow {
  object: Person;
  name: string;
  email: string;
  phone: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './person-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class PersonListComponent implements OnInit, OnDestroy {

  public title = 'People';

  table: Table<Row>;

  delete: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public factoryService: ObjectService,
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
        { name: 'name', sort: true},
        { name: 'email', sort: true },
        { name: 'phone', sort: true },
        'lastModifiedDate'
      ],
      actions: [
        navigateService.overview(),
        this.delete
      ],
      defaultAction: navigateService.overview(),
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Like({ roleType: m.Person.FirstName, parameter: 'firstName' }),
      new Like({ roleType: m.Person.LastName, parameter: 'lasttName' }),
      // new ContainedIn({
      //   propertyType: m.Person.GeneralCorrespondence,
      //   extent: new Filter({
      //       objectType: m.PostalAddress,
      //       predicate: new ContainedIn({
      //         propertyType: m.PostalAddress.Country,
      //         extent: new Filter({
      //           objectType: m.Country,
      //           predicate: new Like({roleType: m.Country.Name, parameter: 'countryName'})
      //         })
      //       })
      //   })
      // })
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        name: [m.Person.FirstName, m.Person.LastName],
        lastModifiedDate: m.Person.LastModifiedDate,
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
            pull.Person({
              predicate,
              sort: sorter.create(sort),
              include: {
                Salutation: x,
                Picture: x,
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
        const people = loaded.collections.People as Person[];
        this.table.total = loaded.values.People_total;
        this.table.data = people.map((v) => {
          return {
            object: v,
            name: v.displayName,
            email: v.displayEmail,
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
