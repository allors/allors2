import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { InvoicePrintComponent } from "./invoice-print.component";
export { InvoicePrintComponent } from "./invoice-print.component";

@NgModule({
  declarations: [
    InvoicePrintComponent,
  ],
  exports: [
    InvoicePrintComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class InvoicePrintModule {}
