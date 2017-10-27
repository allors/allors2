import { NgModule } from "@angular/core";

import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

import { MatSlideToggleModule } from "@angular/material";

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
    MatSlideToggleModule,
  ],
})
export class SlideToggleModule {
}
