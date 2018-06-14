import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule,
         MatMenuModule, MatToolbarModule } from '@angular/material';

import { AllorsMaterialStaticModule, AllorsMaterialChipsModule, AllorsMaterialSelectModule } from '../../../../material';

import { OrganisationsComponent } from './organisations.component';
import { FlexLayoutModule } from '@angular/flex-layout';
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
    FlexLayoutModule,
    RouterModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    MatToolbarModule,
    AllorsMaterialChipsModule,
    AllorsMaterialStaticModule,
    AllorsMaterialSelectModule,
  ],
})
export class OrganisationsModule {}
