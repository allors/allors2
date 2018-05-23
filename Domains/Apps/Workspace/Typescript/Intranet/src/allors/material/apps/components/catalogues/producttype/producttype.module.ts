import { NgModule } from '@angular/core';

import { InlineModule } from '../../inline.module';
import { SharedModule } from '../../shared.module';

import { ProductTypeComponent } from './producttype.component';
export { ProductTypeComponent } from './producttype.component';

@NgModule({
  declarations: [
    ProductTypeComponent,
  ],
  exports: [
    ProductTypeComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class ProductTypeModule {}
