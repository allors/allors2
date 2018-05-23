import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { RequestsOverviewComponent } from './requests-overview.component';
export { RequestsOverviewComponent } from './requests-overview.component';

@NgModule({
  declarations: [
    RequestsOverviewComponent,
  ],
  exports: [
    RequestsOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class RequestsOverviewModule {}
