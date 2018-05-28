import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatCheckboxModule, MatInputModule } from '@angular/material';

import { AllorsMaterialCheckboxComponent } from './checkbox.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { AllorsMaterialCheckboxComponent } from './checkbox.component';

@NgModule({
  declarations: [
    AllorsMaterialCheckboxComponent,
  ],
  exports: [
    AllorsMaterialCheckboxComponent,
  ],
  imports: [
    CommonModule,
    FlexLayoutModule,
    FormsModule,
    MatInputModule,
    MatCheckboxModule,
  ],
})
export class AllorsMaterialCheckboxModule {
}
