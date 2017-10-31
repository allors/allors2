import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../../shared.module";

import { PartyContactMechanismEmailAddressInlineComponent } from "./party-contactmechanism-emailaddress-inline.component";
export { PartyContactMechanismEmailAddressInlineComponent } from "./party-contactmechanism-emailaddress-inline.component";

@NgModule({
  declarations: [
    PartyContactMechanismEmailAddressInlineComponent,
  ],
  exports: [
    PartyContactMechanismEmailAddressInlineComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismEmailAddressInlineModule {}
