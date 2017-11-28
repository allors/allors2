import { NgModule } from "@angular/core";
import { SharedModule } from "../../../shared.module";

import { ProductQuotePrintComponent } from "./productquote-print.component";
export { ProductQuotePrintComponent } from "./productquote-print.component";

@NgModule({
  declarations: [
    ProductQuotePrintComponent,
  ],
  exports: [
    ProductQuotePrintComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class ProductQuotePrintModule {}
