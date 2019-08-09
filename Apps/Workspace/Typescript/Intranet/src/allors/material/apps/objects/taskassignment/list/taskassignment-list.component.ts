import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import * as moment from 'moment';

import { PullRequest, And, Like, ContainedIn, Filter, Equals } from '../../../../../framework';
import { AllorsFilterService, MediaService, ContextService, NavigationService, Action, RefreshService, MetaService, SearchFactory, UserId, TestScope } from '../../../../../angular';
import { Sorter, TableRow, Table, EditService } from '../../../..';
import { TaskAssignment } from '../../../../../domain';
import { ObjectService } from '../../../../core/services/object';

interface Row extends TableRow {
  object: TaskAssignment;
  title: string;
  dateCreated: string;
}

@Component({
  templateUrl: './taskassignment-list.component.html',
  providers: [ContextService, AllorsFilterService]
})
export class TaskAssignmentListComponent extends TestScope implements OnInit, OnDestroy {

  public title = 'Tasks';

  table: Table<Row>;

  edit: Action;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Self() private filterService: AllorsFilterService,
    public metaService: MetaService,
    public factoryService: ObjectService,
    public refreshService: RefreshService,
    public editService: EditService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    private userId: UserId,
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    const { m } = this.metaService;

    this.edit = editService.edit(m.TaskAssignment.Task);
    this.edit.result.subscribe((v) => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        'title',
        'dateCreated'
      ],
      actions: [
        this.edit
      ],
      defaultAction: this.edit,
      pageSize: 50,
      initialSort: 'dateCreated'
    });
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Equals({ propertyType: m.TaskAssignment.User, object: this.userId.value }),
      new ContainedIn({
        propertyType: m.TaskAssignment.Task,
        extent: new Filter({
          objectType: m.Task,
          predicate: new Like({ roleType: m.Task.Title, parameter: 'title' }),
        })
      }),
    ]);

    this.filterService.init(predicate);

    const sorter = new Sorter(
      {
        dateCreated: m.Task.DateCreated,
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
        }, [, , , ,]),
        switchMap(([, filterFields, sort, pageEvent]) => {

          const pulls = [
            pull.TaskAssignment({
              predicate,
              // sort: sorter.create(sort),
              include: {
                Task: {
                  WorkItem: x
                },
                User: x
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
        const taskAssignments = loaded.collections.TaskAssignments as TaskAssignment[];
        this.table.total = loaded.values.People_total;
        this.table.data = taskAssignments.map((v) => {
          return {
            object: v,
            title: v.Task.Title,
            dateCreated: moment(v.Task.DateCreated).fromNow()
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
