import { NgModule } from "@angular/core";

import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

import { MatInputModule } from "@angular/material";

import { LocalisedTextComponent } from "./localisedtext.component";
export { LocalisedTextComponent } from "./localisedtext.component";
export { LocalisedTextModel } from "./LocalisedTextModel";

@NgModule({
  declarations: [
    LocalisedTextComponent,
  ],
  exports: [
    LocalisedTextComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
  ],
})
export class LocalisedTextModule {
}
