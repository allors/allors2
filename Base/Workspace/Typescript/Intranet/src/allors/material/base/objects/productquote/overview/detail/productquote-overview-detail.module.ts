import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatOptionModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';

import { AllorsMaterialDatepickerModule } from '../../../../../core/components/role/datepicker';
import { AllorsMaterialFileModule } from '../../../../../core/components/role/file';
import { AllorsMaterialInputModule } from '../../../../../core/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../../core/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../core/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../core/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../core/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../../core/components/role/textarea';
import { AllorsMaterialAutoCompleteModule } from '../../../../../../../allors/material/core/components/role/autocomplete';

import { CommunicationEventOverviewPanelModule } from '../../../communicationevent/overview/panel/communicationevent-overview-panel.module';
import { PartyContactMechanismOverviewPanelModule } from '../../../partycontactmechanism/overview/panel/partycontactmechanism-overview-panel.module';
import { SerialisedItemOverviewPanelModule } from '../../../serialiseditem/overview/panel/serialiseditem-overview-panel.module';
import { WorkEffortPartyAssignmentOverviewPanelModule } from '../../../workeffortpartyassignment/overview/panel/workeffortpartyassignment-overview-panel.module';

import { PartyInlineModule } from '../../../party/inline/party-inline.module';
import { PersonInlineModule } from '../../../person/inline/person-inline.module';
import { ContactMechanismInlineModule } from '../../../contactmechanism/inline/contactmechanism-inline.module';

import { ProductQuoteOverviewDetailComponent } from './productquote-overview-detail.component';
export { ProductQuoteOverviewDetailComponent } from './productquote-overview-detail.component';

@NgModule({
  declarations: [
    ProductQuoteOverviewDetailComponent,
  ],
  exports: [
    ProductQuoteOverviewDetailComponent,
  ],
  imports: [
    PartyInlineModule,
    PersonInlineModule,
    ContactMechanismInlineModule,

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
export class ProductQuoteOverviewDetailModule { }
