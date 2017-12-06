import { NgModule } from "@angular/core";

import { InlineModule } from "../../../../inline.module";
import { SharedModule } from "../../../../shared.module";

import { PartyCommunicationEventWorkTaskComponent } from "./party-communicationevent-worktask.component";
export { PartyCommunicationEventWorkTaskComponent } from "./party-communicationevent-worktask.component";

@NgModule({
  declarations: [
    PartyCommunicationEventWorkTaskComponent,
  ],
  exports: [
    PartyCommunicationEventWorkTaskComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class PartyCommunicationEventWorkTaskModule {}
