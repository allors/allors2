import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { InlineModelComponent } from './model-inline.component';
export { InlineModelComponent } from './model-inline.component';

@NgModule({
  declarations: [
    InlineModelComponent,
  ],
  exports: [
    InlineModelComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class ModelInlineModule {}
