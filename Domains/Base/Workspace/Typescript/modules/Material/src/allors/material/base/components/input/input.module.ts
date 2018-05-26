import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatInputModule } from '@angular/material';

import { AllorsMaterialInputComponent } from './input.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { AllorsMaterialInputComponent } from './input.component';

@NgModule({
  declarations: [
    AllorsMaterialInputComponent,
  ],
  exports: [
    AllorsMaterialInputComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatInputModule,
  ],
})
export class AllorsMaterialInputModule {
}
