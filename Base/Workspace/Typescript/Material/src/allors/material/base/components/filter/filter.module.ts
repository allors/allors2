import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatDialogModule, MatChipsModule, MatToolbarModule, MatStepperModule, MatSelectModule, MatAutocompleteModule, MatSlideToggleModule } from '@angular/material';

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
    AllorsFocusModule
  ],
})
export class AllorsMaterialFilterModule {
}
