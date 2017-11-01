import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { CategoryEditComponent } from "./edit.component";
export { CategoryEditComponent } from "./edit.component";

@NgModule({
  declarations: [
    CategoryEditComponent,
  ],
  exports: [
    CategoryEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class CategoryEditModule {}
