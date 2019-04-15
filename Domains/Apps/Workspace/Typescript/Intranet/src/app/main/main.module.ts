import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatSidenavModule, MatToolbarModule, MatIconModule, MatButtonModule } from '@angular/material';

import { AllorsMaterialSideMenuModule } from '../../allors/material';
import { InternalOrganisationSelectModule } from '../../allors/material';
import { TaskAssignmentLinkModule } from '../../allors/material/apps/objects/taskassignment/link/taskassignment-link.module';

import { MainComponent } from './main.component';
import { AllorsDevModule } from 'src/allors/material/custom/dev/dev.module';

@NgModule({
  declarations: [
    MainComponent,
  ],
  exports: [
  ],
  imports: [
    CommonModule,
    RouterModule ,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatToolbarModule,
    AllorsMaterialSideMenuModule,
    AllorsDevModule,
    InternalOrganisationSelectModule,
    TaskAssignmentLinkModule,
  ],
})
export class MainModule {
}
