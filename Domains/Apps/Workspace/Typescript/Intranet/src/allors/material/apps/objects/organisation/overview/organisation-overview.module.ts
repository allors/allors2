import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatButtonToggleModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFileModule } from '../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/textarea';

import { CommunicationEventMaxPanelModule } from '../../communicationevent/maxpanel/communicationevent-maxpanel.module';
import { PartyContactMechanismMaxPanelModule } from '../../partycontactmechanisms/maxpanel/partycontactmechanism-maxpanel.module';
import { WorkEffortPartyAssignmentEmbedModule } from '../../workeffortpartyassignment/embed/workeffortpartyassignment-embed.module';
import { SerialisedItemEmbedModule } from '../../serialiseditem/embed/serialiseditem-embed.module';

import { OrganisationOverviewComponent } from './organisation-overview.component';
export { OrganisationOverviewComponent } from './organisation-overview.component';

@NgModule({
  declarations: [
    OrganisationOverviewComponent,
  ],
  exports: [
    OrganisationOverviewComponent,
  ],
  imports: [
    CommunicationEventMaxPanelModule,
    PartyContactMechanismMaxPanelModule,
    WorkEffortPartyAssignmentEmbedModule,
    SerialisedItemEmbedModule,

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
export class OrganisationOverviewModule { }
