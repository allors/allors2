import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialRadioGroupComponent } from './radiogroup.component';
export { AllorsMaterialRadioGroupComponent } from './radiogroup.component';

export { RadioGroupOption } from './radiogroup.component';

@NgModule({
  declarations: [
    AllorsMaterialRadioGroupComponent,
  ],
  exports: [
    AllorsMaterialRadioGroupComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    MatRadioModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialRadioGroupModule {
}
