import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared.module';

import { PartyContactMechanismEmailAddressEditComponent } from './party-contactmechanism-emailaddress-edit.component';
export { PartyContactMechanismEmailAddressEditComponent } from './party-contactmechanism-emailaddress-edit.component';

@NgModule({
  declarations: [
    PartyContactMechanismEmailAddressEditComponent,
  ],
  exports: [
    PartyContactMechanismEmailAddressEditComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismEmailAddressEditModule {}
