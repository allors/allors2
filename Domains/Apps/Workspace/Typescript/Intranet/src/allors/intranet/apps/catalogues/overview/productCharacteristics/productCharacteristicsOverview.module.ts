import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { ProductCharacteristicsOverviewComponent } from "./productCharacteristicsOverview.component";
export { ProductCharacteristicsOverviewComponent } from "./productCharacteristicsOverview.component";

@NgModule({
  declarations: [
    ProductCharacteristicsOverviewComponent,
  ],
  exports: [
    ProductCharacteristicsOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class ProductCharacteristicsOverviewModule {}
