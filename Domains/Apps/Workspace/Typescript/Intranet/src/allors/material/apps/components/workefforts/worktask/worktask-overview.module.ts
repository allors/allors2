import { NgModule } from '@angular/core';


import { WorkTaskOverviewComponent } from './worktask-overview.component';
import { FormsModule } from '@angular/forms';
export { WorkTaskOverviewComponent } from './worktask-overview.component';

@NgModule({
  declarations: [
    WorkTaskOverviewComponent,
  ],
  exports: [
    WorkTaskOverviewComponent,
    
  ],
  imports: [
    FormsModule
  ],
})
export class WorkTaskOverviewModule {}
