import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { formatDistance } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService, UserId } from '@allors/angular/services/core';
import { ObjectService } from '@allors/angular/material/services/core';
import { FilterDefinition, Filter, TestScope, Action } from '@allors/angular/core';

import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, Sorter, EditService } from '@allors/angular/material/core';
import { TaskAssignment } from '@allors/domain/generated';
import { And, Equals, ContainedIn, Extent, Like } from '@allors/data/system';

interface Row extends TableRow {
  object: TaskAssignment;
  title: string;
  dateCreated: string;
}

@Component({
  templateUrl: './taskassignment-list.component.html',
  providers: [ContextService],
})
export class TaskAssignmentListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Tasks';

  table: Table<Row>;

  edit: Action;

  private subscription: Subscription;
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,

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
    this.edit.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: ['title', 'dateCreated'],
      actions: [this.edit],
      defaultAction: this.edit,
      pageSize: 50,
      initialSort: 'dateCreated',
    });
  }

  public ngOnInit(): void {
    const { m, pull, x } = this.metaService;

    const predicate = new And([
      new Equals({ propertyType: m.TaskAssignment.User, object: this.userId.value }),
      new ContainedIn({
        propertyType: m.TaskAssignment.Task,
        extent: new Extent({
          objectType: m.Task,
          predicate: new Like({ roleType: m.Task.Title, parameter: 'title' }),
        }),
      }),
    ]);

    const filterDefinition = new FilterDefinition(predicate);
    this.filter = new Filter(filterDefinition);

    this.subscription = combineLatest(this.refreshService.refresh$, this.filter.fields$, this.table.sort$, this.table.pager$)
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
        switchMap(([, filterFields, , pageEvent]) => {
          const pulls = [
            pull.TaskAssignment({
              predicate,
              // sort: sorter.create(sort),
              include: {
                Task: {
                  WorkItem: x,
                },
                User: x,
              },
              parameters: this.filter.parameters(filterFields),
              skip: pageEvent.pageIndex * pageEvent.pageSize,
              take: pageEvent.pageSize,
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();
        const taskAssignments = loaded.collections.TaskAssignments as TaskAssignment[];
        this.table.total = loaded.values.TaskAssignments_total;
        this.table.data = taskAssignments.map((v) => {
          return {
            object: v,
            title: v.Task.Title,
            dateCreated: formatDistance(new Date(v.Task.DateCreated), new Date()),
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
