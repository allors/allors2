import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AllorsMaterialSideNavToggleModule } from '../../allors/material';

import { DashboardComponent } from './dashboard.component';

@NgModule({
  declarations: [
    DashboardComponent,
  ],
  exports: [
  ],
  imports: [
    CommonModule,
    MatToolbarModule,
    AllorsMaterialSideNavToggleModule
  ],
})
export class DashboardModule {
}
