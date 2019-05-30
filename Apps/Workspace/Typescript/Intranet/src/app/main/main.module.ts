import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AllorsMaterialSideMenuModule } from '../../allors/material';
import { InternalOrganisationSelectModule } from '../../allors/material';
import { TaskAssignmentLinkModule } from '../../allors/material/apps/objects/taskassignment/link/taskassignment-link.module';

import { MainComponent } from './main.component';
import { AllorsDevModule } from '../../allors/material/custom/dev/dev.module';

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
