import { NgModule } from '@angular/core';

import { InlineModule } from '../../inline.module';
import { SharedModule } from '../../shared.module';

import { RequestItemEditComponent } from './requestitem.component';
export { RequestItemEditComponent } from './requestitem.component';

@NgModule({
  declarations: [
    RequestItemEditComponent,
  ],
  exports: [
    RequestItemEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class RequestItemEditModule {}
