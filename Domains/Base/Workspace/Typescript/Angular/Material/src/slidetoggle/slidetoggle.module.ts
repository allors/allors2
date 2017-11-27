import { NgModule } from "@angular/core";

import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

import { MatInputModule, MatSlideToggleModule } from "@angular/material";

import { SlideToggleComponent } from "./slidetoggle.component";
export { SlideToggleComponent } from "./slidetoggle.component";

@NgModule({
  declarations: [
    SlideToggleComponent,
  ],
  exports: [
    SlideToggleComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    MatSlideToggleModule,
  ],
})
export class SlideToggleModule {
}
