import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatButtonToggleModule, MatDialogModule, MatExpansionModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFileModule } from '../../../../base/components/role/file';
import { AllorsMaterialHeaderModule } from '../../../../base/components/header';
import { AllorsMaterialInputModule } from '../../../../base/components/role/input';
import { AllorsMaterialLauncherModule } from '../../../..';
import { AllorsMaterialSelectModule } from '../../../../base/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/role/textarea';

import { CommunicationEventOverviewPanelModule } from '../../communicationevent/overview/panel/communicationevent-overview-panel.module';
import { ContactMechanismOverviewPanelModule } from '../../contactmechanism/overview/panel/contactmechanism-overview-panel.module';
import { PartyContactMechanismOverviewPanelModule } from '../../partycontactmechanism/overview/panel/partycontactmechanism-overview-panel.module';
import { PartyRelationshipOverviewPanelModule } from '../../partyrelationship/overview/panel/partyrelationship-overview-panel.module';
import { SerialisedItemOverviewPanelModule } from '../../serialiseditem/overview/panel/serialiseditem-overview-panel.module';
import { WorkEffortPartyAssignmentOverviewPanelModule } from '../../workeffortpartyassignment/overview/panel/workeffortpartyassignment-overview-panel.module';

import { SerialisedItemOverviewSummaryModule } from './summary/serialiseditem-overview-summary.module';
import { SerialisedItemOverviewDetailModule } from './detail/serialiseditem-overview-detail.module';

import { SerialisedItemOverviewComponent } from './serialiseditem-overview.component';
export { SerialisedItemOverviewComponent } from './serialiseditem-overview.component';

@NgModule({
  declarations: [
    SerialisedItemOverviewComponent,
  ],
  exports: [
    SerialisedItemOverviewComponent,
  ],
  imports: [
    SerialisedItemOverviewSummaryModule,
    SerialisedItemOverviewDetailModule,

    CommunicationEventOverviewPanelModule,
    ContactMechanismOverviewPanelModule,
    PartyContactMechanismOverviewPanelModule,
    PartyRelationshipOverviewPanelModule,
    SerialisedItemOverviewPanelModule,
    WorkEffortPartyAssignmentOverviewPanelModule,

    AllorsMaterialFileModule,
    AllorsMaterialHeaderModule,
    AllorsMaterialInputModule,
    AllorsMaterialLauncherModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
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
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class SerialisedItemOverviewModule { }
