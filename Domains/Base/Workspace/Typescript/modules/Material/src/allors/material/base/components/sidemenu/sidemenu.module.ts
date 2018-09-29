import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatInputModule, MatIconModule, MatListModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsFocusModule } from '../../../../angular';
export { SideMenuItem } from './sidemenuitem';

import { AllorsMaterialSideMenuComponent } from './sidemenu.component';
export { AllorsMaterialSideMenuComponent } from './sidemenu.component';


@NgModule({
  declarations: [
    AllorsMaterialSideMenuComponent,
  ],
  exports: [
    AllorsMaterialSideMenuComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatAutocompleteModule,
    MatInputModule,
    MatIconModule,
    MatListModule,
    ReactiveFormsModule,
    RouterModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialSideMenuModule {
}
