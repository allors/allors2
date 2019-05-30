import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialSelectComponent } from './select.component';
export { AllorsMaterialSelectComponent } from './select.component';

@NgModule({
  declarations: [
    AllorsMaterialSelectComponent,
  ],
  exports: [
    AllorsMaterialSelectComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatSelectModule,
    AllorsFocusModule,
  ],
})
export class AllorsMaterialSelectModule {
}
