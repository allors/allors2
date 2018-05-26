import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatTabsModule, MatDatepickerModule } from '@angular/material';

import { AllorsMaterialAutoCompleteModule } from '../../../../../base/components/autocomplete';
import { AllorsMaterialAvatarModule } from '../../../shared/avatar';
import { AllorsMaterialChipsModule } from '../../../../../base/components/chips';
import { AllorsMaterialDatepickerModule } from '../../../../../base/components/datepicker';
import { AllorsMaterialFileModule } from '../../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../../base/components/select';
import { AllorsMaterialSlideToggleModule } from '../../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../../base/components/textarea';

import { PartyContactMechanismInlineComponent } from './party-contactmechanism-inline.component';
export { PartyContactMechanismInlineComponent } from './party-contactmechanism-inline.component';

import { PartyContactMechanismEmailAddressInlineModule } from './emailaddress/party-contactmechanism-emailaddress-inline.module';
import { PartyContactMechanismPostalAddressInlineModule } from './postaladdress/party-contactmechanism-postaladdress-inline.module';
import { PartyContactMechanismTelecommunicationsNumberInlineModule } from './telecommunicationsnumber/party-contactmechanism-telecommunicationsnumber-inline.module';
import { PartyContactMechanismInlineWebAddressModule } from './webaddress/party-contactmechanism-webaddress-inline.module';

@NgModule({
  declarations: [
    PartyContactMechanismInlineComponent,
  ],
  exports: [
    PartyContactMechanismInlineComponent,
  ],
  imports: [
    AllorsMaterialAutoCompleteModule,
    AllorsMaterialAvatarModule,
    AllorsMaterialChipsModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
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
    PartyContactMechanismEmailAddressInlineModule,
    PartyContactMechanismPostalAddressInlineModule,
    PartyContactMechanismTelecommunicationsNumberInlineModule,
    PartyContactMechanismInlineWebAddressModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class PartyContactMechanismInlineModule { }
