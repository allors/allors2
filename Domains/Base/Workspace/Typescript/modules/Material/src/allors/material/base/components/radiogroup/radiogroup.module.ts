import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule, MatRadioModule } from '@angular/material';

import { RadioGroupComponent } from './radiogroup.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { RadioGroupComponent } from './radiogroup.component';

@NgModule({
  declarations: [
    RadioGroupComponent,
  ],
  exports: [
    RadioGroupComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
    MatRadioModule,
  ],
})
export class RadioGroupModule {
}
