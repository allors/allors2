import { NgModule } from '@angular/core';

import { InlineModule } from '../../../inline.module';
import { SharedModule } from '../../../shared.module';

import { IncoTermEditComponent } from './incoterm.component';
export { IncoTermEditComponent } from './incoterm.component';

@NgModule({
  declarations: [
    IncoTermEditComponent,
  ],
  exports: [
    IncoTermEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class IncoTermEditModule {}
