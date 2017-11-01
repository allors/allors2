import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { WorkEffortsOverviewComponent } from "./workEffortsOverview.component";
export { WorkEffortsOverviewComponent } from "./workEffortsOverview.component";

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
