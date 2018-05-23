import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared.module';

import { PartyContactMechanismTelecommunicationsNumberInlineComponent } from './party-contactmechanism-telecommunicationsnumber-inline.component';
export { PartyContactMechanismTelecommunicationsNumberInlineComponent } from './party-contactmechanism-telecommunicationsnumber-inline.component';

@NgModule({
  declarations: [
    PartyContactMechanismTelecommunicationsNumberInlineComponent,
  ],
  exports: [
    PartyContactMechanismTelecommunicationsNumberInlineComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class PartyContactMechanismTelecommunicationsNumberInlineModule {}
