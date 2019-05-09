import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatButtonToggleModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialDatepickerModule } from '../../../../../base/components/role/datepicker';
import { AllorsMaterialFileModule } from '../../../../../base/components/role/file';
import { AllorsMaterialInputModule } from '../../../../../base/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../../base/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../base/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../base/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../../base/components/role/textarea';
import { AllorsMaterialAutoCompleteModule } from '../../../../../../../allors/material/base/components/role/autocomplete';

import { CommunicationEventOverviewPanelModule } from '../../../communicationevent/overview/panel/communicationevent-overview-panel.module';
import { PartyContactMechanismOverviewPanelModule } from '../../../partycontactmechanism/overview/panel/partycontactmechanism-overview-panel.module';
import { SerialisedItemOverviewPanelModule } from '../../../serialiseditem/overview/panel/serialiseditem-overview-panel.module';
import { WorkEffortPartyAssignmentOverviewPanelModule } from '../../../workeffortpartyassignment/overview/panel/workeffortpartyassignment-overview-panel.module';

import { OrganisationInlineModule } from '../../../organisation/inline/organisation-inline.module';
import { PartyInlineModule } from '../../../party/inline/party-inline.module';
import { PersonInlineModule } from '../../../person/inline/person-inline.module';
import { ContactMechanismInlineModule } from '../../../contactmechanism/inline/contactmechanism-inline.module';
import { PostalAddressInlineModule } from '../../../postaladdress/inline/postaladdress-inline.module';

import { PurchaseInvoiceOverviewDetailComponent } from './purchaseinvoice-overview-detail.component';
export { PurchaseInvoiceOverviewDetailComponent } from './purchaseinvoice-overview-detail.component';

@NgModule({
  declarations: [
    PurchaseInvoiceOverviewDetailComponent,
  ],
  exports: [
    PurchaseInvoiceOverviewDetailComponent,
  ],
  imports: [
    OrganisationInlineModule,
    PartyInlineModule,
    PersonInlineModule,
    ContactMechanismInlineModule,
    PostalAddressInlineModule,

    CommunicationEventOverviewPanelModule,
    PartyContactMechanismOverviewPanelModule,
    SerialisedItemOverviewPanelModule,
    WorkEffortPartyAssignmentOverviewPanelModule,

    AllorsMaterialAutoCompleteModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,

    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDividerModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatRadioModule,
    MatSelectModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,

    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class PurchaseInvoiceOverviewDetailModule { }
