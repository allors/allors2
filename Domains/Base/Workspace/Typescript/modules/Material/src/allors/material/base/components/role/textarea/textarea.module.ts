import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialTextareaComponent } from './textarea.component';
export { AllorsMaterialTextareaComponent as TextareaComponent } from './textarea.component';

@NgModule({
  declarations: [
    AllorsMaterialTextareaComponent,
  ],
  exports: [
    AllorsMaterialTextareaComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialTextAreaModule {
}
