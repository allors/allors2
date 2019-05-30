import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
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
