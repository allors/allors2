import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AllorsMaterialSideNavToggleModule } from '../sidenavtoggle';

import { AllorsMaterialHeaderComponent } from './header.component';
export { AllorsMaterialHeaderComponent } from './header.component';

@NgModule({
  declarations: [
    AllorsMaterialHeaderComponent,
  ],
  exports: [
    AllorsMaterialHeaderComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatToolbarModule,
    AllorsMaterialSideNavToggleModule
  ],
})
export class AllorsMaterialHeaderModule {
}
