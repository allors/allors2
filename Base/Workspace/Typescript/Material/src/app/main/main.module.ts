import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';

import { MainComponent } from './main.component';
import { AllorsMaterialSideMenuModule } from '../../allors/material';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    MainComponent,
  ],
  exports: [
  ],
  imports: [
    CommonModule,
    AllorsMaterialSideMenuModule,
    MatSidenavModule,
    MatToolbarModule,
    RouterModule 
  ],
})
export class MainModule {
}
