import { Component, Self, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, ErrorService, Action, MetaService, Invoked, ContextService } from '../../../../../../angular';
import { RequestItem, RequestForQuote } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, EditService, MethodService } from '../../../../..';
import * as moment from 'moment';

import { MatSnackBar } from '@angular/material';

import { CreateData, ObjectService } from '../../../../../../material/base/services/object';

interface Row extends TableRow {
  object: RequestItem;
  item: string;
  state: string;
  quantity: number;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'requestitem-overview-panel',
  templateUrl: './requestitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class RequestItemOverviewPanelComponent {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  request: RequestForQuote;
  requestItems: RequestItem[];
  table: Table<Row>;
  requestItem: RequestItem;

  delete: Action;
  edit: Action;
  cancel: Action;
  hold: Action;
  submit: Action;

  get createData(): CreateData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.metaService.m.Request.RequestItems,
    };
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public objectService: ObjectService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public methodService: MethodService,
    public deleteService: DeleteService,
    public editService: EditService,
    public snackBar: MatSnackBar
  ) {

    this.m = this.metaService.m;

    panel.name = 'requestitem';
    panel.title = 'Request Items';
    panel.icon = 'contacts';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = this.editService.edit();
    this.cancel = methodService.create(allors.context, this.m.RequestItem.Cancel, { name: 'Cancel'});
    this.hold = methodService.create(allors.context, this.m.RequestItem.Hold, { name: 'Hold'});
    this.submit = methodService.create(allors.context, this.m.RequestItem.Submit, { name: 'Submit'});

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'item', sort },
        { name: 'state', sort },
        { name: 'quantity', sort },
        { name: 'lastModifiedDate', sort },
      ],
      actions: [
        this.edit,
        this.delete,
        this.cancel,
        this.hold,
        this.submit
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${panel.name}_${this.m.RequestItem.name}`;

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
                RequestItemState: x,
                Product: x,
                SerialisedItem: x,
              }
            }
          }
        }),
        pull.Request({
          name: 'Request',
          object: id
        }),
      );
    };

    panel.onPulled = (loaded) => {

      this.requestItems = loaded.collections[pullName] as RequestItem[];
      this.request = loaded.objects.Request as RequestForQuote;
      this.table.total = loaded.values[`${pullName}_total`] || this.requestItems.length;
      this.table.data = this.requestItems.map((v) => {
        return {
          object: v,
          item: (v.Product && v.Product.Name) || (v.SerialisedItem && v.SerialisedItem.Name) || '',
          state: v.RequestItemState ? v.RequestItemState.Name : '',
          quantity: v.Quantity,
          lastModifiedDate: moment(v.LastModifiedDate).fromNow()
        } as Row;
      });
    };
  }
}
