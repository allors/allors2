import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule } from '@angular/material';

import { TextareaComponent } from './textarea.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { TextareaComponent } from './textarea.component';

@NgModule({
  declarations: [
    TextareaComponent,
  ],
  exports: [
    TextareaComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
  ],
})
export class AllorsMaterialTextAreaModule {
}
