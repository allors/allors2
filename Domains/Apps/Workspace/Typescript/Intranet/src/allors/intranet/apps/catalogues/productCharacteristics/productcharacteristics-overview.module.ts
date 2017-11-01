import { NgModule } from "@angular/core";
import { SharedModule } from "../../../shared.module";

import { ProductCharacteristicsOverviewComponent } from "./productcharacteristics-overview.component";
export { ProductCharacteristicsOverviewComponent } from "./productcharacteristics-overview.component";

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
