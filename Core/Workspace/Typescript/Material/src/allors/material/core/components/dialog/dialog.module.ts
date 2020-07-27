import { NgModule } from '@angular/core';

import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

import { AllorsMaterialDialogComponent } from './dialog.component';
import { FormsModule } from '@angular/forms';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
export { AllorsMaterialDialogComponent };

@NgModule({
  declarations: [
    AllorsMaterialDialogComponent,
  ],
  entryComponents: [
    AllorsMaterialDialogComponent
  ],
  exports: [
    AllorsMaterialDialogComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,

    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    MatDatepickerModule
  ],
})
export class AllorsMaterialDialogModule {
}
