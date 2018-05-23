import { NgModule } from '@angular/core';

import { InlineModule } from '../../inline.module';
import { SharedModule } from '../../shared.module';

import { SerialisedGoodComponent } from './serialisedgood.component';
export { SerialisedGoodComponent } from './serialisedgood.component';

@NgModule({
  declarations: [
    SerialisedGoodComponent,
  ],
  exports: [
    SerialisedGoodComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class SerialisedGoodModule {}
