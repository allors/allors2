import { NgModule } from "@angular/core";

import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

import { MatDatepickerModule, MatIconModule, MatInputModule } from "@angular/material";

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
  ],
})
export class DatepickerModule {
}
