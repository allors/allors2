import { NgModule } from '@angular/core';


import { WorkTasksOverviewComponent } from './worktasks-overview.component';
import { FormsModule } from '@angular/forms';
export { WorkTasksOverviewComponent } from './worktasks-overview.component';

@NgModule({
  declarations: [
    WorkTasksOverviewComponent,
  ],
  exports: [
    WorkTasksOverviewComponent,
    
  ],
  imports: [
    FormsModule
  ],
})
export class WorkTasksOverviewModule {}
