import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared.module';

import { PartyContactMechanismInlineWebAddressComponent } from './party-contactmechanism-webaddress-inline.component';
export { PartyContactMechanismInlineWebAddressComponent } from './party-contactmechanism-webaddress-inline.component';

@NgModule({
  declarations: [
    PartyContactMechanismInlineWebAddressComponent,
  ],
  exports: [
    PartyContactMechanismInlineWebAddressComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismInlineWebAddressModule {}
