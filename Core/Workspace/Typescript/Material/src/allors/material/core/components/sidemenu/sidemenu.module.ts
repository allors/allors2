import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
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
