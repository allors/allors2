import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared.module';

import { PartyContactMechanismTelecommunicationsNumberEditComponent } from './party-contactmechanism-telecommunicationsnumber-edit.component';
export { PartyContactMechanismTelecommunicationsNumberEditComponent } from './party-contactmechanism-telecommunicationsnumber-edit.component';

@NgModule({
  declarations: [
    PartyContactMechanismTelecommunicationsNumberEditComponent,
  ],
  exports: [
    PartyContactMechanismTelecommunicationsNumberEditComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismTelecommunicationsNumberEditModule {}
