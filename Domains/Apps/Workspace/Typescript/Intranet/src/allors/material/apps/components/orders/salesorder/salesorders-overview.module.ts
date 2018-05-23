import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { SalesOrdersOverviewComponent } from './salesorders-overview.component';
export { SalesOrdersOverviewComponent } from './salesorders-overview.component';

@NgModule({
  declarations: [
    SalesOrdersOverviewComponent,
  ],
  exports: [
    SalesOrdersOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class SalesOrdersOverviewModule {}
