import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../shared.module";

import { PartyContactMechanismPostalAddressEditComponent } from "./party-contactmechanism-postaladdress-edit.component";
export { PartyContactMechanismPostalAddressEditComponent } from "./party-contactmechanism-postaladdress-edit.component";

@NgModule({
  declarations: [
    PartyContactMechanismPostalAddressEditComponent,
  ],
  exports: [
    PartyContactMechanismPostalAddressEditComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismPostalAddressEditModule {}
