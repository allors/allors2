import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { InvoiceTermEditComponent } from "./invoiceterm.component";
export { InvoiceTermEditComponent } from "./invoiceterm.component";

@NgModule({
  declarations: [
    InvoiceTermEditComponent,
  ],
  exports: [
    InvoiceTermEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class InvoiceTermEditModule {}
