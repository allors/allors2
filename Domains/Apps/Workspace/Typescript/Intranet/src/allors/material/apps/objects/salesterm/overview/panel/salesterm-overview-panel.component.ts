import { Component, Self, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, ErrorService, Action, MetaService, ActionTarget } from '../../../../../../angular';
import { RequestItem as SalesOrderItem, SalesTerm, SalesInvoice } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table } from '../../../../..';

import { ISessionObject } from 'src/allors/framework';
import { MatSnackBar } from '@angular/material';

import { CreateData, ObjectService, EditData, ObjectData } from '../../../../../../material/base/services/object';
interface Row extends TableRow {
  object: SalesTerm;
  name: string;
  value: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'salesterm-overview-panel',
  templateUrl: './salesterm-overview-panel.component.html',
  providers: [PanelService]
})
export class SalesTermOverviewPanelComponent {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: SalesTerm[];
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
      associationRoleType: this.metaService.m.Request.RequestItems,
    };
  }

  constructor(
    @Self() public panel: PanelService,
    public objectService: ObjectService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
    public snackBar: MatSnackBar
  ) {

    this.m = this.metaService.m;

    panel.name = 'salesterm';
    panel.title = 'Sales Terms';
    panel.icon = 'contacts';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'name' },
        { name: 'value' },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit
    });

    const salesOrderPullName = `${panel.name}_${this.m.SalesOrder.name}`;
    const salesInvoicePullName = `${panel.name}_${this.m.SalesInvoice.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.SalesOrder({
          name: salesOrderPullName,
          object: id,
          fetch: {
            SalesTerms: {
              include: {
                TermType: x,
              }
            }
          }
        }),
        pull.SalesInvoice({
          name: salesInvoicePullName,
          object: id,
          fetch: {
            SalesTerms: {
              include: {
                TermType: x,
              }
            }
          }
        })

      );
    };

    panel.onPulled = (loaded) => {

      this.objects = loaded.collections[salesOrderPullName] as SalesTerm[] || loaded.collections[salesInvoicePullName] as SalesTerm[];
      this.table.total = loaded.values[`${salesOrderPullName}_total`] || loaded.values[`${salesInvoicePullName}_total`] || this.objects.length;
      this.table.data = this.objects.map((v) => {
        return {
          object: v,
          name: v.TermType && v.TermType.Name,
          value: v.TermValue,
        } as Row;
      });
    };

  }
}
