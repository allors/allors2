import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AllorsMaterialAutoCompleteModule } from '../../../../base/components/role/autocomplete';

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
