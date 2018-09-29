import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCheckboxModule, MatInputModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../angular';

import { AllorsMaterialCheckboxComponent } from './checkbox.component';
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
    FormsModule,
    MatInputModule,
    MatCheckboxModule,
    AllorsFocusModule,
  ],
})
export class AllorsMaterialCheckboxModule {
}
