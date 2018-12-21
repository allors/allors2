import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatButtonToggleModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFileModule } from '../../../../base/components/role/file';
import { AllorsMaterialInputModule } from '../../../../base/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../base/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/role/textarea';

import { CommunicationEventPanelModule } from '../../communicationevent/overview/panel/communicationevent-panel.module';
import { PartyContactMechanismOverviewPanelModule } from '../../partycontactmechanism/overview/panel/partycontactmechanism-overview-panel.module';
import { SerialisedItemOverviewPanelModule } from '../../serialiseditem/overview/panel/serialiseditem-overview-panel.module';
import { WorkEffortPartyAssignmentOverviewPanelModule } from '../../workeffortpartyassignment/overview/panel/workeffortpartyassignment-overview-panel.module';

import { SalesInvoiceOverviewSummaryModule } from './summary/salesinvoice-overview-summary.module';
import { SalesInvoiceOverviewDetailModule } from './detail/salesinvoice-overview-detail.module';
import { SalesInvoiceItemOverviewPanelModule } from '../../salesinvoiceitem/overview/panel/salesinvoiceitem-overview-panel.module';
import { SalesTermOverviewPanelModule } from '../../salesterm/overview/panel/salesterm-overview-panel.module';

export { SalesInvoiceOverviewComponent } from './salesinvoice-overview.component';
import { SalesInvoiceOverviewComponent } from './salesinvoice-overview.component';

@NgModule({
  declarations: [
    SalesInvoiceOverviewComponent,
  ],
  exports: [
    SalesInvoiceOverviewComponent,
  ],
  imports: [
    SalesInvoiceOverviewSummaryModule,
    SalesInvoiceOverviewDetailModule,
    SalesInvoiceItemOverviewPanelModule,
    SalesTermOverviewPanelModule,

    CommunicationEventPanelModule,
    PartyContactMechanismOverviewPanelModule,
    SerialisedItemOverviewPanelModule,
    WorkEffortPartyAssignmentOverviewPanelModule,

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
export class SalesInvoiceOverviewModule { }
