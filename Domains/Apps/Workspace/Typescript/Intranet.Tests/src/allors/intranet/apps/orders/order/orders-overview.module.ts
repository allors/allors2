import { NgModule } from "@angular/core";
import { SharedModule } from "../../../shared.module";

import { OrdersOverviewComponent } from "./orders-overview.component";
export { OrdersOverviewComponent } from "./orders-overview.component";

@NgModule({
  declarations: [
    OrdersOverviewComponent,
  ],
  exports: [
    OrdersOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class OrdersOverviewModule {}
