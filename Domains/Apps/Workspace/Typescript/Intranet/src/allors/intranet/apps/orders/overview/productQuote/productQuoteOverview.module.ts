import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { ProductQuoteOverviewComponent } from "./productQuoteOverview.component";
export { ProductQuoteOverviewComponent } from "./productQuoteOverview.component";

@NgModule({
  declarations: [
    ProductQuoteOverviewComponent,
  ],
  exports: [
    ProductQuoteOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class ProductQuoteOverviewModule {}
