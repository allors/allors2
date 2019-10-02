import { Component, Self, HostBinding } from '@angular/core';
import { PanelService, NavigationService, RefreshService, Action, MetaService, ContextService, TestScope } from '../../../../../../angular';
import { Payment, PaymentMethod } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, EditService, MethodService } from '../../../../..';
import * as moment from 'moment';

import { MatSnackBar } from '@angular/material/snack-bar';

import { ObjectData, ObjectService } from '../../../../../../material/core/services/object';
import { Equals } from '../../../../../../../allors/framework';

interface Row extends TableRow {
  object: Payment;
  date: string;
  amount: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'payment-overview-panel',
  templateUrl: './payment-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class PaymentOverviewPanelComponent extends TestScope {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  payments: Payment[];
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
    @Self() public allors: ContextService,
    @Self() public panel: PanelService,
    public objectService: ObjectService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public methodService: MethodService,
    public deleteService: DeleteService,
    public editService: EditService,
    public snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'payment';
    panel.title = 'Payments';
    panel.icon = 'money';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'date' },
        { name: 'amount' },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${panel.name}_${this.m.Payment.name}`;

    panel.onPull = (pulls) => {
      const { pull, x, m } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.PaymentApplication({
          name: pullName,
          predicate: new Equals({ propertyType: m.PaymentApplication.Invoice, object: id }),
          fetch: {
            PaymentWherePaymentApplication: {
              include: {
                Sender: x,
                PaymentMethod: x,
              }
            }
          }
        }),
      );
    };

    panel.onPulled = (loaded) => {

      this.payments = loaded.collections[pullName] as Payment[];

      this.table.total = loaded.values[`${pullName}_total`] || this.payments.length;
      this.table.data = this.payments.map((v) => {
        return {
          object: v,
          date: v.EffectiveDate && moment(v.EffectiveDate).format('MMM Do YY'),
          amount: v.Amount
        } as Row;
      });
    };
  }
}
