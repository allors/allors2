import { NgModule } from "@angular/core";

import { InlineModule } from "../../../../inline.module";
import { SharedModule } from "../../../../shared.module";

import { PartyCommunicationEventFaceToFaceCommunicationComponent } from "./party-communicationevent-facetofacecommunication.component";
export { PartyCommunicationEventFaceToFaceCommunicationComponent } from "./party-communicationevent-facetofacecommunication.component";

@NgModule({
  declarations: [
    PartyCommunicationEventFaceToFaceCommunicationComponent,
  ],
  exports: [
    PartyCommunicationEventFaceToFaceCommunicationComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class PartyCommunicationEventFaceToFaceCommunicationModule {}
