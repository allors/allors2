import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';

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
