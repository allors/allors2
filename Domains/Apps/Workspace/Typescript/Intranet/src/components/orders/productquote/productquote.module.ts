import { NgModule } from "@angular/core";

import { InlineModule } from "../../inline.module";
import { SharedModule } from "../../shared.module";

import { ProductQuoteEditComponent } from "./productquote.component";
export { ProductQuoteEditComponent } from "./productquote.component";

@NgModule({
  declarations: [
    ProductQuoteEditComponent,
  ],
  exports: [
    ProductQuoteEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class ProductQuoteEditModule {}
