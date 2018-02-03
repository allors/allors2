import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { PartyContactMechanismPostalAddressAddComponent } from "./party-contactmechanism-postaladdress-add.component";
export { PartyContactMechanismPostalAddressAddComponent } from "./party-contactmechanism-postaladdress-add.component";

@NgModule({
  declarations: [
    PartyContactMechanismPostalAddressAddComponent,
  ],
  exports: [
    PartyContactMechanismPostalAddressAddComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismAddPostalAddressModule {}
