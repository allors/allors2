import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { WorkEffortAssignmentsOverviewComponent } from './workeffortassignments-overview.component';
export { WorkEffortAssignmentsOverviewComponent } from './workeffortassignments-overview.component';

@NgModule({
  declarations: [
    WorkEffortAssignmentsOverviewComponent,
  ],
  exports: [
    WorkEffortAssignmentsOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class WorkEffortAssignmentsOverviewModule {}
