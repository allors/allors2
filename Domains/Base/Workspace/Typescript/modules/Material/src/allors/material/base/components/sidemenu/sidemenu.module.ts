import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatInputModule, MatIconModule, MatListModule } from '@angular/material';

import { AllorsMaterialSideMenuComponent } from './sidemenu.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterModule } from '@angular/router';
export { AllorsMaterialSideMenuComponent } from './sidemenu.component';
export { SideMenuItem } from './sidemenuitem';

@NgModule({
  declarations: [
    AllorsMaterialSideMenuComponent,
  ],
  exports: [
    AllorsMaterialSideMenuComponent,
  ],
  imports: [
    CommonModule,
    FlexLayoutModule,
    FormsModule,
    MatAutocompleteModule,
    MatInputModule,
    MatIconModule,
    MatListModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class AllorsMaterialSideMenuModule {
}
