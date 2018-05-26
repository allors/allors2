import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatSelectModule } from '@angular/material';

import { AllorsMaterialSelectComponent } from './select.component';
import { FlexLayoutModule } from '@angular/flex-layout';
export { AllorsMaterialSelectComponent } from './select.component';

@NgModule({
  declarations: [
    AllorsMaterialSelectComponent,
  ],
  exports: [
    AllorsMaterialSelectComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    CommonModule,
    MatSelectModule,
  ],
})
export class AllorsMaterialSelectModule {
}
