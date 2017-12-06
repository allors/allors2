import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { WorkTaskOverviewComponent } from "./worktask-overview.component";
export { WorkTaskOverviewComponent } from "./worktask-overview.component";

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
