import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { InvoiceComponent } from "./invoice.component";
export { InvoiceComponent } from "./invoice.component";

@NgModule({
  declarations: [
    InvoiceComponent,
  ],
  exports: [
    InvoiceComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class InvoiceModule {}
