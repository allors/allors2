import { Component, OnInit, Self, HostBinding } from '@angular/core';
import { isBefore, isAfter } from 'date-fns';

import { MetaService, NavigationService, PanelService, RefreshService } from '@allors/angular/services/core';
import { WorkEffort, WorkEffortAssignmentRate } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, EditService } from '@allors/angular/material/core';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData } from '@allors/angular/material/services/core';

interface Row extends TableRow {
  object: WorkEffortAssignmentRate;
  // partyAssignment: string;
  // from: string;
  // through: string;
  rateType: string;
  rate: number;
  frequency: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'workeffortassignmentrate-overview-panel',
  templateUrl: './workeffortassignmentrate-overview-panel.component.html',
  providers: [PanelService]
})
export class WorkEffortAssignmentRateOverviewPanelComponent extends TestScope implements OnInit {
  workEffort: WorkEffort;

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: WorkEffortAssignmentRate[];
  table: Table<Row>;

  delete: Action;
  edit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
    };
  }

  collection = 'All';
  currentRates: WorkEffortAssignmentRate[];
  inactiveRates: WorkEffortAssignmentRate[];
  allRates: WorkEffortAssignmentRate[] = [];

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

    this.panel.name = 'workeffortrate';
    this.panel.title = 'Rates';
    this.panel.icon = 'contacts';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'rateType' },
        // { name: 'partyAssignment' },
        // { name: 'from', sort },
        // { name: 'through', sort },
        { name: 'rate', sort },
        { name: 'frequency' },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${this.panel.name}_${this.m.WorkEffortAssignmentRate.name}`;

    this.panel.onPull = (pulls) => {

      const { pull, x } = this.metaService;
      const id = this.panel.manager.id;

      pulls.push(
        pull.WorkEffort({
          name: pullName,
          object: id,
          fetch: {
            WorkEffortAssignmentRatesWhereWorkEffort: {
              include: {
                RateType: x,
                Frequency: x,
                WorkEffortPartyAssignment: {
                  Party: x
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
      this.objects = loaded.collections[pullName] as WorkEffortAssignmentRate[];

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.refreshTable();
      }
    };
  }

  public refreshTable() {
    this.table.data = this.workEffortAssignmentRates.map((v) => {
      return {
        object: v,
        // partyAssignment: v.WorkEffortPartyAssignment.displayName,
        // from: format(new Date(v.FromDate), 'dd-MM-yyyy'),
        // through: v.ThroughDate !== null ? format(new Date(v.ThroughDate), 'dd-MM-yyyy') : '',
        rateType: v.RateType.Name,
        rate: v.Rate,
        frequency: v.Frequency.Name,
      } as Row;
    });
  }

  get workEffortAssignmentRates(): any {

    switch (this.collection) {
      case 'Current':
        return this.objects && this.objects.filter(v => isBefore(new Date(v.FromDate), new Date()) && (!v.ThroughDate || isAfter(new Date(v.ThroughDate), new Date())));
      case 'Inactive':
        return this.objects && this.objects.filter(v => isAfter(new Date(v.FromDate), new Date()) || (v.ThroughDate && isBefore(new Date(v.ThroughDate), new Date())));
      case 'All':
      default:
        return this.objects;
    }
  }
}
