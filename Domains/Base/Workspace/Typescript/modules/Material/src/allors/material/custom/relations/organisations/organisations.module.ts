import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatMenuModule, MatToolbarModule, MatPaginatorModule } from '@angular/material';

import { AllorsMaterialStaticModule, AllorsMaterialChipsModule, AllorsMaterialSelectModule, AllorsMaterialSideNavToggleModule, AllorsMaterialTableModule, AllorsMaterialFilterModule } from '../../../../material';

import { OrganisationsComponent } from './organisations.component';
import { AllorsMaterialErrorDialogModule } from 'src/allors/material/base/components/errordialog';
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
    MatPaginatorModule,
    MatToolbarModule,

    AllorsMaterialChipsModule,
    AllorsMaterialFilterModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialSelectModule,
    AllorsMaterialTableModule
  ],
})
export class OrganisationsModule {}
