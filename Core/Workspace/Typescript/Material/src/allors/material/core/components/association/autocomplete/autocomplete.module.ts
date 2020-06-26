import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialAssociationAutoCompleteComponent } from './autocomplete.component';
export { AllorsMaterialAssociationAutoCompleteComponent } from './autocomplete.component';

@NgModule({
  declarations: [
    AllorsMaterialAssociationAutoCompleteComponent,
  ],
  exports: [
    AllorsMaterialAssociationAutoCompleteComponent,
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
export class AllorsMaterialAssociationAutoCompleteModule {
}
