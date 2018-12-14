import { Component, Self } from '@angular/core';
import { NavigationService, ContextService, Action, PanelService, RefreshService, ErrorService, MetaService } from '../../../../../../angular';
import { WorkEffortPartyAssignment, WorkEffort } from '../../../../../../domain';
import { Meta } from '../../../../../../meta';
import { DeleteService } from '../../../../..';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'workeffortpartyassignment-overview-panel',
  templateUrl: './workeffortpartyassignment-overview-panel.component.html',
  providers: [PanelService]
})
export class WorkEffortPartyAssignmentOverviewPanelComponent {

  m: Meta;

  workEffortPartyAssignments: WorkEffortPartyAssignment[];

  delete: Action;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public refreshService: RefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
  ) {

    this.m = this.metaService.m;
    this.delete = deleteService.delete(panel.manager.context);

    panel.name = 'workeffortpartyassignment';
    panel.title = 'Work Efforts';
    panel.icon = 'work';
    panel.expandable = true;

    const workEffortPartyAssignmentPullName = `${panel.name}_${this.m.WorkEffortPartyAssignment.name}`;

    panel.onPull = (pulls) => {
      const { pull,  x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.Person({
          name: workEffortPartyAssignmentPullName,
          object: id,
          fetch: {
            WorkEffortPartyAssignmentsWhereParty: {
              include: {
                Assignment: {
                  WorkEffortState: x,
                  Priority: x,
                }
              }
            }
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.workEffortPartyAssignments = loaded.collections[workEffortPartyAssignmentPullName] as WorkEffortPartyAssignment[];
    };
  }

}
