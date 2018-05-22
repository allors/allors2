import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule } from '@angular/material';

import { InputComponent } from './input.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { InputComponent } from './input.component';

@NgModule({
  declarations: [
    InputComponent,
  ],
  exports: [
    InputComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
  ],
})
export class InputModule {
}
