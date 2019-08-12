import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
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


import { AllorsMaterialAutoCompleteModule } from '../../../../core/components/role/autocomplete';
import { AllorsMaterialChipsModule } from '../../../../core/components/role/chips';
import { AllorsMaterialDatepickerModule } from '../../../../core/components/role/datepicker';
import { AllorsMaterialFileModule } from '../../../../core/components/role/file';
import { AllorsMaterialFilesModule } from '../../../..//core/components/role/files';
import { AllorsMaterialInputModule } from '../../../../core/components/role/input';
import { AllorsMaterialLocalisedTextModule } from '../../../../core/components/role/localisedtext';
import { AllorsMaterialSelectModule } from '../../../../core/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../core/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../core/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../core/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../core/components/role/textarea';

import { PartyInlineModule } from '../../party/inline/party-inline.module';
import { PersonInlineModule } from '../../person/inline/person-inline.module';
import { ContactMechanismInlineModule } from '../../contactmechanism/inline/contactmechanism-inline.module';
import { PostalAddressInlineModule } from '../../postaladdress/inline/postaladdress-inline.module';

import { SalesInvoiceCreateComponent } from './salesinvoice-create.component';
export { SalesInvoiceCreateComponent } from './salesinvoice-create.component';

@NgModule({
  declarations: [
    SalesInvoiceCreateComponent,
  ],
  exports: [
    SalesInvoiceCreateComponent,
  ],
  imports: [
    AllorsMaterialAutoCompleteModule,
    AllorsMaterialChipsModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialFilesModule,
    AllorsMaterialInputModule,
    AllorsMaterialLocalisedTextModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,

    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
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
    PartyInlineModule,
    PersonInlineModule,
    ContactMechanismInlineModule,
    PostalAddressInlineModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class SalesInvoiceCreateModule { }
