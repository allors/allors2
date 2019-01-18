import { Component, Self, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, ErrorService, Action, MetaService, ActionTarget, Invoked } from '../../../../../../angular';
import { QuoteItem, ProductQuote } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, EditService } from '../../../../..';

import { ISessionObject } from 'src/allors/framework';
import { MatSnackBar } from '@angular/material';

import { CreateData, ObjectService } from '../../../../../../material/base/services/object';

interface Row extends TableRow {
  object: QuoteItem;
  item: string;
  quantity: number;
  lastModifiedDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'quoteitem-overview-panel',
  templateUrl: './quoteitem-overview-panel.component.html',
  providers: [PanelService]
})
export class QuoteItemOverviewPanelComponent {

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

  get createData(): CreateData {
    return {
      associationId: this.panel.manager.id,
      associationObjectType: this.panel.manager.objectType,
      associationRoleType: this.metaService.m.Quote.QuoteItems,
    };
  }

  constructor(
    @Self() public panel: PanelService,
    public objectService: ObjectService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public editService: EditService,
    public deleteService: DeleteService,
    public snackBar: MatSnackBar
  ) {

    this.m = this.metaService.m;

    panel.name = 'quoteitem';
    panel.title = 'Quote Items';
    panel.icon = 'contacts';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'item', sort },
        { name: 'quantity', sort },
        { name: 'lastModifiedDate', sort },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true
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
          quantity: v.Quantity,
        } as Row;
      });
    };
  }
}
