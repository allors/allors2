import { NgModule } from '@angular/core';

import { InlineModule } from '../../inline.module';
import { SharedModule } from '../../shared.module';

import { SalesOrderItemEditComponent } from './salesorderitem.component';
export { SalesOrderItemEditComponent } from './salesorderitem.component';

@NgModule({
  declarations: [
    SalesOrderItemEditComponent,
  ],
  exports: [
    SalesOrderItemEditComponent,

    InlineModule,
    SharedModule,
  ],
  imports: [
    InlineModule,
    SharedModule,
  ],
})
export class SalesOrderItemEditModule {}
