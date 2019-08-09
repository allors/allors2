import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AllorsMaterialFooterModule } from '../footer.module';

import { AllorsMaterialFooterSaveCancelComponent } from './savecancel.component';
import { MatButtonModule } from '@angular/material/button';
export { AllorsMaterialFooterSaveCancelComponent } from './savecancel.component';

@NgModule({
  declarations: [
    AllorsMaterialFooterSaveCancelComponent,
  ],
  exports: [
    AllorsMaterialFooterSaveCancelComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    AllorsMaterialFooterModule,
    MatButtonModule,
  ],
})
export class AllorsMaterialFooterSaveCancelModule {
}
