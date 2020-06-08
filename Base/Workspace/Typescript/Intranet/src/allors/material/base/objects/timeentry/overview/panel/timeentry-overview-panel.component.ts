import { Component, Self, OnInit, HostBinding } from '@angular/core';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, filter, scan } from 'rxjs/operators';

import { PanelService, NavigationService, RefreshService, Action, MetaService, TestScope, ContextService } from '../../../../../../angular';
import { Equals, PullRequest, Sort, ContainedIn, Filter } from '../../../../../../framework';
import { TimeEntry, WorkEffort } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, ObjectData, EditService, Sorter } from '../../../../..';

import * as moment from 'moment/moment';

interface Row extends TableRow {
  object: TimeEntry;
  person: string;
  from: string;
  through: string;
  time: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'timeentry-overview-panel',
  templateUrl: './timeentry-overview-panel.component.html',
  providers: [PanelService, ContextService]
})
export class TimeEntryOverviewPanelComponent extends TestScope implements OnInit {
  workEffort: WorkEffort;
  private subscription: Subscription;

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: TimeEntry[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public deleteService: DeleteService,
    public editService: EditService
  ) {
    super();

    this.m = this.metaService.m;

    this.panel.name = 'timeentry';
    this.panel.title = 'Time Entries';
    this.panel.icon = 'timer';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'person' },
        { name: 'from', sort: true },
        { name: 'through', sort: true },
        { name: 'time', sort: true },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    this.panel.onPull = (pulls) => {

      if (this.panel.isCollapsed) {
        const { pull, x, tree } = this.metaService;
        const id = this.panel.manager.id;

        pulls.push(
          pull.WorkEffort({
            object: id,
            fetch: {
              ServiceEntriesWhereWorkEffort: {
                include: {
                  TimeEntry_Worker: x
                }
              }
            }
          }),
          pull.WorkEffort({
            object: id,
          }),
        );
      }
    };

    this.panel.onPulled = (loaded) => {
      this.workEffort = loaded.objects.WorkEffort as WorkEffort;
      this.objects = loaded.collections.ServiceEntries as TimeEntry[];
    };
  }

  ngOnInit(): void {

    // Maximized
    this.subscription = combineLatest([this.panel.manager.on$, this.table.sort$])
      .pipe(
        filter(() => {
          return this.panel.isExpanded;
        }),
        switchMap(([, sort]) => {
          const { m, pull, x } = this.metaService;
          const id = this.panel.manager.id;

          const sorter = new Sorter(
            {
              from: m.TimeEntry.FromDate,
              through: m.TimeEntry.ThroughDate,
              time: m.TimeEntry.AmountOfTime,
            }
          );

          const pulls = [
            pull.TimeEntry({
              predicate:
                new Equals({
                  propertyType: m.TimeEntry.WorkEffort,
                  object: id,
                }),
              sort: sorter.create(sort),
              include: {
                Worker: x
              }
            }),
            pull.WorkEffort({
              object: id,
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.objects = loaded.collections.TimeEntries as TimeEntry[];

        if (this.objects) {
          this.table.total = this.objects.length;
          this.table.data = this.objects.map((v) => {
            return {
              object: v,
              person: v.Worker.displayName,
              from: moment(v.FromDate).format('L'),
              through: v.ThroughDate !== null ? moment(v.ThroughDate).format('L') : '',
              time: v.AmountOfTime,
            } as Row;
          });
        }
      });
  }
}