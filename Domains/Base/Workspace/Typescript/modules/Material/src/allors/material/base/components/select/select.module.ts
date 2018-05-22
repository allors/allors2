import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatSelectModule } from '@angular/material';

import { SelectComponent } from './select.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { SelectComponent } from './select.component';

@NgModule({
  declarations: [
    SelectComponent,
  ],
  exports: [
    SelectComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatSelectModule,
  ],
})
export class SelectModule {
}
