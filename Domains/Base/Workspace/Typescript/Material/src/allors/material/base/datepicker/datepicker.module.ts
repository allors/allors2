import { NgModule } from "@angular/core";

import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

import { MatAutocompleteModule, MatDatepickerModule, MatIconModule, MatInputModule } from "@angular/material";

import { DatepickerComponent } from "./datepicker.component";
export { DatepickerComponent } from "./datepicker.component";

@NgModule({
  declarations: [
    DatepickerComponent,
  ],
  exports: [
    DatepickerComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    MatIconModule,
    MatDatepickerModule,
    MatAutocompleteModule,
  ],
})
export class DatepickerModule {
}
