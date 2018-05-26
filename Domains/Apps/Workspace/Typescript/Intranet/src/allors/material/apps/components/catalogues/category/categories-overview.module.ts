import { NgModule } from '@angular/core';


import { CategoriesOverviewComponent } from './categories-overview.component';
import { FormsModule } from '@angular/forms';
export { CategoriesOverviewComponent } from './categories-overview.component';

@NgModule({
  declarations: [
    CategoriesOverviewComponent,
  ],
  exports: [
    CategoriesOverviewComponent,
    
  ],
  imports: [
    FormsModule
  ],
})
export class CategoriesOverviewModule {}
