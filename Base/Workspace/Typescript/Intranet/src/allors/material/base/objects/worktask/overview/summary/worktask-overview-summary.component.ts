import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, RefreshService, Invoked, Action } from '../../../../../../angular';
import { WorkTask, SalesInvoice } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PrintService, SaveService } from '../../../../../../../allors/material';
import { Equals, And, ContainedIn, Filter } from '../../../../../../../allors/framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'worktask-overview-summary',
  templateUrl: './worktask-overview-summary.component.html',
  providers: [PanelService]
})
export class WorkTaskOverviewSummaryComponent {

  m: Meta;

  workTask: WorkTask;
  parent: WorkTask;

  print: Action;
  salesInvoices: Set<SalesInvoice>;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    public refreshService: RefreshService,
    public printService: PrintService,
    private saveService: SaveService,
    public snackBar: MatSnackBar) {

    this.m = this.metaService.m;

    this.print = printService.print();

    panel.name = 'summary';

    const workTaskPullName = `${panel.name}_${this.m.WorkTask.name}`;
    const serviceEntryPullName = `${panel.name}_${this.m.ServiceEntry.name}`;
    const workEffortBillingPullName = `${panel.name}_${this.m.WorkEffortBilling.name}`;
    const parentPullName = `${panel.name}_${this.m.WorkTask.name}_parent`;

    panel.onPull = (pulls) => {
      const { m, pull, tree, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.WorkTask({
          name: workTaskPullName,
          object: id,
          include: {
            Customer: x,
            WorkEffortState: x,
            LastModifiedBy: x,
            PrintDocument: {
              Media: x
            }
          }
        }),
        pull.WorkTask({
          name: parentPullName,
          object: id,
          fetch: {
            WorkEffortWhereChild: x
          }
        }),
        pull.WorkEffort({
          name: workEffortBillingPullName,
          object: id,
          fetch: {
            WorkEffortBillingsWhereWorkEffort: {
              InvoiceItem: {
                SalesInvoiceItem_SalesInvoiceWhereSalesInvoiceItem: x
              }
            }
          }
        }),
        pull.TimeEntryBilling({
          name: serviceEntryPullName,
          predicate:
            new ContainedIn({
              propertyType: m.TimeEntryBilling.TimeEntry,
              extent: new Filter({
                objectType: m.ServiceEntry,
                predicate: new Equals({
                  propertyType: m.ServiceEntry.WorkEffort, object: id
                })
              })
            }),
          fetch: {
            InvoiceItem: {
              SalesInvoiceItem_SalesInvoiceWhereSalesInvoiceItem: x
            }
          }
        }),
      );
    };

    panel.onPulled = (loaded) => {
      this.workTask = loaded.objects[workTaskPullName] as WorkTask;
      this.parent = loaded.objects[parentPullName] as WorkTask;
      const salesInvoices1 = loaded.collections[workEffortBillingPullName] as SalesInvoice[];
      const salesInvoices2 = loaded.collections[serviceEntryPullName] as SalesInvoice[];
      this.salesInvoices = new Set([...salesInvoices1, ...salesInvoices2]);
    };
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.workTask.Cancel)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public reopen(): void {

    this.panel.manager.context.invoke(this.workTask.Reopen)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public complete(): void {

    this.panel.manager.context.invoke(this.workTask.Complete)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully completed.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }

  public invoice(): void {

    this.panel.manager.context.invoke(this.workTask.Invoice)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully invoiced.', 'close', { duration: 5000 });
      },
      this.saveService.errorHandler);
  }
}
