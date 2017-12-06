import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { PartyContactMechanismTelecommunicationsNumberAddComponent } from "./party-contactmechanism-telecommunicationsnumber-add.component";
export { PartyContactMechanismTelecommunicationsNumberAddComponent } from "./party-contactmechanism-telecommunicationsnumber-add.component";

@NgModule({
  declarations: [
    PartyContactMechanismTelecommunicationsNumberAddComponent,
  ],
  exports: [
    PartyContactMechanismTelecommunicationsNumberAddComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismTelecommunicationsNumberAddModule {}
