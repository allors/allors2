import { Component, OnInit, Self, HostBinding } from '@angular/core';

import { MetaService, RefreshService, NavigationService, PanelService } from '@allors/angular/services/core';
import { CommunicationEvent } from '@allors/domain/generated';
import { TableRow, Table, EditService, DeleteService } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData, ObjectService } from '@allors/angular/material/services/core';

interface Row extends TableRow {
  object: CommunicationEvent;
  type: string;
  description: string;
  involved: string;
  status: string;
  purpose: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'communicationevent-overview-panel',
  templateUrl: './communicationevent-overview-panel.component.html',
  providers: [PanelService],
})
export class CommunicationEventOverviewPanelComponent extends TestScope implements OnInit {
  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: CommunicationEvent[];
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
    public objectService: ObjectService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public deleteService: DeleteService,
    public editService: EditService
  ) {
    super();

    this.m = this.metaService.m;
  }

  ngOnInit() {
    this.panel.name = 'communicationevent';
    this.panel.title = 'Communication events';
    this.panel.icon = 'message';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'type', sort },
        { name: 'description', sort },
        { name: 'involved', sort },
        { name: 'state', sort },
        { name: 'purpose', sort },
      ],
      actions: [this.edit, this.delete],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${this.panel.name}_${this.m.CommunicationEvent.name}`;

    this.panel.onPull = (pulls) => {
      const { x, pull } = this.metaService;
      const { id } = this.panel.manager;

      pulls.push(
        pull.Party({
          name: pullName,
          object: id,
          fetch: {
            CommunicationEventsWhereInvolvedParty: {
              include: {
                InvolvedParties: x,
                CommunicationEventState: x,
                EventPurposes: x,
              },
            },
          },
        })
      );
    };

    this.panel.onPulled = (loaded) => {
      this.objects = loaded.collections[pullName] as CommunicationEvent[];

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            type: v.objectType.name,
            description: v.Description,
            involved: v.InvolvedParties.map((w) => w.displayName).join(', '),
            status: v.CommunicationEventState.Name,
            purpose: v.EventPurposes.map((w) => w.Name).join(', '),
          } as Row;
        });
      }
    };
  }
}
