import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule, MatSlideToggleModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialSlideToggleComponent } from './slidetoggle.component';
export { AllorsMaterialSlideToggleComponent } from './slidetoggle.component';

@NgModule({
  declarations: [
    AllorsMaterialSlideToggleComponent,
  ],
  exports: [
    AllorsMaterialSlideToggleComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    MatSlideToggleModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialSlideToggleModule {
}
