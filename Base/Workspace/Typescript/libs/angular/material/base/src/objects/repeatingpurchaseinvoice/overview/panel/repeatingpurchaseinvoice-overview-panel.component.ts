import { Component, OnInit, Self, HostBinding, AfterViewInit, OnDestroy, Injector, Input } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { formatDistance, format, isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, RefreshService, Action, NavigationService, PanelService, PanelManagerService, ContextService, NavigationActivatedRoute, ActionTarget } from '@allors/angular/core';
import { CommunicationEvent, ContactMechanism, CustomerShipment, ShipmentItem, Good, SalesInvoice, BillingProcess, SerialisedInventoryItemState, InventoryItem, NonSerialisedInventoryItem, Part, NonUnifiedPart, Organisation, SupplierOffering, PartyContactMechanism, PartyRate, ProductIdentification, PurchaseInvoiceItem, PurchaseInvoice, PurchaseOrderItem, PurchaseOrder, QuoteItem, ProductQuote } from '@allors/domain/generated';
import { TableRow, Table, EditService, DeleteService, ObjectData, ObjectService, OverviewService, MethodService } from '@allors/angular/material/core';
import { Meta } from '@allors/meta/generated';
import { ActivatedRoute } from '@angular/router';
import { InternalOrganisationId } from '@allors/angular/base';
import { PullRequest } from '@allors/protocol/system';
import { RoleType } from '@allors/meta/system';
import { Pull, Fetch, Step } from '@allors/data/system';

interface Row extends TableRow {
  object: RepeatingPurchaseInvoice;
  frequency: string;
  dayOfWeek: string;
  previousExecutionDate: string;
  nextExecutionDate: string;
  finalExecutionDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'repeatingpurchaseinvoice-overview-panel',
  templateUrl: './repeatingpurchaseinvoice-overview-panel.component.html',
  providers: [ContextService, PanelService],
})
export class RepeatingPurchaseInvoiceOverviewPanelComponent extends TestScope {
  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  internalOrganisation: Organisation;
  objects: RepeatingPurchaseInvoice[] = [];
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
    public editService: EditService,
    public deleteService: DeleteService,
    public snackBar: MatSnackBar
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'repeating purchase invoice';
    panel.title = 'Repeating Purchase Invoices';
    panel.icon = 'business';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'internalOrganisation' },
        { name: 'frequency', sort },
        { name: 'dayOfWeek', sort },
        { name: 'previousExecutionDate', sort },
        { name: 'nextExecutionDate', sort },
        { name: 'finalExecutionDate', sort },
      ],
      actions: [this.edit, this.delete],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${panel.name}_${this.m.RepeatingPurchaseInvoice.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Organisation({
          name: pullName,
          object: id,
          fetch: {
            RepeatingPurchaseInvoicesWhereSupplier: {
              include: {
                InternalOrganisation: x,
                Frequency: x,
                DayOfWeek: x,
              },
            },
          },
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.internalOrganisation = loaded.objects.InternalOrganisation as Organisation;

      this.objects = loaded.collections[pullName] as RepeatingPurchaseInvoice[];

      this.table.data = this.objects.map((v) => {
        return {
          object: v,
          internalOrganisation: v.InternalOrganisation.PartyName,
          frequency: v.Frequency.Name,
          dayOfWeek: v.DayOfWeek && v.DayOfWeek.Name,
          previousExecutionDate: v.PreviousExecutionDate && format(new Date(v.PreviousExecutionDate), 'DD-MM-YYYY'),
          nextExecutionDate: v.NextExecutionDate && format(new Date(v.NextExecutionDate), 'DD-MM-YYYY'),
          finalExecutionDate: v.FinalExecutionDate && format(new Date(v.FinalExecutionDate), 'DD-MM-YYYY'),
        } as Row;
      });
    };
  }
}
