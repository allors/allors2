import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared.module';

import { PartyContactMechanismPostalAddressInlineComponent } from './party-contactmechanism-postaladdress-inline.component';
export { PartyContactMechanismPostalAddressInlineComponent } from './party-contactmechanism-postaladdress-inline.component';

@NgModule({
  declarations: [
    PartyContactMechanismPostalAddressInlineComponent,
  ],
  exports: [
    PartyContactMechanismPostalAddressInlineComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismPostalAddressInlineModule {}
