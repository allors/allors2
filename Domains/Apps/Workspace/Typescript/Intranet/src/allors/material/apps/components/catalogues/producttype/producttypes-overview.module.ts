import { NgModule } from '@angular/core';


import { ProductTypesOverviewComponent } from './producttypes-overview.component';
import { FormsModule } from '@angular/forms';
export { ProductTypesOverviewComponent } from './producttypes-overview.component';

@NgModule({
  declarations: [
    ProductTypesOverviewComponent,
  ],
  exports: [
    ProductTypesOverviewComponent,
    
  ],
  imports: [
    FormsModule
  ],
})
export class ProductTypesOverviewModule {}
