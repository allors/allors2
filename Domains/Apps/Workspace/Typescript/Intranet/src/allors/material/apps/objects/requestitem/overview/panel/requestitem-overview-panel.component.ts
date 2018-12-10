import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, RefreshService, ErrorService, Action, MetaService, ActionTarget } from '../../../../../../angular';
import { RequestItem } from '../../../../../../domain';
import { MetaDomain } from '../../../../../../meta';
import { DeleteService, TableRow, Table, StateService } from '../../../../..';

interface Row extends TableRow {
  object: RequestItem;
  item: string;
  quantity: number;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'requestitem-overview-panel',
  templateUrl: './requestitem-overview-panel.component.html',
  providers: [PanelService]
})
export class RequestItemOverviewPanelComponent {
  m: MetaDomain;

  requestItems: RequestItem[];
  table: Table<Row>;
  requestItem: RequestItem;

  delete: Action;
  edit: Action;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
  ) {

    this.m = this.metaService.m;

    panel.name = 'requestitem';
    panel.title = 'Request Items';
    panel.icon = 'contacts';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = {
      name: (target: ActionTarget) => 'Edit',
      description: (target: ActionTarget) => 'Edit',
      disabled: (target: ActionTarget) => false,
      execute: (target: ActionTarget) => this.requestItem = target as RequestItem,
      result: null
    };

    this.table = new Table({
      selection: true,
      columns: [
        { name: 'item' },
        { name: 'quantity' },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
    });

    const pullName = `${panel.name}_${this.m.RequestItem.objectType.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.RequestForQuote({
          name: pullName,
          object: id,
          fetch: {
            RequestItems: {
              include: {
                Product: x,
                SerialisedItem: x,
              }
            }
          }
        }));
    };

    panel.onPulled = (loaded) => {

      this.requestItems = loaded.collections[pullName] as RequestItem[];
      this.table.total = loaded.values[`${pullName}_total`] || this.requestItems.length;
      this.table.data = this.requestItems.map((v) => {
        return {
          object: v,
          item: (v.Product && v.Product.Name) || (v.SerialisedItem && v.SerialisedItem.Name) || '',
          quantity: v.Quantity,
        } as Row;
      });
    };

  }
}
