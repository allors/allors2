import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatInputModule, MatIconModule, MatListModule, MatButtonModule } from '@angular/material';

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
  ],
})
export class AllorsMaterialSideNavToggleModule {
}
