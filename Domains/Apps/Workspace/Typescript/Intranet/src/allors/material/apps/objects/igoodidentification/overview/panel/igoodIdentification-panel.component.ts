import { Component, Self } from '@angular/core';

import { PanelService, MetaService, RefreshService, Action, ActionTarget } from '../../../../../../angular';
import { IGoodIdentification } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table } from '../../../../../../material';
import { MatSnackBar } from '@angular/material';
import { ObjectService, CreateData } from '../../../../../../material/base/services/object';
import { ISessionObject } from '../../../../../../framework';

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
export class IGoodIdentificationsPanel {

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
      // associationRoleType: this.metaService.m.IGoodIdentification.,
    };
  }
  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public objectService: ObjectService,
    public refreshService: RefreshService,
    public deleteService: DeleteService,
    private snackBar: MatSnackBar,
  ) {

    this.m = this.metaService.m;
    this.delete = deleteService.delete(panel.manager.context);

    panel.name = 'igoodidentification';
    panel.title = 'IGood Identification';
    panel.icon = 'fingerprint';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);

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

    const pullName = `${panel.name}_${this.m.IGoodIdentification.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Good({
          name: pullName,
          object: id,
          fetch: {
            GoodIdentifications: {
              include: {
                GoodIdentificationType: x,
              }
            }
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
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
  }
}
