import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AllorsFocusModule } from '../../../../angular';

import { AllorsMaterialFooterComponent } from './footer.component';
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
    MatToolbarModule,
    AllorsFocusModule
  ],
})
export class AllorsMaterialFooterModule {
}
