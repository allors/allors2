import { NgModule } from '@angular/core';

import { MatDialogModule, MatButtonModule, MatFormFieldModule, MatInputModule } from '@angular/material';
import { CommonModule } from '@angular/common';

import { DialogComponent } from './dialog.component';
import { FormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
export { DialogComponent };

@NgModule({
  declarations: [
    DialogComponent,
  ],
  entryComponents: [
    DialogComponent
  ],
  exports: [
    DialogComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    FlexLayoutModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
  ],
})
export class DialogModule {
}
