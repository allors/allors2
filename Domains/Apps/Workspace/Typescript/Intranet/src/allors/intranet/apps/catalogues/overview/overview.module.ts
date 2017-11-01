import { NgModule } from "@angular/core";
import { SharedModule } from "../.././../shared.module";

import { OverviewComponent } from "./overview.component";
export { OverviewComponent } from "./overview.component";

@NgModule({
  declarations: [
    OverviewComponent,
  ],
  exports: [
    OverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class OverviewModule {}
