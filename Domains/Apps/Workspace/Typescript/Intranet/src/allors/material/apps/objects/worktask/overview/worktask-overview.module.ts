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

import { TimeEntryOverviewPanelModule } from '../../timeentry/overview/panel/timeentry-overview-panel.module';
import { WorkEffortAssignmentRateOverviewPanelModule } from '../../workeffortassignmentrate/overview/panel/workeffortassignmentrate-overview-panel.module';
import { WorkEffortFixedAssetAssignmentOverviewPanelModule } from '../../workeffortfixedassetassignment/overview/panel/workeffortfixedassetassignment-overview-panel.module';
import { WorkEffortPartyAssignmentOverviewPanelModule } from '../../workeffortpartyassignment/overview/panel/workeffortpartyassignment-overview-panel.module';

import { WorkTaskOverviewSummaryModule } from './summary/worktask-overview-summary.module';
import { WorkTaskOverviewDetailModule } from './detail/worktask-overview-detail.module';

import { WorkTaskOverviewComponent } from './worktask-overview.component';
export { WorkTaskOverviewComponent } from './worktask-overview.component';

@NgModule({
  declarations: [
    WorkTaskOverviewComponent,
  ],
  exports: [
    WorkTaskOverviewComponent,
  ],
  imports: [
    WorkTaskOverviewSummaryModule,
    WorkTaskOverviewDetailModule,

    TimeEntryOverviewPanelModule,
    WorkEffortAssignmentRateOverviewPanelModule,
    WorkEffortFixedAssetAssignmentOverviewPanelModule,
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
export class WorkTaskDetailModule { }
