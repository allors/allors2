import { NgModule } from "@angular/core";

import { InlineModule } from "../../../../../inline.module";
import { SharedModule } from "../../../../../shared.module";

import { InvoiceInvoiceItemComponent } from "./invoice-invoiceitem.component";
export { InvoiceInvoiceItemComponent } from "./invoice-invoiceitem.component";

@NgModule({
  declarations: [
    InvoiceInvoiceItemComponent,
  ],
  exports: [
    InvoiceInvoiceItemComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class InvoiceInvoiceItemModule {}
