import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AllorsMaterialFooterComponent } from './footer.component';
import { MatToolbarModule } from '@angular/material';
export { AllorsMaterialFooterComponent } from './footer.component';

@NgModule({
  declarations: [
    AllorsMaterialFooterComponent,
  ],
  exports: [
    AllorsMaterialFooterComponent,
  ],
  imports: [
    CommonModule,
    MatToolbarModule
  ],
})
export class AllorsMaterialFooterModule {
}
