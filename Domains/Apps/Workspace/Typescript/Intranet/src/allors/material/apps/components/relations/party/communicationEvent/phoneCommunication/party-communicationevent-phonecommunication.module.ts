import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatTabsModule, MatDatepickerModule } from '@angular/material';


import { AllorsMaterialChipsModule } from '../../../../../../base/components/chips';
import { AllorsMaterialDatepickerModule } from '../../../../../../base/components/datepicker';
import { AllorsMaterialFileModule } from '../../../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../../../base/components/textarea';

import { PersonInlineModule } from '../../../person/person-inline.module';

import { PartyContactMechanismTelecommunicationsNumberInlineModule } from '../../contactmechanism/telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-inline.module';

import { PartyCommunicationEventPhoneCommunicationComponent } from './party-communicationevent-phonecommunication.component';
export { PartyCommunicationEventPhoneCommunicationComponent } from './party-communicationevent-phonecommunication.component';

@NgModule({
  declarations: [
    PartyCommunicationEventPhoneCommunicationComponent,
  ],
  exports: [
    PartyCommunicationEventPhoneCommunicationComponent,
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
    
    FormsModule,
    MatButtonModule,
    MatCardModule,
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
    PartyContactMechanismTelecommunicationsNumberInlineModule,
    PersonInlineModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class PartyCommunicationEventPhoneCommunicationModule {}
