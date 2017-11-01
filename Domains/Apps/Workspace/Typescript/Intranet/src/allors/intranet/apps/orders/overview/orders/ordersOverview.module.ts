import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { OrdersOverviewComponent } from "./ordersOverview.component";
export { OrdersOverviewComponent } from "./ordersOverview.component";

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
