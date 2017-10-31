import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { CommunicationEventOverviewComponent } from "./communicationevent-overview.component";
export { CommunicationEventOverviewComponent } from "./communicationevent-overview.component";

@NgModule({
  declarations: [
    CommunicationEventOverviewComponent,
  ],
  exports: [
    CommunicationEventOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class CommunicationEventOverviewModule {}
