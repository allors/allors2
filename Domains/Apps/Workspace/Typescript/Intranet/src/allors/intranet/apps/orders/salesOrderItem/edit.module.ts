import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { SalesOrderItemEditComponent } from "./edit.component";
export { SalesOrderItemEditComponent } from "./edit.component";

@NgModule({
  declarations: [
    SalesOrderItemEditComponent,
  ],
  exports: [
    SalesOrderItemEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class SalesOrderItemEditModule {}
