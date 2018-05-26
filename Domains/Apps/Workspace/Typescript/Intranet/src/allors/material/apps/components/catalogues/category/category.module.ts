import { NgModule } from '@angular/core';




import { CategoryComponent } from './category.component';
import { FormsModule } from '@angular/forms';
export { CategoryComponent } from './category.component';

@NgModule({
  declarations: [
    CategoryComponent,
  ],
  exports: [
    CategoryComponent,

    
    
  ],
  imports: [
    FormsModule
    
  ],
})
export class CategoryModule {}
