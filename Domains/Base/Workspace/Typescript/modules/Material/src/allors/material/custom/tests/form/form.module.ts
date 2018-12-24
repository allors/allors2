import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import {
  MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule,
  MatMenuModule, MatToolbarModule
} from '@angular/material';

import {
  AllorsMaterialAssociationSelectModule,
  AllorsMaterialStaticModule, AllorsMaterialChipsModule, AllorsMaterialDatetimepickerModule, AllorsMaterialSelectModule, AllorsMaterialInputModule,
  AllorsMaterialAutoCompleteModule, AllorsMaterialCheckboxModule, AllorsMaterialDatepickerModule, AllorsMaterialFileModule, AllorsMaterialFilesModule,
  AllorsMaterialLocalisedTextModule, AllorsMaterialRadioGroupModule, AllorsMaterialSliderModule, AllorsMaterialSlideToggleModule, AllorsMaterialTextAreaModule, AllorsMaterialSideNavToggleModule
} from '../../../../material';

import { FormComponent } from './form.component';
export { FormComponent } from './form.component';

@NgModule({
  declarations: [
    FormComponent,
  ],
  exports: [
    FormComponent,
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

    AllorsMaterialAssociationSelectModule,
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
export class FormModule { }
