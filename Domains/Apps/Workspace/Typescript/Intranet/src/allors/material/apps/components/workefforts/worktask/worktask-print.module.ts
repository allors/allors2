import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { WorkTaskPrintComponent } from './worktask-print.component';
export { WorkTaskPrintComponent } from './worktask-print.component';

@NgModule({
  declarations: [
    WorkTaskPrintComponent,
  ],
  exports: [
    WorkTaskPrintComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class WorkTaskPrintModule {}
