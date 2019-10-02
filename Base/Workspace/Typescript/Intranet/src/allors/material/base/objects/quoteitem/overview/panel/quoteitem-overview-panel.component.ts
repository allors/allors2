import { Component, Self, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, Action, MetaService, ActionTarget, Invoked, ContextService, TestScope } from '../../../../../../angular';
import { QuoteItem, ProductQuote } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, EditService, MethodService } from '../../../../..';
import * as moment from 'moment';

import { MatSnackBar } from '@angular/material/snack-bar';

import { ObjectData, ObjectService } from '../../../../../../material/core/services/object';

interface Row extends TableRow {
  object: QuoteItem;
  item: string;
  state: string;
  quantity: string;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'quoteitem-overview-panel',
  templateUrl: './quoteitem-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class QuoteItemOverviewPanelComponent extends TestScope {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  quote: ProductQuote;
  quoteItems: QuoteItem[];
  table: Table<Row>;
  quoteItem: QuoteItem;

  delete: Action;
  edit: Action;
  cancel: Action;
  reject: Action;
  submit: Action;

  get createData(): ObjectData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.metaService.m.Quote.QuoteItems,
    };
  }

  constructor(
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public objectService: ObjectService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public methodService: MethodService,
    public editService: EditService,
    public deleteService: DeleteService,
    public snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'quoteitem';
    panel.title = 'Quote Items';
    panel.icon = 'contacts';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = this.editService.edit();
    this.cancel = methodService.create(allors.context, this.m.QuoteItem.Cancel, { name: 'Cancel' });
    this.reject = methodService.create(allors.context, this.m.QuoteItem.Reject, { name: 'Reject' });
    this.submit = methodService.create(allors.context, this.m.QuoteItem.Submit, { name: 'Submit' });

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
        this.reject,
        this.submit
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${panel.name}_${this.m.QuoteItem.name}`;
    const quotePullName = `${panel.name}_${this.m.ProductQuote.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Quote({
          name: pullName,
          object: id,
          fetch: {
            QuoteItems: {
              include: {
                Product: x,
                SerialisedItem: x,
              }
            }
          }
        }),
        pull.Quote({
          name: quotePullName,
          object: id
        }),
      );
    };

    panel.onPulled = (loaded) => {

      this.quoteItems = loaded.collections[pullName] as QuoteItem[];
      this.quote = loaded.objects[quotePullName] as ProductQuote;
      this.table.total = loaded.values[`${pullName}_total`] || this.quoteItems.length;
      this.table.data = this.quoteItems.map((v) => {
        return {
          object: v,
          item: (v.Product && v.Product.Name) || (v.SerialisedItem && v.SerialisedItem.Name) || '',
          state: `${v.QuoteItemState && v.QuoteItemState.Name}`,
          quantity: v.Quantity,
          lastModifiedDate: moment(v.LastModifiedDate).fromNow()
        } as Row;
      });
    };
  }
}
