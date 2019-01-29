import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSidenavModule, MatToolbarModule } from '@angular/material';

import { MainComponent } from './main.component';
import { AllorsMaterialSideMenuModule } from 'src/allors/material';
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
