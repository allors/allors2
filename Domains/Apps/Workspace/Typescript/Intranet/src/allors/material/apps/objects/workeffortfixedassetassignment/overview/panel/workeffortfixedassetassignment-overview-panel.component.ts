import { Component, Self, OnInit, HostBinding } from '@angular/core';
import { NavigationService, Action, PanelService, RefreshService, ErrorService, MetaService } from '../../../../../../angular';
import { WorkEffortFixedAssetAssignment } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, EditService, Table, OverviewService, CreateData } from '../../../../..';
import * as moment from 'moment';

interface Row extends TableRow {
  object: WorkEffortFixedAssetAssignment;
  number: string;
  name: string;
  status: string;
  asset: string;
  from: string;
  through: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'workeffortfixedassetassignment-overview-panel',
  templateUrl: './workeffortfixedassetassignment-overview-panel.component.html',
  providers: [PanelService]
})
export class WorkEffortFixedAssetAssignmentOverviewPanelComponent implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: WorkEffortFixedAssetAssignment[] = [];

  delete: Action;
  edit: Action;
  table: Table<TableRow>;

  get createData(): CreateData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
    public editService: EditService,
  ) {

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    this.panel.name = 'workeffortfixedassetassignment';
    this.panel.title = 'Work Effort Fixed Asset Assignments';
    this.panel.icon = 'work';
    this.panel.expandable = true;

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number' },
        { name: 'name' },
        { name: 'status' },
        { name: 'asset' },
        { name: 'from' },
        { name: 'through' },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit
    });

    const serialisedItempullName = `${this.panel.name}_${this.m.WorkEffortFixedAssetAssignment.name}_serialisedItem`;
    const workeffortpullName = `${this.panel.name}_${this.m.WorkEffortFixedAssetAssignment.name}_workeffort`;

    this.panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.SerialisedItem({
          name: serialisedItempullName,
          object: id,
          fetch: {
            WorkEffortFixedAssetAssignmentsWhereFixedAsset: {
              include: {
                FixedAsset: x,
                Assignment: {
                  WorkEffortState: x,
                  Priority: x,
                }
              }
            }
          }
        }),
        pull.WorkEffort({
          name: workeffortpullName,
          object: id,
          fetch: {
            WorkEffortFixedAssetAssignmentsWhereAssignment: {
              include: {
                FixedAsset: x,
                Assignment: {
                  WorkEffortState: x,
                  Priority: x,
                }
              }
            }
          }
        }),
      );
    };

    this.panel.onPulled = (loaded) => {
      const fromSerialiseditem = loaded.collections[serialisedItempullName] as WorkEffortFixedAssetAssignment[];
      const fromWorkEffort = loaded.collections[workeffortpullName] as WorkEffortFixedAssetAssignment[];

      if (fromSerialiseditem !== undefined && fromSerialiseditem.length > 0) {
        this.objects = fromSerialiseditem;
      }

      if (fromWorkEffort !== undefined && fromWorkEffort.length > 0) {
        this.objects = fromWorkEffort;
      }

      if (this.objects) {
        this.table.total = this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            number: v.Assignment.WorkEffortNumber,
            name: v.Assignment.Name,
            status: v.Assignment.WorkEffortState ? v.Assignment.WorkEffortState.Name : '',
            asset: v.FixedAsset.Name,
            from: moment(v.FromDate).format('L'),
            through: v.ThroughDate !== null ? moment(v.ThroughDate).format('L') : '',
          } as Row;
        });
      }
    };
  }
}
