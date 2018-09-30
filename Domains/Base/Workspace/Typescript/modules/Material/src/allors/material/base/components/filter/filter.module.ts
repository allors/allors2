import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatDialogModule, MatChipsModule, MatToolbarModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../angular';
import { AllorsMaterialFilterDialogComponent } from './filter-dialog.component';
import { AllorsMaterialFilterService } from './filter.service';

import { AllorsMaterialFilterComponent } from './filter.component';
export { AllorsMaterialFilterComponent } from './filter.component';

@NgModule({
  declarations: [
    AllorsMaterialFilterComponent,
    AllorsMaterialFilterDialogComponent
  ],
  exports: [
    AllorsMaterialFilterComponent,
    AllorsMaterialFilterDialogComponent
  ],
  entryComponents: [
    AllorsMaterialFilterDialogComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatChipsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatToolbarModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialFilterModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: AllorsMaterialFilterModule,
      providers: [ AllorsMaterialFilterService ]
    };
  }
}
