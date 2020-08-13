import { Component, Self, OnInit, OnDestroy, Inject, HostBinding } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter, format } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/services/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood, Payment, Invoice, PurchaseInvoice } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService, TableRow, Table, ObjectService, MethodService, DeleteService, EditService } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';


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
  receive: boolean;

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
        pull.Invoice({
          object: this.panel.manager.id,
          include: {
            SalesInvoice_SalesInvoiceType: x,
            PurchaseInvoice_PurchaseInvoiceType: x,
          }
        }),
      );
    };

    panel.onPulled = (loaded) => {

      const invoice = loaded.objects.Invoice as Invoice;

      if (invoice.objectType.name === this.m.SalesInvoice.name) {
        const salesInvoice = invoice as SalesInvoice;
        this.receive = salesInvoice.SalesInvoiceType.UniqueId === '92411bf1-835e-41f8-80af-6611efce5b32';
      }

      if (invoice.objectType.name === this.m.PurchaseInvoice.name) {
        const salesInvoice = invoice as PurchaseInvoice;
        this.receive = salesInvoice.PurchaseInvoiceType.UniqueId === '0187d927-81f5-4d6a-9847-58b674ad3e6a';
      }

      this.payments = loaded.collections[pullName] as Payment[];

      this.table.total = loaded.values[`${pullName}_total`] || this.payments.length;
      this.table.data = this.payments.map((v) => {
        return {
          object: v,
          date: v.EffectiveDate && format(new Date(v.EffectiveDate), 'dd-MM-yyyy'),
          amount: v.Amount
        } as Row;
      });
    };
  }
}
