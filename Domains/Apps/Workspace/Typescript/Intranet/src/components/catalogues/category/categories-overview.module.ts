import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { CategoriesOverviewComponent } from "./categories-overview.component";
export { CategoriesOverviewComponent } from "./categories-overview.component";

@NgModule({
  declarations: [
    CategoriesOverviewComponent,
  ],
  exports: [
    CategoriesOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class CategoriesOverviewModule {}
