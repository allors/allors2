import { Component, Self, OnInit, HostBinding } from '@angular/core';
import { NavigationService, Action, PanelService, RefreshService,  MetaService } from '../../../../../../angular';
import { WorkEffortPartyAssignment, OrganisationContactRelationship, Party } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, EditService, Table, OverviewService, CreateData } from '../../../../..';
import * as moment from 'moment';

interface Row extends TableRow {
  object: WorkEffortPartyAssignment;
  number: string;
  name: string;
  status: string;
  party: string;
  from: string;
  through: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'workeffortpartyassignment-overview-panel',
  templateUrl: './workeffortpartyassignment-overview-panel.component.html',
  providers: [PanelService]
})
export class WorkEffortPartyAssignmentOverviewPanelComponent implements OnInit {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: WorkEffortPartyAssignment[] = [];

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
    
    public deleteService: DeleteService,
    public editService: EditService,
  ) {

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    this.panel.name = 'workeffortpartyassignment';
    this.panel.title = 'Party Assignment';
    this.panel.icon = 'work';
    this.panel.expandable = true;

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'number', sort },
        { name: 'name', sort },
        { name: 'state', sort },
        { name: 'party', sort },
        { name: 'from', sort },
        { name: 'through', sort },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const partypullName = `${this.panel.name}_${this.m.WorkEffortPartyAssignment.name}_party`;
    const workeffortpullName = `${this.panel.name}_${this.m.WorkEffortPartyAssignment.name}_workeffort`;

    this.panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Person({
          name: partypullName,
          object: id,
          fetch: {
            WorkEffortPartyAssignmentsWhereParty: {
              include: {
                Party: x,
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
            WorkEffortPartyAssignmentsWhereAssignment: {
              include: {
                Party: x,
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
      const fromParty = loaded.collections[partypullName] as WorkEffortPartyAssignment[];
      const fromWorkEffort = loaded.collections[workeffortpullName] as WorkEffortPartyAssignment[];

      if (fromParty !== undefined && fromParty.length > 0) {
        this.objects = fromParty;
      }

      if (fromWorkEffort !== undefined && fromWorkEffort.length > 0) {
        this.objects = fromWorkEffort;
      }

      this.objects = fromParty || fromWorkEffort;

      if (this.objects) {
        this.table.total = this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            number: v.Assignment.WorkEffortNumber,
            name: v.Assignment.Name,
            party: v.Party.displayName,
            status: v.Assignment.WorkEffortState ? v.Assignment.WorkEffortState.Name : '',
            from: moment(v.FromDate).format('L'),
            through: v.ThroughDate !== null ? moment(v.ThroughDate).format('L') : '',
          } as Row;
        });
      }
    };
  }
}
