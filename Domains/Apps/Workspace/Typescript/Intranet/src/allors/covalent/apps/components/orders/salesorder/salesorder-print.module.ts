import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { SalesOrderPrintComponent } from "./salesorder-print.component";
export { SalesOrderPrintComponent } from "./salesorder-print.component";

@NgModule({
  declarations: [
    SalesOrderPrintComponent,
  ],
  exports: [
    SalesOrderPrintComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class SalesOrderPrintModule {}
