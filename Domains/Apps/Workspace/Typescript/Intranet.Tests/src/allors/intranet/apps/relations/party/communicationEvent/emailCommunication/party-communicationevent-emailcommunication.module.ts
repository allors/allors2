import { NgModule } from "@angular/core";

import { InlineModule } from "../../../../../inline.module";
import { SharedModule } from "../../../../../shared.module";

import { PartyCommunicationEventEmailCommunicationComponent } from "./party-communicationevent-emailcommunication.component";
export { PartyCommunicationEventEmailCommunicationComponent } from "./party-communicationevent-emailcommunication.component";

@NgModule({
  declarations: [
    PartyCommunicationEventEmailCommunicationComponent,
  ],
  exports: [
    PartyCommunicationEventEmailCommunicationComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class PartyCommunicationEventEmailCommunicationModule {}
