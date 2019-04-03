import { NgModule } from '@angular/core';
import { MatDialogModule, MatButtonModule } from '@angular/material';
import { CommonModule } from '@angular/common';

import { AllorsMaterialErrorDialogComponent } from './errordialog.component';
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
