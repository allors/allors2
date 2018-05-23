import { NgModule } from '@angular/core';

import { InlineModule } from '../../../../inline.module';
import { SharedModule } from '../../../../shared.module';

import { PartyCommunicationEventLetterCorrespondenceComponent } from './party-communicationevent-lettercorrespondence.component';
export { PartyCommunicationEventLetterCorrespondenceComponent } from './party-communicationevent-lettercorrespondence.component';

@NgModule({
  declarations: [
    PartyCommunicationEventLetterCorrespondenceComponent,
  ],
  exports: [
    PartyCommunicationEventLetterCorrespondenceComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class PartyCommunicationEventLetterCorrespondenceModule {}
