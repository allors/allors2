import { NgModule } from "@angular/core";
import { SharedModule } from "../../shared.module";

import { WorkTasksOverviewComponent } from "./worktasks-overview.component";
export { WorkTasksOverviewComponent } from "./worktasks-overview.component";

@NgModule({
  declarations: [
    WorkTasksOverviewComponent,
  ],
  exports: [
    WorkTasksOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class WorkTasksOverviewModule {}
