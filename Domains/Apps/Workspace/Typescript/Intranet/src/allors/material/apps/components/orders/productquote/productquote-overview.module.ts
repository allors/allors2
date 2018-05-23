import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { ProductQuoteOverviewComponent } from './productquote-overview.component';
export { ProductQuoteOverviewComponent } from './productquote-overview.component';

@NgModule({
  declarations: [
    ProductQuoteOverviewComponent,
  ],
  exports: [
    ProductQuoteOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class ProductQuoteOverviewModule {}
