import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { WorkTaskOverviewComponent } from "./workTaskOverview.component";
export { WorkTaskOverviewComponent } from "./workTaskOverview.component";

@NgModule({
  declarations: [
    WorkTaskOverviewComponent,
  ],
  exports: [
    WorkTaskOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class WorkTaskOverviewModule {}
