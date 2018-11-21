import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatSidenavModule, MatToolbarModule, MatIconModule } from '@angular/material';

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
    MatIconModule,
    MatSidenavModule,
    MatToolbarModule,

    AllorsMaterialSideMenuModule,
    InternalOrganisationSelectModule,
  ],
})
export class MainModule {
}
