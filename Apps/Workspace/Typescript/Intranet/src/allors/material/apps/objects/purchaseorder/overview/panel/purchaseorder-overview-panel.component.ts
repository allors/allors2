import { Component, Self, HostBinding } from '@angular/core';

import { PanelService, MetaService, RefreshService, Action, NavigationService, TestScope, ContextService } from '../../../../../../angular';
import { PurchaseOrder } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService, TableRow, Table, OverviewService, PrintService, MethodService } from '../../../../..';
import { ObjectService, ObjectData } from '../../../../../base/services/object';

interface Row extends TableRow {
  object: PurchaseOrder;
  type: string;
  description: string;
  reference: string;
  totalExVat: string;
  state: string;
}

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'purchaseorder-overview-panel',
  templateUrl: './purchaseorder-overview-panel.component.html',
  providers: [ContextService, PanelService]
})
export class PurchaseOrderOverviewPanelComponent extends TestScope {

  @HostBinding('class.expanded-panel') get expandedPanelClass() {
    return this.panel.isExpanded;
  }

  m: Meta;

  objects: PurchaseOrder[];
  table: Table<Row>;

  delete: Action;
  invoice: Action;
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
    public metaService: MetaService,
    public objectService: ObjectService,
    public methodService: MethodService,
    public refreshService: RefreshService,
    public navigationService: NavigationService,
    public overviewService: OverviewService,
    public deleteService: DeleteService,
    public printService: PrintService,
  ) {
    super();

    this.m = this.metaService.m;

    this.panel.name = 'purchaseorder';
    this.panel.title = 'Purchase Orders';
    this.panel.icon = 'message';
    this.panel.expandable = true;

    this.delete = this.deleteService.delete(this.panel.manager.context);
    this.invoice = methodService.create(allors.context, this.m.PurchaseOrder.Invoice, { name: 'Invoice' });
    this.invoice.result.subscribe((v) => {
      this.table.selection.clear();
    });

    const sort = true;
    this.table = new Table({
      selection: true,
      columns: [
        { name: 'type', sort },
        { name: 'description', sort },
        { name: 'reference', sort },
        { name: 'totalExVat', sort },
        { name: 'state', sort },
      ],
      actions: [
        this.overviewService.overview(),
        this.printService.print(),
        this.delete,
      ],
      defaultAction: this.overviewService.overview(),
      autoSort: true,
      autoFilter: true,
    });

    const pullName = `${this.panel.name}_${this.m.PurchaseOrder.name}`;

    this.panel.onPull = (pulls) => {
      const { x, pull } = this.metaService;
      const { id } = this.panel.manager;

      pulls.push(
        pull.Organisation({
          name: pullName,
          object: id,
          fetch: {
            PurchaseOrdersWhereTakenViaSupplier: {
              include: {
                PurchaseOrderState: x,
                PurchaseOrderPaymentState: x,
                PrintDocument: {
                  Media: x
                },
              }
            }
          }
        }));
  };

    this.panel.onPulled = (loaded) => {
      this.objects = loaded.collections[pullName] as PurchaseOrder[];

      if (this.objects) {
        this.table.total = loaded.values[`${pullName}_total`] || this.objects.length;
        this.table.data = this.objects.map((v) => {
          return {
            object: v,
            type: v.objectType.name,
            description: v.Description,
            reference: v.CustomerReference,
            totalExVat: v.TotalExVat.toString(),
            state: v.PurchaseOrderState.Name,
          } as Row;
        });
      }
    };
  }
}
