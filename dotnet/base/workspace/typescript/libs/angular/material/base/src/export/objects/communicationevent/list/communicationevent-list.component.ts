import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, scan } from 'rxjs/operators';
import { format, formatDistance } from 'date-fns';

import { ContextService, MetaService, RefreshService, NavigationService, MediaService } from '@allors/angular/services/core';
import { CommunicationEvent } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { TableRow, Table, EditService, DeleteService } from '@allors/angular/material/core';
import { Action, TestScope, Filter } from '@allors/angular/core';

interface Row extends TableRow {
  object: CommunicationEvent;
  name: string;
  type: string;
  state: string;
  subject: string;
  involved: string;
  started: string;
  ended: string;
  lastModifiedDate: string;
}

@Component({
  templateUrl: './communicationevent-list.component.html',
  providers: [ContextService],
})
export class CommunicationEventListComponent extends TestScope implements OnInit, OnDestroy {
  public title = 'Communications';

  table: Table<Row>;

  delete: Action;
  edit: Action;

  private subscription: Subscription;
  filter: Filter;

  constructor(
    @Self() public allors: ContextService,

    public metaService: MetaService,
    public refreshService: RefreshService,
    public deleteService: DeleteService,
    public editService: EditService,
    public navigation: NavigationService,
    public mediaService: MediaService,
    titleService: Title
  ) {
    super();

    titleService.setTitle(this.title);

    this.delete = deleteService.delete(allors.context);
    this.edit = editService.edit();

    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'type' },
        { name: 'state' },
        { name: 'subject', sort: true },
        { name: 'involved' },
        { name: 'started' },
        { name: 'ended' },
        { name: 'lastModifiedDate', sort: true },
      ],
      actions: [this.edit, this.delete],
      defaultAction: this.edit,
      pageSize: 50,
    });
  }

  ngOnInit(): void {
    const { m, pull, x } = this.metaService;
    this.filter = m.CommunicationEvent.filter = m.CommunicationEvent.filter ?? new Filter(m.CommunicationEvent.filterDefinition);

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
            pull.CommunicationEvent({
              predicate: this.filter.definition.predicate,
              sort: sort ? m.CommunicationEvent.sorter.create(sort) : null,
              include: {
                CommunicationEventState: x,
                InvolvedParties: x,
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
        const communicationEvents = loaded.collections.CommunicationEvents as CommunicationEvent[];
        this.table.total = loaded.values.CommunicationEvents_total;
        this.table.data = communicationEvents.map((v) => {
          return {
            object: v,
            type: v.objectType.name,
            state: v.CommunicationEventState && v.CommunicationEventState.Name,
            subject: v.Subject,
            involved: v.InvolvedParties.map((w) => w.displayName).join(', '),
            started: v.ActualStart && format(new Date(v.ActualStart), 'dd-MM-yyyy'),
            ended: v.ActualEnd && format(new Date(v.ActualEnd), 'dd-MM-yyyy'),
            lastModifiedDate: formatDistance(new Date(v.LastModifiedDate), new Date()),
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
