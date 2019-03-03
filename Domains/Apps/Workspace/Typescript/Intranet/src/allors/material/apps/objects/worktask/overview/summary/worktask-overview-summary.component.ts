import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService, RefreshService, Invoked, ErrorService, Action } from '../../../../../../angular';
import { WorkTask } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { MatSnackBar } from '@angular/material';
import { PrintService } from 'src/allors/material';

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

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService,
    public refreshService: RefreshService,
    public printService: PrintService,
    public snackBar: MatSnackBar,
    public errorService: ErrorService) {

    this.m = this.metaService.m;

    this.print = printService.print();

    panel.name = 'summary';

    const workTaskPullName = `${panel.name}_${this.m.WorkTask.name}`;
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
          }
        }),
        pull.WorkTask({
          name: parentPullName,
          object: id,
          fetch: {
            WorkEffortWhereChild: x
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.workTask = loaded.objects[workTaskPullName] as WorkTask;
      this.parent = loaded.objects[parentPullName] as WorkTask;
    };
  }

  public cancel(): void {

    this.panel.manager.context.invoke(this.workTask.Cancel)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public reopen(): void {

    this.panel.manager.context.invoke(this.workTask.Reopen)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public complete(): void {

    this.panel.manager.context.invoke(this.workTask.Complete)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully completed.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public invoice(): void {

    this.panel.manager.context.invoke(this.workTask.Invoice)
      .subscribe((invoked: Invoked) => {
        this.refreshService.refresh();
        this.snackBar.open('Successfully invoiced.', 'close', { duration: 5000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }
}
