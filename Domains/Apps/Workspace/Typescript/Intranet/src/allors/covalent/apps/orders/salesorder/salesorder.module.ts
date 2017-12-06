import { NgModule } from "@angular/core";

import { InlineModule } from "../../inline.module";
import { SharedModule } from "../../shared.module";

import { SalesOrderEditComponent } from "./salesorder.component";
export { SalesOrderEditComponent } from "./salesorder.component";

@NgModule({
  declarations: [
    SalesOrderEditComponent,
  ],
  exports: [
    SalesOrderEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class SalesOrderEditModule {}
