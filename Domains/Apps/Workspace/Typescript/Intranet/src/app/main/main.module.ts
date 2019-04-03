import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatSidenavModule, MatToolbarModule, MatIconModule, MatButtonModule } from '@angular/material';

import { AllorsDevModule } from '../../allors/angular';
import { AllorsMaterialSideMenuModule } from '../../allors/material';
import { InternalOrganisationSelectModule } from '../../allors/material';

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

    AllorsDevModule,
    AllorsMaterialSideMenuModule,
    InternalOrganisationSelectModule,
  ],
})
export class MainModule {
}
