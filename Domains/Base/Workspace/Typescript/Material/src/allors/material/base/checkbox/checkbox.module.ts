import { NgModule } from "@angular/core";

import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

import { MatCheckboxModule, MatInputModule } from "@angular/material";

import { CheckboxComponent } from "./checkbox.component";
export { CheckboxComponent } from "./checkbox.component";

@NgModule({
  declarations: [
    CheckboxComponent,
  ],
  exports: [
    CheckboxComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    MatCheckboxModule,
  ],
})
export class CheckboxModule {
}
