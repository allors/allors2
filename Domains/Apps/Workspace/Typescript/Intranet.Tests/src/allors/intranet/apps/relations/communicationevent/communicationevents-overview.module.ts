import { NgModule } from "@angular/core";
import { SharedModule } from "../../../shared.module";

import { CommunicationEventsOverviewComponent } from "./communicationevents-overview.component";
export { CommunicationEventsOverviewComponent } from "./communicationevents-overview.component";

@NgModule({
  declarations: [
    CommunicationEventsOverviewComponent,
  ],
  exports: [
    CommunicationEventsOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class CommunicationEventsOverviewModule {}
