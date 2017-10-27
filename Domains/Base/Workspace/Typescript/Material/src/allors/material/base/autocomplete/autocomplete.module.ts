import { NgModule } from "@angular/core";

import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { MatAutocompleteModule, MatChipsModule, MatIconModule, MatInputModule } from "@angular/material";

import { AutocompleteComponent } from "./autocomplete.component";
export { AutocompleteComponent } from "./autocomplete.component";

@NgModule({
  declarations: [
    AutocompleteComponent,
  ],
  exports: [
    AutocompleteComponent,
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    MatInputModule,
    MatIconModule,
    MatChipsModule,
    MatAutocompleteModule,
  ],
})
export class AutoCompleteModule {
}
