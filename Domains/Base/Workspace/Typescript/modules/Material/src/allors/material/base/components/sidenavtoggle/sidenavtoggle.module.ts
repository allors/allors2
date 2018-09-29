import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule, MatButtonModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../angular';

import { AllorsMaterialSideNavToggleComponent } from './sidenavtoggle.component';
export { AllorsMaterialSideNavToggleComponent } from './sidenavtoggle.component';

@NgModule({
  declarations: [
    AllorsMaterialSideNavToggleComponent,
  ],
  exports: [
    AllorsMaterialSideNavToggleComponent,
  ],
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialSideNavToggleModule {
}
