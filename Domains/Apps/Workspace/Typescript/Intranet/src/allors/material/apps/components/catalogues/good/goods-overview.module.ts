import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { GoodsOverviewComponent } from './goods-overview.component';
export { GoodsOverviewComponent } from './goods-overview.component';

@NgModule({
  declarations: [
    GoodsOverviewComponent,
  ],
  exports: [
    GoodsOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class GoodsOverviewModule {}
