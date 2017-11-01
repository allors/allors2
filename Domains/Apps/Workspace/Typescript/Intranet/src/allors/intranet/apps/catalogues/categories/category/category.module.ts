import { NgModule } from "@angular/core";

import { InlineModule } from "../../../../inline.module";
import { SharedModule } from "../../../../shared.module";

import { CategoryComponent } from "./category.component";
export { CategoryComponent } from "./category.component";

@NgModule({
  declarations: [
    CategoryComponent,
  ],
  exports: [
    CategoryComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class CategoryModule {}
