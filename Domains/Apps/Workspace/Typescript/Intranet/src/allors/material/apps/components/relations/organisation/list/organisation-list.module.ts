import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule } from '../../../../../../material';

import { OrganisationListComponent } from './organisation-list.component';
export { OrganisationListComponent } from './organisation-list.component';

@NgModule({
  declarations: [
    OrganisationListComponent,
  ],
  exports: [
    OrganisationListComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
  ]
})
export class OrganisationsOverviewModule { }
