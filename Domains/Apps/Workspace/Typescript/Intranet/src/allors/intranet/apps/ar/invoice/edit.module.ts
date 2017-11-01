import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { InvoiceEditComponent } from "./edit.component";
export { InvoiceEditComponent } from "./edit.component";

@NgModule({
  declarations: [
    InvoiceEditComponent,
  ],
  exports: [
    InvoiceEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class InvoiceEditModule {}
