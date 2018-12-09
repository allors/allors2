import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatButtonToggleModule, MatDialogModule, MatExpansionModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFileModule } from '../../../../base/components/file';
import { AllorsMaterialHeaderModule } from '../../../../base/components/header';
import { AllorsMaterialInputModule } from '../../../../base/components/input';
import { AllorsMaterialLauncherModule } from '../../../..';
import { AllorsMaterialSelectModule } from '../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/textarea';

import { CommunicationEventOverviewPanelModule } from '../../communicationevent/overview/panel/communicationevent-overview-panel.module';
import { PartyContactMechanismPanelModule } from '../../partycontactmechanisms/panel/partycontactmechanism-panel.module';
import { SerialisedItemPanelModule } from '../../serialiseditem/panel/serialiseditem-panel.module';
import { WorkEffortPartyAssignmentPanelModule } from '../../workeffortpartyassignment/panel/workeffortpartyassignment-panel.module';

import { PersonOverviewSummaryModule } from './summary/person-overview-summary.module';
import { PersonOverviewDetailModule } from './detail/person-overview-detail.module';

import { PersonOverviewComponent } from './person-overview.component';
export { PersonOverviewComponent } from './person-overview.component';

@NgModule({
  declarations: [
    PersonOverviewComponent,
  ],
  exports: [
    PersonOverviewComponent,
  ],
  imports: [
    PersonOverviewSummaryModule,
    PersonOverviewDetailModule,

    CommunicationEventOverviewPanelModule,
    PartyContactMechanismPanelModule,
    SerialisedItemPanelModule,
    WorkEffortPartyAssignmentPanelModule,

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
export class PersonDetailModule { }
