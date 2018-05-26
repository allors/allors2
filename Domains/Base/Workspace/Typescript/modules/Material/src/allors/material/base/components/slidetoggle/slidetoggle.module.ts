import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule, MatSlideToggleModule } from '@angular/material';

import { AllorsMaterialSlideToggleComponent } from './slidetoggle.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { AllorsMaterialSlideToggleComponent } from './slidetoggle.component';

@NgModule({
  declarations: [
    AllorsMaterialSlideToggleComponent,
  ],
  exports: [
    AllorsMaterialSlideToggleComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
    MatSlideToggleModule,
  ],
})
export class AllorsMaterialSlideToggleModule {
}
