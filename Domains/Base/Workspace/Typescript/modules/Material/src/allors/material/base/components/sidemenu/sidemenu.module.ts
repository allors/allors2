import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatInputModule, MatIconModule, MatListModule } from '@angular/material';

import { SideMenuComponent } from './sidemenu.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterModule } from '@angular/router';
export { SideMenuComponent } from './sidemenu.component';
export { SideMenuItem } from './sidemenuitem';

@NgModule({
  declarations: [
    SideMenuComponent,
  ],
  exports: [
    SideMenuComponent,
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
export class SideMenuModule {
}
