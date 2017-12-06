import { NgModule } from "@angular/core";

import { InlineModule } from "../../inline.module";
import { SharedModule } from "../../shared.module";

import { QuoteItemEditComponent } from "./quoteitem.component";
export { QuoteItemEditComponent } from "./quoteitem.component";

@NgModule({
  declarations: [
    QuoteItemEditComponent,
  ],
  exports: [
    QuoteItemEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class QuoteItemEditModule {}
