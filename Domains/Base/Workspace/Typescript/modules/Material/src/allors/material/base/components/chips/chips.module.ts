import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatChipsModule, MatIconModule, MatInputModule } from '@angular/material';

import { AllorsMaterialChipsComponent } from './chips.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { AllorsMaterialChipsComponent } from './chips.component';

@NgModule({
  declarations: [
    AllorsMaterialChipsComponent,
  ],
  exports: [
    AllorsMaterialChipsComponent,
  ],
  imports: [
    CommonModule,
    FlexLayoutModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatChipsModule,
    MatIconModule,
    MatInputModule,
  ],
})
export class AllorsMaterialChipsModule {}
