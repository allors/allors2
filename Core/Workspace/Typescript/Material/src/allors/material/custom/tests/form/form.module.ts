import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';

import {
  AllorsMaterialAssociationSelectModule,
  AllorsMaterialStaticModule, AllorsMaterialChipsModule, AllorsMaterialDatetimepickerModule,
  AllorsMaterialSelectModule, AllorsMaterialInputModule,
  AllorsMaterialAutoCompleteModule, AllorsMaterialCheckboxModule, AllorsMaterialDatepickerModule,
  AllorsMaterialFileModule, AllorsMaterialFilesModule,
  AllorsMaterialLocalisedTextModule, AllorsMaterialRadioGroupModule, AllorsMaterialSliderModule,
  AllorsMaterialSlideToggleModule, AllorsMaterialTextAreaModule, AllorsMaterialSideNavToggleModule,
  AllorsMaterialMarkdownModule,
  AllorsMaterialQuillModule
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
    AllorsMaterialMarkdownModule,
    AllorsMaterialQuillModule,
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
