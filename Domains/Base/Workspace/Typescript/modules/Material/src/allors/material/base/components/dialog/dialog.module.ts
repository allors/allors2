import { NgModule } from '@angular/core';

import { MatDialogModule, MatButtonModule, MatFormFieldModule, MatInputModule } from '@angular/material';
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
  ],
})
export class AllorsMaterialDialogModule {
}
