import { NgModule } from "@angular/core";
import { SharedModule } from "../../../../../shared.module";

import { PartyContactMechanismEditWebAddressComponent } from "./party-contactmechanism-webaddress-edit.component";
export { PartyContactMechanismEditWebAddressComponent } from "./party-contactmechanism-webaddress-edit.component";

@NgModule({
  declarations: [
    PartyContactMechanismEditWebAddressComponent,
  ],
  exports: [
    PartyContactMechanismEditWebAddressComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismEditWebAddressModule {}
