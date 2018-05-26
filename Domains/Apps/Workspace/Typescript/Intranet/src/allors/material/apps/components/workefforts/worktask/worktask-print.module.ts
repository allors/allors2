import { NgModule } from '@angular/core';


import { WorkTaskPrintComponent } from './worktask-print.component';
import { FormsModule } from '@angular/forms';
export { WorkTaskPrintComponent } from './worktask-print.component';

@NgModule({
  declarations: [
    WorkTaskPrintComponent,
  ],
  exports: [
    WorkTaskPrintComponent,
    
  ],
  imports: [
    FormsModule
  ],
})
export class WorkTaskPrintModule {}
