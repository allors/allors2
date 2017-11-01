import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { CategoriesOverviewComponent } from "./categoriesOverview.component";
export { CategoriesOverviewComponent } from "./categoriesOverview.component";

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
