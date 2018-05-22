import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatCheckboxModule, MatInputModule } from '@angular/material';

import { CheckboxComponent } from './checkbox.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { CheckboxComponent } from './checkbox.component';

@NgModule({
  declarations: [
    CheckboxComponent,
  ],
  exports: [
    CheckboxComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
    MatCheckboxModule,
  ],
})
export class CheckboxModule {
}
