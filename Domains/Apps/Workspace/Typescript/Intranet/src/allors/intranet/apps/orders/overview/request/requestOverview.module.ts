import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { RequestOverviewComponent } from "./requestOverview.component";
export { RequestOverviewComponent } from "./requestOverview.component";

@NgModule({
  declarations: [
    RequestOverviewComponent,
  ],
  exports: [
    RequestOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class RequestOverviewModule {}
