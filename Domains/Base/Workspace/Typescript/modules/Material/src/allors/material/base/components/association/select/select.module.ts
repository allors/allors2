import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material';

import { AllorsFocusModule } from '../../../../../angular';

import { AllorsMaterialAssociationSelectComponent } from './select.component';
export { AllorsMaterialAssociationSelectComponent } from './select.component';

@NgModule({
  declarations: [
    AllorsMaterialAssociationSelectComponent,
  ],
  exports: [
    AllorsMaterialAssociationSelectComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    MatSelectModule,
    AllorsFocusModule,
  ],
})
export class AllorsMaterialAssociationSelectModule {
}
