import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { RequestsOverviewComponent } from "./requestsOverview.component";
export { RequestsOverviewComponent } from "./requestsOverview.component";

@NgModule({
  declarations: [
    RequestsOverviewComponent,
  ],
  exports: [
    RequestsOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class RequestsOverviewModule {}
