import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule, MatMenuModule } from '@angular/material';

import { AllorsMaterialAutoCompleteModule, AllorsMaterialChipsModule, AllorsMaterialFileModule, AllorsMaterialFilesModule, AllorsMaterialInputModule, AllorsMaterialSelectModule, AllorsMaterialStaticModule, AllorsMaterialCheckboxModule, AllorsMaterialDatepickerModule, AllorsMaterialLocalisedTextModule, AllorsMaterialRadioGroupModule, AllorsMaterialSideNavToggleModule, AllorsMaterialSlideToggleModule, AllorsMaterialSliderModule, AllorsMaterialTextAreaModule } from '../../../../../material';

import { PersonComponent } from './person.component';
import { RouterModule } from '../../../../../../../node_modules/@angular/router';
import { AllorsMaterialDatetimepickerModule } from '../../../../base/components/datetimepicker';

export { PersonComponent } from './person.component';

@NgModule({
  declarations: [
    PersonComponent,
  ],
  exports: [
    PersonComponent,
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
export class PersonModule {}
