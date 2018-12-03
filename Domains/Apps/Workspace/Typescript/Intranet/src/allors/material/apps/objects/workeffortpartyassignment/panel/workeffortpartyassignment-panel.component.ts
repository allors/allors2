import { Component, Self } from '@angular/core';
import { NavigationService, SessionService, Action, AllorsPanelService, AllorsRefreshService, ErrorService } from '../../../../../angular';
import { WorkEffortPartyAssignment, WorkEffort } from '../../../../../domain';
import { MetaDomain } from '../../../../../meta';
import { DeleteService } from '../../../../../material';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'workeffortpartyassignment-panel',
  templateUrl: './workeffortpartyassignment-panel.component.html',
  providers: [AllorsPanelService]
})
export class WorkEffortPartyAssignmentPanelComponent {

  m: MetaDomain;

  workEffortPartyAssignments: WorkEffortPartyAssignment[];

  delete: Action;

  constructor(
    public allors: SessionService,
    @Self() public panelService: AllorsPanelService,
    public refreshService: AllorsRefreshService,
    public navigation: NavigationService,
    public errorService: ErrorService,
    public deleteService: DeleteService,
  ) {

    this.m = this.allors.m;
    this.delete = deleteService.delete(allors);

    panelService.name = 'workeffortpartyassignment';
    panelService.title = 'Work Efforts';
    panelService.icon = 'work';
    panelService.maximizable = true;

    const workEffortPartyAssignmentPullName = `${panelService.name}_${this.m.WorkEffortPartyAssignment.objectType.name}`;

    panelService.prePull = (pulls) => {
      const { m, pull, tree, x } = this.allors;

      const id = this.panelService.panelsService.id;

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

    panelService.postPull = (loaded) => {
      this.workEffortPartyAssignments = loaded.collections[workEffortPartyAssignmentPullName] as WorkEffortPartyAssignment[];
    };
  }

}
