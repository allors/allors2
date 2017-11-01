import { NgModule } from "@angular/core";

import { InlineModule } from "../../../inline.module";
import { SharedModule } from "../../../shared.module";

import { SalesOrderEditComponent } from "./edit.component";
export { SalesOrderEditComponent } from "./edit.component";

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
