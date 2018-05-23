import { NgModule } from '@angular/core';

import { ErrorDialogComponent } from './errordialog.component';
import { MatDialogModule, MatButtonModule } from '@angular/material';
import { CommonModule } from '@angular/common';
export { ErrorDialogComponent };

@NgModule({
  declarations: [
    ErrorDialogComponent,
  ],
  entryComponents: [
    ErrorDialogComponent
  ],
  exports: [
    ErrorDialogComponent,
  ],
  imports: [
    MatDialogModule,
    MatButtonModule,
    CommonModule,
  ],
})
export class ErrorDialogModule {
}
