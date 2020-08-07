import { Component, Self, OnInit, OnDestroy, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';
import { PrintService } from '../../../../services/actions';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'worktask-overview-summary',
  templateUrl: './worktask-overview-summary.component.html',
  providers: [PanelService, PrintService]
})
export class WorkTaskOverviewSummaryComponent {

  m: Meta;

  workTask: WorkTask;
  parent: WorkTask;

  print: Action;
  printForWorker: Action;
  salesInvoices: Set<SalesInvoice>;
  assets: FixedAsset[];

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
    this.printForWorker = {
      name: 'printforworker',
      displayName: () => 'printforworker',
      description: () => 'printforworker',
      disabled: () => false,
      execute: (target: ActionTarget) => {

        const printable = target as Printable;

        const url = `${this.printService.config.url}printforworker/${printable.id}`;
        window.open(url);
      },
      result: null,
    };

    panel.name = 'summary';

    const workTaskPullName = `${panel.name}_${this.m.WorkTask.name}`;
    const serviceEntryPullName = `${panel.name}_${this.m.ServiceEntry.name}`;
    const workEffortBillingPullName = `${panel.name}_${this.m.WorkEffortBilling.name}`;
    const fixedAssetPullName = `${panel.name}_${this.m.FixedAsset.name}`;
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
        pull.WorkEffort({
          name: fixedAssetPullName,
          object: id,
          fetch: {
            WorkEffortFixedAssetAssignmentsWhereAssignment: {
              FixedAsset: x,
            }
          }
        }),
        pull.TimeEntryBilling({
          name: serviceEntryPullName,
          predicate:
            new ContainedIn({
              propertyType: m.TimeEntryBilling.TimeEntry,
              extent: new Extent({
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

      this.assets = loaded.collections[fixedAssetPullName] as FixedAsset[];

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

  public revise(): void {

    this.panel.manager.context.invoke(this.workTask.Revise)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Revise successfully executed.', 'close', { duration: 5000 });
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
