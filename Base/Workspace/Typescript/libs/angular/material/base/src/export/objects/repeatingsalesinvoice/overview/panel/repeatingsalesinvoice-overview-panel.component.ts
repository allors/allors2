import { Component, Self, HostBinding } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { format } from 'date-fns';

import { MetaService, NavigationService, PanelService, RefreshService, ContextService } from '@allors/angular/services/core';
import { SalesInvoice, RepeatingSalesInvoice } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { TableRow, Table, DeleteService, EditService, MethodService } from '@allors/angular/material/core';
import { TestScope, Action } from '@allors/angular/core';
import { ObjectData, ObjectService } from '@allors/angular/material/services/core';

interface Row extends TableRow {
  object: RepeatingSalesInvoice;
  frequency: string;
  dayOfWeek: string;
  previousExecutionDate: string;
  nextExecutionDate: string;
  finalExecutionDate: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'repeatingsalesinvoice-overview-panel',
  templateUrl: './repeatingsalesinvoice-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class RepeatingSalesInvoiceOverviewPanelComponent extends TestScope {
  repeatingInvoice: RepeatingSalesInvoice;

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: RepeatingSalesInvoice[] = [];
  invoice: SalesInvoice;
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

    panel.name = 'repeatinginvoice';
    panel.title = 'Repeating Sales Invoices';
    panel.icon = 'business';
    panel.expandable = true;

    this.delete = deleteService.delete(panel.manager.context);
    this.edit = this.editService.edit();

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'frequency', sort },
        { name: 'dayOfWeek', sort },
        { name: 'previousExecutionDate', sort },
        { name: 'nextExecutionDate', sort },
        { name: 'finalExecutionDate', sort },
      ],
      actions: [
        this.edit,
        this.delete,
      ],
      defaultAction: this.edit,
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${panel.name}_${this.m.RepeatingSalesInvoice.name}`;
    const invoicePullName = `${panel.name}_${this.m.SalesInvoice.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.SalesInvoice({
          name: pullName,
          object: id,
          fetch: {
            RepeatingSalesInvoiceWhereSource: {
              include: {
                Frequency: x,
                DayOfWeek: x
              }
            }
          }
        }),
        pull.SalesInvoice({
          name: invoicePullName,
          object: id
        }),
      );
    };

    panel.onPulled = (loaded) => {

      this.repeatingInvoice = loaded.objects[pullName] as RepeatingSalesInvoice;
      this.invoice = loaded.objects[invoicePullName] as SalesInvoice;

      if (this.repeatingInvoice) {
        this.objects.splice(0, this.objects.length);
        this.objects.push(this.repeatingInvoice);
      }

      this.table.data = this.objects.map((v) => {
        return {
          object: v,
          frequency: v.Frequency.Name,
          dayOfWeek: v.DayOfWeek && v.DayOfWeek.Name,
          previousExecutionDate: v.PreviousExecutionDate && format(new Date(v.PreviousExecutionDate), 'dd-MM-yyyy'),
          nextExecutionDate: v.NextExecutionDate && format(new Date(v.NextExecutionDate), 'dd-MM-yyyy'),
          finalExecutionDate: v.FinalExecutionDate && format(new Date(v.FinalExecutionDate), 'dd-MM-yyyy'),
        } as Row;
      });
    };
  }
}
