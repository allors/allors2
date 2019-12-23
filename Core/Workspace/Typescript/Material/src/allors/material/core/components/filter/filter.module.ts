import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatStepperModule } from '@angular/material/stepper';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDatepickerModule } from '@angular/material/datepicker';

import { AllorsFocusModule } from '../../../../angular';
import { AllorsMaterialFilterDialogComponent } from './filter-dialog.component';
import { AllorsMaterialFilterSearchComponent } from './filter-search.component';

import { AllorsMaterialFilterComponent } from './filter.component';
export { AllorsMaterialFilterComponent } from './filter.component';

@NgModule({
  declarations: [
    AllorsMaterialFilterComponent,
    AllorsMaterialFilterDialogComponent,
    AllorsMaterialFilterSearchComponent,
  ],
  exports: [
    AllorsMaterialFilterComponent,
  ],
  entryComponents: [
    AllorsMaterialFilterDialogComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatChipsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatStepperModule,
    MatToolbarModule,
    MatDatepickerModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialFilterModule {
}
