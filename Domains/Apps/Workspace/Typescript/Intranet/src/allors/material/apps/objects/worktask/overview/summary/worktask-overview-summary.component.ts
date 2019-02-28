import { Component, Self } from '@angular/core';
import { PanelService, NavigationService, MetaService } from '../../../../../../angular';
import { WorkTask } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';

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

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

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
}
