import { NgModule } from '@angular/core';
import { MatButtonModule, MatDialogModule } from '@angular/material';

import { NewGoodDialogComponent } from './newgood-dialog-component';
export { NewGoodDialogComponent } from './newgood-dialog-component';

@NgModule({
  declarations: [
    NewGoodDialogComponent,
  ],
  entryComponents: [
    NewGoodDialogComponent,
  ],
  exports: [
    NewGoodDialogComponent,
    MatDialogModule,
    
  ],
  imports: [
    
      MatDialogModule,
      MatButtonModule,
    
  ],
})
export class NewGoodDialogModule {}
