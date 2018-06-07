import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatTabsModule, MatDatepickerModule } from '@angular/material';

import { AllorsMaterialChipsModule } from '../../../../../../base/components/chips';
import { AllorsMaterialDatepickerModule } from '../../../../../../base/components/datepicker';
import { AllorsMaterialFileModule } from '../../../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../../../base/components/input';
import { AllorsMaterialSideNavToggleModule } from '../../../../../../base/components/sidenavtoggle';
import { AllorsMaterialSelectModule } from '../../../../../../base/components/select';
import { AllorsMaterialSlideToggleModule } from '../../../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../../../base/components/textarea';

import { PersonInlineModule } from '../../../person/person-inline.module';

import { PartyContactMechanismPostalAddressInlineModule } from '../../contactmechanism/postaladdress/party-contactmechanism-postaladdress-inline.module';

import { PartyCommunicationEventLetterCorrespondenceComponent } from './party-communicationevent-lettercorrespondence.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { PartyCommunicationEventLetterCorrespondenceComponent } from './party-communicationevent-lettercorrespondence.component';

@NgModule({
  declarations: [
    PartyCommunicationEventLetterCorrespondenceComponent,
  ],
  exports: [
    PartyCommunicationEventLetterCorrespondenceComponent,
  ],
  imports: [
    
    AllorsMaterialChipsModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FlexLayoutModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatDividerModule,
    MatFormFieldModule,  
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatRadioModule,
    MatSelectModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    PartyContactMechanismPostalAddressInlineModule,
    PersonInlineModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class PartyCommunicationEventLetterCorrespondenceModule {}
