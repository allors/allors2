import { Component, OnInit, Self, HostBinding } from '@angular/core';
import { format } from 'date-fns';

import { MetaService, NavigationService, PanelService, RefreshService, ContextService } from '@allors/angular/services/core';
import { WorkEffort, WorkEffortPartyAssignment } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, EditService } from '@allors/angular/material/core';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData } from '@allors/angular/material/services/core';

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
  providers: [PanelService, ContextService]
})
export class WorkEffortPartyAssignmentOverviewPanelComponent extends TestScope implements OnInit {
  workEffort: WorkEffort;
  fromParty: WorkEffortPartyAssignment[];
  fromWorkEffort: WorkEffortPartyAssignment[];

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: WorkEffortPartyAssignment[] = [];

  delete: Action;
  edit: Action;
  table: Table<TableRow>;

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
    public navigation: NavigationService,

    public deleteService: DeleteService,
    public editService: EditService,
  ) {
    super();

    this.m = this.metaService.m;

    this.delete = deleteService.delete(allors.context);
    this.delete.result.subscribe(() => {
      this.table.selection.clear();
    });
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
        pull.WorkEffort({
          object: id,
        }),
      );
    };

    this.panel.onPulled = (loaded) => {
      this.workEffort = loaded.objects.WorkEffort as WorkEffort;
      this.fromParty = loaded.collections[partypullName] as WorkEffortPartyAssignment[];
      this.fromWorkEffort = loaded.collections[workeffortpullName] as WorkEffortPartyAssignment[];

      if (this.fromParty !== undefined && this.fromParty.length > 0) {
        this.objects = this.fromParty;
      }

      if (this.fromWorkEffort !== undefined && this.fromWorkEffort.length > 0) {
        this.objects = this.fromWorkEffort;
      }

      this.objects = this.fromParty || this.fromWorkEffort;

      if (this.objects) {
        this.table.total = this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            number: v.Assignment.WorkEffortNumber,
            name: v.Assignment.Name,
            party: v.Party.displayName,
            status: v.Assignment.WorkEffortState ? v.Assignment.WorkEffortState.Name : '',
            from: format(new Date(v.FromDate), 'dd-MM-yyyy'),
            through: v.ThroughDate !== null ? format(new Date(v.ThroughDate), 'dd-MM-yyyy') : '',
          } as Row;
        });
      }
    };
  }
}
