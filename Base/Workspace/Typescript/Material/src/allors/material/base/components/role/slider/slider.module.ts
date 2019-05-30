import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSliderModule } from '@angular/material/slider';

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
