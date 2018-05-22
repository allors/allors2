import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule, MatSlideToggleModule } from '@angular/material';

import { SlideToggleComponent } from './slidetoggle.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { SlideToggleComponent } from './slidetoggle.component';

@NgModule({
  declarations: [
    SlideToggleComponent,
  ],
  exports: [
    SlideToggleComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
    MatSlideToggleModule,
  ],
})
export class SlideToggleModule {
}
