import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AllorsMaterialSideMenuModule, AllorsMaterialScannerModule } from '../../allors/material';
import { InternalOrganisationSelectModule } from '../../allors/material';
import { TaskAssignmentLinkModule } from '../../allors/material/base/objects/taskassignment/link/taskassignment-link.module';
import { UserProfileLinkModule } from '../../allors/material/base/objects/userprofile/link/userprofile-link.module';

import { MainComponent } from './main.component';

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
    AllorsMaterialScannerModule,
    InternalOrganisationSelectModule,
    TaskAssignmentLinkModule,
    UserProfileLinkModule
  ],
})
export class MainModule {
}
