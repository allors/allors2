import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { WorkEffortsOverviewComponent } from "./workefforts-overview.component";
export { WorkEffortsOverviewComponent } from "./workefforts-overview.component";

@NgModule({
  declarations: [
    WorkEffortsOverviewComponent,
  ],
  exports: [
    WorkEffortsOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class WorkEffortsOverviewModule {}
