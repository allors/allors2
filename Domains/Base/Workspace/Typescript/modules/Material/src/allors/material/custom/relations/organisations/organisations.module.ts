import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatMenuModule, MatToolbarModule } from '@angular/material';

import { AllorsMaterialStaticModule, AllorsMaterialDialogModule, AllorsMaterialChipsModule, AllorsMaterialSelectModule, AllorsMaterialSideNavToggleModule, AllorsMaterialTableModule } from '../../../../material';

import { OrganisationsComponent } from './organisations.component';
export { OrganisationsComponent } from './organisations.component';

@NgModule({
  declarations: [
    OrganisationsComponent,
  ],
  exports: [
    OrganisationsComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,

    MatAutocompleteModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    MatToolbarModule,

    AllorsMaterialChipsModule,
    AllorsMaterialDialogModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialSelectModule,
    AllorsMaterialTableModule
  ],
})
export class OrganisationsModule {}
