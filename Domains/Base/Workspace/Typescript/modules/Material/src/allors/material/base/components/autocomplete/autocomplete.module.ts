import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatInputModule } from '@angular/material';

import { AutocompleteComponent } from './autocomplete.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { AutocompleteComponent } from './autocomplete.component';

@NgModule({
  declarations: [
    AutocompleteComponent,
  ],
  exports: [
    AutocompleteComponent,
  ],
  imports: [
    CommonModule,
    FlexLayoutModule,
    FormsModule,
    MatAutocompleteModule,
    MatInputModule,
    ReactiveFormsModule,
  ],
})
export class AutoCompleteModule {
}
