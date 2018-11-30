import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatButtonToggleModule, MatDialogModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFileModule } from '../../../../base/components/file';
import { AllorsMaterialHeaderModule } from '../../../../base/components/header';
import { AllorsMaterialInputModule } from '../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/textarea';

import { PersonPanelModule } from '../panel/person-panel.module';
import { CommunicationEventMaxPanelModule } from '../../communicationevent/maxpanel/communicationevent-maxpanel.module';
import { CommunicationEventMinPanelModule } from '../../communicationevent/minpanel/communicationevent-minpanel.module';
import { PartyContactMechanismMaxPanelModule } from '../../partycontactmechanisms/maxpanel/partycontactmechanism-maxpanel.module';
import { PartyContactMechanismMinPanelModule } from '../../partycontactmechanisms/minpanel/partycontactmechanism-minpanel.module';
import { WorkEffortPartyAssignmentEmbedModule } from '../../workeffortpartyassignment/embed/workeffortpartyassignment-embed.module';
import { SerialisedItemEmbedModule } from '../../serialiseditem/embed/serialiseditem-embed.module';

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
    PersonPanelModule,
    CommunicationEventMaxPanelModule,
    CommunicationEventMinPanelModule,
    PartyContactMechanismMaxPanelModule,
    PartyContactMechanismMinPanelModule,
    WorkEffortPartyAssignmentEmbedModule,
    SerialisedItemEmbedModule,

    AllorsMaterialFileModule,
    AllorsMaterialHeaderModule,
    AllorsMaterialInputModule,
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
export class PersonOverviewModule { }
