import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatInputModule } from '@angular/material';

import { ChipsComponent } from './chips.component';
export { ChipsComponent } from './chips.component';

@NgModule({
  declarations: [
    ChipsComponent,
  ],
  exports: [
    ChipsComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
  ],
})
export class ChipsModule {}
