import { NgModule } from "@angular/core";
import { SharedModule } from "../../../shared.module";

import { PartyContactMechanismInlineComponent } from "./party-contactmechanism-inline.component";
export { PartyContactMechanismInlineComponent } from "./party-contactmechanism-inline.component";

import { PartyContactMechanismEmailAddressInlineModule } from "./emailaddress/party-contactmechanism-emailaddress-inline.module";
import { PartyContactMechanismPostalAddressInlineModule } from "./postaladdress/party-contactmechanism-postaladdress-inline.module";
import { PartyContactMechanismTelecommunicationsNumberInlineModule } from "./telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-inline.module";
import { PartyContactMechanismInlineWebAddressModule } from "./webaddress/party-contactmechanism-webaddress-inline.module";

@NgModule({
  declarations: [
    PartyContactMechanismInlineComponent,
  ],
  exports: [
    PartyContactMechanismInlineComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,

    PartyContactMechanismEmailAddressInlineModule,
    PartyContactMechanismPostalAddressInlineModule,
    PartyContactMechanismTelecommunicationsNumberInlineModule,
    PartyContactMechanismInlineWebAddressModule,
  ],
})
export class PartyContactMechanismInlineModule {}
