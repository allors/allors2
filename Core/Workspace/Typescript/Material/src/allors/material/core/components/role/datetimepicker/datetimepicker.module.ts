import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialDatetimepickerComponent } from './datetimepicker.component';
export { AllorsMaterialDatetimepickerComponent } from './datetimepicker.component';

@NgModule({
  declarations: [
    AllorsMaterialDatetimepickerComponent,
  ],
  exports: [
    AllorsMaterialDatetimepickerComponent,
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
export class AllorsMaterialDatetimepickerModule {
}
