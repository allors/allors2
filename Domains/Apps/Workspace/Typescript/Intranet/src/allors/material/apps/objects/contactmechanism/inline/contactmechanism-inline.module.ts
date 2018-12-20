import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatTabsModule, MatDatepickerModule } from '@angular/material';

import { AllorsMaterialAutoCompleteModule } from '../../../../base/components/role/autocomplete';

import { AllorsMaterialChipsModule } from '../../../../base/components/role/chips';
import { AllorsMaterialDatepickerModule } from '../../../../base/components/role/datepicker';
import { AllorsMaterialFileModule } from '../../../../base/components/role/file';
import { AllorsMaterialInputModule } from '../../../../base/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../base/components/role/select';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/role/textarea';

import { ContactMechanismInlineComponent } from './contactmechanism-inline.component';
export { ContactMechanismInlineComponent } from './contactmechanism-inline.component';

import { EmailAddressInlineModule } from '../../emailaddress/inline/emailaddress-inline.module';
import { PostalAddressInlineModule } from '../../postaladdress/inline/postaladdress-inline.module';
import { TelecommunicationsNumberInlineModule } from '../../telecommunicationsnumber/inline/telecommunicationsnumber-inline.module';
import { InlineWebAddressModule } from '../../webaddress/inline/webaddress-inline.module';

@NgModule({
  declarations: [
    ContactMechanismInlineComponent,
  ],
  exports: [
    ContactMechanismInlineComponent,
  ],
  imports: [
    AllorsMaterialAutoCompleteModule,

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
    EmailAddressInlineModule,
    PostalAddressInlineModule,
    TelecommunicationsNumberInlineModule,
    InlineWebAddressModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class ContactMechanismInlineModule { }
