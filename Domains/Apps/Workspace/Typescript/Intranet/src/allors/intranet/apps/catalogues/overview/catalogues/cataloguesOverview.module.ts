import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { CataloguesOverviewComponent } from "./cataloguesOverview.component";
export { CataloguesOverviewComponent } from "./cataloguesOverview.component";

@NgModule({
  declarations: [
    CataloguesOverviewComponent,
  ],
  exports: [
    CataloguesOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class CataloguesOverviewModule {}
