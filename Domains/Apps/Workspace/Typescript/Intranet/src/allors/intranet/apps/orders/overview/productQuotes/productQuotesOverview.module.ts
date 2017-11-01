import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { ProductQuotesOverviewComponent } from "./productQuotesOverview.component";
export { ProductQuotesOverviewComponent } from "./productQuotesOverview.component";

@NgModule({
  declarations: [
    ProductQuotesOverviewComponent,
  ],
  exports: [
    ProductQuotesOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class ProductQuotesOverviewModule {}
