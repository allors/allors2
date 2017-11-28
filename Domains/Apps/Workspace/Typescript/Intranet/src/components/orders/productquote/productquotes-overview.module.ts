import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { ProductQuotesOverviewComponent } from "./productquotes-overview.component";
export { ProductQuotesOverviewComponent } from "./productquotes-overview.component";

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
