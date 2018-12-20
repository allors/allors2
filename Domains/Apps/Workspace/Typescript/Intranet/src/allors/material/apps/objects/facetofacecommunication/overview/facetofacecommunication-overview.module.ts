import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFileModule } from '../../../../base/components/role/file';
import { AllorsMaterialInputModule } from '../../../../base/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../base/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/role/textarea';

import { FaceToFaceCommunicationOverviewSummaryModule } from './summary/facetofacecommunication-overview-summary.module';
import { FaceToFaceCommunicationOverviewDetailModule } from './detail/facetofacecommunication-overview-detail.module';

import { FaceToFaceCommunicationOverviewComponent } from './facetofacecommunication-overview.component';
export { FaceToFaceCommunicationOverviewComponent } from './facetofacecommunication-overview.component';

@NgModule({
  declarations: [
    FaceToFaceCommunicationOverviewComponent,
  ],
  exports: [
    FaceToFaceCommunicationOverviewComponent,
  ],
  imports: [
    FaceToFaceCommunicationOverviewSummaryModule,
    FaceToFaceCommunicationOverviewDetailModule,

    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,

    FormsModule,
    MatButtonModule,
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
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class FaceToFaceCommunicationOverviewModule { }
