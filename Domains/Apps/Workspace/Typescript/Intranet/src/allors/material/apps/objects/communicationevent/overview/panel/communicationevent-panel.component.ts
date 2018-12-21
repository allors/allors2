import { Component, Self, Input, OnInit } from '@angular/core';

import { PanelService, MetaService, RefreshService, Action, ActionTarget } from '../../../../../../angular';
import { CommunicationEvent } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table } from '../../../../..';
import { ObjectService, CreateData } from '../../../../../base/services/object';
import { ISessionObject, RoleType, Fetch, Pull, Tree } from '../../../../../../framework';
import { Step } from 'src/allors/framework/database/data/Step';

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
  selector: 'communicationevent-panel',
  templateUrl: './communicationevent-panel.component.html',
  providers: [PanelService]
})
export class CommunicationEventsPanel implements OnInit {

  m: Meta;

  objects: CommunicationEvent[];
  table: Table<Row>;

  delete: Action;

  edit: Action = {
    name: (target: ActionTarget) => 'Edit',
    description: (target: ActionTarget) => 'Edit',
    disabled: (target: ActionTarget) => !this.objectService.hasEditControl(target as ISessionObject),
    execute: (target: ActionTarget) => this.objectService.edit(target as ISessionObject).subscribe((v) => this.refreshService.refresh()),
    result: null
  };

  get createData(): CreateData {
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
    public deleteService: DeleteService,
  ) {

    this.m = this.metaService.m;
  }

  ngOnInit() {

    this.delete = this.deleteService.delete(this.panel.manager.context);

    this.panel.name = 'abc';
    this.panel.title = 'Communication events';
    this.panel.icon = 'message';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'type' },
        { name: 'description' },
        { name: 'involved' },
        { name: 'status' },
        { name: 'purpose' },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
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
              }
            }
          }
        }));
    };

    this.panel.onPulled = (loaded) => {
      this.objects = loaded.collections[pullName] as CommunicationEvent[];
      this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
      this.table.data = this.objects.map((v) => {
        return {
          object: v,
          type: v.objectType.name,
          description: v.Description,
          involved: v.InvolvedParties.join(', '),
          status: v.CommunicationEventState.Name,
          purpose: v.EventPurposes.join(', '),
        } as Row;
      });
    };
  }
}
