import { NgModule } from "@angular/core";

import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

import { MatIconModule, MatInputModule, MatSelectModule } from "@angular/material";

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
    MatInputModule,
    MatIconModule,
    MatSelectModule,
  ],
})
export class SelectModule {
}
