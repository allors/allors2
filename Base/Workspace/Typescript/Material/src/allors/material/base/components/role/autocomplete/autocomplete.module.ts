import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

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
