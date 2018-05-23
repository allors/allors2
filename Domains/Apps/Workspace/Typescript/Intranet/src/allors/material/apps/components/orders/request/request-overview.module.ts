import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { RequestOverviewComponent } from './request-overview.component';
export { RequestOverviewComponent } from './request-overview.component';

@NgModule({
  declarations: [
    RequestOverviewComponent,
  ],
  exports: [
    RequestOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class RequestOverviewModule {}
