import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatInputModule } from '@angular/material';

import { AllorsMaterialAutocompleteComponent } from './autocomplete.component';
import { FlexLayoutModule } from '@angular/flex-layout';
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
    FlexLayoutModule,
    FormsModule,
    MatAutocompleteModule,
    MatInputModule,
    ReactiveFormsModule,
  ],
})
export class AllorsMaterialAutoCompleteModule {
}
