import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { ProductTypesOverviewComponent } from "./productTypesOverview.component";
export { ProductTypesOverviewComponent } from "./productTypesOverview.component";

@NgModule({
  declarations: [
    ProductTypesOverviewComponent,
  ],
  exports: [
    ProductTypesOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class ProductTypesOverviewModule {}
