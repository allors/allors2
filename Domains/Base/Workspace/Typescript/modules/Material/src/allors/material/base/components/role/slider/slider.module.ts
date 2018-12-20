import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule, MatSliderModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialSliderComponent } from './slider.component';
export { AllorsMaterialSliderComponent } from './slider.component';

@NgModule({
  declarations: [
    AllorsMaterialSliderComponent,
  ],
  exports: [
    AllorsMaterialSliderComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    MatSliderModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialSliderModule {
}
