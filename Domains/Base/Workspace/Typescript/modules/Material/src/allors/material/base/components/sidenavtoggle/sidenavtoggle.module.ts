import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatInputModule, MatIconModule, MatListModule, MatButtonModule } from '@angular/material';

import { SideNavToggleComponent } from './sidenavtoggle.component';
export { SideNavToggleComponent } from './sidenavtoggle.component';

@NgModule({
  declarations: [
    SideNavToggleComponent,
  ],
  exports: [
    SideNavToggleComponent,
  ],
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
  ],
})
export class SideNavToggleModule {
}
