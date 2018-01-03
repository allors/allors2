import { NgModule } from "@angular/core";

import { InlineModule } from "../../inline.module";
import { SharedModule } from "../../shared.module";

import { SalesTermEditComponent } from "./salesterm.component";
export { SalesTermEditComponent } from "./salesterm.component";

@NgModule({
  declarations: [
    SalesTermEditComponent,
  ],
  exports: [
    SalesTermEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class SalesTermEditModule {}
