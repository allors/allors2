import { NgModule } from '@angular/core';
import { MatDialogModule, MatButtonModule } from '@angular/material';
import { CommonModule } from '@angular/common';

import { AllorsFocusModule } from '../../../../angular';

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
    AllorsFocusModule
  ],
})
export class AllorsMaterialErrorDialogModule {
}
