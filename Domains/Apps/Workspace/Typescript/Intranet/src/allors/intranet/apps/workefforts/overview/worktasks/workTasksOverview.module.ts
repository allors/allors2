import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { WorkTasksOverviewComponent } from "./workTasksOverview.component";
export { WorkTasksOverviewComponent } from "./workTasksOverview.component";

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
