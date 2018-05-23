import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { SalesOrderOverviewComponent } from './salesorder-overview.component';
export { SalesOrderOverviewComponent } from './salesorder-overview.component';

@NgModule({
  declarations: [
    SalesOrderOverviewComponent,
  ],
  exports: [
    SalesOrderOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class SalesOrderOverviewModule {}
