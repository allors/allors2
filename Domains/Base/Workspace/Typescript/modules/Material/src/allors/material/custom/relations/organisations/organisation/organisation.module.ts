import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule, MatMenuModule } from '@angular/material';

import { AllorsMaterialAutoCompleteModule, AllorsMaterialDatetimepickerModule, AllorsMaterialInputModule, AllorsMaterialStaticModule, AllorsMaterialChipsModule, AllorsMaterialSelectModule, AllorsMaterialCheckboxModule, AllorsMaterialDatepickerModule, AllorsMaterialFileModule, AllorsMaterialFilesModule, AllorsMaterialLocalisedTextModule, AllorsMaterialSideNavToggleModule, AllorsMaterialSlideToggleModule, AllorsMaterialSliderModule, AllorsMaterialTextAreaModule, AllorsMaterialRadioGroupModule } from '../../../../../material';

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
    RouterModule,

    MatAutocompleteModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    MatToolbarModule,

    AllorsMaterialAutoCompleteModule,
    AllorsMaterialCheckboxModule,
    AllorsMaterialChipsModule,
    AllorsMaterialDatepickerModule,
    AllorsMaterialDatetimepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialFilesModule,
    AllorsMaterialInputModule,
    AllorsMaterialLocalisedTextModule,
    AllorsMaterialRadioGroupModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialSliderModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
  ],
})
export class OrganisationModule {}
