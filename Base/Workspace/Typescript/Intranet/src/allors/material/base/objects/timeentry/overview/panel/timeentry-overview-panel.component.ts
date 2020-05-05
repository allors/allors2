import { Component, Self, OnInit, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, Action, MetaService, TestScope } from '../../../../../../angular';
import { TimeEntry, WorkEffort } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, ObjectData, EditService } from '../../../../..';
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
  providers: [PanelService]
})
export class TimeEntryOverviewPanelComponent extends TestScope implements OnInit {
  workEffort: WorkEffort;

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
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public deleteService: DeleteService,
    public editService: EditService
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.panel.name = 'timeentry';
    this.panel.title = 'Time Entries';
    this.panel.icon = 'timer';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'person' },
        { name: 'from', sort },
        { name: 'through', sort },
        { name: 'time', sort },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${this.panel.name}_${this.m.TimeEntry.name}`;

    this.panel.onPull = (pulls) => {

      const { pull, x, tree } = this.metaService;
      const id = this.panel.manager.id;

      pulls.push(
        pull.WorkEffort({
          name: pullName,
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
    };

    this.panel.onPulled = (loaded) => {
      this.workEffort = loaded.objects.WorkEffort as WorkEffort;
      this.objects = loaded.collections[pullName] as TimeEntry[];

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
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
    };
  }
}
