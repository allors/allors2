import { NgModule } from "@angular/core";

import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

import { MatSelectModule } from "@angular/material";

import { SelectComponent } from "./select.component";
export { SelectComponent } from "./select.component";

@NgModule({
  declarations: [
    SelectComponent,
  ],
  exports: [
    SelectComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatSelectModule,
  ],
})
export class SelectModule {
}
