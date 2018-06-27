import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatSelectModule } from '@angular/material';

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
  ],
})
export class AllorsMaterialSelectModule {
}
