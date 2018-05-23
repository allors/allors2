import { NgModule } from '@angular/core';

import { InlineModule } from '../../inline.module';
import { SharedModule } from '../../shared.module';

import { NonSerialisedGoodComponent } from './nonserialisedgood.component';
export { NonSerialisedGoodComponent } from './nonserialisedgood.component';

@NgModule({
  declarations: [
    NonSerialisedGoodComponent,
  ],
  exports: [
    NonSerialisedGoodComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class NonSerialisedGoodModule {}
