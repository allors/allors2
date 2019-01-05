import { Component, Self, Input, OnInit } from '@angular/core';

import { PanelService, MetaService, RefreshService, Action, ActionTarget } from '../../../../../../angular';
import { IGoodIdentification } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table } from '../../../../../../material';
import { ObjectService, CreateData } from '../../../../../../material/base/services/object';
import { ISessionObject, RoleType, Fetch, Pull, Tree } from '../../../../../../framework';
import { Step } from 'src/allors/framework/database/data/Step';

interface Row extends TableRow {
  object: IGoodIdentification;
  type: string;
  identification: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'igoodidentification-panel',
  templateUrl: './igoodIdentification-panel.component.html',
  providers: [PanelService]
})
export class IGoodIdentificationsPanelComponent implements OnInit {
  @Input() roleType: RoleType;

  m: Meta;

  objects: IGoodIdentification[];
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
      associationRoleType: this.roleType,
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

    this.panel.name = 'igoodidentification';
    this.panel.title = 'IGood Identification';
    this.panel.icon = 'fingerprint';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'type' },
        { name: 'identification' },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
    });

    const pullName = `${this.panel.name}_${this.m.IGoodIdentification.name}`;

    this.panel.onPull = (pulls) => {
      const { x, tree } = this.metaService;
      const { id, objectType } = this.panel.manager;

      pulls.push(
        new Pull(objectType, {
          name: pullName,
          object: id,
          fetch: new Fetch({
            step: new Step({
              propertyType: this.roleType,
              include: tree.IGoodIdentification({
                GoodIdentificationType: x,
              })
            })
          })
        })
      );

      this.panel.onPulled = (loaded) => {
        this.objects = loaded.collections[pullName] as IGoodIdentification[];
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            type: v.GoodIdentificationType && v.GoodIdentificationType.Name,
            identification: v.Identification,
          } as Row;
        });
      };
    };
  }
}
