import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatTabsModule } from '@angular/material';

import { AllorsMaterialChipsModule } from '../../../../base/components/role/chips';
import { AllorsMaterialFooterModule } from '../../../../base/components/footer';
import { AllorsMaterialDatepickerModule } from '../../../../base/components/role/datepicker';
import { AllorsMaterialDatetimepickerModule } from '../../../../base/components/role/datetimepicker';
import { AllorsMaterialFileModule } from '../../../../base/components/role/file';
import { AllorsMaterialInputModule } from '../../../../base/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../base/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/role/textarea';

import { PersonInlineModule } from '../../person/inline/person-inline.module';
import { TelecommunicationsNumberInlineModule } from '../../telecommunicationsnumber/inline/telecommunicationsnumber-inline.module';

import { PhoneCommunicationCreateComponent } from './phonecommunication-create.component';
export { PhoneCommunicationCreateComponent } from './phonecommunication-create.component';

@NgModule({
  declarations: [
    PhoneCommunicationCreateComponent,
  ],
  exports: [
    PhoneCommunicationCreateComponent,
  ],
  imports: [
    PersonInlineModule,
    TelecommunicationsNumberInlineModule,

    AllorsMaterialChipsModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialDatetimepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialFooterModule,
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
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class PhoneCommunicationCreateModule { }
