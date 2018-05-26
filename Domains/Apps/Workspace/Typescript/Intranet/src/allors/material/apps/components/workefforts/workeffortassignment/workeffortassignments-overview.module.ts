import { NgModule } from '@angular/core';


import { WorkEffortAssignmentsOverviewComponent } from './workeffortassignments-overview.component';
import { FormsModule } from '@angular/forms';
export { WorkEffortAssignmentsOverviewComponent } from './workeffortassignments-overview.component';

@NgModule({
  declarations: [
    WorkEffortAssignmentsOverviewComponent,
  ],
  exports: [
    WorkEffortAssignmentsOverviewComponent,
    
  ],
  imports: [
    FormsModule
  ],
})
export class WorkEffortAssignmentsOverviewModule {}
