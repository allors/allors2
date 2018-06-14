import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule } from '@angular/material';

import { AllorsMaterialAutoCompleteModule, AllorsMaterialInputModule, AllorsMaterialStaticModule, AllorsMaterialChipsModule, AllorsMaterialSelectModule } from '../../../../../material';

import { OrganisationComponent } from './organisation.component';
export { OrganisationComponent } from './organisation.component';

@NgModule({
  declarations: [
    OrganisationComponent,
  ],
  exports: [
    OrganisationComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatToolbarModule,
    AllorsMaterialAutoCompleteModule,
    AllorsMaterialChipsModule,
    AllorsMaterialInputModule,
    AllorsMaterialStaticModule,
    AllorsMaterialSelectModule
  ],
})
export class OrganisationModule {}
