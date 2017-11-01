import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { ProductTypeEditComponent } from "./edit.component";
export { ProductTypeEditComponent } from "./edit.component";

@NgModule({
  declarations: [
    ProductTypeEditComponent,
  ],
  exports: [
    ProductTypeEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class ProductTypeEditModule {}
