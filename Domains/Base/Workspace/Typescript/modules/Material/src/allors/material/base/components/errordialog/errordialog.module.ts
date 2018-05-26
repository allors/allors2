import { NgModule } from '@angular/core';

import { AllorsMaterialErrorDialogComponent } from './errordialog.component';
import { MatDialogModule, MatButtonModule } from '@angular/material';
import { CommonModule } from '@angular/common';
export { AllorsMaterialErrorDialogComponent };

@NgModule({
  declarations: [
    AllorsMaterialErrorDialogComponent,
  ],
  entryComponents: [
    AllorsMaterialErrorDialogComponent
  ],
  exports: [
    AllorsMaterialErrorDialogComponent,
  ],
  imports: [
    MatDialogModule,
    MatButtonModule,
    CommonModule,
  ],
})
export class AllorsMaterialErrorDialogModule {
}
