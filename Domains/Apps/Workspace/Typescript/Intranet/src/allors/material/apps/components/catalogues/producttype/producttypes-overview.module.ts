import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { ProductTypesOverviewComponent } from './producttypes-overview.component';
export { ProductTypesOverviewComponent } from './producttypes-overview.component';

@NgModule({
  declarations: [
    ProductTypesOverviewComponent,
  ],
  exports: [
    ProductTypesOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class ProductTypesOverviewModule {}
