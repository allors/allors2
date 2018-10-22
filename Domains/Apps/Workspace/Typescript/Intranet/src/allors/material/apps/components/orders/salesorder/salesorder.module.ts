import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatTabsModule, MatDatepickerModule } from '@angular/material';


import { AllorsMaterialAutoCompleteModule } from '../../../../base/components/autocomplete';

import { AllorsMaterialChipsModule } from '../../../../base/components/chips';
import { AllorsMaterialDatepickerModule } from '../../../../base/components/datepicker';
import { AllorsMaterialFileModule } from '../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/textarea';

import { PersonInlineModule } from '../../relations/person/inline/person-inline.module';
import { PostalAddressInlineModule } from '../../relations/postaladdress/inline/postaladdress-inline.module';
import { ContactMechanismInlineModule } from '../../relations/contactmechanism/inline/contactmechanism-inline.module';

import { SalesOrderEditComponent } from './salesorder.component';
export { SalesOrderEditComponent } from './salesorder.component';

@NgModule({
  declarations: [
    SalesOrderEditComponent,
  ],
  exports: [
    SalesOrderEditComponent,
  ],
  imports: [
    AllorsMaterialAutoCompleteModule,
    AllorsMaterialChipsModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSideNavToggleModule,
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
    ContactMechanismInlineModule,
    PostalAddressInlineModule,
    PersonInlineModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class SalesOrderEditModule { }
