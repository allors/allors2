import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDatepickerModule, MatIconModule, MatInputModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../angular';

import { AllorsMaterialDatepickerComponent } from './datepicker.component';
export { AllorsMaterialDatepickerComponent } from './datepicker.component';

@NgModule({
  declarations: [
    AllorsMaterialDatepickerComponent,
  ],
  exports: [
    AllorsMaterialDatepickerComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    MatIconModule,
    MatDatepickerModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialDatepickerModule {
}
