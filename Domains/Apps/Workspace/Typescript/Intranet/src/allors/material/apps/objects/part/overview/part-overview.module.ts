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

import { IGoodIdentificationPanelModule } from '../../igoodidentification/overview/panel/igoodIdentification-panel.module';
import { SerialisedInventoryItemOverviewPanelModule } from '../../serialisedinventoryitem/overview/panel/serialisedinventoryitem-overview-panel.module';
import { SerialisedItemOverviewPanelModule } from '../../serialiseditem/overview/panel/serialiseditem-overview-panel.module';

import { PartOverviewSummaryModule } from './summary/part-overview-summary.module';
import { PartOverviewDetailModule } from './detail/part-overview-detail.module';

import { PartOverviewComponent } from './part-overview.component';
export { PartOverviewComponent } from './part-overview.component';

@NgModule({
  declarations: [
    PartOverviewComponent,
  ],
  exports: [
    PartOverviewComponent,
  ],
  imports: [
    PartOverviewSummaryModule,
    PartOverviewDetailModule,

    IGoodIdentificationPanelModule,
    SerialisedInventoryItemOverviewPanelModule,
    SerialisedItemOverviewPanelModule,

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
export class PartOverviewModule { }
