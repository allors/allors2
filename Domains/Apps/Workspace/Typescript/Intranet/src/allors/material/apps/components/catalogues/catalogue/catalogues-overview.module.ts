import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared.module';

import { CataloguesOverviewComponent } from './catalogues-overview.component';
export { CataloguesOverviewComponent } from './catalogues-overview.component';

@NgModule({
  declarations: [
    CataloguesOverviewComponent,
  ],
  exports: [
    CataloguesOverviewComponent,
    SharedModule,
  ],
  imports: [
    SharedModule,
  ],
})
export class CataloguesOverviewModule {}
