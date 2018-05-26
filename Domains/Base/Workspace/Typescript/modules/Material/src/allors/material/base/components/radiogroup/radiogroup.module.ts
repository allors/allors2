import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule, MatRadioModule } from '@angular/material';

import { AllorsMaterialRadioGroupComponent } from './radiogroup.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { AllorsMaterialRadioGroupComponent } from './radiogroup.component';

@NgModule({
  declarations: [
    AllorsMaterialRadioGroupComponent,
  ],
  exports: [
    AllorsMaterialRadioGroupComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
    MatRadioModule,
  ],
})
export class AllorsMaterialRadioGroupModule {
}
