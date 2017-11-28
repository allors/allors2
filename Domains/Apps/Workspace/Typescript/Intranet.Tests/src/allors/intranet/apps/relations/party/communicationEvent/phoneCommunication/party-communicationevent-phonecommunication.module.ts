import { NgModule } from "@angular/core";

import { InlineModule } from "../../../../../inline.module";
import { SharedModule } from "../../../../../shared.module";

import { PartyCommunicationEventPhoneCommunicationComponent } from "./party-communicationevent-phonecommunication.component";
export { PartyCommunicationEventPhoneCommunicationComponent } from "./party-communicationevent-phonecommunication.component";

@NgModule({
  declarations: [
    PartyCommunicationEventPhoneCommunicationComponent,
  ],
  exports: [
    PartyCommunicationEventPhoneCommunicationComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class PartyCommunicationEventPhoneCommunicationModule {}
