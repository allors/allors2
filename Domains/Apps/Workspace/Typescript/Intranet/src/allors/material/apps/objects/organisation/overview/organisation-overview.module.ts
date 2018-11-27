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

import { PartyCommunicationEventsModule } from '../../party/communicationevents/party-communicationevents.module';
import { PartyPartyContactMechanismsModule } from '../../party/partycontactmechanisms/party-partycontactmechanisms.module';
import { PartyWorkEffortPartyAssignmentsModule } from '../../party/workefforts/party-workeffortpartyassignments.module';
import { SerialisedItemsModule } from '../../serialiseditem/overview/serialiseditems.module';

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
    PartyCommunicationEventsModule,
    PartyPartyContactMechanismsModule,
    PartyWorkEffortPartyAssignmentsModule,
    SerialisedItemsModule,

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
