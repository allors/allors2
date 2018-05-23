import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { InlineBrandComponent } from './brand-inline.component';
export { InlineBrandComponent } from './brand-inline.component';

@NgModule({
  declarations: [
    InlineBrandComponent,
  ],
  exports: [
    InlineBrandComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class BrandInlineModule {}
