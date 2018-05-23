import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared.module';

import { PartyContactMechanismEmailAddressAddComponent } from './party-contactmechanism-emailaddress-add.component';
export { PartyContactMechanismEmailAddressAddComponent } from './party-contactmechanism-emailaddress-add.component';

@NgModule({
  declarations: [
    PartyContactMechanismEmailAddressAddComponent,
  ],
  exports: [
    PartyContactMechanismEmailAddressAddComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismEmailAddressAddModule {}
