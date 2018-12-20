import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatInputModule, MatIconModule, MatButtonModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialAutocompleteComponent } from './autocomplete.component';
export { AllorsMaterialAutocompleteComponent } from './autocomplete.component';

@NgModule({
  declarations: [
    AllorsMaterialAutocompleteComponent,
  ],
  exports: [
    AllorsMaterialAutocompleteComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    ReactiveFormsModule,
    AllorsFocusModule,
  ],
})
export class AllorsMaterialAutoCompleteModule {
}
