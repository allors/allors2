import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../angular';

import { TextareaComponent } from './textarea.component';
export { TextareaComponent } from './textarea.component';

@NgModule({
  declarations: [
    TextareaComponent,
  ],
  exports: [
    TextareaComponent,
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
