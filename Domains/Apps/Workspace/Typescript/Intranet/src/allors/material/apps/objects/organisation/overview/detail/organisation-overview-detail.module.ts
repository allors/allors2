import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatButtonToggleModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFileModule } from '../../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../../base/components/textarea';

import { CommunicationEventOverviewPanelModule } from '../../../communicationevent/overview/panel/communicationevent-overview-panel.module';
import { PartyContactMechanismPanelModule } from '../../../partycontactmechanisms/panel/partycontactmechanism-panel.module';
import { SerialisedItemPanelModule } from '../../../serialiseditem/panel/serialiseditem-panel.module';
import { WorkEffortPartyAssignmentPanelModule } from '../../../workeffortpartyassignment/panel/workeffortpartyassignment-panel.module';

import { OrganisationOverviewDetailComponent } from './organisation-overview-detail.component';
export { OrganisationOverviewDetailComponent } from './organisation-overview-detail.component';

@NgModule({
  declarations: [
    OrganisationOverviewDetailComponent,
  ],
  exports: [
    OrganisationOverviewDetailComponent,
  ],
  imports: [
    CommunicationEventOverviewPanelModule,
    PartyContactMechanismPanelModule,
    SerialisedItemPanelModule,
    WorkEffortPartyAssignmentPanelModule,

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
export class OrganisationOverviewDetailModule { }
