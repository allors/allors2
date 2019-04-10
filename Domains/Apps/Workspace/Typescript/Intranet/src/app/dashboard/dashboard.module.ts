import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule, MatIconModule, MatCardModule } from '@angular/material';

import { AllorsMaterialSideNavToggleModule } from '../../allors/material';

import { DashboardComponent } from './dashboard.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    DashboardComponent,
  ],
  exports: [
  ],
  imports: [
    CommonModule,
    MatIconModule,
    MatCardModule,
    MatToolbarModule,
    RouterModule,
    AllorsMaterialSideNavToggleModule
  ],
})
export class DashboardModule {
}
