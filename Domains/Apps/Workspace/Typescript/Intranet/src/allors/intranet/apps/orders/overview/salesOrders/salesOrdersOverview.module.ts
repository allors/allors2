import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { SalesOrdersOverviewComponent } from "./salesOrdersOverview.component";
export { SalesOrdersOverviewComponent } from "./salesOrdersOverview.component";

@NgModule({
  declarations: [
    SalesOrdersOverviewComponent,
  ],
  exports: [
    SalesOrdersOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class SalesOrdersOverviewModule {}
