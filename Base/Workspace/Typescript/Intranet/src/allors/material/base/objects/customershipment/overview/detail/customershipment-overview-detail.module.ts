import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
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

import { AllorsMaterialAutoCompleteModule } from '../../../../../core/components/role/autocomplete';
import { AllorsMaterialFileModule } from '../../../../../core/components/role/file';
import { AllorsMaterialHeaderModule } from '../../../../../core/components/header';
import { AllorsMaterialInputModule } from '../../../../../core/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../../core/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../core/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../core/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../core/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../../core/components/role/textarea';
import { AllorsMaterialFooterModule } from '../../../../../core/components/footer';
import { AllorsMaterialLocalisedTextModule } from '../../../../../core/components/role/localisedtext';
import { AllorsMaterialFilesModule } from '../../../../../core/components/role/files';
import { AllorsMaterialDatepickerModule } from '../../../../../core/components/role/datepicker';

import { OrganisationInlineModule } from '../../../organisation/inline/organisation-inline.module';
import { PartyInlineModule } from '../../../party/inline/party-inline.module';
import { PersonInlineModule } from '../../../person/inline/person-inline.module';
import { PostalAddressInlineModule } from '../../../postaladdress/inline/postaladdress-inline.module';
import { ContactMechanismInlineModule } from '../../../contactmechanism/inline/contactmechanism-inline.module';

import { CustomerShipmentOverviewDetailComponent } from './customershipment-overview-detail.component';
export { CustomerShipmentOverviewDetailComponent } from './customershipment-overview-detail.component';

@NgModule({
  declarations: [
    CustomerShipmentOverviewDetailComponent,
  ],
  exports: [
    CustomerShipmentOverviewDetailComponent,
  ],
  imports: [
    AllorsMaterialAutoCompleteModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialFilesModule,
    AllorsMaterialHeaderModule,
    AllorsMaterialFooterModule,
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
    MatDatepickerModule,
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
    ContactMechanismInlineModule,
    OrganisationInlineModule,
    PostalAddressInlineModule,
    PartyInlineModule,
    PersonInlineModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class CustomerShipmentOverviewDetailModule { }
