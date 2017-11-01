import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { GoodsOverviewComponent } from "./goodsOverview.component";
export { GoodsOverviewComponent } from "./goodsOverview.component";

@NgModule({
  declarations: [
    GoodsOverviewComponent,
  ],
  exports: [
    GoodsOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class GoodsOverviewModule {}
