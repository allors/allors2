import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatChipsModule, MatIconModule, MatInputModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialChipsComponent } from './chips.component';
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
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatChipsModule,
    MatIconModule,
    MatInputModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialChipsModule {}
