import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared.module';

import { PartyContactMechanismAddWebAddressComponent } from './party-contactmechanism-webaddress-add.component';
export { PartyContactMechanismAddWebAddressComponent } from './party-contactmechanism-webaddress-add.component';

@NgModule({
  declarations: [
    PartyContactMechanismAddWebAddressComponent,
  ],
  exports: [
    PartyContactMechanismAddWebAddressComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismAddWebAddressModule {}
